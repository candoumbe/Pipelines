using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;

using System.Collections.Generic;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can perform mutation tests using <see href="https://stryker-mutator.io/">Stryker</see>.
/// </summary>
/// <remarks>
/// <see cref="MutationTests"/> target required <see href="https://www.nuget.org/packages/dotnet-stryker">Stryker dotnet tool</see> to be referenced in order to run.
/// </remarks>
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
    /// This should be projects that contains unit tests
    /// </remarks>
    IEnumerable<Project> MutationTestsProjects { get; }

    /// <summary>
    /// Executes mutation tests for the specified <see cref="MutationTestsProjects"/>.
    /// </summary>
    public Target MutationTests => _ => _
        .Description("Runs mutation tests using Stryker tool")
        .TryDependsOn<IClean>(x => x.Clean)
        .DependsOn(Compile)
        .Produces(MutationTestResultDirectory / "*")
        .Executes(() =>
        {
            int count = 0;

            Arguments args = new();
            args.Add("--open-report:html", IsLocalBuild);
            args.Add($"--dashboard-api-key {StrykerDashboardApiKey}", IsServerBuild || StrykerDashboardApiKey is not null);
            args.Add("--reporter markdown");
            args.Add("--reporter html");
            args.Add("--reporter progress", IsLocalBuild);

            MutationTestsProjects.ForEach(csproj =>
            {
                count++;
                DotNet($"stryker {args.RenderForExecution()}", workingDirectory: csproj.Path.Parent);
            });

            Verbose("Running mutation tests for {ProjectCount} project(s)", count);

        });
}
