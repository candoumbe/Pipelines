using Nuke.Common;
using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines an output directory
/// </summary>
public interface IHaveOutputDirectory : INukeBuild
{
    /// <summary>
    /// Directory where to store all output builds output
    /// </summary>
    public AbsolutePath OutputDirectory => RootDirectory / "output";
}