using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;

namespace Candoumbe.Pipelines.Components.Formatting;

/// <summary>
/// Represents an interface for formatting code.
/// It defines a set of files that the component will try to format.
/// </summary>
public interface IFormat : INukeBuild
{
    /// <summary>
    /// The default target of the component
    /// </summary>
    Target Format => _ => _
        .TryDependsOn<IRestore>()
        .TryBefore<ICompile>();
}
