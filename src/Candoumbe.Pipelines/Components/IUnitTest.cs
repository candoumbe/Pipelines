using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can run unit tests
/// </summary>
public interface IUnitTest : ICompile, IHaveTests, IHaveCoverage
{
    /// <summary>
    /// Projects that contains unit tests
    /// </summary>
    IEnumerable<Project> UnitTestsProjects { get; }

    /// <summary>
    /// Directory where to unit test results will be published.
    /// </summary>
    AbsolutePath UnitTestResultsDirectory => TestResultDirectory / "unit-tests";

    /// <summary>
    /// Runs unit tests
    /// </summary>
    public Target UnitTests => _ => _
        .DependsOn(Compile)
        .Description("Run unit tests and collect code coverage")
        .Produces(UnitTestResultsDirectory / "*.trx")
        .Produces(UnitTestResultsDirectory / "*.xml")
        .TryTriggers<IReportUnitTestCoverage>()
        .Executes(() =>
        {
            UnitTestsProjects.ForEach(project => Information(project));

            DotNetTest(s => s
                .Apply(UnitTestSettingsBase)
                .Apply(UnitTestSettings)
                .CombineWith(UnitTestsProjects, (cs, project) => cs.SetProjectFile(project)
                                                               .CombineWith(project.GetTargetFrameworks(),
                                                                            (setting, framework) => setting.Apply<DotNetTestSettings, (Project, string)>(ProjectUnitTestSettingsBase, (project, framework))
                                                                                                           .Apply<DotNetTestSettings, (Project, string)>(ProjectUnitTestSettings, (project, framework))
                    )));
        });

    internal sealed Configure<DotNetTestSettings> UnitTestSettingsBase => _ => _
        .SetConfiguration(Configuration.ToString())
                .EnableUseSourceLink()
                .SetNoBuild(SucceededTargets.Contains(Compile))
                .SetResultsDirectory(UnitTestResultsDirectory)
                .WhenNotNull(this.As<IReportCoverage>(), (settings, _) => settings.EnableCollectCoverage()
                                                                                   .SetCoverletOutputFormat(CoverletOutputFormat.lcov))
                .AddProperty("ExcludeByAttribute", "Obsolete");

    /// <summary>
    /// Configures the Unit test target
    /// </summary>
    public Configure<DotNetTestSettings> UnitTestSettings => _ => _;

    internal Configure<DotNetTestSettings, (Project project, string framework)> ProjectUnitTestSettingsBase => (settings, tuple) => settings.SetFramework(tuple.framework)
                                                                                                                                    .AddLoggers($"trx;LogFileName={tuple.project.Name}.{tuple.framework}.trx")
                                                                                                                                    .WhenNotNull(this.As<IReportCoverage>(), (options, _) => options.SetCoverletOutput(UnitTestResultsDirectory / $"{tuple.project.Name}.{tuple.framework}.xml"));

    /// <summary>
    /// Configure / override unit test settings at project level
    /// </summary>
    Configure<DotNetTestSettings, (Project project, string framework)> ProjectUnitTestSettings => (settings, _) => settings;
}