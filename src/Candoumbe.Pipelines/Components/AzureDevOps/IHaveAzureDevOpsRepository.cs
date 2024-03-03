using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// Component for pipelines that can interact with an Azure DevOps repository
/// </summary>
public interface IHaveAzureDevOpsRepository : IHaveGitRepository, IHaveBuildServer<AzurePipelines>
{
    /// <summary>
    /// Interface with the Azure DevOps repository
    /// </summary>
    AzurePipelines IHaveBuildServer<AzurePipelines>.Server => AzurePipelines.Instance;

    /// <summary>
    /// Gets or sets the token used to access Azure DevOps instance in a secure way.
    /// </summary>
    /// <remarks>
    /// This property returns the value of the `Token` property if it is not <see langword="null"/>, otherwise it
    /// returns the value of <see cref="Server"/>`.
    /// </remarks>
    [Parameter("Token used to access Azure DevOps instance in a secure way")]
    [Secret]
    string Token => TryGetValue<string>(() => Token) ?? this.Server.AccessToken;
}