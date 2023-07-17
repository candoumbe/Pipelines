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
    /// Directory where mutattion test results should be published
    /// </summary>
    AbsolutePath MutationTestResultDirectory => TestResultDirectory / "mutation-tests";

    /// <summary>
    /// Api Key us
    /// </summary>
    [Parameter]
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
                                .Add("--output {value}", MutationTestResultDirectory / framework);

                            RunMutationTestsForTheProject(sourceProject, testsProjects, args);
                        });
                    }
                    else
                    {
                        args = new();
                        string framework = testedFrameworks.Single();
                        args.Add("--target-framework {value}", framework)
                            .Add("--output {value}", MutationTestResultDirectory / framework);

                        RunMutationTestsForTheProject(sourceProject, testsProjects, args);
                    }
                });
            }
            else
            {
                Arguments args = new();
                args.Apply(StrykerArgumentsSettingsBase)
                    .Apply(StrykerArgumentsSettings);

                args.Add("--target-framework {value}", frameworks.Single())
                    .Add("--output {value}", MutationTestResultDirectory);

                MutationTestsProjects.ForEach(tuple =>
                {
                    mutationProjectCount++;
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
        strykerArgs.Add("stryker");
        strykerArgs.Concatenate(args);

        testsProjects.ForEach(project => strykerArgs.Add(@"--test-project {value}", project.Path));

        DotNet(strykerArgs.RenderForExecution(), workingDirectory: sourceProject.Path.Parent);
    }

    internal Configure<Arguments> StrykerArgumentsSettingsBase => _
        => _
           .When(IsLocalBuild, args => args.Add("--open-report:{0}", "html"))
           .When(StrykerDashboardApiKey is not null, args => args.Add("--dashboard-api-key {value}", StrykerDashboardApiKey, secret: true))
           .Add("--reporter markdown")
           .Add("--reporter html")
           .When(IsLocalBuild, args => args.Add("--reporter progress"))
           .WhenNotNull(this.Get<IGitFlow>()?.GitRepository?.Branch,
                        (args, branch) => args.Add("--with-baseline:{0}", branch.ToLowerInvariant() switch
                        {
                            string branchName when branchName == IGitFlow.MainBranchName || branchName == IGitFlow.DevelopBranchName => branchName,
                            string branchName when branchName.Like($"{this.Get<IGitFlow>().FeatureBranchPrefix}/*", true) => this.Get<IWorkflow>().FeatureBranchSourceName,
                            string branchName when branchName.Like($"{this.Get<IGitFlow>().HotfixBranchPrefix}/*", true) => this.Get<IWorkflow>().HotfixBranchSourceName,
                            string branchName when branchName.Like($"{this.Get<IGitFlow>().ColdfixBranchPrefix}/*", true) => this.Get<IGitFlow>().ColdfixBranchSourceName,
                            _ => IGitFlow.MainBranchName
                        }))
           .WhenNotNull(this.Get<IGitHubFlow>(), (args, flow) => args.Add("--with-baseline:{0}", IGitHubFlow.MainBranchName)
                                                                  .Add("--version:{0}", flow.GitRepository?.Commit ?? flow.GitRepository?.Branch));

    /// <summary>
    /// Configures arguments that will be used by when running Stryker tool
    /// </summary>
    Configure<Arguments> StrykerArgumentsSettings => _ => _;
}
