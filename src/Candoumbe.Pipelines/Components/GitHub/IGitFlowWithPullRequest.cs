using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Candoumbe.Pipelines.Components.Workflows;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Octokit;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// An <see cref="IWorkflow"/> implementation that opens a github pull request
/// </summary>
public interface IGitFlowWithPullRequest : IGitFlow, IPullRequest
{
    string IPullRequest.Title => TryGetValue(() => Title) ?? ((GitRepository.IsOnFeatureBranch(), GitRepository.IsOnReleaseBranch(), GitRepository.IsOnHotfixBranch(), GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*")) switch
    {
        (true, _, _, _) => $"✨ {GitRepository.Branch?.Replace($"{FeatureBranchPrefix}/", string.Empty).ToTitleCase()}",
        (_, _, true, _) => $"🛠️ {GitRepository.Branch?.Replace($"{HotfixBranchPrefix}/", string.Empty).ToTitleCase()}",
        (_, _, _, true) => $"🧹 {GitRepository.Branch?.Replace($"{ColdfixBranchPrefix}/", string.Empty).ToTitleCase()}",
        _ => $"💪🏾 {GitRepository.Branch?.ToTitleCase()}"
    }).Replace('-', ' ');

    ///<inheritdoc/>
    async ValueTask IDoFeatureWorkflow.FinishFeature()
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
            Information(@"Title of the pull request (or ""{PullRequestName}"" if empty)", Title);

            string title = Title;
            string token = GitHubToken;
            if (!SkipConfirmation)
            {
                title = Console.ReadLine() switch
                {
                    { } value when !string.IsNullOrWhiteSpace(value) => value.Trim(),
                    _ => Title
                };

                token ??= PromptForInput("Token (leave empty to exit)", string.Empty);
            }

            Information("Creating {PullRequestName} for {Repository}", title, repositoryName);
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

                if (SkipConfirmation)
                {
                    DeleteLocalBranchIf(DeleteLocalOnSuccess 
                                        && PromptForChoice("Delete branch {BranchName} ?  (Y/N)", BuildChoices()) == ConsoleKey.Y, branchName, switchToBranchName: FeatureBranchSourceName);
                }
                else
                {
                    DeleteLocalBranchIf(DeleteLocalOnSuccess, branchName, switchToBranchName: FeatureBranchSourceName);
                }
                Information("PR {PullRequestUrl} created successfully", pullRequest.HtmlUrl);

                OpenUrl(pullRequest.HtmlUrl);
            }

            return;

            static void DeleteLocalBranchIf(in bool condition, in string branchName, in string switchToBranchName)
            {
                if (!condition)
                {
                    return;
                }

                Git($"switch {switchToBranchName}");
                Git($"branch -D {branchName}");
            }

            static (ConsoleKey key, string description)[] BuildChoices() => new[]
            {
                (key: ConsoleKey.Y, "Delete the local branch"),
                (key: ConsoleKey.N, "Keep the local branch"),
            };

            static void GitPushToRemote()
            {
                Git($"push origin --set-upstream {GitCurrentBranch()}");
            }

            static void OpenUrl(string url)
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