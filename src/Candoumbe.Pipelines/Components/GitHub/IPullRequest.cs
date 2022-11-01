using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;

using Octokit;

using System;
using System.Threading.Tasks;

using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub
{
    /// <summary>
    /// This interface adds a target to open a pull request
    /// </summary>
    public interface IPullRequest : IGitFlow
    {
        /// <summary>
        /// The title of the PR that will be created
        /// </summary>
        [Parameter("Title that will be used when creating a PR")]
        string Title => TryGetValue(() => Title) ?? (GitRepository.IsOnFeatureBranch(), GitRepository.IsOnReleaseBranch(), GitRepository.IsOnHotfixBranch()) switch
        {
            (true, _, _) => $"[FEATURE] {GitRepository.Branch.ToTitleCase()}",
            (_, _, true) => $"[HOTFIX] {GitRepository.Branch.ToTitleCase()}",
            _ => GitRepository.Branch.ToTitleCase()
        };

        /// <summary>
        /// Description of the pull request
        /// </summary>
        [Parameter("Description of the pull request")]
        string Description => TryGetValue(() => Description) ?? this.Get<IHaveChangeLog>()?.ReleaseNotes;

        /// <summary>
        /// Should the pipeline delete the branch after pull request ?
        /// </summary>
        public bool DeleteLocalBranchAfterPullRequest => false;

        /// <summary>
        /// Defines, when set to <see langword="true"/>, to open the pull request as draft.
        /// </summary>
        public bool Draft => true;

        ///<inheritdoc/>
        async ValueTask IGitFlow.FinishFeature()
        {
            string repositoryName = GitRepository.GetGitHubName();
            string branchName = GitRepository.Branch;

            // Push to the remote branch
            GitPushToRemote();

            Information("Creating a pull request for {Repository}", repositoryName);

            Information("Title of the pull request :");
            string title = Console.ReadLine() ?? Title;
            Information("Creating {PullRequestName} for {Repository}", title, repositoryName);

            GitHubClient gitHubClient = new(new ProductHeaderValue(repositoryName));

            NewPullRequest pullRequest = new(title, branchName, DevelopBranch)
            {
                Draft = Draft,
                Body = Description
            };

            await gitHubClient.PullRequest.Create(GitRepository.GetGitHubOwner(), repositoryName, pullRequest);

            Information("{PullRequestName} created successfully", title, repositoryName);
            DeleteLocalBranchIf(DeleteLocalBranchAfterPullRequest, branchName, switchToBranchName: DevelopBranch);
        }

        private void GitPushToRemote()
        {
            Git($"push origin --set-upstream {GitCurrentBranch()}");
        }

        private void DeleteLocalBranchIf(bool condition, string branchName, string switchToBranchName)
        {
            if (condition)
            {
                Information("Delete branch {BranchName} ?  (Y/N)", branchName);
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Git($"switch {switchToBranchName}");
                    Git($"branch -D {branchName}");
                }
            }
        }

        ///<inheritdoc/>
        async ValueTask IGitFlow.FinishReleaseOrHotfix()
        {
            GitPushToRemote();

            string repositoryName = GitRepository.GetGitHubName();
            Information("Creating a pull request for {Repository}", repositoryName);

            Information("Creating {PullRequestName} for {Repository}", Title, repositoryName);

            NewPullRequest pullRequest = new(Title, GitRepository.Branch, MainBranchName);

            GitHubClient gitHubClient = new(new ProductHeaderValue(repositoryName));
            await gitHubClient.PullRequest.Create(GitRepository.GetGitHubOwner(), repositoryName, pullRequest);

            DeleteLocalBranchIf(DeleteLocalBranchAfterPullRequest, GitCurrentBranch(), switchToBranchName: DevelopBranch);
        }
    }
}
