using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines a directory to store artifacts
/// </summary>
public interface IHaveArtifacts : IHaveOutputDirectory
{
    /// <summary>
    /// Directory where to publish all artifacts
    /// </summary>
    public AbsolutePath ArtifactsDirectory => OutputDirectory / "artifacts";
}