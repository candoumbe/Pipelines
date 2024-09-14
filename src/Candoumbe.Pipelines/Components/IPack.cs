using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Contract to extend when the build can build nuget packages
/// </summary>
public interface IPack : IHaveArtifacts, IHaveConfiguration
{
    /// <summary>
    /// Directory that will contains packages
    /// </summary>
    public AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    /// <summary>
    /// Indicates which projects should be packed
    /// </summary>
    IEnumerable<AbsolutePath> PackableProjects { get; }

    /// <summary>
    /// Builds nuget packages
    /// </summary>
    public Target Pack => _ => _
        .TryDependsOn<IUnitTest>(x => x.UnitTests)
        .TryDependsOn<ICompile>()
        .Produces(PackagesDirectory / "*.nupkg",
                  PackagesDirectory / "*.snupkg")
        .Executes(() =>
        {
            int packageCount = PackableProjects.TryGetNonEnumeratedCount(out int count)
                ? count
                : PackableProjects.Count();

            DotNetPack(s => s
                .Apply(PackSettingsBase)
                .Apply(PackSettings)
                .CombineWith(PackableProjects, (setting, csproj) => setting.SetProject(csproj)));

            ReportSummary(summary => summary.AddPair("Packages", packageCount));
        });

    /// <summary>
    /// Configures <see cref="Pack"/> execution
    /// </summary>
    public Configure<DotNetPackSettings> PackSettings => _ => _;

    internal sealed Configure<DotNetPackSettings> PackSettingsBase => _ => _
        .EnableIncludeSource()
        .EnableIncludeSymbols()
        .SetOutputDirectory(PackagesDirectory)
        .WhenNotNull(this.As<ICompile>(), (_, compile) => _.SetNoBuild(SucceededTargets.Contains(compile.Compile) || SkippedTargets.Contains(compile.Compile)))
        .WhenNotNull(this.As<IRestore>(), (_, restore) => _.SetNoRestore(SucceededTargets.Contains(restore.Restore) || SucceededTargets.Contains(restore.Restore)))
        .SetConfiguration(Configuration)
        .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
        .WhenNotNull(this.As<IHaveGitVersion>(),
                     (_, versioning) => _.SetAssemblyVersion(versioning.GitVersion.AssemblySemVer)
                                        .SetFileVersion(versioning.GitVersion.AssemblySemFileVer)
                                        .SetInformationalVersion(versioning.GitVersion.InformationalVersion)
                                        .SetVersion(versioning.GitVersion.SemVer)
                                    )
        .WhenNotNull(this.As<IHaveChangeLog>(), (_, changelog) => _.SetPackageReleaseNotes(changelog.ReleaseNotes))
        .WhenNotNull(this.As<IHaveGitRepository>(), (_, repository) => _.SetRepositoryType("git")
                                                                     .SetRepositoryUrl(repository.GitRepository.HttpsUrl));
}
