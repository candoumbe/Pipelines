namespace Candoumbe.Pipelines.Components;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.Codecov;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;

using System.Linq;

using static Nuke.Common.Tools.Codecov.CodecovTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

public interface IReportCoverage : IUnitTest
{
    [Parameter]
    public string CodecovToken { get; }

    public Target ReportCoverage => _ => _
       .DependsOn(UnitTests)
       .OnlyWhenDynamic(() => IsServerBuild || CodecovToken != null)
       .Consumes(UnitTests, TestResultDirectory / "*.xml")
       .Produces(CoverageReportDirectory / "*.xml")
       .Produces(CoverageReportHistoryDirectory / "*.xml")
       .Executes(() =>
       {
           ReportGeneratorSettings reportSettings = new ReportGeneratorSettings()
                   .SetFramework("net5.0")
                   .SetReports(TestResultDirectory / "*.xml")
                   .SetReportTypes(ReportTypes.Badges, ReportTypes.HtmlChart, ReportTypes.HtmlInline_AzurePipelines_Dark)
                   .SetTargetDirectory(CoverageReportDirectory)
                   .SetHistoryDirectory(CoverageReportHistoryDirectory);

           CodecovSettings codeCovSettings = new CodecovSettings().SetFiles(TestResultDirectory.GlobFiles("*.xml").Select(x => x.ToString()))
                                                                  .SetToken(CodecovToken)
                                                                  .SetFramework("netcoreapp3.0");

           if (this is IHaveGitRepository git)
           {
               reportSettings = reportSettings.SetTag(git.GitRepository.Commit);
               codeCovSettings = codeCovSettings.SetBranch(git.GitRepository.Branch)
                                                .SetSha(git.GitRepository.Commit);
           }

           if (this is IHaveGitVersion gitVersion)
           {
               codeCovSettings = codeCovSettings.SetBuild(gitVersion.GitVersion.FullSemVer);
           }

           ReportGenerator(reportSettings);
           Codecov(codeCovSettings);
       });
}
