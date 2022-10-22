using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines directories used for reporting code coverage
/// </summary>
public interface IHaveCoverage : IHaveOutputDirectory
{
    /// <summary>
    /// Directory where to report code coverage
    /// </summary>
    public AbsolutePath CoverageReportDirectory => OutputDirectory / "coverage-report";

    /// <summary>
    /// Directory where to publish converage history report
    /// </summary>
    public AbsolutePath CoverageReportHistoryDirectory => OutputDirectory / "coverage-history";

}
