using Candoumbe.Pipelines.Components.Workflows;
using Fallout.Common;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Represents an enhanced version of the <see cref="FalloutBuild"/> class which adds various options support from components.
/// </summary>
public abstract class EnhancedNukeBuild : FalloutBuild, ICanSkipConfirmation, ICanAutoStash
{
    /// <inheritdoc />
    [Parameter("Set to true to not ask any confirmation to the end user (default: false)")]
    public bool SkipConfirmation { get; set; }

    /// <inheritdoc />
    [Parameter("Indicates if any changes should be stashed automatically prior to switching branch (Default : true)")]
    public bool AutoStash { get; set; } = true;
}