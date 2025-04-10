using System;
using System.Collections.Generic;
using System.Linq;
using Candoumbe.Pipelines.Components.Workflows;
using JetBrains.Annotations;
using Microsoft.Win32;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can perform mutation tests using <see href="https://stryker-mutator.io/">Stryker</see>.
/// </summary>
/// <remarks>
/// <see cref="MutationTests"/> target required <see href="https://www.nuget.org/packages/dotnet-stryker">Stryker dotnet tool</see> to be referenced in order to run.<br />
/// Simply adds <c>&lt;PackageReference Include="dotnet-stryker" Version="" ExcludeAssets="all" &gt;</c> to your build project file
/// </remarks>
public interface IMutationTest : IHaveTests
{
    /// <summary>
    /// Name of the property set when <see href="SourceLink">SourceLink</see> is enabled on a project.
    /// </summary>
    private const string ContinuousIntegrationBuild = nameof(ContinuousIntegrationBuild);

    /// <summary>
    /// Directory where mutation test results should be published
    /// </summary>
    AbsolutePath MutationTestResultDirectory => TestResultDirectory / "mutation-tests";

    /// <summary>
    /// Api Key us
    /// </summary>
    [Parameter("API KEY used to submit mutation report to a stryker dashboard")]
    [Secret]
    public string StrykerDashboardApiKey => TryGetValue(() => StrykerDashboardApiKey);

    /// <summary>
    /// Defines projects onto which mutation tests will be performed
    /// </summary>
    /// <remarks>
    /// The source project has
    /// </remarks>
    IEnumerable<MutationProjectConfiguration> MutationTestsProjects { get; }

