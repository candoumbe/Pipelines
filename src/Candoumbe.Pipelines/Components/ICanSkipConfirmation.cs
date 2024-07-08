using Nuke.Common;
using Nuke.Common.Tools.CorFlags;

namespace Candoumbe.Pipelines.Components;
/// <summary>
/// Interface for components that allow skipping confirmation to end user
/// </summary>/// <summary>
/// Interface for components that allow skipping confirmation to end user
/// </summary>
public interface ICanSkipConfirmation
{
    /// <summary>
    /// Allow to skip asking confirmation to end user
    /// </summary>
    [Parameter("Set to true to not ask any confirmation to the end user (default: false)")]
    bool Quiet => false;
}