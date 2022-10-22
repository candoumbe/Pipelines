using System;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Wraps configuration used to publish packages to nuget.org feed.
/// </summary>
public record GitHubPublishConfiguration : PublishConfiguration
{
    /// <summary>
    /// Builds a new <see cref="NugetPublishConfiguration"/> instance.
    /// </summary>
    /// <param name="githubToken">API Key used to interact with <see href="nuget.org"/> API</param>
    /// <param name="source">Nuget API endpoint</param>
    /// <param name="canBeUsed">Lazily indicates if the configuration can be used.</param>
    public GitHubPublishConfiguration(string githubToken, Uri source, Func<bool> canBeUsed = null) : base("GitHub", githubToken, source, canBeUsed ?? new Func<bool>(() => true))
    {
    }
}