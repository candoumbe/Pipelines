using Nuke.Common;
using Nuke.Common.CI.GitHubActions;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// Marks a pipeline for repositories that are stored on <see href="https://github.com">GitHub</see>.
/// </summary>
public interface IHaveGitHubRepository : IHaveGitRepository, IHaveSecret
{
    /// <summary>
    /// Token used to create a new GitHub release
    /// </summary>
    [Parameter("Token used to create a new release in GitHub")]
    [Secret]
    string GitHubToken => TryGetValue(() => GitHubToken) ?? GitHubActions.Instance?.Token;
}
