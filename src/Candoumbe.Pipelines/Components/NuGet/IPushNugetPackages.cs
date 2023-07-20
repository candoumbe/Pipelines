using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.NuGet;

/// <summary>
/// Marks a pipeline that can push NuGet packages to various repositories
/// </summary>
/// <remarks>
/// By default, this component publishes nuget packages specified by <see cref="PublishPackageFiles"/> to repositories defined by <see cref="PublishConfigurations"/>.
/// This behavior can be overriden by explicitely specifying the <see cref="ConfigName"/>.
/// </remarks>
public interface IPushNugetPackages : IPack
{
    /// <summary>
    /// Defines which files should be pushed when calling <see cref="Publish"/> target
    /// </summary>
    IEnumerable<AbsolutePath> PublishPackageFiles => PackagesDirectory.GlobFiles("*.nupkg", "*.snupkg");

    /// <summary>
    /// Explicitely
    /// </summary>
    [Parameter("Defines the name of the configuration to use to publish packages.")]
    public string ConfigName => TryGetValue(() => ConfigName)?.Trim();

    /// <summary>
    /// Publish all <see cref="PublishPackageFiles"/> to <see cref="PublishConfigurations"/>.
    /// </summary>
    public Target Publish => _ => _
        .Description($"Published packages (*.nupkg and *.snupkg) to the destination server set using either {nameof(PublishConfigurations)} settings or the configuration {nameof(ConfigName)} configuration ")
        .Consumes(Pack, ArtifactsDirectory / "*.nupkg", ArtifactsDirectory / "*.snupkg")
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => (!string.IsNullOrWhiteSpace(ConfigName) && PublishConfigurations.Once(config => config.Name == ConfigName))
                                || PublishConfigurations.AtLeastOnce(config => config.CanBeUsed()))
        .WhenNotNull(this.As<IHaveGitRepository>(),
                     (_, repository) => _.Requires(() => GitHasCleanWorkingCopy())
                                         .OnlyWhenDynamic(() => IsLocalBuild
                                                                || repository.GitRepository.IsOnMainOrMasterBranch()
                                                                || repository.GitRepository.IsOnReleaseBranch()
                                                                || repository.GitRepository.IsOnHotfixBranch()))
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            int numberOfPackages = PublishPackageFiles.TryGetNonEnumeratedCount(out numberOfPackages)
                ? numberOfPackages
                : PublishPackageFiles.Count();
            DotNetNuGetPushSettings pushSettings = new();
            pushSettings.Apply(PublishSettingsBase)
                        .Apply(PublishSettings);

            if (string.IsNullOrWhiteSpace(ConfigName))
            {
                PublishConfigurations.ForEach(config =>
                {
                    if (config.CanBeUsed())
                    {
                        Information("{PackageCount} package(s) will be published to {SourceName}({SourceUrl})", numberOfPackages, config.Name, config.Source);
                    }
                    else
                    {
                        Warning("{PackageCount} package(s) will not be published to {SourceName}({SourceUrl})", numberOfPackages, config.Name, config.Source);
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
                                                                                                                      .SetSource(config.Source))),
                                                      degreeOfParallelism: PushDegreeOfParallelism,
                                                      completeOnFailure: PushCompleteOnFailure);
            }
            else
            {
                PushNugetPackageConfiguration publishConfiguration = PublishConfigurations.Single(config => string.Equals(config.Name, ConfigName, System.StringComparison.OrdinalIgnoreCase));
                DotNetNuGetPush(s => s.Apply(PublishSettingsBase)
                                      .Apply(PublishSettings)
                                      .When(publishConfiguration.CanBeUsed(),
                                            _ => _.SetApiKey(publishConfiguration.Key)
                                                  .SetSource(publishConfiguration.Source))
                                      .CombineWith(PublishPackageFiles,
                                                   (_, file) => _.SetTargetPath(file))
                                                                 .Apply(PackagePublishSettings),
                                degreeOfParallelism: PushDegreeOfParallelism,
                                completeOnFailure: PushCompleteOnFailure);
            }

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
