using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

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
                IReadOnlyCollection<Output> outputs = Git("tag --sort=-v:refname");
                if (outputs.Count == 0)
                {
                    Warning("No version released found. No hotfix branch will be created");
                }
                else
                {
                    (int Major, int Minor, int Patch) lastReleaseVersion = outputs.Select(output => ExtractFromText(output.Text))
                        .Where(version => version is not null)
                        .Select(version => (version.Value.Major, version.Value.Minor, version.Value.Patch))
                        .First();

                    Checkout($"{HotfixBranchPrefix}/{lastReleaseVersion.Major}.{lastReleaseVersion.Minor}.{lastReleaseVersion.Patch + 1}", start: HotfixBranchSourceName);
                }
            }
            else
            {
                await FinishHotfix();
            }

            (int Major, int Minor, int Patch)? ExtractFromText(string text)
            {
                const string semVerRegex = @"^(?<major>(0|[1-9]\d*)+)\.(?<minor>(0|[1-9]\d*))+\.(?<patch>(0|[1-9]\d*)+)$";

                Match match = Regex.Match(text, semVerRegex, RegexOptions.None, TimeSpan.FromMilliseconds(100));

                (int Major, int Minor, int Patch)? version = null;

                if (match.Success)
                {
                    int major = int.Parse(match.Groups["major"].Value);
                    int minor = int.Parse(match.Groups["minor"].Value);
                    int patch = int.Parse(match.Groups["patch"].Value);

                    version = (major, minor, patch);
                }

                return version;
            }
        });

    /// <summary>
    /// Merges a hotfix branch back to <see cref="HotfixBranchSourceName"/>.
    /// </summary>
    ValueTask FinishHotfix() => ValueTask.CompletedTask;
}