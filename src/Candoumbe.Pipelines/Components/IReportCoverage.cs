using Nuke.Common;
using Nuke.Common.Tooling;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Build component that can report unit tests code coverage
/// </summary>
/// <remarks>
/// This component requires <see href="https://nuget.org/packages/reportgenerator">ReportGenerator tool</see>
/// </remarks>
public interface IReportCoverage : IRequireNuGetPackage, IHaveCoverage
{
    /// <summary>
    /// The API key used to push code coverage to CodeCov
    /// </summary>
    [Parameter("The API key used to push code coverage to CodeCov")]
    [Secret]
    public string CodecovToken => TryGetValue(() => CodecovToken);

    /// <summary>
    /// Defines if the component should publish results to <see href="codecov.io">Code Cov</see>.
    /// </summary>
    bool ReportToCodeCov { get; }
}