    /// <summary>
    /// Executes mutation tests for the specified <see cref="MutationTestsProjects"/>.
    /// </summary>
    /// <remarks></remarks>
    public Target MutationTests => _ => _
        .Description("Runs mutation tests using Stryker tool")
        .TryDependsOn<IClean>(x => x.Clean)
        .TryBefore<IPack>()
        .TryDependsOn<ICompile>(x => x.Compile)
        .Produces(MutationTestResultDirectory / "*")
        .Executes(() =>
        {
            // List of frameworks that source projects are tested against
            IReadOnlyCollection<string> frameworks = MutationTestsProjects.Select(csprojAndTest => csprojAndTest.TestProjects)
                                                                          .SelectMany(projects => projects)
                                                                          .Select(csproj => csproj.GetTargetFrameworks())
                                                                          .SelectMany(x => x)
                                                                          .Select(targetFramework => targetFramework.Trim())
                                                                          .AsParallel()
                                                                          .Distinct(StringComparer.OrdinalIgnoreCase)
                                                                          .ToArray();

            if (frameworks.AtLeast(2))
            {
                // We iterate over each test project
                MutationTestsProjects.ForEach(mutationProject =>
                {
                    // We retrieve the current set of frameworks the current test project are tested against
                    IReadOnlyCollection<string> testedFrameworks = mutationProject.TestProjects.Select(csproj => csproj.GetTargetFrameworks())
                                                                                               .SelectMany(frmworks => frmworks)
                                                                                               .Distinct(StringComparer.OrdinalIgnoreCase)
                                                                                               .ToArray();

                    testedFrameworks.ForEach(framework => RunMutationTestsForTheProject(mutationProject, framework));
                });
            }
            else
            {
                string framework = frameworks.Single();

                MutationTestsProjects.ForEach(mutationProject => RunMutationTestsForTheProject(mutationProject, framework));
            }

            // Run mutation tests for the specified project using the specified arguments
            void RunMutationTestsForTheProject(MutationProjectConfiguration mutationProject, string framework)
            {
                (Project sourceProject, IEnumerable<Project> testsProjects, AbsolutePath configFile) = mutationProject;

                Verbose("{ProjectName} will run mutation tests for the following frameworks : {@Frameworks}", sourceProject.Name, sourceProject.GetTargetFrameworks());

                StrykerOptions strykerArgs = new();
                strykerArgs = StrykerArgumentsSettingsBase.Invoke(strykerArgs);
                strykerArgs = StrykerArgumentsSettings.Invoke(strykerArgs);

                strykerArgs.TargetFramework = framework;
                strykerArgs.Output = MutationTestResultDirectory / sourceProject.Name / framework;
                strykerArgs.Project = $"{sourceProject.Name}{sourceProject.Path.Extension}";

                if (configFile is not null)
                {
                    strykerArgs.ConfigFile = configFile;
                }

                strykerArgs.TestProjects = [.. testsProjects.Select(project => project.Path)];

                switch (this)
                {
                    case IGitFlow { GitRepository: { } gitflowRepository } gitFlow:
                        {
                            strykerArgs.ProjectInfoVersion = $"{gitflowRepository.Commit ?? gitflowRepository.Branch}";
                            switch (gitflowRepository.Branch)
                            {
                                case { } branchName when string.Equals(branchName, IHaveDevelopBranch.DevelopBranchName, StringComparison.InvariantCultureIgnoreCase):
                                    {
                                        // we are in git flow, so we can compare develop with main branch
                                        strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--with-baseline:{IHaveMainBranch.MainBranchName}");
                                    }
                                    break;
                                case { } branchName when branchName.Like($"{gitFlow.FeatureBranchPrefix}/*", true):
                                    {
                                        strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--with-baseline:{gitFlow.FeatureBranchSourceName}");
                                    }
                                    break;
                                case { } branchName when branchName.Like($"{gitFlow.ColdfixBranchPrefix}/*", true):
                                    {
                                        strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--with-baseline:{gitFlow.ColdfixBranchSourceName}");
                                    }
                                    break;
                                default:
                                    {
                                        // By default, we try to compare the current commit with the previous one (if any).
                                        if (gitflowRepository.Head is { } head)
                                        {
                                            strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--with-baseline:{head}");
                                        }
                                    }
                                    break;
                            }

                            break;
                        }

                    case IGitHubFlow { GitRepository: { } githubFlowRepository }:
                        {
                            strykerArgs.ProjectInfoVersion = githubFlowRepository.Commit ?? githubFlowRepository.Branch;
                            if (githubFlowRepository.Branch is { Length: > 0 } branchName && !string.Equals(branchName, IHaveMainBranch.MainBranchName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--with-baseline:{IHaveMainBranch.MainBranchName}");
                            }

                            break;
                        }

                    default:
                    {
                        if (!sourceProject.IsSourceLinkEnabled())
                        {
                            switch (this)
                            {
                                case IHaveGitVersion gitVersion:
                                    strykerArgs.ProjectInfoVersion = gitVersion.MajorMinorPatchVersion;
                                    break;
                                case IHaveGitRepository gitRepository:
                                    strykerArgs.ProjectInfoVersion = gitRepository.GitRepository?.Commit ?? gitRepository.GitRepository?.Branch;
                                    break;
                            }
                        }
                        break;
                    }
                }

                strykerArgs = strykerArgs.SetProcessWorkingDirectory(sourceProject.Path.Parent);
                DotNet(strykerArgs.ToString() , workingDirectory: strykerArgs.ProcessWorkingDirectory);
            }
        });

    internal Configure<StrykerOptions> StrykerArgumentsSettingsBase => _
        =>
    {
        StrykerOptions args = new();
        if (IsLocalBuild)
        {
            args.OpenReport = StrykerOpenReport.Html;
            args.Reporters = [..args.Reporters, StrykerReporter.Progress];
        }

        if (StrykerDashboardApiKey is not null)
        {
            args.DashboardApiKey = StrykerDashboardApiKey;
            args.Reporters = [..args.Reporters, StrykerReporter.Dashboard];
        }

        return args;
    };

    /// <summary>
    /// Configures arguments that will be used by when running Stryker tool
    /// </summary>
    Configure<StrykerOptions> StrykerArgumentsSettings => _ => _;
}

/// <summary>
/// Wraps information on mutation tests for a specific project.
/// </summary>
[UsedImplicitly]
public record MutationProjectConfiguration
{
    /// <summary>
    /// The project for which mutation tests will be run.
    /// </summary>
    public Project SourceProject { get; }

    /// <summary>
    /// The set of projects used to validate the mutation performed.
    /// </summary>
    public IReadOnlySet<Project> TestProjects { get; }

