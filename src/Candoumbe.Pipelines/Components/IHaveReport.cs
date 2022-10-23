using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can output "reports" of any kind (code coverage, ...)
/// </summary>
public interface IHaveReport : IHaveArtifacts
{
    /// <summary>
    /// Defines the directory where report files will be published
    /// </summary>
    public AbsolutePath ReportDirectory => ArtifactsDirectory / "reports";
}
