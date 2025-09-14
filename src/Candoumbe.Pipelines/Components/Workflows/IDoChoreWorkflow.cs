using System;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Represents an interface for chore branch workflows in a Git repository.
/// </summary>
/// <remarks>
/// This interface extends the <see cref="IWorkflow"/> interface and adds properties and methods specific to feature branch workflows.
/// </remarks>
public interface IDoChoreWorkflow : IWorkflow
{
    /// <summary>
    /// Gets the prefix used to name feature branches.
    /// </summary>
    [Parameter("The name of the chore branch prefix")]
    public string ChoreBranchPrefix => TryGetValue(() => ChoreBranchPrefix) ?? "chore";

    /// <summary>
    /// Gets the name of the branch to use when starting a new feature.
    /// </summary>
    /// <remarks>
    /// This property should never return <see langword="null"/>.
    /// </remarks>
    string ChoreBranchSourceName { get; }

    /// <summary>
    /// Merges a chore branch back to <see cref="ChoreBranchSourceName"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask FinishChore() => ValueTask.CompletedTask;

    /// <summary>
    /// Starts a new chore development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {FeatureBranchSourceName}.
    /// </summary>
    /// <remarks>
    /// This target will instead end a feature if the current branch is a feature/* branch with no pending changes.
    /// </remarks>
    /// <returns>A <see cref="Target"/> representing the feature development.</returns>
    public Target Chore => _ => _
       .Description($"Starts a new chore development by creating the associated branch {ChoreBranchPrefix}/{{chore-name}} from {ChoreBranchSourceName}")
       .Requires(() => IsLocalBuild)
       .Requires(() => !GitRepository.IsOnFeatureBranch() || GitHasCleanWorkingCopy())
       .Executes(async () =>
       {
           if (!GitRepository.Branch.Like($"{ChoreBranchPrefix}/*", true) || !GitHasCleanWorkingCopy())
           {
               Information("Enter the name of the chore. It will be used as the name of the chore/branch (leave empty to exit) :");
               AskBranchNameAndSwitchToIt(ChoreBranchPrefix, sourceBranch: ChoreBranchSourceName);
               Information("Good bye !");
           }
           else
           {
               await FinishChore();
           }
       });
}