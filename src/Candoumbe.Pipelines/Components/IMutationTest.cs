using Candoumbe.Pipelines.Components.Workflows;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;

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
[NuGetPackageRequirement("dotnet-stryker")]
public interface IMutationTest : IHaveTests
{
    /// <summary>
    /// Name of the property set when <see href="SourceLink">SourceLink</see> is enabled on a projet.
    /// </summary>
    private const string ContinuousIntegrationBuild = nameof(ContinuousIntegrationBuild);

    /// <summary>
    /// Directory where mutattion test results should be published
    /// </summary>
    AbsolutePath MutationTestResultDirectory => TestResultDirectory / "mutation-tests";

    /// <summary>
    /// Api Key us
    /// </summary>
    [Parameter("API KEY used to submit mutation report to a stryker dashboard")]
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
                                                                                               .SelectMany(frameworks => frameworks)
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

                Verbose("{ProjetName} will run mutation tests for the following frameworks : {@Frameworks}", sourceProject.Name, sourceProject.GetTargetFrameworks());

                MutationTestRunConfiguration mutationTestRunConfiguration = (configFile?.FileExists() is true
                                ? configFile.ReadJson<MutationTestRunConfiguration>()
                                : null);

                Arguments args = new();
                args = args.Apply(StrykerArgumentsSettingsBase)
                           .Apply(StrykerArgumentsSettings);

                args.Add("--target-framework {value}", framework)
                    .Add("--output {value}", MutationTestResultDirectory / sourceProject.Name / framework);

                Arguments strykerArgs = new();
                strykerArgs = strykerArgs.Add("stryker");

                strykerArgs = strykerArgs.Concatenate(args);

                strykerArgs.Add(@"--project {value}", sourceProject.Name);

                testsProjects.ForEach(project =>
                {
                    strykerArgs = strykerArgs.Add(@"--test-project {value}", project.Path);
                });

                switch (this)
                {
                    case IGitFlow gitFlow when gitFlow.GitRepository is { } gitflowRepository:
                        {
                            strykerArgs = strykerArgs.Add("--version {value}", gitflowRepository.Commit ?? gitflowRepository.Branch);
                            switch (gitflowRepository.Branch)
                            {
                                case string branchName when string.Equals(branchName, IGitFlow.DevelopBranchName, StringComparison.InvariantCultureIgnoreCase):
                                    {
                                        // we are in git flow so we can compare develop with main branch
                                        strykerArgs = strykerArgs.Add("--with-baseline:{value}", IGitFlow.MainBranchName);
                                    }
                                    break;
                                case string branchName when branchName.Like($"{gitFlow.FeatureBranchPrefix}/*", true):
                                    {
                                        strykerArgs = strykerArgs.Add("--with-baseline:{value}", gitFlow.FeatureBranchSourceName);
                                    }
                                    break;
                                case string branchName when branchName.Like($"{gitFlow.ColdfixBranchPrefix}/*", true):
                                    {
                                        strykerArgs = strykerArgs.Add("--with-baseline:{value}", gitFlow.ColdfixBranchSourceName);
                                    }
                                    break;
                                default:
                                    {
                                        // By default we try to compare the current commit with the previous one (if any).
                                        if (gitflowRepository.Head is string head)
                                        {
                                            strykerArgs = strykerArgs.Add("--with-baseline:{value}", head);
                                        }
                                    }
                                    break;
                            }

                            break;
                        }

                    case IGitHubFlow gitHubFlow when gitHubFlow.GitRepository is { } githubFlowRepository:
                        {
                            strykerArgs = strykerArgs.Add("--version {value}", githubFlowRepository.Commit ?? githubFlowRepository.Branch);
                            if (githubFlowRepository.Branch is { Length: > 0 } branchName && !string.Equals(branchName, IGitHubFlow.MainBranchName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                strykerArgs = strykerArgs.Add("--with-baseline:{0}", IGitHubFlow.MainBranchName);
                            }

                            break;
                        }

                    default:
                        if (!sourceProject.IsSourceLinkEnabled())
                        {
                            if (this is IHaveGitVersion gitVersion)
                            {
                                strykerArgs = strykerArgs.Add("--version {value}", gitVersion.MajorMinorPatchVersion);
                            }
                            else if (this is IHaveGitRepository gitRepository)
                            {
                                strykerArgs = strykerArgs.Add("--version {value}", gitRepository.GitRepository?.Commit ?? gitRepository?.GitRepository?.Branch);
                            }
                        }

                        break;
                }

                DotNet(strykerArgs.RenderForExecution(), workingDirectory: sourceProject.Path.Parent);
            }
        });

    internal Configure<Arguments> StrykerArgumentsSettingsBase => _
        => _
           .When(IsLocalBuild, args => args.Add("--open-report:{value}", "html"))
           .WhenNotNull(StrykerDashboardApiKey,
                        (args, apiKey) => args.Add("--dashboard-api-key {value}", apiKey, secret: true)
                                              .Add("--reporter dashboard"))
           .Add("--reporter markdown")
           .Add("--reporter html")
           .When(IsLocalBuild, args => args.Add("--reporter progress"));

    /// <summary>
    /// Configures arguments that will be used by when running Stryker tool
    /// </summary>
    Configure<Arguments> StrykerArgumentsSettings => _ => _;
}

/// <summary>
/// Wraps information on mutation tests for a specific project.
/// </summary>
public record MutationProjectConfiguration
{
    /// <summary>
    /// The project for which mutation tests will be run.
    /// </summary>
    public Project SourceProject { get; }

    /// <summary>
    /// Set
    /// </summary>
    public IReadOnlySet<Project> TestProjects { get; }

    /// <summary>
    /// Path to the JSON configuration file to use when running mutation tests for <see cref="SourceProject"/>
    /// </summary>
    public AbsolutePath ConfigurationFile { get; init; }

    /// <summary>
    /// Builds a new <see cref="MutationProjectConfiguration"/>
    /// </summary>
    /// <param name="sourceProject">The project onto which mutation will be performed</param>
    /// <param name="testProjects">List of projects that will be used to validate mutation performed. These are usually</param>
    /// <param name="configurationFile">Path to the configuration file that can be used by Stryker when running mutation tests.</param>
    public MutationProjectConfiguration(Project sourceProject, IEnumerable<Project> testProjects, AbsolutePath configurationFile = default)
    {
        SourceProject = sourceProject ?? throw new ArgumentNullException(nameof(sourceProject));
        TestProjects = testProjects.ToHashSet();
        ConfigurationFile = configurationFile;
    }

    ///<inheritdoc/>
    public void Deconstruct(out Project sourceProject, out IReadOnlySet<Project> testProjects, out AbsolutePath configurationFile)
    {
        sourceProject = SourceProject;
        testProjects = TestProjects;
        configurationFile = ConfigurationFile;
    }
}

file record MutationTestRunConfiguration
{
    /// <summary>
    /// Name of the module under which regroup result of the current mutation test run
    /// </summary>
    public string Module { get; init; }

    public string[] TestsProjects { get; init; }

    public string[] Frameworks { get; init; }

    public StrykerProjectInfo ProjectInfo { get; init; }
}

file record StrykerProjectInfo
{
    public string Module { get; init; }

    public string Version { get; init; }
}
