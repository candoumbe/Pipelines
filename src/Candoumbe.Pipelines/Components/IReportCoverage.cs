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

    /// <summary>
    /// Defines settings used by the component
    /// </summary>
    /// <remarks>
    /// These settings are only used when <see cref="ReportToCodeCov"/> is <see langword="true"/>.
    /// </remarks>
    Configure<CodecovSettings> CodecovSettings => _ => _;

    /// <summary>
    /// Defines settings used to report coverage
    /// </summary>
    Configure<ReportGeneratorSettings> ReportGeneratorSettings => _ => _;
}
