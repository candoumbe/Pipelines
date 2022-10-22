namespace Candoumbe.Pipelines.Build;

using Components;

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

using System.Collections.Generic;

[GitHubActions("integration",
    GitHubActionsImage.UbuntuLatest,
    OnPushBranchesIgnore = new[] { IGitFlow.MainBranchName },
    FetchDepth = 0,
    InvokedTargets = new[] { nameof(ICompile.Compile), nameof(IPack.Pack) },
    ImportSecrets = new[]
    {
        nameof(ICreateGithubRelease.GitHubToken),
    })]
[DotNetVerbosityMapping]
[HandleVisualStudioDebugging]
[ShutdownDotNetAfterServerBuild]
public class Pipeline : NukeBuild,
    IHaveSourceDirectory,
    IHaveSolution,
    IHaveChangeLog,
    IClean,
    ICompile,
    IPack,
    IHaveGitVersion,
    IHaveGitRepository,
    IHaveArtifacts,
    IGitFlow
{
    ///<inheritdoc/>
    IEnumerable<AbsolutePath> IClean.DirectoriesToDelete => SourceDirectory.GlobDirectories("**/bin", "**/obj");

    ///<inheritdoc/>
    IEnumerable<AbsolutePath> IClean.DirectoriesToEnsureExistance => new[]
    {
        From<IHaveArtifacts>().OutputDirectory,
        From<IHaveArtifacts>().ArtifactsDirectory,
    };

    [CI]
    public GitHubActions GitHubActions;

    [Required]
    [Solution]
    public Solution Solution;

    ///<inheritdoc/>
    Solution IHaveSolution.Solution => Solution;

    ///<inheritdoc/>
    public AbsolutePath SourceDirectory => RootDirectory / "src";

    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Pipeline>(x => ((ICompile)x).Compile);

    ///<inheritdoc/>
    public IEnumerable<AbsolutePath> PackableProjects => SourceDirectory.GlobFiles("**/*.csproj");

    private T From<T>() where T : INukeBuild
        => (T)(object)this;


}
