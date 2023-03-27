using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using System.Collections.Generic;
using System.Linq;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.NuGet;

/// <summary>
/// Marks a pipeline that can push NuGet packages to various repositories
/// </summary>
public interface IPushNugetPackages : IPack
{
    /// <summary>
    /// Defines which files should be pushed when calling <see cref="Publish"/> target
    /// </summary>
    IEnumerable<AbsolutePath> PublishPackageFiles => PackagesDirectory.GlobFiles("*.nupkg", "*.snupkg");

    /// <summary>
    /// Publish all <see cref="PublishPackageFiles"/> to <see cref="PublishConfigurations"/>.
    /// </summary>
    public Target Publish => _ => _
        .Description($"Published packages (*.nupkg and *.snupkg) to the destination server set using {nameof(PublishConfigurations)} settings ")
        .Consumes(Pack, ArtifactsDirectory / "*.nupkg", ArtifactsDirectory / "*.snupkg")
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => PublishConfigurations.AtLeastOnce(config => config.CanBeUsed()))
        .WhenNotNull(this as IHaveGitRepository,
                     (_, repository) => _.Requires(() => GitHasCleanWorkingCopy())
                                         .OnlyWhenDynamic(() => repository.GitRepository.IsOnMainBranch()
                                                                || repository.GitRepository.IsOnReleaseBranch()))
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            int numberOfPackages = PublishPackageFiles.TryGetNonEnumeratedCount(out numberOfPackages)
                ? numberOfPackages
                : PublishPackageFiles.Count();

            PublishConfigurations.ForEach(config =>
            {
                if (config.CanBeUsed())
                {
                    Information("{PackageCount} package(s) will be published to {SourceName}", numberOfPackages, config.Name);
                }
                else
                {
                    Warning("{PackageCount} package(s) will not be published to {SourceName}", numberOfPackages, config.Name);
                }
            });

            DotNetNuGetPush(s => s.Apply(PublishSettingsBase)
                                  .Apply(PublishSettings)
                                  .CombineWith(PublishPackageFiles,
                                               (_, file) => _.SetTargetPath(file))
                                                             .Apply(PackagePublishSettings)
                                                             .CombineWith(PublishConfigurations,
                                                                          (setting, config) => setting.When(config.CanBeUsed(),
                                                                                                            _ => _.SetApiKey(config.Key)
                                                                                                                  .SetSource(config.Source.ToString()))),
                                                  degreeOfParallelism: PushDegreeOfParallelism,
                                                  completeOnFailure: PushCompleteOnFailure);

            ReportSummary(summary =>
            {
                summary.Add("Packages published", numberOfPackages.ToString());
                return summary;
            });
        });

    internal Configure<DotNetNuGetPushSettings> PublishSettingsBase => _ => _
                .EnableSkipDuplicate()
                .EnableNoSymbols();

    /// <summary>
    /// Defines the settings that will be used to push packages to Nuget
    /// </summary>
    Configure<DotNetNuGetPushSettings> PublishSettings => _ => _;

    /// <summary>
    /// Defines the settings that will be used to push each package
    /// </summary>
    Configure<DotNetNuGetPushSettings> PackagePublishSettings => _ => _;

    /// <summary>
    /// Should the <see cref="Publish"/> target complete on failure ?
    /// </summary>
    bool PushCompleteOnFailure => true;

    /// <summary>
    /// Indicates the degree
    /// </summary>
    int PushDegreeOfParallelism => 1;

    /// <summary>
    /// Determines where <see cref="Publish"/> will push packages
    /// </summary>
    IEnumerable<PushNugetPackageConfiguration> PublishConfigurations { get; }
}
