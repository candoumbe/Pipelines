using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fallout.Common;
using Fallout.Common.Tools.GitHub;
using Spectre.Console;
using static Fallout.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// Component responsible for deleting GitHub container packages.
/// </summary>
public interface ICanDeleteContainerPackages : IFalloutBuild, IHaveGitHubRepository
{
    /// <summary>
    /// Gets the names of the packages to delete.
    /// </summary>
    [Parameter("Name of packages to delete")]
    string[] Packages => TryGetValue(() => Packages);

    /// <summary>
    /// Gets the tag pattern used to identify which tags to delete.
    /// </summary>
    [Parameter("Tag pattern used to identify which tags to delete. The pattern supports '*' and '?' wildcards. For example, 'v1.*' will match all tags starting with 'v1.'.")]
    string TagPattern => TryGetValue(() => TagPattern);

    /// <summary>
    /// Gets the token with delete:packages access to the GitHub Container Registry.
    /// </summary>
    [Parameter("Token with delete:packages access to the GitHub Container Registry")]
    [Secret]
    string ImageAdminToken => TryGetValue(() => ImageAdminToken);


    /// <summary>
    /// Gets the URI of the GitHub Container Registry.
    /// </summary>
    [Parameter("URI of the GitHub Container Registry")]
    string RegistryUri => TryGetValue(() => RegistryUri) ?? $"https://ghcr.io/{this.Get<IHaveGitHubRepository>().GitRepository.GetGitHubOwner()}";

    /// <summary>
    /// Target responsible for deleting container images from the GitHub Container Registry.
    /// </summary>
    public Target DeleteContainerImages => _ => _.OnlyWhenStatic(() => IsLocalBuild)
        .Description("Deletes all packages from the GitHub Container Registry that matches pattern specified by the TagPattern property")
        .Requires(() => !string.IsNullOrWhiteSpace(ImageAdminToken))
        .Requires(() => Packages != null && Packages.Length > 0)
        .Executes(async () =>
        {
            (string Name, string Uri) registry = ("GitHub Container Registry", RegistryUri);

            if (await AnsiConsole.ConfirmAsync($"You're about to clean up the following images from {registry.Name} ({registry.Uri}) : {string.Join(", ", Packages)}.{Environment.NewLine}Proceed ?",
                                               defaultValue: true))
            {
                Information("Cleaning up images from {RegistryName} ({RegistryUri})", registry.Name, registry.Uri);

                string owner = this.Get<IHaveGitHubRepository>().GitRepository.GetGitHubOwner();

                // Choose which image to delete
                string imageToDelete = PromptForChoice("Select image to delete: ", [.. Packages.Select(image => (image, image))]);
                if (string.IsNullOrWhiteSpace(imageToDelete))
                {
                    Information("Operation cancelled by the user.");
                    return;
                }

                Information("Deleting image {ImageName} from {RegistryName} ({RegistryUri})", imageToDelete, registry.Name, registry.Uri);

                // Choose which tag to delete
                Octokit.GitHubClient client = new(new Octokit.ProductHeaderValue("Agenda.Pipelines"))
                {
                    Credentials = new Octokit.Credentials(ImageAdminToken)
                };

                Information("Retrieving tags for image {ImageName} from {RegistryName} ({RegistryUri})",
                            imageToDelete,
                            registry.Name,
                            registry.Uri);

                Octokit.Package package = await client.Packages.GetForUser(owner, Octokit.PackageType.Container, imageToDelete);
                if (package is null)
                {
                    Information("Image {ImageName} not found in {RegistryName} ({RegistryUri})", imageToDelete, registry.Name, registry.Uri);
                    return;
                }

                string tagPatternToDelete = TagPattern switch
                {
                    null or "" => await AnsiConsole.AskAsync("Enter the tag pattern to delete (0.2-? or 0.2-develop.*)", string.Empty),
                    _ => TagPattern
                };

                if (string.IsNullOrWhiteSpace(tagPatternToDelete))
                {
                    Information("No tag pattern provided. Aborting deletion.");
                    return;
                }

                Octokit.ApiOptions apiOptions = new() { PageSize = 100 };
                int page = 1;
                List<Octokit.PackageVersion> allVersions = new(capacity: 300);
                IReadOnlyList<Octokit.PackageVersion> pageOfVersions = Array.Empty<Octokit.PackageVersion>();
                do
                {
                    Verbose("Listing tags for image {ImageName} from {RegistryName} ({RegistryUri}) page {Page}", imageToDelete, registry.Name, registry.Uri, page);
                    apiOptions.StartPage = apiOptions.StartPage.HasValue ? apiOptions.StartPage.Value + 1 : 1;
                    pageOfVersions = await client.Packages.PackageVersions.GetAllForUser(owner, Octokit.PackageType.Container, imageToDelete, options: apiOptions);
                    Verbose("Retrieved {Count} tags for image {ImageName} from {RegistryName} ({RegistryUri}) page {Page}", pageOfVersions.Count, imageToDelete, registry.Name, registry.Uri, page);
                    allVersions.AddRange(pageOfVersions);
                    page++;
                } while (pageOfVersions.Count == 100);

                (int versionId, string[] tags)[] tagToVersionIdMapper = [.. allVersions.Select<Octokit.PackageVersion, (int versionId, string[] tags)>(v => (Convert.ToInt32(v.Id), tags: [.. v.Metadata.Container.Tags]))];

                string[] tagsToBeDeleted = [.. allVersions.SelectMany(v => v.Metadata.Container.Tags).Where(tag => tag.Like(tagPatternToDelete))];

                if (tagsToBeDeleted.Length == 0)
                {
                    Information("No tags match the pattern {TagPattern} for image {ImageName} from {RegistryName} ({RegistryUri})", tagPatternToDelete, imageToDelete, registry.Name, registry.Uri);
                    return;
                }

                Information("The following tags matches the pattern {TagPattern} for image {ImageName} from {RegistryName} ({RegistryUri}){Tags}",
                            tagPatternToDelete,
                            imageToDelete,
                            registry.Name,
                            registry.Uri,
                            tagsToBeDeleted);

                if (PromptForChoice("Delete these matching tags ??", [(ConsoleKey.Y, $"I want to delete these {tagsToBeDeleted.Length} tags"), (ConsoleKey.N, "No, I changed my mind")]) == ConsoleKey.N)
                {
                    Information("Aborted deletion of matching tags for image {ImageName} from {RegistryName} ({RegistryUri})", imageToDelete, registry.Name, registry.Uri);
                    return;
                }

                IReadOnlyList<int> versionsToDelete = [.. allVersions.Where(v => v.Metadata.Container.Tags.All(tag => tag.Like(tagPatternToDelete))).Select(v => Convert.ToInt32(v.Id))];

                Information("Found {Count} versions to delete for image {ImageName} from {RegistryName} ({RegistryUri})",
                            versionsToDelete.Count,
                            imageToDelete,
                            registry.Name,
                            registry.Uri);

                await AnsiConsole.Progress()
                    .Start(async ctx =>
                    {
                        ProgressTask task = ctx.AddTask("Deleting versions...", maxValue: versionsToDelete.Count);
                        foreach (int versionToDelete in versionsToDelete)
                        {
                            task.Description = $"Deleting version {versionToDelete} for image {imageToDelete} from {registry.Name} ({registry.Uri})";
                            await client.Packages.PackageVersions.DeleteForUser(owner, Octokit.PackageType.Container, imageToDelete, versionToDelete);
                            task.Increment(1);
                            Thread.Sleep(100); // Small delay to avoid hitting API rate limits
                        }
                    });

            }

        });

}