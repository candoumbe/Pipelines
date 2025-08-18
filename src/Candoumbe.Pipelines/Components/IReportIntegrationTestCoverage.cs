using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Codecov;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.Tools.Codecov.CodecovTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

namespace Candoumbe.Pipelines.Components;


/// <summary>
/// Build component that can report integration tests code coverage.
/// </summary>
/// <remarks>
/// This component requires <see href="https://nuget.org/packages/reportgenerator">ReportGenerator tool</see>
/// </remarks>
public interface IReportIntegrationTestCoverage : IReportCoverage, IIntegrationTest
{
    /// <summary>
    /// Name of the generated artifacts when publishing code coverage report
    /// </summary>
    public string CodeCoverageReportArtifactName => "integration-tests-coverage";

    /// <summary>
    /// Name of artifact when publishing code coverage history report
    /// </summary>
    public string CodeCoverageHistoryReportArtifactName => "integration-tests-coverage-history";

    /// <summary>
    /// Directory where coverage report history files will be pushed
    /// </summary>
    public AbsolutePath IntegrationTestCoverageReportDirectory => CoverageReportDirectory / CodeCoverageReportArtifactName;

    /// <summary>
    /// Directory where coverage report history files will be pushed
    /// </summary>
    public AbsolutePath IntegrationTestCoverageReportHistoryDirectory => CoverageReportHistoryDirectory / CodeCoverageHistoryReportArtifactName;

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
    /// Allows to override default settings when generating code coverage report.
    /// </summary>
    /// <remarks>
    /// These settings are only used when <see cref="IReportCoverage.ReportToCodeCov"/> is <see langword="true"/>.
    /// </remarks>
    Configure<CodecovSettings> CodecovSettings => _ => _;

    /// <summary>
    /// Allows to override default settings used to report code coverage.
    /// </summary>
    Configure<ReportGeneratorSettings> ReportGeneratorSettings => _ => _;

    /// <summary>
    /// Pushes code coverage to CodeCov
    /// </summary>
    public Target ReportIntegrationTestCoverage => _ => _
       .DependsOn(IntegrationTests)
       .Requires(() => !ReportToCodeCov || CodecovToken != null)
       .Consumes(IntegrationTests, IntegrationTestResultsDirectory / "*.xml")
       .Produces(IntegrationTestCoverageReportDirectory / "*.xml")
       .Produces(IntegrationTestCoverageReportHistoryDirectory / "*.xml")
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