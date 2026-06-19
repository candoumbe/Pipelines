using System;
using System.Threading.Tasks;
using static Fallout.Common.Tools.Git.GitTasks;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// A lightweight version of <see cref="IGitFlow"/> where only a main branch exists.
/// </summary>
public interface IGitHubFlow : IDoHotfixWorkflow, IDoFeatureWorkflow, IDoChoreWorkflow
{
    ///<inheritdoc/>
    string IDoFeatureWorkflow.FeatureBranchSourceName => MainBranchName;

    ///<inheritdoc/>
    string IDoHotfixWorkflow.HotfixBranchSourceName => MainBranchName;

    /// <inheritdoc />
    string IDoChoreWorkflow.ChoreBranchSourceName => MainBranchName;

    /// <inheritdoc />
    ValueTask IDoFeatureWorkflow.FinishFeature()
    {
        string sourceBranch = GitRepository.Branch;
        if (sourceBranch == MainBranchName)
        {
            throw new InvalidOperationException($"Cannot finalize branch '{sourceBranch}' into '{MainBranchName}'. Switch to a non-main branch first.");
        }

        CheckoutAndUpdateMain();
        MergeNoFastForward(sourceBranch, MainBranchName);
        ExecuteGit($"branch -D {sourceBranch}", $"Unable to delete local branch '{sourceBranch}'.");
        ExecuteGit($"push origin {MainBranchName}", $"Unable to push branch '{MainBranchName}' to origin.");

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    ValueTask IDoHotfixWorkflow.FinishHotfix() => FinishFeature();

    /// <inheritdoc />
    ValueTask IDoChoreWorkflow.FinishChore() => FinishFeature();

    private void CheckoutAndUpdateMain()
    {
        ExecuteGit($"checkout {MainBranchName}", $"Unable to checkout branch '{MainBranchName}'.");
        ExecuteGit("pull", $"Unable to update branch '{MainBranchName}' from remote.");
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
}