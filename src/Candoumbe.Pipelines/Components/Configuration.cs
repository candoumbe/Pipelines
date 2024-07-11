using System.ComponentModel;
using Nuke.Common.Tooling;

namespace Candoumbe.Pipelines.Components
{
    /// <summary>
    /// Configuration that can be used to compile an application
    /// </summary>
    [TypeConverter(typeof(TypeConverter<Configuration>))]
    public class Configuration : Enumeration
    {
        /// <summary>
        /// Debug mode
        /// </summary>
        public static readonly Configuration Debug = new() { Value = nameof(Debug) };

        /// <summary>
        /// Release mode
        /// </summary>
        public static readonly Configuration Release = new() { Value = nameof(Release) };

        
        /// <summary>
        /// Implicit operator to convert a <see cref="Configuration"/> to <see langword="string"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>The value of the configuration</returns>
        public static implicit operator string(Configuration configuration) => configuration.Value;
    }
}
