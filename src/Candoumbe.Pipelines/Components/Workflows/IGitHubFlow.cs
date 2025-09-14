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
}