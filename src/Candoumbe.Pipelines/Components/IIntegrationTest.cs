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
/// Marks a pipeline that can run integration tests
/// </summary>
public interface IIntegrationTest : ICompile, IHaveTests, IHaveCoverage
{
    /// <summary>
    /// Projects that contains integration tests
    /// </summary>
    IEnumerable<Project> IntegrationTestsProjects { get; }

    /// <summary>
    /// Directory where to integration test results will be published.
    /// </summary>
    AbsolutePath IntegrationTestResultsDirectory => TestResultDirectory / "integration-tests";

    /// <summary>
    /// Runs integration tests
    /// </summary>
    public Target IntegrationTests => _ => _
        .DependsOn(Compile)
        .Description("Run integration tests and collect code coverage")
        .Produces(IntegrationTestResultsDirectory / "*.trx")
        .Produces(IntegrationTestResultsDirectory / "*.xml")
        .TryTriggers<IReportCoverage>()
        .Executes(() =>
        {
            IntegrationTestsProjects.ForEach(project => Information(project));

            DotNetTest(s => s
                .Apply(IntegrationTestSettingsBase)
                .Apply(IntegrationTestSettings)
                .CombineWith(IntegrationTestsProjects, (cs, project) => cs.SetProjectFile(project)
                                                               .CombineWith(project.GetTargetFrameworks(),
                                                                            (setting, framework) => setting.Apply<DotNetTestSettings, (Project, string)>(ProjectIntegrationTestSettingsBase, (project, framework))
                                                                                                           .Apply<DotNetTestSettings, (Project, string)>(ProjectIntegrationTestSettings, (project, framework))
                    )));
        });

    internal sealed Configure<DotNetTestSettings> IntegrationTestSettingsBase => _ => _
        .SetConfiguration(Configuration.ToString())
                .EnableUseSourceLink()
                .SetNoBuild(SucceededTargets.Contains(Compile))
                .SetResultsDirectory(IntegrationTestResultsDirectory)
                .WhenNotNull(this.As<IReportCoverage>(), (settings, _) => settings.EnableCollectCoverage()
                                                                                   .SetCoverletOutputFormat(CoverletOutputFormat.lcov))
                .AddProperty("ExcludeByAttribute", "Obsolete");

    /// <summary>
    /// Configures the Unit test target
    /// </summary>
    public Configure<DotNetTestSettings> IntegrationTestSettings => _ => _;

    internal Configure<DotNetTestSettings, (Project project, string framework)> ProjectIntegrationTestSettingsBase => (settings, tuple) => settings.SetFramework(tuple.framework)
                                                                                                                                    .AddLoggers($"trx;LogFileName={tuple.project.Name}.{tuple.framework}.trx")
                                                                                                                                    .WhenNotNull(this.As<IReportCoverage>(), (settings, _) => settings.SetCoverletOutput(IntegrationTestResultsDirectory / $"{tuple.project.Name}.{tuple.framework}.xml"));

    /// <summary>
    /// Configure / override integration test settings at project level
    /// </summary>
    Configure<DotNetTestSettings, (Project project, string framework)> ProjectIntegrationTestSettings => (settings, _) => settings;
}