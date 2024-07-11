using System;
using System.Threading.Tasks;
using Nuke.Common;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Represents an interface for coldfix branch workflows in a Git repository.
/// </summary>
/// <remarks>
/// This interface extends the <see cref="IWorkflow"/> interface and adds properties and methods specific to coldfix branch workflows.
/// </remarks>
public interface IDoColdfixWorkflow : IDoFeatureWorkflow
{
    /// <summary>
    /// Gets the prefix used to name coldfix branches.
    /// </summary>
    public string ColdfixBranchPrefix => "coldfix";

    /// <summary>
    /// Defines the name of the branch where a "coldfix/*" branch should be merged back to (once finished).
    /// </summary>
    /// <remarks>
    /// This property should never return <see langword="null"/>.
    /// </remarks>
    string ColdfixBranchSourceName => FeatureBranchSourceName;

    /// <summary>
    /// Merges a feature branch back to <see cref="ColdfixBranchSourceName"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask FinishColdfix() => ValueTask.CompletedTask;

    /// <summary>
    /// Creates a new coldfix branch from the develop branch.
    /// </summary>
    public Target Coldfix => _ => _
        .Description($"Starts a new coldfix development by creating the associated '{ColdfixBranchPrefix}/{{name}}' from {ColdfixBranchSourceName}")
        .Requires(() => IsLocalBuild)
        .Requires(() => !GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*", true) || GitHasCleanWorkingCopy())
        .Executes(async () =>
        {
            if (!GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*"))
            {
                AskBranchNameAndSwitchToIt(ColdfixBranchPrefix, ColdfixBranchSourceName);
                Information($"{EnvironmentInfo.NewLine}Good bye !");
            }
            else
            {
                await FinishColdfix();
            }
        });
}