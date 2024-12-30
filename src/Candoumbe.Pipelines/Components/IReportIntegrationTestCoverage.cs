namespace Candoumbe.Pipelines.Components;

using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Codecov;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.Tools.Codecov.CodecovTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

/// <summary>
/// Build component that can report integration tests code coverage
/// </summary>
/// <remarks>
/// This component requires <see href="https://nuget.org/packages/reportgenerator">ReportGenerator tool</see>
/// </remarks>
public interface IReportIntegrationTestCoverage : IReportCoverage, IIntegrationTest
{
    /// <summary>
    /// Directory where coverage report history files will be pushed
    /// </summary>
    public AbsolutePath IntegrationTestCoverageReportDirectory => CoverageReportDirectory / "unit-tests";

    /// <summary>
    /// Directory where coverage report history files will be pushed
    /// </summary>
    public AbsolutePath IntegrationTestCoverageReportHistoryDirectory => CoverageReportHistoryDirectory / "unit-tests";

    
    internal sealed Configure<ReportGeneratorSettings> ReportGeneratorSettingsBase => _ => _
        .SetFramework("net5.0")
        .SetReports(IntegrationTestResultsDirectory / "*.xml")
        .SetReportTypes(ReportTypes.Badges, ReportTypes.HtmlChart, ReportTypes.HtmlInline)
        .WhenNotNull(this.As<IHaveGitRepository>()?.GitRepository,
            (_, repository) => !string.IsNullOrWhiteSpace(repository.Branch)
                ? _.SetTargetDirectory(IntegrationTestCoverageReportDirectory / repository.Branch)
                    .SetHistoryDirectory(IntegrationTestCoverageReportHistoryDirectory / repository.Branch)
                    .SetTag(repository.Commit)
                : _.SetTargetDirectory(IntegrationTestCoverageReportDirectory)
                    .SetHistoryDirectory(IntegrationTestCoverageReportHistoryDirectory)
                    .SetTag(repository.Commit)
        )
        .When(_ => this.As<IHaveGitRepository>() is null,
            _ => _.SetTargetDirectory(IntegrationTestCoverageReportDirectory)
                .SetHistoryDirectory(IntegrationTestCoverageReportHistoryDirectory));

    internal sealed Configure<CodecovSettings> CodeCovSettingsBase => _ => _
        .SetFiles(IntegrationTestResultsDirectory.GlobFiles("*.xml").Select(x => x.ToString()))
        .SetToken(CodecovToken)
        .SetFramework("netcoreapp3.0")
        .WhenNotNull(this.As<IHaveGitVersion>(),
            (_, version) => _.SetBuild(version.GitVersion.FullSemVer))
        .WhenNotNull(this.As<IHaveGitRepository>(),
            (_, repository) => _.SetBranch(repository.GitRepository.Branch)
                .SetSha(repository.GitRepository.Commit));

    /// <summary>
    /// Pushes code coverage to CodeCov
    /// </summary>
    public Target ReportIntegrationTestCoverage => _ => _
       .DependsOn(IntegrationTests)
       .Requires(() => !ReportToCodeCov || CodecovToken != null)
       .Consumes(IntegrationTests, IntegrationTestResultsDirectory / "*.xml")
       .Produces(CoverageReportDirectory / "*.xml")
       .Produces(CoverageReportHistoryDirectory / "*.xml")
       .Executes(() =>
       {
           ReportGenerator(s => s.Apply(ReportGeneratorSettingsBase)
                                 .Apply(ReportGeneratorSettings));

           if (ReportToCodeCov)
           {
               Codecov(s => s.Apply(CodeCovSettingsBase)
                             .Apply(CodecovSettings));
           }
       });
}
