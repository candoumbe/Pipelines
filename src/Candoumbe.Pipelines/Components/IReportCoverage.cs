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
/// Build component that can report unit tests code coverage
/// </summary>
/// <remarks>
/// This component requires <see href="https://nuget.org/packages/reportgenerator">ReportGenerator tool</see>
/// </remarks>
[NuGetPackageRequirement("ReportGenerator")]
public interface IReportCoverage : IUnitTest, IRequireNuGetPackage
{
    /// <summary>
    /// The API key used to push code coverage to CodeCov
    /// </summary>
    [Parameter("The API key used to push code coverage to CodeCov")]
    [Secret]
    public string CodecovToken => TryGetValue(() => CodecovToken);

    /// <summary>
    /// Defines if <see cref="ReportCoverage"/> should publish results to <see href="codecov.io">Code Cov</see>.
    /// </summary>
    bool ReportToCodeCov { get; }

    /// <summary>
    /// Pushes code coverage to CodeCov
    /// </summary>
    public Target ReportCoverage => _ => _
       .DependsOn(UnitTests)
       .Requires(() => !ReportToCodeCov || CodecovToken != null)
       .Consumes(UnitTests, UnitTestResultsDirectory / "*.xml")
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

    internal sealed Configure<CodecovSettings> CodeCovSettingsBase => _ => _
        .SetFiles(UnitTestResultsDirectory.GlobFiles("*.xml").Select(x => x.ToString()))
        .SetToken(CodecovToken)
        .SetFramework("netcoreapp3.0")
        .WhenNotNull(this.As<IHaveGitVersion>(),
                     (_, version) => _.SetBuild(version.GitVersion.FullSemVer))
        .WhenNotNull(this.As<IHaveGitRepository>(),
                     (_, repository) => _.SetBranch(repository.GitRepository.Branch)
                                         .SetSha(repository.GitRepository.Commit));

    /// <summary>
    /// Defines settings used by <see cref="ReportCoverage"/> target
    /// </summary>
    /// <remarks>
    /// These settings are only used when <see cref="ReportToCodeCov"/> is <see langword="true"/>.
    /// </remarks>
    Configure<CodecovSettings> CodecovSettings => _ => _;

    internal sealed Configure<ReportGeneratorSettings> ReportGeneratorSettingsBase => _ => _
        .SetFramework("net5.0")
        .SetReports(UnitTestResultsDirectory / "*.xml")
        .SetReportTypes(ReportTypes.Badges, ReportTypes.HtmlChart, ReportTypes.HtmlInline)
        .WhenNotNull(this.As<IHaveGitRepository>()?.GitRepository,
                     (_, repository) => !string.IsNullOrWhiteSpace(repository.Branch)
                                        ? _.SetTargetDirectory(CoverageReportDirectory / repository.Branch)
                                          .SetHistoryDirectory(CoverageReportHistoryDirectory / repository.Branch)
                                          .SetTag(repository.Commit)
                        : _.SetTargetDirectory(CoverageReportDirectory)
                           .SetHistoryDirectory(CoverageReportHistoryDirectory)
                           .SetTag(repository.Commit)
                     )
        .When(this.As<IHaveGitRepository>() is null,
              _ => _.SetTargetDirectory(CoverageReportDirectory)
                    .SetHistoryDirectory(CoverageReportHistoryDirectory));

    /// <summary>
    /// Defines settings used by <see cref="ReportCoverage"/> target
    /// </summary>
    Configure<ReportGeneratorSettings> ReportGeneratorSettings => _ => _;
}
