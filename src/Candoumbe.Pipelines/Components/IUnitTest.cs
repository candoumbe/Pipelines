﻿using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;

using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    /// Runs unit tests
    /// </summary>
    public Target UnitTests => _ => _
        .DependsOn(Compile)
        .Description("Run unit tests and collect code coverage")
        .Produces(TestResultDirectory / "*.trx")
        .Produces(TestResultDirectory / "*.xml")
        .TryTriggers<IReportCoverage>()
        .Executes(() =>
        {
            UnitTestsProjects.ForEach(project => Information(project));

            DotNetTest(s => s
                .Apply(UnitTestSettingsBase)
                .Apply(UnitTestSettings)
                .CombineWith(UnitTestsProjects, (cs, project) => cs.SetProjectFile(project)
                                                               .CombineWith(project.GetTargetFrameworks(), (setting, framework) => setting.SetFramework(framework)
                                                                                                                                          .AddLoggers($"trx;LogFileName={project.Name}.trx")
                                                                                                                                          .SetCoverletOutput(TestResultDirectory / $"{project.Name}.{framework}.xml"))),
                                                                                                                                          completeOnFailure: true
                );

            TestResultDirectory.GlobFiles("*.trx")
                               .ForEach(testFileResult => AzurePipelines.Instance?.PublishTestResults(type: AzurePipelinesTestResultsType.VSTest,
                                                                                                      title: $"{Path.GetFileNameWithoutExtension(testFileResult)} ({AzurePipelines.Instance.StageDisplayName})",
                                                                                                      files: new string[] { testFileResult })
                    );

            TestResultDirectory.GlobFiles("*.xml")
                               .ForEach(file => AzurePipelines.Instance?.PublishCodeCoverage(coverageTool: AzurePipelinesCodeCoverageToolType.Cobertura,
                                                                                             summaryFile: file,
                                                                                             reportDirectory: CoverageReportDirectory));
        });

    internal sealed Configure<DotNetTestSettings> UnitTestSettingsBase => _ => _
        .SetConfiguration(Configuration)
                .ResetVerbosity()
                .EnableCollectCoverage()
                .EnableUseSourceLink()
                .SetNoBuild(SucceededTargets.Contains(Compile))
                .SetResultsDirectory(TestResultDirectory)
                .SetCoverletOutputFormat(CoverletOutputFormat.lcov)
                .AddProperty("ExcludeByAttribute", "Obsolete");

    /// <summary>
    /// Configures the Unit test target
    /// </summary>
    public Configure<DotNetTestSettings> UnitTestSettings => _ => _;
}
