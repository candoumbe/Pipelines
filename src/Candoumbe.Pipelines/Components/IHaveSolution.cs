using JetBrains.Annotations;

using Fallout.Common;
using Fallout.Common.ProjectModel;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that handle a Solution
/// </summary>
[PublicAPI]
public interface IHaveSolution : IFalloutBuild
{
    /// <summary>
    /// The solution to build
    /// </summary>
    [Required]
    [Solution]
    Solution Solution => TryGetValue(() => Solution);
}