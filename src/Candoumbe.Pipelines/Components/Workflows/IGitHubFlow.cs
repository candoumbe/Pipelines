namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// A lightweight version of <see cref="IGitFlow"/> where only a main branch exists.
/// </summary>
public interface IGitHubFlow : IDoHotfixWorkflow, IDoFeatureWorkflow
{
    ///<inheritdoc/>
    string IDoFeatureWorkflow.FeatureBranchSourceName => MainBranchName;

    ///<inheritdoc/>
    string IDoHotfixWorkflow.HotfixBranchSourceName => MainBranchName;
}