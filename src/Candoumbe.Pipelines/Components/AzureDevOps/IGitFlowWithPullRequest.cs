using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Candoumbe.Pipelines.Components.Workflows;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// An <see cref="IWorkflow"/> implementation that opens a github pull request
/// </summary>
public interface IGitFlowWithPullRequest : IGitFlow, IDoPullRequest
{
    string IDoPullRequest.Title => TryGetValue(() => Title) ?? ( ( GitRepository.IsOnFeatureBranch(),
            GitRepository.IsOnReleaseBranch(), GitRepository.IsOnHotfixBranch(),
            GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*") ) switch
        {
            (true, _, _, _) =>
                $"✨ {GitRepository.Branch?.Replace($"{FeatureBranchPrefix}/", string.Empty).ToTitleCase()}",
            (_, _, true, _) =>
                $"🛠️ {GitRepository.Branch?.Replace($"{HotfixBranchPrefix}/", string.Empty).ToTitleCase()}",
            (_, _, _, true) =>
                $"🧹 {GitRepository.Branch?.Replace($"{ColdfixBranchPrefix}/", string.Empty).ToTitleCase()}",
            _ => $"💪🏾 {GitRepository.Branch?.ToTitleCase()}"
        } ).Replace('-', ' ');

    ///<inheritdoc/>
    async ValueTask IDoFeatureWorkflow.FinishFeature()
    {
        string linkToIssueKeyWord = Issues.AtLeastOnce()
            ? string.Join(',', Issues.Select(issueNumber => $"Resolves #{issueNumber}").ToArray())
            : null;

        // Push to the remote branch
        GitPushToRemote();

        string repositoryName = GitRepository.RemoteName;
        string branchName = GitCurrentBranch();
        
        Information("Creating a pull request for {Repository}", repositoryName);
        Information(@"Title of the pull request (or ""{PullRequestName}"" if empty)", Title);

        string title = Console.ReadLine() switch
        {
            { } value when !string.IsNullOrWhiteSpace(value) => value.Trim(),
            _ => Title
        };

        string token = Token ?? PromptForInput("Token (leave empty to exit)", string.Empty);

        if (!string.IsNullOrWhiteSpace(token))
        {
            Information("Creating {PullRequestName} for {Repository}", title, repositoryName);
            Information("{SourceBranch} ==> {TargetBranch}", branchName, FeatureBranchSourceName);
            VssConnection connection = new (new Uri(GitRepository.HttpsUrl!),
                new VssOAuthAccessTokenCredential(token));
            using GitHttpClient gitClient = await connection.GetClientAsync<GitHttpClient>().ConfigureAwait(false);
            
            GitPullRequest newPullRequest = new()
            {
                Title = title,
                TargetRefName = FeatureBranchSourceName,
                IsDraft = Draft,
                Description = linkToIssueKeyWord is not null
                    ? $"{Description}{Environment.NewLine}{Environment.NewLine}{linkToIssueKeyWord}"
                    : Description
            };
            
            newPullRequest = await gitClient.CreatePullRequestAsync(newPullRequest, repositoryName, supportsIterations: true);
            
            // Try to link the pull request to its work items if any
            await LinkPullRequestToRelatedWorkItems(connection, newPullRequest, Issues.Select(number => (int)number).ToArray());

            DeleteLocalBranchIf(DeleteLocalOnSuccess, branchName, switchToBranchName: FeatureBranchSourceName);
            Information("PR {PullRequestUrl} created successfully", newPullRequest.Url);

            OpenUrl(newPullRequest.Url);
        }

        static void GitPushToRemote()
        {
            Git($"push origin --set-upstream {GitCurrentBranch()}");
        }

        static void DeleteLocalBranchIf(in bool condition, in string branchName, in string switchToBranchName)
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
                ( key: ConsoleKey.Y, "Delete the local branch" ),
                ( key: ConsoleKey.N, "Keep the local branch" ),
            };
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

        async Task LinkPullRequestToRelatedWorkItems(VssConnection connection, GitPullRequest newPullRequest, int[] issues)
        {
            if (issues is { Length: > 0 })
            {
                using WorkItemTrackingHttpClient workItemClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

                IReadOnlyList<WorkItem> workItems = await workItemClient.GetWorkItemsAsync(ids: Issues.Select(number => (int)number))
                    .ConfigureAwait(false);
                workItems.ForEach(workItem => workItem.Links.AddLink(newPullRequest.Title, newPullRequest.Url));
            }
        }
    }
}