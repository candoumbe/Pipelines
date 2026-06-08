using Fallout.Common;
using Fallout.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines an output directory
/// </summary>
public interface IHaveOutputDirectory : IFalloutBuild
{
    /// <summary>
    /// Name of the root directory that contain all output
    /// </summary>
    public string OutputDirectoryName => "output";

    /// <summary>
    /// Directory where to store all files
    /// </summary>
    public AbsolutePath OutputDirectory => RootDirectory / OutputDirectoryName;
}