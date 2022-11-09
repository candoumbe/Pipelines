namespace Candoumbe.Pipelines.Build;

using Components;
using Components.GitHub;

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

using System;
using System.Collections.Generic;

[GitHubActions("integration",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = false,
    OnPushBranchesIgnore = new[] { IGitFlow.MainBranchName, IGitFlow.ReleaseBranch + "/*" },
    FetchDepth = 0,
    InvokedTargets = new[] { nameof(ICompile.Compile), nameof(IPack.Pack), nameof(IPublish.Publish) },
    CacheKeyFiles = new[] { "global.json", "src/**/*.csproj" },
    EnableGitHubToken = true,
    ImportSecrets = new[]
    {
        nameof(NugetApiKey)
    },
    PublishArtifacts = true,
    OnPushExcludePaths = new[]
        {
            "docs/*",
            "README.md",
            "CHANGELOG.md",
            "LICENSE"
        })]
[GitHubActions("delivery",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = false,
    OnPushBranches = new[] { IGitFlow.MainBranchName, IGitFlow.ReleaseBranch + "/*" },
    FetchDepth = 0,
    InvokedTargets = new[] { nameof(ICompile.Compile), nameof(IPack.Pack), nameof(IPublish.Publish) },
    CacheKeyFiles = new[] { "global.json", "src/**/*.csproj" },
    EnableGitHubToken = true,
    ImportSecrets = new[]
    {
        nameof(NugetApiKey)
    },
    PublishArtifacts = true,
    OnPushExcludePaths = new[]
        {
            "docs/*",
            "README.md",
            "CHANGELOG.md",
            "LICENSE"
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
    IHaveGitHubRepository,
    IHaveArtifacts,
    IPublish,
    ICreateGithubRelease,
    IGitFlowWithPullRequest,
    IHaveSecret
{
    ///<inheritdoc/>
    IEnumerable<AbsolutePath> IClean.DirectoriesToDelete => RootDirectory.GlobDirectories("**/bin", "**/obj");

    ///<inheritdoc/>
    IEnumerable<AbsolutePath> IClean.DirectoriesToEnsureExistance => new[]
    {
        this.Get<IHaveArtifacts>().OutputDirectory,
        this.Get<IHaveArtifacts>().ArtifactsDirectory,
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

    /// <summary>
    /// Token used to interact with GitHub API
    /// </summary>
    [Parameter("Token used to interact with Nuget API")]
    [Secret]
    public readonly string NugetApiKey;


    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Pipeline>(x => ((ICompile)x).Compile);

    ///<inheritdoc/>
    public IEnumerable<AbsolutePath> PackableProjects => SourceDirectory.GlobFiles("**/*.csproj");


    ///<inheritdoc/>
    public IEnumerable<PublishConfiguration> PublishConfigurations => new PublishConfiguration[]
    {
        new NugetPublishConfiguration(
            apiKey: NugetApiKey,
            source: new Uri("https://api.nuget.org/v3/index.json"),
            canBeUsed: () => NugetApiKey is not null
        ),
        new GitHubPublishConfiguration(
            githubToken: this.Get<ICreateGithubRelease>()?.GitHubToken,
            source: new Uri($"https://nuget.pkg.github.com/{GitHubActions?.RepositoryOwner}/index.json"),
            canBeUsed: () => this is ICreateGithubRelease createRelease && createRelease.GitHubToken is not null
        ),
    };
}
