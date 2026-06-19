using System;
using Fallout.Common;

namespace Candoumbe.Pipelines.Components.AzureDevOps;

/// <summary>
/// Marks a pipeline that can create pull requests
/// </summary>
public interface IPullRequest : IHaveGitRepository
{
    /// <summary>
    /// The title of the PR that will be created
    /// </summary>
    [Parameter("Title that will be used when creating a PR")]
    string Title => TryGetValue(() => Title) ?? GitRepository.Branch;

    /// <summary>
    /// Description of the pull request
    /// </summary>
    [Parameter("Description of the pull request")]
    string Description => TryGetValue(() => Description) ?? this.As<IHaveChangeLog>()?.ReleaseNotes;

    /// <summary>
    /// Should the local branch be deleted after the pull request was created successfully?
    /// </summary>
    [Parameter("Should the local branch be deleted after the pull request was created successfully ?")]
    bool DeleteLocalOnSuccess => TryGetValue<bool?>(() => DeleteLocalOnSuccess) ?? false;

    /// <summary>
    /// Defines, when set to <see langword="true"/>, to open the pull request as draft.
    /// </summary>
    [Parameter("Indicates to open the pull request as 'draft'")]
    bool Draft => TryGetValue<bool?>(() => Draft) ?? false;

    /// <summary>
    /// The issue ID for witch pull request will be created.
    /// </summary>
    [Parameter("Issues that will be closed once the pull request is completed.")]
    uint[] Issues => TryGetValue(() => Issues) ?? [];
}