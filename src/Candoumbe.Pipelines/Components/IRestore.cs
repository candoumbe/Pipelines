using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines restore steps
/// </summary>
public interface IRestore : INukeBuild, IHaveSolution
{
    public Target Restore => _ => _
        .TryDependsOn<IClean>(x => x.Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .Apply(RestoreSettingsBase)
                .Apply(RestoreSettings)
            );

            DotNetToolRestore();
        });

    public sealed Configure<DotNetRestoreSettings> RestoreSettingsBase => _ => _
            .SetProjectFile(Solution)
            .SetIgnoreFailedSources(IgnoreFailedSources);

    /// <summary>
    /// Options used to restore dependencies
    /// </summary>
    Configure<DotNetRestoreSettings> RestoreSettings => _ => _;

    [Parameter("Ignore unreachable sources during " + nameof(Restore))]
    public bool IgnoreFailedSources => TryGetValue<bool?>(() => IgnoreFailedSources) ?? false;
}