using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines directories used for reporting code coverage
/// </summary>
public interface IHaveCoverage : IHaveReport
{
    /// <summary>
    /// Directory where to report code coverage
    /// </summary>
    public AbsolutePath CoverageReportDirectory => ReportDirectory / "coverage-report";

    /// <summary>
    /// Directory where to publish code coverage history report
    /// </summary>
    public AbsolutePath CoverageReportHistoryDirectory => ReportDirectory / "coverage-history";
}