    /// <summary>
    /// The path to the configuration file used by the mutation testing tool.
    /// </summary>
    public AbsolutePath ConfigurationFile { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutationProjectConfiguration"/> class.
    /// </summary>
    /// <param name="sourceProject">The project for which mutation tests will be run.</param>
    /// <param name="testProjects">The set of projects used to validate the mutation performed.</param>
    /// <param name="configurationFile">The path to the configuration file used by the mutation testing tool.</param>
    /// <exception cref="ArgumentNullException">if either <paramref name="sourceProject"/> or <paramref name="testProjects"/> is <see langword="null"/>.</exception>
    public MutationProjectConfiguration(Project sourceProject, IEnumerable<Project> testProjects, AbsolutePath configurationFile = default)
    {
        SourceProject = sourceProject ?? throw new ArgumentNullException(nameof(sourceProject));
        TestProjects = testProjects.ToHashSet();
        ConfigurationFile = configurationFile;
    }

    /// <summary>
    /// Deconstructs the <see cref="MutationProjectConfiguration"/> into its individual properties.
    /// </summary>
    /// <param name="sourceProject">The project for which mutation tests will be run.</param>
    /// <param name="testProjects">The set of projects used to validate the mutation performed.</param>
    /// <param name="configurationFile">The path to the configuration file used by the mutation testing tool.</param>
    public void Deconstruct(out Project sourceProject, out IReadOnlySet<Project> testProjects, out AbsolutePath configurationFile)
    {
        sourceProject = SourceProject;
        testProjects = TestProjects;
        configurationFile = ConfigurationFile;
    }
}

/// <summary>
/// Represents the configuration for a mutation test run.
/// </summary>
/// <remarks>
/// The <see cref="MutationTestRunConfiguration"/> class contains information such as the module name, test projects, frameworks, and project info.
/// </remarks>
/// <example>
/// <code>
/// MutationTestRunConfiguration config = new MutationTestRunConfiguration
/// {
///     Module = "MyModule",
///     TestsProjects = ["TestProject1", "TestProject2" ],
///     Frameworks = [ "netcoreapp3.1", "net5.0" ],
///     ProjectInfo = new StrykerProjectInfo
///     {
///         Module = "MyProject",
///         Version = "1.0.0"
///     }
/// };
/// </code>
/// </example>
public record MutationTestRunConfiguration
{
    /// <summary>
    /// Gets or sets the name of the module under which the results of the mutation test run are grouped.
    /// </summary>
    public string Module { get; init; }

    /// <summary>
    /// Gets or sets the array of test project names that will be used to validate the mutation performed.
    /// </summary>
    public string[] TestsProjects { get; init; }

    /// <summary>
    /// Gets or sets the array of target frameworks for the mutation test run.
    /// </summary>
    public string[] Frameworks { get; init; }

    /// <summary>
    /// Gets or sets the information about the Stryker project, including the module name and version.
    /// </summary>
    public StrykerProjectInfo ProjectInfo { get; init; }
}

/// <summary>
/// Represents information about a Stryker project, including its module and version.
/// </summary>
/// <example>
/// <code>
/// StrykerProjectInfo projectInfo = new StrykerProjectInfo
/// {
///     Module = "MyProject",
///     Version = "1.0.0"
/// };
/// </code>
/// </example>
public record StrykerProjectInfo
{
    /// <summary>
    /// Gets or sets the name of the module under which the project is grouped.
    /// </summary>
    public string Module { get; init; }

    /// <summary>
    /// Gets or sets the version of the project.
    /// </summary>
    public string Version { get; init; }
}

public class StrykerOptions : ToolOptions
{
    /// <summary>
    /// Path / Name of the configuration file. You can specify a custom path to the config file.
    /// For example if you want to add the stryker config section to your appsettings file. The section should still be called <c>stryker-config</c>
    /// </summary>
    [CanBeNull] public AbsolutePath ConfigFile { get; set; }

    /// <summary>
    /// The solution path can be supplied to help with dependency resolution.
    /// If stryker is run from the solution file location the solution file will be analyzed and all projects in the solution will be tested by stryker.
    /// </summary>
    [CanBeNull] public AbsolutePath Solution { get; set; }

    /// <summary>
    /// The project file name is required when your test project has more than one project reference.
    /// Stryker can currently mutate one project under test for 1..N test projects but not 1..N projects under test for one test project.<br /><i>Do not pass a path to this option. Pass the project file <strong>name</strong> as it appears in your test project's references</i>
    /// </summary>
    [CanBeNull] public string Project { get; set; }

    /// <summary>
    /// When you have multiple test projects covering one project under test you may specify all relevant test projects in the config file.
    /// You must run stryker from the project under test instead of the test project directory when using multiple test projects.
    /// </summary>
    public List<AbsolutePath> TestProjects { get; set; } = [];

