using System;
using System.Collections.Generic;
using Nuke.Common;
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
                                          .TryAfter<IUnitTest>()
                                          .TryBefore<IPack>()
                                          .Description("Run integration tests and collect code coverage")
                                          .Produces(IntegrationTestResultsDirectory / "*.trx")
                                          .Produces(IntegrationTestResultsDirectory / "*.xml")
                                          .Executes(() =>
                                                    {
                                                        IntegrationTestsProjects.ForEach(project => Information(project));

                                                        DotNetTest(s => s
                                                                       .Apply(IntegrationTestSettingsBase)
                                                                       .Apply(IntegrationTestSettings)
                                                                       .CombineWith(IntegrationTestsProjects,
                                                                                    (cs, project) => cs.SetProjectFile(project)
                                                                                        .CombineWith(project.GetTargetFrameworks(),
                                                                                                     (setting, framework) => setting.Apply<DotNetTestSettings, (Project, string)>(ProjectIntegrationTestSettingsBase, (project, framework))
                                                                                                         .Apply<DotNetTestSettings, (Project, string)>(ProjectIntegrationTestSettings, (project, framework))
                                                                                                    )));
                                                    });

    internal sealed Configure<DotNetTestSettings> IntegrationTestSettingsBase => _ => _.SetConfiguration(Configuration)
                                                                                     .EnableUseSourceLink()
                                                                                     .SetNoBuild(SucceededTargets.Contains(Compile))
                                                                                     .SetNoRestore(SucceededTargets.Contains(Compile))
                                                                                     .SetResultsDirectory(IntegrationTestResultsDirectory)
                                                                                     .AddProperty("ExcludeByAttribute", "Obsolete");

    /// <summary>
    /// Configures the integration test target
    /// </summary>
    public Configure<DotNetTestSettings> IntegrationTestSettings => _ => _;

    internal Configure<DotNetTestSettings, (Project project, string framework)> ProjectIntegrationTestSettingsBase
        => (settings, tuple) => settings.SetFramework(tuple.framework)
               .WhenNotNull(this.As<IReportCoverage>(),
                            (coverageSettings, _) => tuple.project.IsMicrosoftTestingPlatformEnabled() switch
                            {
                                true => coverageSettings.AddProcessAdditionalArguments(
                                [
                                    "--",
                                    "--coverage", // Enable to collect coverage
                                    "--coverage-output-format 'cobertura'",
                                    $" --coverage-output '{IntegrationTestResultsDirectory / $"{tuple.project.Name}.{tuple.framework}.xml"}'"
                                ]),
                                _ => coverageSettings.EnableCollectCoverage()
                                    .AddLoggers($"trx;LogFileName={tuple.project.Name}.{tuple.framework}.trx")
                                    .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                                    .SetCoverletOutput(IntegrationTestResultsDirectory / $"{tuple.project.Name}.{tuple.framework}.xml")
                                    .SetResultsDirectory(IntegrationTestResultsDirectory)
                            });

    /// <summary>
    /// Configure / override integration test settings at project level
    /// </summary>
    Configure<DotNetTestSettings, (Project project, string framework)> ProjectIntegrationTestSettings => (settings, _) => settings;
}