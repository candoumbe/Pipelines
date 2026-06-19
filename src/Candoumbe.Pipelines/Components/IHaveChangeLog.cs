using Fallout.Common;
using Fallout.Common.IO;

using static Fallout.Common.ChangeLog.ChangelogTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that has a changelog file.
/// </summary>
public interface IHaveChangeLog : IFalloutBuild
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
