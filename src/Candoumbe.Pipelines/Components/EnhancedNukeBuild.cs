using Nuke.Common;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Represents an enhanced version of the <see cref="NukeBuild"/> class which adds various options support from components.
/// </summary>
public abstract class EnhancedNukeBuild : NukeBuild, ICanSkipConfirmation
{
    /// <inheritdoc />
    [Parameter("Set to true to not ask any confirmation to the end user (default: false)")]
    public bool SkipConfirmation { get; set; }
}