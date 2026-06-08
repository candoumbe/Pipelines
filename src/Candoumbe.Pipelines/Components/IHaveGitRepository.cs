using Fallout.Common;
using Fallout.Common.Git;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that has a git repository
/// </summary>
public interface IHaveGitRepository : IFalloutBuild
{
    /// <summary>
    /// The git repository of the project which a pipeline handles
    /// </summary>
    [GitRepository]
    [Required]
    public GitRepository GitRepository => TryGetValue(() => GitRepository);
}