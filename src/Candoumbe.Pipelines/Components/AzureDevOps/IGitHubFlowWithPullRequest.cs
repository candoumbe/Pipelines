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
using GitRepository = Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// An <see cref="IWorkflow"/> implementation that opens a merge request on Azure DevOps.
/// </summary>
public interface IGitHubFlowWithPullRequest : IGitHubFlow, IPullRequest, IHaveAzureDevOpsRepository
{
    /// <inheritdoc />
    string IPullRequest.Title => TryGetValue(() => Title) ?? ((GitRepository.IsOnFeatureBranch(), GitRepository.IsOnReleaseBranch(), GitRepository.IsOnHotfixBranch()) switch
    {
        (true, _, _) => $"✨ {GitRepository.Branch?.Replace($"{FeatureBranchPrefix}/", string.Empty).ToTitleCase()}",
        (_, _, true) => $"🛠️ {GitRepository.Branch?.Replace($"{HotfixBranchPrefix}/", string.Empty).ToTitleCase()}",
        _ => $"💪🏾 {GitRepository.Branch?.ToTitleCase()}"
    }).Replace('-', ' ');

    ///<inheritdoc/>
    async ValueTask IDoFeatureWorkflow.FinishFeature()
        {
            // Push to the remote branch
            GitPushToRemote();

            string gitRepositoryHttpsUrl = GitRepository.HttpsUrl!;
            string fullRepositoryUri = gitRepositoryHttpsUrl.EndsWith(".git")
                ? gitRepositoryHttpsUrl.AsSpan().TrimEnd(".git")[..(gitRepositoryHttpsUrl.Length - 4)].ToString()
                : gitRepositoryHttpsUrl.AsSpan()[..gitRepositoryHttpsUrl.Length].ToString();
            const string repositoryBaseUrl = "https://dev.azure.com";
            const int repositoryBaseUrlLength = 22;
            string organization = fullRepositoryUri[repositoryBaseUrlLength..fullRepositoryUri.IndexOf('/', repositoryBaseUrlLength)];
            string organizationUrl = $"{repositoryBaseUrl}/{organization}";

            // Extract the project name from the repository URL
            string projectName = fullRepositoryUri[(organizationUrl.Length + 6)..];

            string branchName = GitCurrentBranch();

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

                Debug("Login to Azure DevOps");

                VssConnection vssConnection = new(new Uri(organizationUrl), new VssBasicCredential(string.Empty, token));
                GitHttpClient gitHttpClient = await vssConnection.GetClientAsync<GitHttpClient>();

                Debug("Logged into Azure DevOps");
                Debug("Getting repositories");

                IReadOnlyList<GitRepository> repositories = await gitHttpClient.GetRepositoriesAsync().ConfigureAwait(false);
                Debug("{RepositoryCount} Repositories retrieved", repositories.Count);
                Debug("Repositories : {@Repositories}", repositories.Select(r => new {RepositoryName = r.Name, ProjectName = r.ProjectReference.Name, ProjectId = r.ProjectReference.Id }));

                Guid? currentRepositoryId = repositories.SingleOrDefault(repository => repository.ProjectReference.Name == projectName)?.Id;

                if (currentRepositoryId is not null)
                {
                    GitPullRequest pullRequest = new ()
                    {
                        Title = title,
                        IsDraft = Draft,
                        Description = Description,
                        SourceRefName = $"refs/head/{branchName}",
                        TargetRefName = $"refs/head/{FeatureBranchSourceName}",
                        CompletionOptions = new GitPullRequestCompletionOptions
                        {
                            DeleteSourceBranch = true,
                            TransitionWorkItems = true,
                        }
                    };

                    pullRequest = await gitHttpClient.CreatePullRequestAsync(pullRequest, project: projectName, repositoryId: currentRepositoryId.ToString()).ConfigureAwait(false);

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
                else
                {
                    Error("Repository {Repository} not found in project {ProjectName}", fullRepositoryUri, projectName);
                }
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
}