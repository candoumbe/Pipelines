namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that offers configuration support.
/// </summary>
/// <remarks>
/// The default implementation offers support for <see cref="Configuration.Debug"/> and <see cref="Configuration.Release"/>
/// </remarks>
public interface IHaveConfiguration : INukeBuild
{
    /// <summary>
    /// Configuration currently supported by the pipeline
    /// </summary>
    [Parameter("Defines the configuratoin to use when building the application")]
    Configuration Configuration => TryGetValue(() => Configuration) ?? (IsLocalBuild ? Configuration.Debug : Configuration.Release);
}