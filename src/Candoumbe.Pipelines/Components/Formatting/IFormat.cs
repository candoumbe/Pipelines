using Nuke.Common;

namespace Candoumbe.Pipelines.Components.Formatting;

/// <summary>
/// Represents an interface for formatting code.
/// </summary>
/// <remarks>
/// The format target will run <strong>AFTER</strong> <see cref="IRestore"/> component and <strong>BEFORE</strong> <see cref="ICompile"/> component.
/// </remarks>
public interface IFormat : INukeBuild
{
    /// <summary>
    /// The default target of the format component.
    /// </summary>
    /// <remarks>
    /// By default, this target will run <strong>after</strong> <see cref="IRestore"/> component and <strong>before</strong> <see cref="ICompile"/> components.
    /// </remarks>
    Target Format => _ => _
        .TryAfter<IRestore>()
        .TryBefore<ICompile>();
}
