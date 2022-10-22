using JetBrains.Annotations;

using Nuke.Common;
using Nuke.Common.Tools.GitVersion;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that manages a Solution that follow GitVersion
/// </summary>
[PublicAPI]
public interface IHaveGitVersion : INukeBuild
{
    /// <summary>
    /// The GitVersion tool that can be used to version the project
    /// </summary>
    [GitVersion(Framework = "net5.0", NoFetch = true)]
    [Required]
    public GitVersion GitVersion => TryGetValue(() => GitVersion);

    /// <summary>
    /// Major.Minor.Patch Version number
    /// </summary>
    string MajorMinorPatchVersion => GitVersion.MajorMinorPatch;
}