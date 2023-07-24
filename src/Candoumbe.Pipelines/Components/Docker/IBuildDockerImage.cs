using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Utilities.Collections;

using System;
using System.Collections.Generic;

using static Nuke.Common.Tools.Docker.DockerTasks;

namespace Candoumbe.Pipelines.Components.Docker;

/// <summary>
/// Marks a pipeline that can compile a <see cref="IHaveSolution.Solution"/>
/// </summary>
public interface IBuildDockerImage : IHaveConfiguration
{
    /// <summary>
    /// Gets the docker files to build
    /// </summary>
    IEnumerable<DockerFile> DockerFiles { get; }

    /// <summary>
    /// Build docker images
    /// </summary>
    public Target BuildDockerImages => _ => _
        .Description("Build docker images")
        .OnlyWhenStatic(() => DockerFiles.AtLeastOnce())
        .Executes(() =>
        {
            ReportSummary(_ => _.WhenNotNull(this.As<IHaveGitVersion>(),
                                             (_, version) => _.AddPair("Version", version.GitVersion.FullSemVer)));

            DockerBuild(s => s
                .Apply(BuildSettingsBase)
                .Apply(BuildSettings)
                .CombineWith(DockerFiles,
                             (setting, dockerFile) => setting.SetFile(dockerFile.Path)
                                                             .SetTag($"{dockerFile.Name}:{dockerFile.Tag}")));
        });

    /// <summary>
    /// Default image settings
    /// </summary>
    public sealed Configure<DockerBuildSettings> BuildSettingsBase => _ => _.SetPath(".");

    /// <summary>
    /// Configures the docker image settings
    /// </summary>
    public Configure<DockerBuildSettings> BuildSettings => _ => _;
}