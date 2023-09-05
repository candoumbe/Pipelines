using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Represents an interface for feature branch workflows in a Git repository.
/// </summary>
/// <remarks>
/// This interface extends the <see cref="IWorkflow"/> interface and adds properties and methods specific to feature branch workflows.
/// </remarks>
public interface IDoFeatureWorkflow : IWorkflow
{
    /// <summary>
    /// Gets the prefix used to name feature branches.
    /// </summary>
    public string FeatureBranchPrefix => "feature";

    /// <summary>
    /// Gets the name of the branch to use when starting a new feature.
    /// </summary>
    /// <remarks>
    /// This property should never return <see langword="null"/>.
    /// </remarks>
    string FeatureBranchSourceName { get; }

    /// <summary>
    /// Merges a feature branch back to <see cref="FeatureBranchSourceName"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask FinishFeature() => ValueTask.CompletedTask;

    /// <summary>
    /// Starts a new feature development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {FeatureBranchSourceName}.
    /// </summary>
    /// <remarks>
    /// This target will instead end a feature if the current branch is a feature/* branch with no pending changes.
    /// </remarks>
    /// <returns>A <see cref="Target"/> representing the feature development.</returns>
    public Target Feature => _ => _
       .Description($"Starts a new feature development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {FeatureBranchSourceName}")
       .Requires(() => IsLocalBuild)
       .Requires(() => !GitRepository.IsOnFeatureBranch() || GitHasCleanWorkingCopy())
       .Executes(async () =>
       {
           if (!GitRepository.IsOnFeatureBranch())
           {
               Information("Enter the name of the feature. It will be used as the name of the feature/branch (leave empty to exit) :");
               AskBranchNameAndSwitchToIt(FeatureBranchPrefix, sourceBranch: FeatureBranchSourceName);
               Information("Good bye !");
           }
           else
           {
               await FinishFeature();
           }
       });
}