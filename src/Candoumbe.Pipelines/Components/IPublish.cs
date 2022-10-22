using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;

using System.Collections.Generic;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Candoumbe.Pipelines.Components;

public interface IPublish : IPack, IUnitTest
{
    [Parameter]
    [Secret]
    string NugetApiKey => TryGetValue(() => NugetApiKey);

    string PackageSource => TryGetValue(() => PackageSource) ?? "https://api.nuget.org/v3/index.json";

    /// <summary>
    /// Defines which files should be pushed when calling <see cref="Publish"/> target
    /// </summary>
    IEnumerable<AbsolutePath> PushPackageFiles => PackagesDirectory.GlobFiles("*.nupkg", "*.snupkg");

    /// <summary>
    /// Publish all <see cref="PushPackageFiles"/> to <see cref="PackageSource"/> after authenticating using
    /// <see cref="NugetApiKey"/>.
    /// </summary>
    public Target Publish => _ => _
        .Description($"Published packages (*.nupkg and *.snupkg) to the destination server set with {nameof(PackageSource)} settings ")
        .DependsOn(UnitTests, Pack)
        .Consumes(Pack, ArtifactsDirectory / "*.nupkg", ArtifactsDirectory / "*.snupkg")
        .OnlyWhenDynamic(() => !NugetApiKey.IsNullOrEmpty())
        .Requires(() => GitHasCleanWorkingCopy())
        .When(this is IHaveGitRepository,
              _ => _.OnlyWhenDynamic(() => ((IHaveGitRepository)this).GitRepository.IsOnMainBranch()
                        || ((IHaveGitRepository)this).GitRepository.IsOnReleaseBranch()
                        || ((IHaveGitRepository)this).GitRepository.IsOnDevelopBranch()))
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            DotNetNuGetPush(s => s.Apply(PushSettingsBase)
                                  .Apply(PushSettingsBase)
                                  .CombineWith(PushPackageFiles,
                                               (_, file) => _.SetTargetPath(file))
                                                             .Apply(PackagePushSettings),
                                                completeOnFailure: PushCompleteOnFailure,
                                                degreeOfParallelism: PushDegreeOfParallelism);
        });

    public Configure<DotNetNuGetPushSettings> PushSettingsBase => _ => _
                .SetApiKey(NugetApiKey)
                .SetSource(PackageSource)
                .EnableSkipDuplicate()
                .EnableNoSymbols();

    /// <summary>
    /// Defines the settings that will be used to push packages to Nuget
    /// </summary>
    Configure<DotNetNuGetPushSettings> PushSettings => _ => _;

    /// <summary>
    /// Defines the settings that will be used to push each package
    /// </summary>
    Configure<DotNetNuGetPushSettings> PackagePushSettings => _ => _;

    /// <summary>
    /// Should the <see cref="Publish"/> target complete on failure ?
    /// </summary>
    bool PushCompleteOnFailure => true;

    /// <summary>
    /// Indicates the degree
    /// </summary>
    int PushDegreeOfParallelism => 1;
}