using Candoumbe.Pipelines.Components.Workflows;

using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;

using Octokit;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
    public interface IGitHubFlowWithPullRequest : IGitHubFlow, IHaveGitHubRepository
    {
        /// <summary>
        /// The title of the PR that will be created
        /// </summary>
        [Parameter("Title that will be used when creating a PR")]
        string Title => TryGetValue(() => Title) ?? ((GitRepository.IsOnFeatureBranch(), GitRepository.IsOnReleaseBranch(), GitRepository.IsOnHotfixBranch(), GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*")) switch
        {
            (true, _, _, _) => $"[FEATURE] {GitRepository.Branch.Replace($"{FeatureBranchPrefix}/", string.Empty).ToTitleCase()}",
            (_, _, true, _) => $"[HOTFIX] {GitRepository.Branch.Replace($"{HotfixBranchPrefix}/", string.Empty).ToTitleCase()}",
            (_, _, _, true) => $"[COLDFIX] {GitRepository.Branch.Replace($"{ColdfixBranchPrefix}/", string.Empty).ToTitleCase()}",
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
        /// Should the local branch be deleted after the pull request was created successfully ?
        /// </summary>
        [Parameter("Should the local branch be deleted after the pull request was created successfully ?")]
        bool DeleteLocalOnSuccess => false;

        /// <summary>
        /// Defines, when set to <see langword="true"/>, to open the pull request as draft.
        /// </summary>
        [Parameter("Indicates to open the pull request as 'draft'")]
        public bool Draft => false;

        /// <summary>
        /// The issue ID for witch pull request will be created.
        /// </summary>
        [Parameter("Issues that will be closed once the pull request is merged.")]
        uint[] Issues => TryGetValue(() => Issues) ?? Array.Empty<uint>();

        ///<inheritdoc/>
        async ValueTask IWorkflow.FinishFeature()
        {
            string linkToIssueKeyWord = Issues.AtLeastOnce()
                ? string.Join(',', Issues.Select(issueNumber => $"Resolves #{issueNumber}").ToArray())
                : null;

            // Push to the remote branch
            GitPushToRemote();

            string repositoryName = GitRepository.GetGitHubName();
            string branchName = GitCurrentBranch();
            string owner = GitRepository.GetGitHubOwner();

            Information("Creating a pull request for {Repository}", repositoryName);
            string title = PromptForInput("Title of the pull request :", Title);

            Information("Creating {PullRequestName} for {Repository}", title, repositoryName);
            string token = Token ?? PromptForInput("Token (leave empty to exit)", string.Empty);

            if (!string.IsNullOrWhiteSpace(token))
            {
                Information("{SourceBranch} ==> {TargetBranch}", branchName, FeatureBranchSourceName);
                GitHubClient gitHubClient = new(new ProductHeaderValue(repositoryName))
                {
                    Credentials = new Credentials(token)
                };

                NewPullRequest newPullRequest = new(title, branchName, FeatureBranchSourceName)
                {
                    Draft = Draft,
                    Body = linkToIssueKeyWord is not null
                        ? $"{Description}{Environment.NewLine}{Environment.NewLine}{linkToIssueKeyWord}"
                        : Description
                };

                PullRequest pullRequest = await gitHubClient.PullRequest.Create(owner, repositoryName, newPullRequest);

                DeleteLocalBranchIf(DeleteLocalOnSuccess, branchName, switchToBranchName: FeatureBranchSourceName);
                Information("PR {PullRequestUrl} created successfully", pullRequest.HtmlUrl);

                OpenUrl(pullRequest.HtmlUrl);
            }
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
        async ValueTask IWorkflow.FinishHotfix()
        {
            GitPushToRemote();

            string repositoryName = GitRepository.GetGitHubName();
            string branchName = GitCurrentBranch();

            Information("Creating a pull request for {Repository}", repositoryName);
            string title = PromptForInput("Title of the pull request :", Title);

            Information("Creating {PullRequestName} for {Repository}", title, repositoryName);
            string token = Token ?? PromptForInput("Token (leave empty to exit)", string.Empty);
            if (!string.IsNullOrWhiteSpace(token))
            {
                Verbose("{SourceBranch} ==> {TargetBranch}", branchName, HotfixBranchSourceName, token);
                GitHubClient gitHubClient = new(new ProductHeaderValue(repositoryName))
                {
                    Credentials = new Credentials(token)
                };
                NewPullRequest newPullRequest = new(Title, GitRepository.Branch, HotfixBranchSourceName)
                {
                    Draft = Draft,
                    Body = Issues.AtLeastOnce() switch
                    {
                        true => $"{Description}{Environment.NewLine}{Environment.NewLine}",
                        _ => Description
                    }
                };
                PullRequest pullRequest = await gitHubClient.PullRequest.Create(GitRepository.GetGitHubOwner(), repositoryName, newPullRequest);

                Information("PR {PullRequestUrl} created successfully", pullRequest.HtmlUrl);
                DeleteLocalBranchIf(DeleteLocalOnSuccess, GitCurrentBranch(), switchToBranchName: HotfixBranchSourceName);

                ControlFlow.SuppressErrors(() => OpenUrl(pullRequest.HtmlUrl));
            }
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
