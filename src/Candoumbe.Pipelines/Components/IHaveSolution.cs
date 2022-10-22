using JetBrains.Annotations;

using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace Candoumbe.Pipelines.Components;

[PublicAPI]
public interface IHaveSolution : INukeBuild
{
    /// <summary>
    /// The solution to build
    /// </summary>
    [Required]
    [Solution]
    Solution Solution => TryGetValue(() => Solution);
}