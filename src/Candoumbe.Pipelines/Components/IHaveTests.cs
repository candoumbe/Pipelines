using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines a directory to output various tests results
/// </summary>
public interface IHaveTests : IHaveArtifacts
{
    /// <summary>
    /// Directory where to publish all test results
    /// </summary>
    public AbsolutePath TestResultDirectory => ArtifactsDirectory / "tests-results";
}
