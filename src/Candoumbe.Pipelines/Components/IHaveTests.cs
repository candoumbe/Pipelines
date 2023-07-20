using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines a directory to output various tests results
/// </summary>
public interface IHaveTests : IHaveArtifacts
{
    /// <summary>
    /// Name of the directory where all test results will be published.
    /// </summary>
    public const string TestResultDirectoryName = "tests-results";

    /// <summary>
    /// Directory where to publish all test results
    /// </summary>
    /// <remarks>
    /// By default, the root directory for all test result will be will be in <c>{ArtifactsDirectory} / "tests-results"</c>
    /// </remarks>
    public AbsolutePath TestResultDirectory => ArtifactsDirectory / TestResultDirectoryName;
}
