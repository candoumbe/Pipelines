using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;

using System.Collections.Generic;
using System.Linq;

using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// Marks a pipeline that can create a GitHub release
/// </summary>
public interface ICreateGithubRelease : IHaveGitHubRepository, IHaveChangeLog, IHaveGitVersion
{
    /// <summary>
    /// Collection of assets to add to the published release.
    /// </summary>
    /// <remarks>
    /// Files will be zipped and added to the release
    /// </remarks>
    public IEnumerable<AbsolutePath> Assets => Enumerable.Empty<AbsolutePath>();

    /// <summary>
    /// Creates a Github Release
    /// </summary>
    public Target AddGithubRelease => _ => _
        .Unlisted()
        .OnlyWhenStatic(() => GitHubActions.Instance != null)
        .TriggeredBy<IPublish>(x => x.Publish)
        .Description("Creates a new GitHub release after *.nupkgs/*.snupkg were successfully published.")
        .OnlyWhenDynamic(() => IsServerBuild && GitRepository.IsOnMainBranch())
        .Executes(async () =>
        {
            string repositoryName = GitRepository.GetGitHubName();
            Information("Creating a new release for {Repository}", repositoryName);
            Octokit.GitHubClient gitHubClient = new(new Octokit.ProductHeaderValue(repositoryName))
            {
                Credentials = new Octokit.Credentials(GitHubToken)
            };

            string repositoryOwner = GitRepository.GetGitHubOwner();
            IReadOnlyList<Octokit.Release> releases = await gitHubClient.Repository.Release.GetAll(repositoryOwner, repositoryName)
                                                                                           .ConfigureAwait(false);

            if (!releases.AtLeastOnce(release => release.Name == MajorMinorPatchVersion))
            {
                string[] releaseNotes = ExtractChangelogSectionNotes(ChangeLogFile, MajorMinorPatchVersion).Select(line => $"{line}\n").ToArray();
                Octokit.NewRelease newRelease = new(MajorMinorPatchVersion)
                {
                    TargetCommitish = GitRepository.Commit,
                    Body = string.Join("- ", releaseNotes),
                    Name = MajorMinorPatchVersion,
                };

                Octokit.Release release = await gitHubClient.Repository.Release.Create(repositoryOwner, repositoryName, newRelease)
                                                                               .ConfigureAwait(false);

                Information($"Github release {release.TagName} created successfully");
            }
            else
            {
                Information($"Release '{MajorMinorPatchVersion}' already exists - skipping ");
            }
        });
}
