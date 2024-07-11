using Nuke.Common;

namespace Candoumbe.Pipelines.Components;
/// <summary>
/// Interface for components that allow skipping confirmation to end user
/// </summary>/// <summary>
/// Interface for components that allow skipping confirmation to end user
/// </summary>
public interface ICanSkipConfirmation : INukeBuild
{
    /// <summary>
    /// Allow to skip asking confirmation to end user
    /// </summary>
    bool SkipConfirmation { get; }
}