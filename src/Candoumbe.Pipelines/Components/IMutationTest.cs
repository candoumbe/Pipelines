using Candoumbe.Pipelines.Components.Workflows;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;

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
public interface IMutationTest : IUnitTest
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
    IEnumerable<(Project SourceProject, IEnumerable<Project> TestProjects)> MutationTestsProjects { get; }

    /// <summary>
    /// Executes mutation tests for the specified <see cref="MutationTestsProjects"/>.
    /// </summary>
    /// <remarks></remarks>
    public Target MutationTests => _ => _
        .Description("Runs mutation tests using Stryker tool")
        .TryDependsOn<IClean>(x => x.Clean)
        .TryBefore<IPack>()
        .DependsOn(Compile)
        .Produces(MutationTestResultDirectory / "*")
        .Executes(() =>
        {
            int mutationProjectCount = 0;

            // List of frameworks that source project are tested against
            string[] frameworks = MutationTestsProjects.Select(csprojAndTest => csprojAndTest.TestProjects)
                                                       .SelectMany(projects => projects)
                                                       .Select(csproj => csproj.GetTargetFrameworks())
                                                       .SelectMany(x => x)
                                                       .Select(targetFramework => targetFramework.ToLower().Trim())
                                                       .AsParallel()
                                                       .Distinct()
                                                       .ToArray();


            if (frameworks.AtLeast(2))
            {
                MutationTestsProjects.ForEach(tuple =>
                {
                    mutationProjectCount++;
                    (Project sourceProject, IEnumerable<Project> testsProjects) = tuple;
                    AbsolutePath mutationTestOutputDirectory = MutationTestResultDirectory / sourceProject.Name;

                    IReadOnlyCollection<string> testedFrameworks = testsProjects.Select(csproj => csproj.GetTargetFrameworks())
                                                                                   .SelectMany(x => x)
                                                                                   .Distinct()
                                                                                   .ToArray();
                    Arguments args;
                    if (testedFrameworks.AtLeast(2))
                    {
                        testedFrameworks.ForEach(framework =>
                        {
                            args = new();
                            args.Apply(StrykerArgumentsSettingsBase)
                                .Apply(StrykerArgumentsSettings);

                            args.Add("--target-framework {value}", framework)
                                .Add("--output {value}", mutationTestOutputDirectory / framework);

                            RunMutationTestsForTheProject(sourceProject, testsProjects, args);
                        });
                    }
                    else
                    {
                        args = new();
                        string framework = testedFrameworks.Single();
                        args.Add("--target-framework {value}", framework)
                            .Add("--output {value}", mutationTestOutputDirectory / framework);

                        RunMutationTestsForTheProject(sourceProject, testsProjects, args);
                    }
                });
            }
            else
            {
                Arguments args = new();
                args.Apply(StrykerArgumentsSettingsBase)
                    .Apply(StrykerArgumentsSettings);

                args.Add("--target-framework {value}", frameworks.Single());

                MutationTestsProjects.ForEach(tuple =>
                {
                    mutationProjectCount++;
                    args = args.Add("--output {value}", MutationTestResultDirectory / tuple.SourceProject.Name);
                    RunMutationTestsForTheProject(tuple.SourceProject, tuple.TestProjects, args);
                });
            }
        });

    /// <summary>
    /// Run mutation tests for the specified project using the specified arguments
    /// </summary>
    private void RunMutationTestsForTheProject(Project sourceProject, IEnumerable<Project> testsProjects, Arguments args)
    {
        Verbose("{ProjetName} will run mutation tests for the following frameworks : {@Frameworks}", sourceProject.Name, sourceProject.GetTargetFrameworks());

        Arguments strykerArgs = new();
        strykerArgs = strykerArgs.Add("stryker");
        strykerArgs = strykerArgs.Concatenate(args);

        strykerArgs.Add(@"--project {value}", sourceProject.Name);

        testsProjects.ForEach(project =>
        {
            strykerArgs = strykerArgs.Add(@"--test-project {value}", project.Path);
        });

        if (!sourceProject.IsSourceLinkEnabled() && StrykerDashboardApiKey is not null)
        {
            if (this is IHaveGitVersion gitVersion)
            {
                strykerArgs = strykerArgs.Add("--version {0}", gitVersion.MajorMinorPatchVersion);
            }
            else if (this is IHaveGitRepository gitRepository && gitRepository.GitRepository.Branch is { } branch)
            {
                strykerArgs = strykerArgs.Add("--version {0}", branch);
            }
        }

        if (this is IGitFlow gitFlow && gitFlow.GitRepository is { } gitflowRepository)
        {
            strykerArgs = strykerArgs.Add("--version {0}", gitflowRepository.Commit ?? gitflowRepository.Branch);
            switch (gitflowRepository.Branch)
            {
                case string branchName when string.Equals(branchName, IGitFlow.DevelopBranchName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        // we are in git flow so comparison we can compare develop against main branch
                        strykerArgs = strykerArgs.Add("--with-baseline:{0}", IGitFlow.MainBranchName);

                    }
                    break;
                case string branchName when branchName.Like($"{gitFlow.FeatureBranchPrefix}/*", true):
                    {
                        strykerArgs = strykerArgs.Add("--with-baseline:{0}", gitFlow.FeatureBranchSourceName);
                    }
                    break;
                case string branchName when branchName.Like($"{gitFlow.ColdfixBranchPrefix}/*", true):
                    {
                        strykerArgs = strykerArgs.Add("--with-baseline:{0}", gitFlow.ColdfixBranchSourceName);
                    }
                    break;
                default:
                    break;
            }
        }
        else if (this is IGitHubFlow gitHubFlow && gitHubFlow.GitRepository is { } githubFlowRepository)
        {
            strykerArgs = strykerArgs.Add("--version:{0}", githubFlowRepository.Commit ?? githubFlowRepository.Branch);
            if (githubFlowRepository.Branch is { Length: > 0 } branchName && !string.Equals(branchName, IGitHubFlow.MainBranchName, StringComparison.InvariantCultureIgnoreCase))
            {
                strykerArgs = strykerArgs.Add("--with-baseline:{0}", IGitHubFlow.MainBranchName);
            }
        }

        DotNet(strykerArgs.RenderForExecution(), workingDirectory: sourceProject.Path.Parent);
    }

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
