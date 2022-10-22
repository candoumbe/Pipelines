using Nuke.Common;
using Nuke.Common.IO;

using static Nuke.Common.ChangeLog.ChangelogTasks;

namespace Candoumbe.Pipelines.Components;

public interface IHaveChangeLog : INukeBuild
{
    /// <summary>
    /// Path to the CHANGELOG.md file
    /// </summary>
    AbsolutePath ChangeLogFile => RootDirectory / "CHANGELOG.md";

    /// <summary>
    /// Gets the release notes
    /// </summary>
    string ReleaseNotes => GetNuGetReleaseNotes(ChangeLogFile, (this as IHaveGitRepository)?.GitRepository);
}
