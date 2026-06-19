using System;
using System.Threading.Tasks;
using Candoumbe.Pipelines.Components.Workflows;
using Fallout.Common;
using Fallout.Common.Git;
using static Fallout.Common.Tools.Git.GitTasks;
using static Serilog.Log;

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
        string sourceBranch = GitRepository.Branch;
        if (!GitRepository.IsOnHotfixBranch() && !GitRepository.IsOnReleaseBranch())
        {
            throw new InvalidOperationException($"Cannot finalize branch '{sourceBranch}'. Expected a '{HotfixBranchPrefix}/*' or '{ReleaseBranchPrefix}/*' branch.");
        }

        bool mergedMain = false;
        bool tagged = false;

        try
        {
            CheckoutAndUpdate(MainBranchName);
            MergeNoFastForward(sourceBranch, MainBranchName);
            mergedMain = true;

            ExecuteGit($"tag {MajorMinorPatchVersion}", $"Unable to tag '{MainBranchName}' with '{MajorMinorPatchVersion}'.");
            tagged = true;

            CheckoutAndUpdate(DevelopBranchName);
            MergeNoFastForward(sourceBranch, DevelopBranchName);

            ExecuteGit($"branch -D {sourceBranch}", $"Unable to delete local branch '{sourceBranch}'.");
            ExecuteGit($"push origin --follow-tags {MainBranchName} {DevelopBranchName} {MajorMinorPatchVersion}",
                $"Unable to push branches '{MainBranchName}', '{DevelopBranchName}' and tag '{MajorMinorPatchVersion}' to origin.");
        }
        catch (Exception ex)
        {
            if (mergedMain)
            {
                TryRollbackMainBranch(MajorMinorPatchVersion, tagged);
            }

            throw new InvalidOperationException(
                $"Failed to finalize '{sourceBranch}'. Resolve merge conflicts and retry once the repository is clean.",
                ex);
        }

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
        string sourceBranch = GitRepository.Branch;
        if (!GitRepository.IsOnFeatureBranch() && !sourceBranch.Like($"{ColdfixBranchPrefix}/*", true) && !sourceBranch.Like($"{ChoreBranchPrefix}/*", true))
        {
            throw new InvalidOperationException($"Cannot finalize branch '{sourceBranch}'. Expected '{FeatureBranchPrefix}/*', '{ColdfixBranchPrefix}/*' or '{ChoreBranchPrefix}/*'.");
        }

        RebaseOntoDevelop(sourceBranch);
        CheckoutAndUpdate(DevelopBranchName);
        MergeNoFastForward(sourceBranch, DevelopBranchName);

        ExecuteGit($"branch -D {sourceBranch}", $"Unable to delete local branch '{sourceBranch}'.");
        ExecuteGit($"push origin {DevelopBranchName}", $"Unable to push branch '{DevelopBranchName}' to origin.");

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Merges the current feature branch back to <see cref="IHaveDevelopBranch.DevelopBranchName"/>.
    /// </summary>
    ValueTask IDoChoreWorkflow.FinishChore() => FinishFeature();

    private static void ExecuteGit(string command, string errorMessage)
    {
        try
        {
            Git(command);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"{errorMessage} (git {command})", ex);
        }
    }

    private void CheckoutAndUpdate(string branchName)
    {
        ExecuteGit($"checkout {branchName}", $"Unable to checkout branch '{branchName}'.");
        ExecuteGit("pull", $"Unable to update branch '{branchName}' from remote.");
    }

    private static void MergeNoFastForward(string sourceBranch, string targetBranch)
    {
        try
        {
            Git($"merge --no-ff --no-edit {sourceBranch}");
        }
        catch (Exception ex)
        {
            TryMergeAbort();
            throw new InvalidOperationException(
                $"Merge conflict while merging '{sourceBranch}' into '{targetBranch}'. Resolve conflicts and retry.",
                ex);
        }
    }

    private void RebaseOntoDevelop(string sourceBranch)
    {
        try
        {
            Git($"rebase {DevelopBranchName}");
        }
        catch (Exception ex)
        {
            TryRebaseAbort();
            throw new InvalidOperationException(
                $"Rebase conflict while rebasing '{sourceBranch}' onto '{DevelopBranchName}'. Resolve conflicts and retry.",
                ex);
        }
    }

    private void TryRollbackMainBranch(string releaseTag, bool tagExists)
    {
        Warning("Rolling back merge on '{BranchName}' after finalization failure.", MainBranchName);
        TryMergeAbort();
        TryRebaseAbort();

        try
        {
            Git($"checkout {MainBranchName}");
            Git("reset --hard ORIG_HEAD");
        }
        catch (Exception rollbackException)
        {
            Warning(rollbackException,
                "Unable to rollback branch '{BranchName}' automatically. Manual cleanup may be required.",
                MainBranchName);
        }

        if (tagExists)
        {
            try
            {
                Git($"tag -d {releaseTag}");
            }
            catch (Exception deleteTagException)
            {
                Warning(deleteTagException, "Unable to delete local tag '{TagName}' during rollback.", releaseTag);
            }
        }
    }

    private static void TryMergeAbort()
    {
        try
        {
            Git("merge --abort");
        }
        catch
        {
            // Nothing to abort.
        }
    }

    private static void TryRebaseAbort()
    {
        try
        {
            Git("rebase --abort");
        }
        catch
        {
            // Nothing to abort.
        }
    }
}