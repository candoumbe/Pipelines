using Nuke.Common;
using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

public interface IHaveSourceDirectory : INukeBuild
{
    /// <summary>
    /// Directory of source code projects
    /// </summary>
    AbsolutePath SourceDirectory => RootDirectory / "src";
}
