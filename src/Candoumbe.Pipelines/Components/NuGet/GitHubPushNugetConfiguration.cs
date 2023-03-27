using System;

namespace Candoumbe.Pipelines.Components.NuGet;

/// <summary>
/// Wraps configuration used to push nuget packages to nuget.org feed.
/// </summary>
public record GitHubPushNugetConfiguration : PushNugetPackageConfiguration
{
    /// <summary>
    /// Builds a new <see cref="GitHubPushNugetConfiguration"/> instance.
    /// </summary>
    /// <param name="githubToken">API Key used to interact with <see href="gihub.com"/> API</param>
    /// <param name="source">Nuget API endpoint</param>
    /// <param name="canBeUsed">Lazily indicates if the configuration can be used.</param>
    public GitHubPushNugetConfiguration(string githubToken, Uri source, Func<bool> canBeUsed = null) : base("GitHub", githubToken, source, canBeUsed ?? new Func<bool>(() => true))
    {
    }
}