using System.Linq.Expressions;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static System.Linq.Expressions.ExpressionExtensions;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines restore steps
/// </summary>
public interface IRestore : INukeBuild, IHaveSolution
{
    /// <summary>
    /// Restore dotnet project dependency
    /// </summary>
    public Target Restore => _ => _
        .TryDependsOn<IClean>(x => x.Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .Apply(RestoreSettingsBase)
                .Apply(RestoreSettings)
            );

            //TODO remove the settings => settings part once Nuke 9.0.3 is out
            DotNetToolRestore(s => s
                .Apply(RestoreToolSettings)
            );
        });

    internal sealed Configure<DotNetRestoreSettings> RestoreSettingsBase => _ => _
            .SetProjectFile(Solution)
            .SetIgnoreFailedSources(IgnoreFailedSources);

    /// <summary>
    /// Options used to restore dependencies
    /// </summary>
    Configure<DotNetRestoreSettings> RestoreSettings => _ => _;

    /// <summary>
    /// Options used to restore tools dependencies
    /// </summary>
    Configure<DotNetToolRestoreSettings> RestoreToolSettings => _ => _;

    /// <summary>
    /// Defines when set to <see langword="true"/> if unreachable sources should make the "restore" process fail.
    /// </summary>
    [Parameter("Ignore unreachable sources during " + nameof(Restore))]
    public bool IgnoreFailedSources => TryGetValue<bool?>(() => IgnoreFailedSources) ?? false;
}