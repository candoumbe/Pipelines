using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;

using System;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can compile a <see cref="IHaveSolution.Solution"/>
/// </summary>
public interface ICompile : IRestore, IHaveConfiguration
{
    /// <summary>
    /// Compiles the specified
    /// </summary>
    public Target Compile => _ => _
        .Description($"Compiles {Solution}")
        .DependsOn<IRestore>()
        .Executes(() =>
        {
            Information("Compiling {Solution}", Solution);

            ReportSummary(_ => _.When(this is IHaveGitVersion, _ => _.AddPair("Version", ((IHaveGitVersion)this).GitVersion.FullSemVer)));

            DotNetBuild(s => s
                .Apply(CompileSettingsBase)
                .Apply(CompileSettings)
            );
        });

    /// <summary>
    /// Default compilation settings
    /// </summary>
    public sealed Configure<DotNetBuildSettings> CompileSettingsBase => _ => _
                .SetNoRestore(SucceededTargets.Contains(Restore) || SkippedTargets.Contains(Restore))
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetContinuousIntegrationBuild(IsServerBuild)
                .When(this is IHaveGitVersion, settings => settings
                    .SetAssemblyVersion(((IHaveGitVersion)this).GitVersion.AssemblySemVer)
                    .SetFileVersion(((IHaveGitVersion)this).GitVersion.AssemblySemFileVer)
                    .SetInformationalVersion(((IHaveGitVersion)this).GitVersion.InformationalVersion)
                    .SetVersion(((IHaveGitVersion)this).GitVersion.NuGetVersion)
                    .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                    .When(this is IHaveChangeLog, setting => setting.SetPackageReleaseNotes(((IHaveChangeLog)this).ReleaseNotes))
                );

    /// <summary>
    /// Configures the compilation settings
    /// </summary>
    public Configure<DotNetBuildSettings> CompileSettings => _ => _;

}
