using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines a directory to output various tests results
/// </summary>
public interface IHaveTests : IHaveArtifacts
{
    public const string TestResultDirectoryName = "tests-results";

    /// <summary>
    /// Directory where to publish all test results
    /// </summary>
    /// <remarks>
    /// By default, the root directory for all test result will be will be in
    /// <list type="bullet">
    /// <item><c>{ArtifactsDirectory} / {branchName}</c> when the current project is a part of a git repository </item>
    /// <item><c>{ArtifactDirectoryName}</c> when the current project is not a part of a git repository or is in a detached branch.</item>
    /// </list>
    /// </remarks>ArtifactsDirectory / "tests-results";
    public AbsolutePath TestResultDirectory => this.Get<IHaveGitRepository>()?.GitRepository?.Branch switch
    {
        string branchName when !string.IsNullOrWhiteSpace(branchName) => ArtifactsDirectory / TestResultDirectoryName / branchName,
        _ => ArtifactsDirectory / TestResultDirectoryName
    };
}
