using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines a directory to output various tests results
/// </summary>
public interface IHaveTests : IHaveOutputDirectory
{
    /// <summary>
    /// Directory where to publish all test results
    /// </summary>
    public AbsolutePath TestResultDirectory => OutputDirectory / "tests-results";
}
