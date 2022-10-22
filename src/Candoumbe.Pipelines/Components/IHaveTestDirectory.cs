using Nuke.Common;
using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can specify a folder for source files
/// </summary>
public interface IHaveTestDirectory : INukeBuild
{
    /// <summary>
    /// Directory of source code projects
    /// </summary>
    public AbsolutePath TestDirectory => RootDirectory / "test";
}
