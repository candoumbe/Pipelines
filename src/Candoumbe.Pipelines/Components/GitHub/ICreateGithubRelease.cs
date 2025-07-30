using System.Collections.Generic;
using System.IO;
using System.Linq;
using Candoumbe.Pipelines.Components.Docker;
using Candoumbe.Pipelines.Components.NuGet;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// Marks a pipeline that can create a GitHub release
/// </summary>
public interface ICreateGithubRelease : IHaveGitHubRepository, IHaveChangeLog, IHaveGitVersion
{
    /// <summary>
    /// Collection of assets/files to add to the published release.
    /// </summary>
    /// <remarks>
    /// Files will be zipped and added to the release
    /// </remarks>
    public IEnumerable<AbsolutePath> Assets => [];

    /// <summary>
    /// Creates a GiHub Release
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
            IReadOnlyList<Release> releases = await gitHubClient.Repository.Release.GetAll(repositoryOwner, repositoryName)
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
                    AbsolutePath fileToUpload = null;
                    if (asset.FileExists())
                    {
                        fileToUpload = asset;
                    }
                    else if (asset.DirectoryExists())
                    {
                        fileToUpload = asset.Parent / $"{asset.NameWithoutExtension}.zip";
                        asset.CompressTo(fileToUpload);
                    }

                    if (fileToUpload is not null)
                    {
                        ReleaseAssetUpload assetToUpload = new()
                        {
                            ContentType = nameof(ContentType.File),
                            FileName = asset.Name,
                            RawData = File.OpenRead(asset)
                        };

                        Information("Uploading {AssetName} ({AssetPath})", asset.Name, asset.ToString() );
                        await gitHubClient.Repository.Release.UploadAsset(release, assetToUpload);
                        Information("{AssetName} uploaded successfully", asset.Name );
                    }
                    else
                    {
                        Warning("{AssetName}({AssetPath}) could not be uploaded as its not a file or directory", asset.Name, asset.ToString());
                    }
                });
                Information("Github release {TagName} created successfully", release.TagName);
            }
            else
            {
                Information("Release '{Version}' already exists - skipping ", MajorMinorPatchVersion);
            }
        });
}