    /// <summary>
    /// <para>With <c>mutate</c> you configure the subset of files to use for mutation testing.
    /// Only source files part of your project will be taken into account.
    /// When this option is not specified the whole project will be mutated.
    /// You can add an <c>!</c> in front of the pattern to exclude instead of include matching files.
    /// This can be used to for example ignore generated files while mutating.</para>
    /// <para>When only exclude patterns are provided, all files will be included that do not match any exclude pattern.
    /// If both include and exclude patterns are provided, only the files that match an include pattern but not also an exclude pattern will be included. The order of the patterns is irrelevant.</para>
    /// <para>The patterns support <see href="https://en.wikipedia.org/wiki/Glob_(programming)">globbing syntax</see> to allow wildcards.</para>
    /// <para><strong>Example</strong> :</para>
    /// <table>
    ///     <thead><tr><th>Patterns</th><th>File</th><th>Will be mutated</th></tr></thead>
    ///     <tbody>
    ///         <tr><td>null</td><td>MyFolder/MyFactory.cs</td><td>Yes</td></tr>
    ///         <tr><td>'**/*.*'</td><td>MyFolder/MyFactory.cs</td><td>Yes</td></tr>
    ///         <tr><td>'!**/MyFactory.cs'</td><td>MyFolder/MyFactory.cs</td><td>No</td></tr>
    ///     </tbody>
    /// </table>
    /// <para>To allow more fine-grained filtering you can also specify the span of text that should be in- or excluded. A span is defined by the indices of the first character and the last character. <c>dotnet stryker -m "MyFolder/MyService.cs{10..100}"</c></para>
    /// </summary>
    public List<string> Mutate { get; set; } = [];

    /// <summary>
    /// Allows you to specify the build configuration to use when building the project. This can be useful when you want to test the release build of your project
    /// </summary>
    [CanBeNull]
    public Configuration Configuration{ get; set; }

    /// <summary>
    /// Randomly selected. If the project targets multiple frameworks, this way you can specify the particular framework to build against.
    /// If you specify a non-existent target, Stryker will build the project against a random one (or the only one if so).
    /// </summary>
    [CanBeNull]
    public string TargetFramework { get; set; }

    /// <summary>
    /// The version of the report. This should be filled with the branch name, git tag or git sha (although no validation is done).
    /// You can override a report of a specific version, like docker tags. Slashes in the version should not be encoded. For example, it's valid to use "feat/logging"
    /// </summary>
    [CanBeNull]
    public string ProjectInfoVersion { get; set; }
  
    /// <summary>
    /// Stryker supports multiple <see href="https://stryker-mutator.io/docs/stryker-net/configuration/#mutation-level-level\">mutation level</see>s.
    /// Each level comes with a specific set of mutations. Each level contains the mutations of the levels below it.
    /// By setting the level to <see cref="StrykerMutationLevel.Complete"/> you will get all possible mutations and thus the strictest mutation test. This comes at the price of longer runtime as more mutations will be generated and tested.
    /// </summary>
    /// <remarks>
    /// By default, the value is <see cref="StrykerMutationLevel.Standard"/>
    /// </remarks>
    [CanBeNull]
    public StrykerMutationLevel MutationLevel { get; set; }

    public List<StrykerReporter> Reporters { get; set; } = [];

        /// <summary>
        /// When this option is passed, generated reports should open in the browser automatically once Stryker starts testing mutants, and will update the report till Stryker is done. Both html and dashboard reports can be opened automatically.
        /// </summary>
        [CanBeNull]

    public StrykerOpenReport OpenReport { get; set; }

    /// <summary>
    /// Change the amount of concurrent workers Stryker uses for the mutation test run.
    /// Defaults to using half your logical (virtual) processor count.
    /// <para>
    /// <b>Example</b>: an intel i7 quad-core with hyper-threading has 8 logical cores
    /// and 4 physical cores. Stryker will use 4 concurrent workers when using the default.</para>
    /// </summary>
    [CanBeNull] public uint? Concurrency { get; set; }

    /// <summary>
    /// Must be less than or equal to threshold low. When threshold break is set to anything other than 0 and the mutation score is lower than the threshold Stryker will exit with a non-zero code.
    /// This can be used in a CI pipeline to fail the pipeline when your mutation score is not sufficient.
    /// </summary>
    public uint? BreakAt { get; set; }

    /// <summary>
    /// Minimum good mutation score. Must be higher than or equal to threshold low. Must be higher than 0.
    /// </summary>
    public short? ThresholdHigh { get; set; }

    /// <summary>
    /// Minimum acceptable mutation score. Must be less than or equal to threshold high and more than or equal to threshold break.
    /// </summary>
    public short? ThresholdLow { get; set; }
    
