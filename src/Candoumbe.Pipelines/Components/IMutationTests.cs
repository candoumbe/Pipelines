using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;

using System.Collections.Generic;
using System.Linq;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

public interface IMutationTests : IUnitTest
{
    /// <summary>
    /// Api Key
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
        .Produces(TestResultDirectory / "*.html")
        .Executes(() =>
        {
            Verbose("Running mutation tests for {ProjectCount} project(s)", MutationTestsProjects.TryGetNonEnumeratedCount(out int count) ? count : MutationTestsProjects.Count());

            Arguments args = new();
            args.Add("--open-report:html", IsLocalBuild);
            args.Add($"--dashboard-api-key {StrykerDashboardApiKey}", IsServerBuild || StrykerDashboardApiKey is not null);

            MutationTestsProjects.ForEach(csproj =>
            {
                Information("Running tests for '{ProjectName}' (directory : '{Directory}') ", csproj.Name, csproj.Path.Parent);
                DotNet($"stryker {args.RenderForExecution()}", workingDirectory: csproj.Path.Parent);
            });
        });
}
