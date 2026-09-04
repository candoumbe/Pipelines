using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Candoumbe.Pipelines.Components.Workflows;
using Fallout.Common.Git;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using static Fallout.Common.Tools.Git.GitTasks;
using static Fallout.Common.Utilities.ConsoleUtility;
using static Serilog.Log;
using GitRepository = Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// An <see cref="IWorkflow"/> implementation that opens a merge request on Azure DevOps.
/// </summary>
public interface IGitFlowWithPullRequest : IGitFlow, IPullRequest, IHaveAzureDevOpsRepository
{
    /// <summary>
    /// Regulat expression to extract the organization, project and repository name from a Azure DevOps repository URL.
    /// </summary>
    static readonly Regex AzureDevOpsRegex = new (@"https:\/\/dev\.azure\.com\/(?<organisation>[^\/]+)\/(?<projet>[^\/]+)\/(?:_git\/)?(?<repository>[^\/]+?)(?:\.git)?",
                                                    RegexOptions.Compiled | RegexOptions.IgnoreCase,
                                                    TimeSpan.FromSeconds(1));

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

        GitPushToRemote();

        string gitRepositoryHttpsUrl = GitRepository.HttpsUrl!;

        Match match = AzureDevOpsRegex.Match(gitRepositoryHttpsUrl!);

        if (match.Success)
        {
            const string repositoryBaseUrl = "https://dev.azure.com";

            string organization = match.Groups["organisation"].Value;
            string project = match.Groups["projet"].Value;
            string repository = match.Groups["repository"].Value;

            Information("Creating a pull request for {Repository}", repository);

            string organizationUrl = $"{repositoryBaseUrl}/{organization}";

            string branchName = GitCurrentBranch();

            Information("Creating a pull request for {Repository}", repository);
            Debug("Project name : {ProjectName}", project);
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

            Information("Creating {PullRequestName} for {Repository}", title, repository);

            if (!string.IsNullOrWhiteSpace(token))
            {
                Information("{SourceBranch} ==> {TargetBranch}", branchName, FeatureBranchSourceName);

                Debug("Login to Azure DevOps");

                VssConnection vssConnection = new(new Uri(organizationUrl), new VssBasicCredential(string.Empty, token));
                GitHttpClient gitHttpClient = await vssConnection.GetClientAsync<GitHttpClient>();

                Debug("Logged into Azure DevOps");
                Debug("Getting repositories");

                IReadOnlyList<GitRepository> repositories = await gitHttpClient.GetRepositoriesAsync().ConfigureAwait(false);
                Debug("{RepositoryCount} Repositories retrieved", repositories.Count);
                Debug("Repositories : {@Repositories}", repositories.Select(r => new { RepositoryName = r.Name, ProjectName = r.ProjectReference.Name, ProjectId = r.ProjectReference.Id }));

                Guid? currentRepositoryId = repositories.SingleOrDefault(repo => repo.ProjectReference.Name == project)?.Id;

                if (currentRepositoryId is not null)
                {
                    GitPullRequest pullRequest = new()
                    {
                        Title = title,
                        IsDraft = Draft,
                        Description = Description,
                        SourceRefName = $"refs/heads/{branchName}",
                        TargetRefName = $"refs/heads/{FeatureBranchSourceName}",
                        CompletionOptions = new GitPullRequestCompletionOptions { DeleteSourceBranch = true, TransitionWorkItems = true, }
                    };

                    pullRequest = await gitHttpClient.CreatePullRequestAsync(pullRequest, project: project, repositoryId: currentRepositoryId.ToString()).ConfigureAwait(false);

                    if (SkipConfirmation)
                    {
                        DeleteLocalBranchIf(DeleteLocalOnSuccess
                                            && PromptForChoice("Delete branch {BranchName} ?  (Y/N)", BuildChoices()) == ConsoleKey.Y, branchName, switchToBranchName: FeatureBranchSourceName);
                    }
                    else
                    {
                        DeleteLocalBranchIf(DeleteLocalOnSuccess, branchName, switchToBranchName: FeatureBranchSourceName);
                    }

                    string pullRequestUrl = $"{organizationUrl}/{project}/_git/{repository}/pullrequest/{pullRequest.PullRequestId}";
                    Information("PR {PullRequestUrl} created successfully", pullRequestUrl);

                    OpenUrl(pullRequestUrl);
                }
                else
                {
                    Error("Repository {Repository} not found in project {ProjectName}", repository, project);
                }
            }
        }
        else
        {
            Error("Repository URL {GitRepositoryHttpsUrl} is not a valid Azure DevOps repository URL", gitRepositoryHttpsUrl);
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

        static (ConsoleKey key, string description)[] BuildChoices() =>
        [
            (key: ConsoleKey.Y, "Delete the local branch"),
            (key: ConsoleKey.N, "Keep the local branch")
        ];

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

    /// <inheritdoc />
    async ValueTask IDoHotfixWorkflow.FinishHotfix()
    {
        await ((IGitFlow)this).FinishHotfix().ConfigureAwait(false);

        if (Issues.AtLeastOnce() && !string.IsNullOrWhiteSpace(AccessToken))
        {
            string gitRepositoryHttpsUrl = GitRepository.HttpsUrl!;
            Match match = AzureDevOpsRegex.Match(gitRepositoryHttpsUrl!);

            if (match.Success)
            {
                const string repositoryBaseUrl = "https://dev.azure.com";
                string organization = match.Groups["organisation"].Value;
                string organizationUrl = $"{repositoryBaseUrl}/{organization}";

                VssConnection vssConnection = new(new Uri(organizationUrl), new VssBasicCredential(string.Empty, AccessToken));
                WorkItemTrackingHttpClient workItemClient = await vssConnection.GetClientAsync<WorkItemTrackingHttpClient>().ConfigureAwait(false);

                foreach (uint workItemId in Issues)
                {
                    Information("Closing work item #{WorkItemId}", workItemId);

                    JsonPatchDocument patchDocument = new()
                    {
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/System.State",
                            Value = "Closed"
                        }
                    };

                    try
                    {
                        await workItemClient.UpdateWorkItemAsync(patchDocument, (int)workItemId).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Hotfix/release merge succeeded, but closing Azure DevOps work item #{workItemId} failed.", ex);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"Hotfix/release merge succeeded, but '{gitRepositoryHttpsUrl}' is not a valid Azure DevOps repository URL.");
            }
        }
    }
}