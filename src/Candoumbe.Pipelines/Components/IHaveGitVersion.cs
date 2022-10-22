using JetBrains.Annotations;

using Nuke.Common;
using Nuke.Common.Tools.GitVersion;

namespace Candoumbe.Pipelines.Components;

[PublicAPI]
public interface IHaveGitVersion : INukeBuild
{
    [GitVersion(Framework = "net5.0", NoFetch = true)]
    [Required]
    public GitVersion GitVersion => TryGetValue(() => GitVersion);

    /// <summary>
    /// Major.Minor.Patch Version number
    /// </summary>
    string MajorMinorPatchVersion => GitVersion.MajorMinorPatch;
}