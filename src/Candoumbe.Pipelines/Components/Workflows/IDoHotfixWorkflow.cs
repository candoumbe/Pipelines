using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// <para>
/// Represents a nuke component that extends the <see cref="IWorkflow"/> and <see cref="IHaveGitRepository"/> interfaces.
/// </para>
/// <para>
/// It provides properties and a target for managing hotfix workflows, including starting a new hotfix branch and merging it back to the main branch.
/// </para>
/// </summary>
public interface IDoHotfixWorkflow : IWorkflow, IHaveMainBranch
{
    /// <summary>
    /// Gets the prefix used to name hotfix branches.
    /// </summary>
    public string HotfixBranchPrefix => "hotfix";

    /// <summary>
    /// Gets the name of the branch to use when starting a new hotfix branch.
    /// </summary>
    /// <remarks>
    /// This property should never return <see langword="null"/>.
    /// </remarks>
    string HotfixBranchSourceName { get; }

    /// <summary>
    /// Creates a new hotfix branch from the .
    /// </summary>
    public Target Hotfix => _ => _
        .Description($"Starts a new hotfix branch '{HotfixBranchPrefix}/*' from {HotfixBranchSourceName}")
        .DependsOn(Changelog)
        .Requires(() => IsLocalBuild)
        .Requires(() => !GitRepository.IsOnHotfixBranch() || GitHasCleanWorkingCopy())
        .Executes(async () =>
        {
            if (!GitRepository.IsOnHotfixBranch())
            {
                Checkout($"{HotfixBranchPrefix}/{GitVersion.Major}.{GitVersion.Minor}.{GitVersion.Patch + 1}", start: HotfixBranchSourceName);
            }
            else
            {
                await FinishHotfix();
            }
        });

    /// <summary>
    /// Merges a hotfix branch back to <see cref="HotfixBranchSourceName"/>.
    /// </summary>
    ValueTask FinishHotfix() => ValueTask.CompletedTask;
}