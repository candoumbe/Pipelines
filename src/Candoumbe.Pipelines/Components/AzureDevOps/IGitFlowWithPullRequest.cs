using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Candoumbe.Pipelines.Components.Workflows;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// An <see cref="IWorkflow"/> implementation that opens a merge request on Azure DevOps.
/// </summary>
public interface IGitFlowWithPullRequest : IGitFlow, IPullRequest, IHaveAzureDevOpsRepository
{
    /// <inheritdoc />
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

            string gitRepositoryHttpsUrl = GitRepository.HttpsUrl!;
            string fullRepositoryUri = gitRepositoryHttpsUrl.AsSpan()[.. (gitRepositoryHttpsUrl.Length - 4)].ToString();
            string branchName = GitCurrentBranch();
            string owner = fullRepositoryUri.Substring(fullRepositoryUri.LastIndexOf('/') + 1, fullRepositoryUri.LastIndexOf('.') - fullRepositoryUri.LastIndexOf('/') - 1);

            Information("Creating a pull request for {Repository}", fullRepositoryUri);
            Information(@"Title of the pull request (or ""{PullRequestName}"" if empty)", Title);

            string title = Title;
            string token = AccessToken;
            if (!SkipConfirmation)
            {
                title = Console.ReadLine() switch
                {
                    { } value when !string.IsNullOrWhiteSpace(value) => value.Trim(),
                    _ => Title
                };

                token ??= PromptForInput("Token (leave empty to exit)", string.Empty);
            }

            Information("Creating {PullRequestName} for {Repository}", title, fullRepositoryUri);
            if (!string.IsNullOrWhiteSpace(token))
            {
                Information("{SourceBranch} ==> {TargetBranch}", branchName, FeatureBranchSourceName);

                VssConnection vssConnection = new(new Uri(fullRepositoryUri), new VssBasicCredential(string.Empty, token));
                GitHttpClient gitHttpClient = await vssConnection.GetClientAsync<GitHttpClient>();

                GitPullRequest pullRequest = new GitPullRequest()
                {
                    Title = title,
                    IsDraft = Draft,
                    Description = Description,
                    TargetRefName = this.Get<IGitFlow>().FeatureBranchSourceName,
                };

                pullRequest = await gitHttpClient.CreatePullRequestAsync(pullRequest, fullRepositoryUri);

                if (SkipConfirmation)
                {
                    DeleteLocalBranchIf(DeleteLocalOnSuccess 
                                        && PromptForChoice("Delete branch {BranchName} ?  (Y/N)", BuildChoices()) == ConsoleKey.Y, branchName, switchToBranchName: FeatureBranchSourceName);
                }
                else
                {
                    DeleteLocalBranchIf(DeleteLocalOnSuccess, branchName, switchToBranchName: FeatureBranchSourceName);
                }
                Information("PR {PullRequestUrl} created successfully", pullRequest);

                OpenUrl(pullRequest.Url);
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

    /// <inheritdoc />
    async ValueTask IDoChoreWorkflow.FinishChore() => await FinishFeature();
}