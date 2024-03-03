using Nuke.Common;

namespace Candoumbe.Pipelines;

/// <summary>
/// Defines the build server
/// </summary>
/// <typeparam name="TBuildServer">Type of the build server abstraction to use.</typeparam>
public interface IHaveBuildServer<TBuildServer> where TBuildServer : Host
{
    /// <summary>
    /// The server onto which the pipeline could run
    /// </summary>
    TBuildServer Server { get; }
}