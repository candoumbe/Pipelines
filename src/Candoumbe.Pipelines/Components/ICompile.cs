using System;
using Candoumbe.Pipelines.Components.Formatting;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can compile a <see cref="IHaveSolution.Solution"/>
/// </summary>
public interface ICompile : IHaveSolution, IHaveConfiguration
{
    /// <summary>
    /// Compiles the specified
    /// </summary>
    public Target Compile => _ => _
        .Description($"Compiles {Solution}")
        .TryDependsOn<IRestore>()
        .TryDependsOn<IFormat>()
        .Executes(() =>
        {
            Information("Compiling {Solution}", Solution);

            ReportSummary(_ => _.WhenNotNull(this.As<IHaveGitVersion>(),
                                             (_, version) => _.AddPair("Version", version.GitVersion.FullSemVer)));

            DotNetBuild(s => s
                .Apply(CompileSettingsBase)
                .Apply(CompileSettings)
            );
        });

    /// <summary>
    /// Default compilation settings
    /// </summary>
    public sealed Configure<DotNetBuildSettings> CompileSettingsBase => _ => _
                .WhenNotNull(this.As<IRestore>(),
                             (_, restore) => _.SetNoRestore(SucceededTargets.Contains(restore.Restore) || SkippedTargets.Contains(restore.Restore)))
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetContinuousIntegrationBuild(IsServerBuild)
                .SetProcessArgumentConfigurator(args => args.Add("--tl"))
                .WhenNotNull(this.As<IHaveGitVersion>(),
                             (settings, gitVersion) => settings.SetAssemblyVersion(gitVersion.GitVersion.AssemblySemVer)
                                                               .SetFileVersion(gitVersion.GitVersion.AssemblySemFileVer)
                                                               .SetInformationalVersion(gitVersion.GitVersion.InformationalVersion)
                                                               .SetVersion(gitVersion.GitVersion.NuGetVersion)
                                                               .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                                                               .WhenNotNull(this as IHaveChangeLog,
                                                                            (setting, changelog) => setting.SetPackageReleaseNotes(changelog.ReleaseNotes))
                );

    /// <summary>
    /// Configures the compilation settings
    /// </summary>
    public Configure<DotNetBuildSettings> CompileSettings => _ => _;
}
