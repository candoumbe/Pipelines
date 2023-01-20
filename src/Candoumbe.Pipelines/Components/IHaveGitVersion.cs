﻿using JetBrains.Annotations;

using Nuke.Common;
using Nuke.Common.Tools.GitVersion;

using Octokit;

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
    /// Hint to create a major release.
    /// </summary>
    /// <remarks>
    /// The value of this property is only taken into account when running <see cref="Release"/> target.
    /// </remarks>
    [Parameter("Hint to create a major release.")]
    bool Major => false;

    /// <summary>
    /// Major.Minor.Patch Version number
    /// </summary>
    string MajorMinorPatchVersion => Major ? $"{GitVersion.Major + 1}.0.0" : GitVersion.MajorMinorPatch;
}