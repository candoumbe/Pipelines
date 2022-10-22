using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;

using System.Collections.Generic;
using System.Linq;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Contract to extend when the build can build nuget packages
/// </summary>
public interface IPack : IHaveArtifacts, ICompile
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
        .TryDependsOn<IMutationTests>(x => x.MutationTests)
        .DependsOn(Compile)
        .Consumes(Compile)
        .Produces(PackagesDirectory / "*.nupkg")
        .Produces(PackagesDirectory / "*.snupkg")
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

    public sealed Configure<DotNetPackSettings> PackSettingsBase => _ => _
        .EnableIncludeSource()
        .EnableIncludeSymbols()
        .SetOutputDirectory(PackagesDirectory)
        .SetNoBuild(SucceededTargets.Contains(Compile))
        .SetNoRestore(SucceededTargets.Contains(Restore) || SucceededTargets.Contains(Compile))
        .SetConfiguration(Configuration)
        .When(this is IHaveGitVersion, _ => _
            .SetAssemblyVersion(((IHaveGitVersion)this).GitVersion.AssemblySemVer)
            .SetFileVersion(((IHaveGitVersion)this).GitVersion.AssemblySemFileVer)
            .SetInformationalVersion(((IHaveGitVersion)this).GitVersion.InformationalVersion)
            .SetVersion(((IHaveGitVersion)this).GitVersion.NuGetVersion))
        .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
        .When(this is IHaveChangeLog, _ => _.SetPackageReleaseNotes(((IHaveChangeLog)this).ReleaseNotes))
        .When(this is IHaveGitRepository, _ => _.SetRepositoryType("git")
                                                .SetRepositoryUrl(((IHaveGitRepository)this).GitRepository.HttpsUrl));
}
