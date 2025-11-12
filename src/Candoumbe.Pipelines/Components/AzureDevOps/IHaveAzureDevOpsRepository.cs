using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// Marks a pipeline for repositories that are stored on <see href="https://dev.azure.com">Azure Devops</see>.
/// </summary>
public interface IHaveAzureDevOpsRepository : IHaveGitRepository
{
    /// <summary>
    /// Token used to interact with Azure DevOps API.
    /// </summary>
    [Parameter("Token used to create a new release in Azure DevOps")]
    [Secret]
    string AccessToken => TryGetValue(() => AccessToken) ?? AzurePipelines.Instance?.AccessToken;
}