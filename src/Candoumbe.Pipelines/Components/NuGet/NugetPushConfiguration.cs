using System;

namespace Candoumbe.Pipelines.Components.NuGet;

/// <summary>
/// Wraps configuration used to publish packages to <see href="nuget.org">Nuget.org</see> feed.
/// </summary>
public record NugetPushConfiguration : PushNugetPackageConfiguration
{
    /// <summary>
    /// Builds a new <see cref="NugetPushConfiguration"/> instance.
    /// </summary>
    /// <param name="apiKey">API Key used to interact with <see href="nuget.org"/> API</param>
    /// <param name="source">Nuget API endpoint</param>
    /// <param name="canBeUsed">Lazily indicates if the configuration can be used.</param>
    public NugetPushConfiguration(string apiKey, Uri source, Func<bool> canBeUsed = null) : this(apiKey, source.ToString(), canBeUsed ?? new Func<bool>(() => true))
    {
    }

    /// <summary>
    /// Builds a new <see cref="NugetPushConfiguration"/> instance.
    /// </summary>
    /// <param name="apiKey">API Key used to interact with <see href="nuget.org"/> API</param>
    /// <param name="source">Nuget API endpoint</param>
    /// <param name="canBeUsed">Lazily indicates if the configuration can be used.</param>
    public NugetPushConfiguration(string apiKey, string source, Func<bool> canBeUsed = null) : base("Nuget", apiKey, source, canBeUsed ?? new Func<bool>(() => true))
    {
    }
}
