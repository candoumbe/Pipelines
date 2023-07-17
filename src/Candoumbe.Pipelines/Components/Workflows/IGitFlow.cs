using Candoumbe.Pipelines.Components.Workflows;

using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;

using System;
using System.Threading.Tasks;

using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a build as supporting the <see href="https://datasift.github.io/gitflow/IntroducingGitFlow.html">git flow</see> workflow.
/// </summary>
/// <remarks>
/// This interface will provide several ready to use targets to effectively manages this workflow even when the underlying "git" command does not support the gitflow verbs.
/// </remarks>
public interface IGitFlow : IWorkflow, IHaveDevelopBranch
{
    /// <summary>
    /// Name of the release branch
    /// </summary>
    public const string ReleaseBranch = "release";

    /// <summary>
    /// Prefix used to name release branches
    /// </summary>
    public string ReleaseBranchPrefix => "release";

    /// <summary>
    /// Defines the name of the branch where a "coldfix/*" branch should be merged back to (once finished).
    /// </summary>
    /// <remarks>
    /// This property should never return <see langword="null"/>.
    /// </remarks>
    string ColdfixBranchSourceName => FeatureBranchSourceName;

    ///<inheritdoc/>
    string IWorkflow.FeatureBranchSourceName => DevelopBranchName;

    ///<inheritdoc/>
    string IWorkflow.HotfixBranchSourceName => MainBranchName;

    /// <summary>
    /// Creates a new release branch from the develop branch.
    /// </summary>
    /// <remarks>
    /// A major release can be created by setting <see cref="IHaveGitVersion.Major"/> to <see langword="true"/>
    /// </remarks>
    public Target Release => _ => _
        .DependsOn(Changelog)
        .Description($"Starts a new {ReleaseBranchPrefix}/{{version}} from {DevelopBranchName} if not currently on {ReleaseBranchPrefix}/{{version}}.\n" +
                     $"When already on {ReleaseBranchPrefix}/{{version}}, merges back either to {MainBranchName} or {DevelopBranchName} ")
        .Requires(() => IsLocalBuild)
        .Requires(() => !GitRepository.IsOnReleaseBranch() || GitHasCleanWorkingCopy())
        .Executes(async () =>
        {

            if (!GitRepository.IsOnReleaseBranch())
            {
                string majorMinorPatchVersion = Major
                    ? $"{GitVersion.Major + 1}.0.0"
                    : GitVersion.MajorMinorPatch;

                Checkout($"{ReleaseBranchPrefix}/{majorMinorPatchVersion}", start: DevelopBranchName);
            }
            else
            {
                await FinishRelease();
            }
        });

    /// <summary>
    /// Creates a new coldfix branch from the develop branch.
    /// </summary>
    public Target Coldfix => _ => _
        .Description($"Starts a new coldfix development by creating the associated '{ColdfixBranchPrefix}/{{name}}' from {DevelopBranchName}")
        .Requires(() => IsLocalBuild)
        .Requires(() => !GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*", true) || GitHasCleanWorkingCopy())
        .Executes(async () =>
        {
            if (!GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*"))
            {
                Information("Enter the name of the coldfix. It will be used as the name of the coldfix/branch (leave empty to exit) :");
                AskBranchNameAndSwitchToIt(ColdfixBranchPrefix, DevelopBranchName);
                Information($"{EnvironmentInfo.NewLine}Good bye !");
            }
            else
            {
                await FinishColdfix();
            }
        });

    /// <summary>
    /// Merges a <see cref="IWorkflow.ColdfixBranchPrefix"/> back to <see cref="IHaveDevelopBranch.DevelopBranchName"/> branch
    /// </summary>
    async virtual ValueTask FinishColdfix() => await FinishFeature();

    /// <summary>
    /// Merges the current <see cref="ReleaseBranch"/> or <see cref="IWorkflow.HotfixBranchPrefix"/> branch back to <see cref="IHaveMainBranch.MainBranchName"/>.
    /// </summary>
    ValueTask IWorkflow.FinishHotfix()
    {
        Git($"checkout {MainBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");
        Git($"tag {MajorMinorPatchVersion}");

        Git($"checkout {DevelopBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        Git($"branch -D {GitRepository.Branch}");

        Git($"push origin --follow-tags {MainBranchName} {DevelopBranchName} {MajorMinorPatchVersion}");

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Merges a release branch back to the trunk branch.
    /// </summary>
    public async ValueTask FinishRelease() => await FinishHotfix().ConfigureAwait(false);

    /// <summary>
    /// Merges the current feature branch back to <see cref="IHaveDevelopBranch.DevelopBranchName"/>.
    /// </summary>
    ValueTask IWorkflow.FinishFeature()
    {
        Git($"rebase {DevelopBranchName}");
        Git($"checkout {DevelopBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        Git($"branch -D {GitRepository.Branch}");
        Git($"push origin {DevelopBranchName}");

        return ValueTask.CompletedTask;
    }
}