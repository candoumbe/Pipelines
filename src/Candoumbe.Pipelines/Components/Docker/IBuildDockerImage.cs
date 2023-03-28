using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Utilities.Collections;

using System.Collections.Generic;
using System.Linq;

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
    IEnumerable<(AbsolutePath Path, string Name, string Tag)> DockerFiles => Enumerable.Empty<(AbsolutePath, string, string)>();

    /// <summary>
    /// Build docker images
    /// </summary>
    public Target Build => _ => _
        .Description("Build docker images")
        .OnlyWhenStatic(() => DockerFiles.AtLeastOnce())
        .Executes(() =>
        {
            ReportSummary(_ => _.WhenNotNull(this as IHaveGitVersion,
                                             (_, version) => _.AddPair("Version", version.GitVersion.FullSemVer)));

            DockerBuild(s => s
                .Apply(BuildSettingsBase)
                .Apply(BuildSettings)
                .CombineWith(DockerFiles,
                             (setting, dockerFile) => setting.SetFile(dockerFile.Path)
                                                             .WhenNotNull(this as IHaveGitVersion,
                                                                          (_, version) => _.SetTag($"{dockerFile.Name}:{version.MajorMinorPatchVersion}"))));
        });

    /// <summary>
    /// Default image settings
    /// </summary>
    public sealed Configure<DockerBuildSettings> BuildSettingsBase => _ => _;

    /// <summary>
    /// Configures the docker image settings
    /// </summary>
    public Configure<DockerBuildSettings> BuildSettings => _ => _;
}