using System.ComponentModel;
using Nuke.Common.Tooling;

namespace Candoumbe.Pipelines.Components.Formatting;

/// <summary>
/// Formatter that can be used to format an application
/// </summary>
[TypeConverter(typeof(TypeConverter<DotNetFormatter>))]
public class DotNetFormatter : Enumeration
{
    /// <summary>
    /// Applies formatters related to analyzers
    /// </summary>
    public static readonly DotNetFormatter Analyzers = new() { Value = nameof(Analyzers) };

    /// <summary>
    /// Applies code style
    /// </summary>
    public static readonly DotNetFormatter Style = new() { Value = nameof(Style) };

    /// <summary>
    /// Applies code style for whitespaces
    /// </summary>
    public static readonly DotNetFormatter Whitespace = new() { Value = nameof(Whitespace) };

    ///<inheritdoc/>
    public static implicit operator string(DotNetFormatter configuration) => configuration.Value;
}