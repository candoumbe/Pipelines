using Nuke.Common;
using Nuke.Common.Git;

namespace Candoumbe.Pipelines.Components;

public interface IHaveGitRepository : INukeBuild
{
    [GitRepository][Required]
    public GitRepository GitRepository => TryGetValue(() => GitRepository);
}