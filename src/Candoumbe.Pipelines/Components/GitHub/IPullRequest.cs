using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;

using Octokit;

using System;
using System.Threading.Tasks;

using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.GitHub.GitHubTasks;
using static Nuke.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub
{
    /// <summary>
    /// This interface adds a target to open a pull request
    /// </summary>
    public interface IPullRequest : IGitFlow, IHaveGitHubRepository
    {
        /// <summary>
        /// The title of the PR that will be created
        /// </summary>
        [Parameter("Title that will be used when creating a PR")]
        string Title => TryGetValue(() => Title) ?? ((GitRepository.IsOnFeatureBranch(), GitRepository.IsOnReleaseBranch(), GitRepository.IsOnHotfixBranch()) switch
        {
            (true, _, _) => $"[FEATURE] {GitRepository.Branch.ToTitleCase()}",
            (_, _, true) => $"[HOTFIX] {GitRepository.Branch.ToTitleCase()}",
            _ => GitRepository.Branch.ToTitleCase()
        }).Replace('-', ' ');

        /// <summary>
        /// Token that will be used to connect to GitHub
        /// </summary>
        [Parameter("Token used to create a pull request")]
        [Secret]
        string Token => TryGetValue(() => Token);

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
            // Push to the remote branch
            GitPushToRemote();

            string repositoryName = GitRepository.GetGitHubName();
            string branchName = GitCurrentBranch();
            string owner = GitRepository.GetGitHubOwner();

            Information("Creating a pull request for {Repository}", repositoryName);
            string title = PromptForInput("Title of the pull request :", Title);

            Information("Creating {PullRequestName} for {Repository}", title, repositoryName);
            string token = Token ?? PromptForInput("Token (leave empty to exit)", string.Empty);

            Information("{SourceBranch} ==> {TargetBranch} on the behalf of {UserName}", branchName, DevelopBranch, token);
            GitHubClient gitHubClient = new(new ProductHeaderValue(repositoryName))
            {
                Credentials = new Credentials(token)
            };

            NewPullRequest newPullRequest = new(title, branchName, DevelopBranch)
            {
                Draft = Draft,
                Body = Description
            };

            PullRequest pullRequest = await gitHubClient.PullRequest.Create(owner, repositoryName, newPullRequest);

            Information("PR {PullRequestId} created successfully", pullRequest.Number);
            DeleteLocalBranchIf(DeleteLocalBranchAfterPullRequest, branchName, switchToBranchName: DevelopBranch);
        }

        private static void GitPushToRemote()
        {
            Git($"push origin --set-upstream {GitCurrentBranch()}");
        }
        private static void DeleteLocalBranchIf(in bool condition, in string branchName, in string switchToBranchName)
        {
            if (condition)
            {
                if (PromptForChoice("Delete branch {BranchName} ?  (Y/N)", BuildChoices()) == ConsoleKey.Y)
                {
                    Git($"switch {switchToBranchName}");
                    Git($"branch -D {branchName}");
                }
            }

            static (ConsoleKey key, string description)[] BuildChoices() => new[]
            {
                (key: ConsoleKey.Y, "Delete the local branch"),
                (key: ConsoleKey.N, "Keep the local branch"),
            };
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
