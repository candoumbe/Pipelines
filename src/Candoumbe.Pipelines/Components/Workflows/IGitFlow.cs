using System.Threading.Tasks;
using Candoumbe.Pipelines.Components.Workflows;
using Nuke.Common;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a build as supporting the <see href="https://datasift.github.io/gitflow/IntroducingGitFlow.html">git flow</see> workflow.
/// </summary>
/// <remarks>
/// This interface will provide several ready-to-use targets to effectively manage this workflow even when the underlying "git" command does not support gitflow verbs.
/// </remarks>
public interface IGitFlow : IDoHotfixWorkflow, IDoColdfixWorkflow, IDoChoreWorkflow, IHaveDevelopBranch
{
    /// <summary>
    /// Name of the release branch
    /// </summary>
    public const string ReleaseBranch = "release";

    /// <summary>
    /// Prefix used to name release branches
    /// </summary>
    public string ReleaseBranchPrefix => "release";

    ///<inheritdoc/>
    string IDoFeatureWorkflow.FeatureBranchSourceName => DevelopBranchName;

    /// <inheritdoc />
    string IDoChoreWorkflow.ChoreBranchSourceName => FeatureBranchSourceName;

    ///<inheritdoc/>
    string IDoHotfixWorkflow.HotfixBranchSourceName => MainBranchName;

    /// <summary>
    /// Creates a new release branch from the develop branch.
    /// </summary>
    /// <remarks>
    /// A major release can be created by setting <see cref="IHaveGitVersion.Major"/> to <see langword="true"/>
    /// </remarks>
    public Target Release => _ => _
        .DependsOn(Changelog)
        .Description($$"""
                       Starts a new {{ReleaseBranchPrefix}}/{version} from {{DevelopBranchName}} if not currently on {{ReleaseBranchPrefix}}/{version}.
                       When already on {{ReleaseBranchPrefix}}/{version}, merges back either to {{MainBranchName}} or {{DevelopBranchName}} ")
                       """)
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
    /// Merges a <see cref="IDoColdfixWorkflow.ColdfixBranchPrefix"/> back to <see cref="IHaveDevelopBranch.DevelopBranchName"/> branch
    /// </summary>
    async ValueTask IDoColdfixWorkflow.FinishColdfix() => await FinishFeature();

    /// <summary>
    /// Merges the current hotfix branch back to <see cref="IHaveMainBranch.MainBranchName"/>.
    /// </summary>
    ValueTask IDoHotfixWorkflow.FinishHotfix()
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
    ValueTask IDoFeatureWorkflow.FinishFeature()
    {
        Git($"rebase {DevelopBranchName}");
        Git($"checkout {DevelopBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        Git($"branch -D {GitRepository.Branch}");
        Git($"push origin {DevelopBranchName}");

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Merges the current feature branch back to <see cref="IHaveDevelopBranch.DevelopBranchName"/>.
    /// </summary>
    ValueTask IDoChoreWorkflow.FinishChore() => FinishFeature();
}