    /// <summary>
    /// Changes the output path for Stryker logs and reports. This can be an absolute or relative path.
    /// </summary>
    public AbsolutePath Output { get; set; }

    /// <summary>
    /// Stryker aborts a unit testrun for a mutant as soon as one test fails because this is enough to confirm the mutant is killed.
    /// This can reduce the total runtime but also means you miss information about individual unit tests (e.g. if a unit test does not kill any mutants and is therefore useless).
    /// You can disable this behavior and run all unit tests for a mutant to completion. This can be especially useful when you want to find useless unit tests.
    /// </summary>
    public bool? DisableBail { get; set; }

    /// <summary>
    /// <para>Use git information to test only code changes since the given target. Stryker will only report on mutants within the changed code. All other mutants will not have a result.</para>
    /// <para>If you wish to test only changed sources and tests but would like to have a complete mutation report see with-baseline.</para>
    /// <para>
    /// Set the diffing target on the command line by passing a committish with the "since" flag in the format <c>--since:&lt;committish&gt;</c>.
    /// Set the diffing target in the config file by setting the "since" target option.
    /// </para>
    /// <para>
    /// <i>* For changes on test project files all mutants covered by tests in that file will be seen as changed.</i>
    /// </para>
    /// </summary>
    [CanBeNull] public string Since { get; set; }

    /// <summary>
    /// Change the console <c>verbosity</c> of Stryker when you want more or less details about the mutation testrun.
    /// </summary>
    [CanBeNull]
    public StrykerVerbosity Verbosity { get; set; }

    /// <summary>
    /// When creating an issue on GitHub you can include a logfile so the issue can be diagnosed easier.<para><i>File logging always uses loglevel trace.</i></para>"
    /// </summary>
    public bool? LogToFile { get; set; }

    /// <summary>
    /// Stryker will not gracefully recover from compilation errors, but instead crash immediately. Used during development to quickly diagnose errors.<para>Also enables more debug logs not generally useful to normal users.</para>
    /// </summary>
    public bool? DevMode { get; set; }

    /// <summary>
    /// The API key for authentication with the Stryker dashboard.<br />Get your api key at the <see href="https://dashboard.stryker-mutator.io/">stryker dashboard</see>.
    /// To keep your api key safe, store it in an encrypted variable in your pipeline.
    /// </summary>
    [CanBeNull] public string DashboardApiKey { get; set; }

    /// <summary>
    /// By default, Stryker tries to auto-discover msbuild on your system. If Stryker fails to discover msbuild you may supply the path to msbuild manually with this option.
    /// </summary>
    [CanBeNull] public AbsolutePath MsBuildPath { get; set; }

