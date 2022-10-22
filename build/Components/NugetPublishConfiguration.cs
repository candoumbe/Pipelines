using System;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Wraps configuration used to publish packages to nuget.org feed.
/// </summary>
public record NugetPublishConfiguration : PublishConfiguration
{
    /// <summary>
    /// Builds a new <see cref="NugetPublishConfiguration"/> instance.
    /// </summary>
    /// <param name="apiKey">API Key used to interact with <see href="nuget.org"/> API</param>
    /// <param name="source">Nuget API endpoint</param>
    /// <param name="canBeUsed">Lazily indicates if the configuration can be used.</param>
    public NugetPublishConfiguration(string apiKey, Uri source, Func<bool> canBeUsed = null) : base("Nuget", apiKey, source, canBeUsed ?? new Func<bool>(() => true))
    {
    }
}
