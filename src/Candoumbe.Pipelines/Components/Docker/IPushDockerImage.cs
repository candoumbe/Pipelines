using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

using System.Collections.Generic;

using static Nuke.Common.Tools.Docker.DockerTasks;

namespace Candoumbe.Pipelines.Components.Docker;

/// <summary>
/// Marks a pipeline that pushed images to docker registries
/// </summary>
public interface IPushDockerImages : IBuildDockerImage
{
    /// <summary>
    /// Gets names of the images to push
    /// </summary>
    IEnumerable<string> Images { get; }

    /// <summary>
    /// Defines where to push <see cref="Images"/>.
    /// </summary>
    IEnumerable<PushDockerImageConfiguration> Registries { get; }

    /// <summary>
    /// Build 
    /// </summary>
    public Target Push => _ => _
        .Description("Push docker images")
        .Executes(() =>
        {
            Registries.ForEach(registry =>
            {
                if (registry.LoginSettings is not null)
                {
                    DockerLogin(registry.LoginSettings);
                }

                DockerPush(s => s
                    .Apply(PushSettingsBase)
                    .Apply(PushSettings)
                    .CombineWith(Images,
                                 (settings, image) => settings.SetName(image)));
            });
        });

    /// <summary>
    /// Configure default settings to push images
    /// </summary>
    public sealed Configure<DockerPushSettings> PushSettingsBase => _ => _;

    /// <summary>
    /// Configures settings
    /// </summary>
    public Configure<DockerPushSettings> PushSettings => _ => _;
}
