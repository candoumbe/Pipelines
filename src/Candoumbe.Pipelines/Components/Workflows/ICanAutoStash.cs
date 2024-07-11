namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Enable/Disable support for stashing ongoing work
/// </summary>
public interface ICanAutoStash
{
    /// <summary>
    /// Indicates if any changes should be stashed automatically prior to switching branch (Default : true
    /// </summary>
    bool AutoStash { get; }
}