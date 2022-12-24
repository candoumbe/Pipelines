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

        ///<inheritdoc/>
        public static implicit operator string(Configuration configuration) => configuration.Value;
    }
}
