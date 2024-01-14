namespace Candoumbe.Pipelines.Build;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Candoumbe.Pipelines.Components;
using Candoumbe.Pipelines.Components.Formatting;
using Candoumbe.Pipelines.Components.GitHub;
using Candoumbe.Pipelines.Components.NuGet;
using Candoumbe.Pipelines.Components.Workflows;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using static Nuke.Common.Tools.Git.GitTasks;

[GitHubActions("integration",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    OnPushBranchesIgnore = [IHaveMainBranch.MainBranchName],
    FetchDepth = 0,
    InvokedTargets = [nameof(ICompile.Compile), nameof(IPack.Pack), nameof(IPushNugetPackages.Publish)],
    CacheKeyFiles = ["global.json", "src/**/*.csproj"],
    EnableGitHubToken = true,
    ImportSecrets =
    [
        nameof(NugetApiKey)
    ],
    PublishArtifacts = true,
    OnPullRequestExcludePaths =
        [
            "docs/*",
            "README.md",
            "CHANGELOG.md",
            "LICENSE"
        ])]
[GitHubActions("delivery",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    OnPushBranches = [IHaveMainBranch.MainBranchName],
    FetchDepth = 0,
    InvokedTargets = [nameof(ICompile.Compile), nameof(IPack.Pack), nameof(IPushNugetPackages.Publish)],
    CacheKeyFiles = ["global.json", "src/**/*.csproj"],
    EnableGitHubToken = true,
    ImportSecrets =
    [
        nameof(NugetApiKey)
    ],
    PublishArtifacts = true,
    OnPullRequestExcludePaths =
        [
            "docs/*",
            "README.md",
            "CHANGELOG.md",
            "LICENSE"
        ])]
public class Pipeline : NukeBuild,
    IHaveSourceDirectory,
    IHaveSolution,
    IHaveChangeLog,
    IClean,
    IRestore,
    IDotnetFormat,
    ICompile,
    IPack,
    IHaveGitVersion,
    IHaveGitHubRepository,
    IHaveArtifacts,
    IPushNugetPackages,
    ICreateGithubRelease,
    IGitFlowWithPullRequest
{
    ///<inheritdoc/>
    IEnumerable<AbsolutePath> IClean.DirectoriesToDelete => this.Get<IHaveSourceDirectory>().SourceDirectory.GlobDirectories("**/*/bin", "**/*/obj");

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
    IEnumerable<AbsolutePath> IPack.PackableProjects => SourceDirectory.GlobFiles("**/*.csproj");

    ///<inheritdoc/>
    IEnumerable<AbsolutePath> ICreateGithubRelease.Assets => this.Get<IPack>().OutputDirectory.GlobFiles("**/*.nupkg;**/*.snupkg");


    ///<inheritdoc/>
    IEnumerable<PushNugetPackageConfiguration> IPushNugetPackages.PublishConfigurations => new PushNugetPackageConfiguration[]
    {
        new NugetPushConfiguration(
            apiKey: NugetApiKey,
            source: new Uri("https://api.nuget.org/v3/index.json"),
            canBeUsed: () => NugetApiKey is not null
        ),
        new GitHubPushNugetConfiguration(
            githubToken: this.Get<ICreateGithubRelease>()?.GitHubToken,
            source: new Uri($"https://nuget.pkg.github.com/{GitHubActions?.RepositoryOwner}/index.json"),
            canBeUsed: () => this is ICreateGithubRelease createRelease && createRelease.GitHubToken is not null
        ),
    };

    ///<inheritdoc/>
    ValueTask IGitFlow.FinishRelease()
    {
        Git($"checkout {IHaveMainBranch.MainBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {this.Get<IHaveGitRepository>().GitRepository.Branch}");

        string majorMinorPatchVersion = this.Get<IHaveGitVersion>().MajorMinorPatchVersion;

        Git($"tag {majorMinorPatchVersion}");

        Git($"checkout {IHaveDevelopBranch.DevelopBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {this.Get<IHaveGitRepository>().GitRepository.Branch}");

        Git($"branch -D {this.Get<IHaveGitRepository>().GitRepository.Branch}");

        Git($"push origin --follow-tags {IHaveMainBranch.MainBranchName} {IHaveDevelopBranch.DevelopBranchName} {majorMinorPatchVersion}");

        return ValueTask.CompletedTask;
    }

    ///<inheritdoc/>
    AbsolutePath IDotnetFormat.Workspace => Solution;

    ///<inheritdoc/>
    bool IDotnetFormat.VerifyNoChanges => IsServerBuild;

    ///<inheritdoc/>
    (IReadOnlyCollection<AbsolutePath> IncludedFiles, IReadOnlyCollection<AbsolutePath> ExcludedFiles) IDotnetFormat.Files
    {
        get
        {
            IReadOnlyCollection<AbsolutePath> includedFiles = Git("diff --name-status", workingDirectory: Solution.Directory)
                        .Where(output => output.Text.AsSpan().StartsWith("M") || output.Text.AsSpan().StartsWith("A"))
                        .Select(output => AbsolutePath.Create(output.Text.AsSpan()[1..].TrimStart().ToString()))
                        .ToArray();

            IReadOnlyCollection<AbsolutePath> excludedFiles = [];

            return (includedFiles, excludedFiles);
        }
    }
}
