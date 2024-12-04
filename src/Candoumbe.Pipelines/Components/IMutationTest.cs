using System;
using System.Collections.Generic;
using System.Linq;
using Candoumbe.Pipelines.Components.Workflows;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
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

                DotNetRunSettings strykerArgs = default;
                strykerArgs = StrykerArgumentsSettingsBase.Invoke(strykerArgs);
                strykerArgs = StrykerArgumentsSettings.Invoke(strykerArgs);

                strykerArgs.AddProcessAdditionalArguments($"--target-framework {framework}",
                                                          $"--output {MutationTestResultDirectory / sourceProject.Name / framework}");

                strykerArgs = strykerArgs.AddProcessAdditionalArguments("stryker");

                strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--project {sourceProject.Name}{sourceProject.Path.Extension}");

                if (configFile is not null)
                {
                    strykerArgs = strykerArgs.AddProcessAdditionalArguments("--config-file {value}", configFile);
                }

                strykerArgs = testsProjects.Aggregate(strykerArgs, (current, project) => current.AddProcessAdditionalArguments($"--test-project {project.Path}"));

                switch (this)
                {
                    case IGitFlow { GitRepository: { } gitflowRepository } gitFlow:
                        {
                            strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--version {gitflowRepository.Commit ?? gitflowRepository.Branch}");
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
                            strykerArgs = strykerArgs.AddProcessAdditionalArguments($"--version {githubFlowRepository.Commit ?? githubFlowRepository.Branch}");
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
                            strykerArgs = this switch
                            {
                                IHaveGitVersion gitVersion => strykerArgs.AddProcessAdditionalArguments($"--version {gitVersion.MajorMinorPatchVersion}"),
                                IHaveGitRepository gitRepository => strykerArgs.AddProcessAdditionalArguments($"--version {gitRepository.GitRepository?.Commit ?? gitRepository.GitRepository?.Branch}"),
                                _ => strykerArgs
                            };
                        }

                        break;
                    }
                }

                DotNet(strykerArgs.ToString(), workingDirectory: sourceProject.Path.Parent);
            }
        });

    internal Configure<DotNetRunSettings> StrykerArgumentsSettingsBase => _
        =>
    {
        DotNetRunSettings args = new();
        if (IsLocalBuild)
        {
            args = args.AddProcessAdditionalArguments("--open-report:html", "--reporter progress");
        }

        return args.WhenNotNull(StrykerArgumentsSettings,
                (args, apiKey) => args.AddProcessRedactedSecrets($"--dashboard-api-key {apiKey}")
                                                .AddProcessAdditionalArguments("--reporter dashboard"))
            .AddProcessAdditionalArguments("--reporter markdown", "--reporter html");
    };

    /// <summary>
    /// Configures arguments that will be used by when running Stryker tool
    /// </summary>
    Configure<DotNetRunSettings> StrykerArgumentsSettings => _ => _;
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
