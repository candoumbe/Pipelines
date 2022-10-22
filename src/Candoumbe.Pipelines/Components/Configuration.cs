using JetBrains.Annotations;

using Nuke.Common.Tooling;

using System.ComponentModel;

namespace Candoumbe.Pipelines.Components
{
    /// <summary>
    /// Configuration that can be used throughout CI/CD pipelines
    /// </summary>
    [PublicAPI]
    [TypeConverter(typeof(TypeConverter<Configuration>))]
    public class Configuration : Enumeration
    {
        /// <summary>
        /// The "Debug" mode
        /// </summary>
        public static Configuration Debug => new() { Value = nameof(Debug) };

        /// <summary>
        /// The "release" mode
        /// </summary>
        public static Configuration Release => new() { Value = nameof(Release) };

        ///<inheritdoc/>
        public static implicit operator string(Configuration configuration) => configuration.Value;
    }
}
