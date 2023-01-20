namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Indicates that the pipeline has a "develop" branch
/// </summary>
public interface IHaveDevelopBranch
{
    /// <summary>
    /// Name of the development branch
    /// </summary>
    const string DevelopBranchName = "develop";
}