using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

using System;

namespace Candoumbe.Pipelines.Components.Docker;

/// <summary>
/// Wraps configuration required to push docker images
/// </summary>
/// <param name="Registry">Uri of the registry where to push docker images</param>
/// <param name="LoginSettings">Configures how to log into the registry defined by <paramref name="Registry"/></param>
public record PushDockerImageConfiguration(Uri Registry, Configure<DockerLoginSettings> LoginSettings = null);
