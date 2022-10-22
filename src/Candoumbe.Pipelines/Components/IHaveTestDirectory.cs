using Nuke.Common;
using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

public interface IHaveTestDirectory : INukeBuild
{
    /// <summary>
    /// Directory of source code projects
    /// </summary>
    public AbsolutePath TestDirectory => RootDirectory / "test";
}