    /// <summary>
    /// Instruct Stryker to break execution when at least one test failed on initial test run.
    /// </summary>
    public bool? BreakOnInitialTestFailure { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        ArgumentStringHandler args = new();
        args.AppendLiteral("stryker");

        if (Solution is not null)
        {
            args.AppendLiteral($" --solution {Solution}");
        }

        if (Project is not null)
        {
            args.AppendLiteral($" --project {Project}");
        }

        if (LogToFile is true)
        {
            args.AppendLiteral(" --log-to-file");
        }

        if (TestProjects is [..])
        {
            foreach (AbsolutePath testProjectPath in TestProjects)
            {
                args.AppendLiteral($" --test-project {testProjectPath}");
            }
        }

        if (Mutate is [..])
        {
            foreach (string item in Mutate)
            {
                args.AppendLiteral($" --mutate {item}");
            }
        }

        if (Configuration is not null)
        {
            args.AppendLiteral($" --configuration {Configuration}");
        }

        if (TargetFramework is not null)
        {
            args.AppendLiteral($" --target-framework {TargetFramework}");
        }

        if (ProjectInfoVersion is not null)
        {
            args.AppendLiteral($" --version {ProjectInfoVersion}");
        }

        if (MutationLevel is not null)
        {
            args.AppendLiteral($" --mutation-level {MutationLevel}");
        }

        if (Reporters is [..])
        {
            foreach (StrykerReporter reporter in Reporters)
            {
                args.AppendLiteral($" --reporter {reporter}");
            }
        }

        if (OpenReport is not null)
        {
            args.AppendLiteral($"--open-report:{OpenReport}");
        }

        if (Concurrency is not null)
        {
            args.AppendLiteral($"--concurrency {Concurrency}");
        }

        if (BreakAt is not null)
        {
            args.AppendLiteral($"--break-at {BreakAt}");
        }

        if (ThresholdLow is not null)
        {
            args.AppendLiteral($"--threshold-low {ThresholdLow}");
        }

        if (ThresholdHigh is not null)
        {
            args.AppendLiteral($"--threshold-high {ThresholdHigh}");
        }

        if (Output is not null)
        {
            args.AppendLiteral($"--output {Output}");
        }

        if (DisableBail is true)
        {
            args.AppendLiteral("--disable-bail");
        }

        if (Since is not null)
        {
            args.AppendLiteral($"--since {Since}");
        }

        if (Verbosity is not null)
        {
            args.AppendLiteral($"--verbosity {Verbosity}");
        }

        if (DevMode is true)
        {
            args.AppendLiteral("--dev-mode");
        }

        if (DashboardApiKey is not null)
        {
            args.AppendFormatted($"--dashboard-api-key {DashboardApiKey}", format:"r");
        }

        if (MsBuildPath is not null)
        {
            args.AppendLiteral($"--msbuild-path {MsBuildPath}");
        }

        if (BreakOnInitialTestFailure is true)
        {
            args.AppendLiteral("--break-on-initial-test-failure");
        }

        return args.ToStringAndClear();
    }
}

public class StrykerVerbosity : Enumeration
{
    public static StrykerVerbosity Trace { get; } = new(){ Value = nameof(Trace)};
    public static StrykerVerbosity Error { get; } = new(){ Value = nameof(Error)};
    public static StrykerVerbosity Warning { get; } = new(){ Value = nameof(Warning)};
    public static StrykerVerbosity Info { get; } = new(){ Value = nameof(Info)};
    public static StrykerVerbosity Debug { get; } = new(){ Value = nameof(Debug)};
}

public class StrykerOpenReport : Enumeration
{
    public static readonly StrykerOpenReport Html = new (){ Value = nameof(Html) };
    public static readonly StrykerOpenReport Dashboard = new (){ Value = nameof(Dashboard) };

    /// <summary>
    /// Implicit conversion of <see cref="StrykerOpenReport"/> to <see cref="string"/>
    /// </summary>
    /// <param name="strykerOpenReport"></param>
    /// <returns></returns>
    public static implicit operator string(StrykerOpenReport strykerOpenReport) => strykerOpenReport.Value;
}

// TODO Remove this class once https://github.com/nuke-build/nuke/pull/1259 is merged
public class StrykerMutationLevel : Enumeration
{
    /// <summary>
    /// Basic mutation level
    /// </summary>
    public static readonly StrykerMutationLevel Basic = new (){ Value = nameof(Basic) };
    
    /// <summary>
    /// Standard mutation level
    /// </summary>
    public static readonly StrykerMutationLevel Standard = new (){ Value = nameof(Standard) };
    
    /// <summary>
    /// Advanced mutation level
    /// </summary>
    public static readonly StrykerMutationLevel Advanced = new (){ Value = nameof(Advanced) };
    
    /// <summary>
    /// Complete mutation level
    /// </summary>
    public static readonly StrykerMutationLevel Complete = new (){ Value = nameof(Complete) };
    
    /// <summary>
    /// Implicit conversion of <see cref="StrykerMutationLevel"/> to <see cref="string"/>
    /// </summary>
    /// <param name="strykerLevel"></param>
    /// <returns></returns>
    public static implicit operator string(StrykerMutationLevel strykerLevel) => strykerLevel.Value;
}

public class StrykerReporter : Enumeration
{
    public static readonly StrykerReporter Html = new() { Value = nameof(Html) };
    public static readonly StrykerReporter Progress = new () { Value = nameof(Progress) };
    public static readonly StrykerReporter Dashboard = new () { Value = nameof(Dashboard) };
    public static readonly StrykerReporter Cleartext = new () { Value = nameof(Cleartext) };
    public static readonly StrykerReporter CleartextTree = new () { Value = nameof(CleartextTree) };
    public static readonly StrykerReporter Dots = new () { Value = nameof(Dots) };
    public static readonly StrykerReporter Json = new () { Value = nameof(Json) };

    /// <summary>
    /// Implicit conversion of <see cref="StrykerMutationLevel"/> to <see cref="string"/>
    /// </summary>
    /// <param name="strykerReporter"></param>
    /// <returns></returns>
    public static implicit operator string(StrykerReporter strykerReporter) => strykerReporter.Value;
}
