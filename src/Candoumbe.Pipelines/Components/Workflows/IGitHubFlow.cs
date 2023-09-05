namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// A lightweight version of <see cref="IGitFlow"/> where only a main branch exists.
/// </summary>
public interface IGitHubFlow : IWorkflow, IHaveHotfixWorkflow
{
    ///<inheritdoc/>
    string IWorkflow.FeatureBranchSourceName => MainBranchName;

    ///<inheritdoc/>
    string IHaveHotfixWorkflow.HotfixBranchSourceName => MainBranchName;
}