using Candoumbe.Pipelines.Components.Docker;
using Candoumbe.Pipelines.Components.NuGet;

using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
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
        .OnlyWhenDynamic(() => !string.IsNullOrWhiteSpace(GitHubToken))
        .OnlyWhenDynamic(() => GitRepository.IsOnMainBranch())
        .TryTriggeredBy<IPushNugetPackages>(x => x.Publish)
        .TryTriggeredBy<IPushDockerImages>(x => x.PushImages)
        .Description("Creates a new GitHub release after artifacts were successfully published.")
        .Executes(async () =>
        {
            string repositoryName = GitRepository.GetGitHubName();
            Information("Creating a new release for {Repository}", repositoryName);
            GitHubClient gitHubClient = new(new ProductHeaderValue(repositoryName))
            {
                Credentials = new Credentials(GitHubToken)
            };

            string repositoryOwner = GitRepository.GetGitHubOwner();
            IReadOnlyList<Octokit.Release> releases = await gitHubClient.Repository.Release.GetAll(repositoryOwner, repositoryName)
                                                                                           .ConfigureAwait(false);

            if (!releases.AtLeastOnce(release => release.Name == MajorMinorPatchVersion))
            {
                string[] releaseNotes = ExtractChangelogSectionNotes(ChangeLogFile, MajorMinorPatchVersion).Select(line => $"{line}\n").ToArray();
                NewRelease newRelease = new(MajorMinorPatchVersion)
                {
                    TargetCommitish = GitRepository.Commit,
                    Body = string.Join("- ", releaseNotes),
                    Name = MajorMinorPatchVersion,
                };

                Release release = await gitHubClient.Repository.Release.Create(repositoryOwner, repositoryName, newRelease)
                                                                               .ConfigureAwait(false);
                await Assets.ForEachAsync(async asset =>
                {
                    ReleaseAssetUpload assetToUpload = new ReleaseAssetUpload
                    {
                        ContentType = ContentType.File.ToString(),
                        FileName = asset.ToFileInfo().Name,
                        RawData = new MemoryStream(File.ReadAllBytes(asset))
                    };

                    await gitHubClient.Repository.Release.UploadAsset(release, assetToUpload);
                });
                Information($"Github release {release.TagName} created successfully");
            }
            else
            {
                Information($"Release '{MajorMinorPatchVersion}' already exists - skipping ");
            }
        });
}
