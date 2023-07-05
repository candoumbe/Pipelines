using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;

using System;
using System.IO;
using System.Threading.Tasks;

using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Marker interface for repository that support a workflow
/// </summary>
public interface IWorkflow : IHaveGitRepository, IHaveMainBranch, IHaveGitVersion, IHaveChangeLog
{
    /// <summary>
    /// Prefix used to name coldfix branches
    /// </summary>
    public string ColdfixBranchPrefix => "coldfix";

    /// <summary>
    /// Prefix used to name feature branches
    /// </summary>
    public string FeatureBranchPrefix => "feature";

    /// <summary>
    /// Prefix used to name hotfix branches
    /// </summary>
    public string HotfixBranchPrefix => "hotfix";

    /// <summary>
    /// Name of the branch to use when starting a new feature
    /// </summary>
    string FeatureBranchSourceName { get; }

    /// <summary>
    /// Name of the branch to use when starting a new hotfix branch
    /// </summary>
    string HotfixBranchSourceName { get; }

    /// <summary>
    /// Indicates if any changes should be stashed automatically prior to switching branch (Default : true
    /// </summary>
    [Parameter("Indicates if any changes should be stashed automatically prior to switching branch (Default : true)")]
    bool AutoStash => true;

    /// <summary>
    /// Name of the branch to create
    /// </summary>
    [Parameter("Name of the branch to create")]
    string Name => TryGetValue(() => Name);

    /// <summary>
    /// Creates a new hotfix branch from the main branch.
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
                Checkout($"{HotfixBranchPrefix}/{GitVersion.Major}.{GitVersion.Minor}.{GitVersion.Patch + 1}", start: MainBranchName);
            }
            else
            {
                await FinishHotfix();
            }
        });

    /// <summary>
    /// Finalizes the change log so that its up to date for the release.
    /// </summary>
    public Target Changelog => _ => _
        .OnlyWhenStatic(() => GitRepository.IsOnReleaseBranch() || GitRepository.IsOnHotfixBranch())
        .Description("Finalizes the change log so that its up to date for the release.")
        .Executes(() =>
        {
            FinalizeChangelog(ChangeLogFile, GitVersion.MajorMinorPatch, GitRepository);

            if (IsLocalBuild)
            {
                Information("Please review CHANGELOG.md ({ChangeLogFile}) and press 'Y' to validate (any other key will cancel changes)...", ChangeLogFile);
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    Git($"add {ChangeLogFile}");
                    Git($"commit -m \"Finalize {Path.GetFileName(ChangeLogFile)} for {GitVersion.MajorMinorPatch}\"");
                }
            }
            else
            {
                Information("Committing CHANGELOG.md ({ChangeLogFile})", ChangeLogFile);
                CommitChangelogChanges(ChangeLogFile, GitVersion.MajorMinorPatch);
                Information("CHANGELOG.md ({ChangeLogFile}) successfully commited", ChangeLogFile);
            }

            static void CommitChangelogChanges(AbsolutePath changelogFilePath, string version)
            {
                Git($"add {changelogFilePath}");
                Git($"commit -m \"Finalize {Path.GetFileName(changelogFilePath)} for {version}\"");
            }
        });
    /// <summary>
    /// Starts a new feature development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {DevelopBranch}
    /// </summary>
    /// <remarks>
    /// This target will instead ends a feature if the current branch is a feature/* branch with no pending changes.
    /// </remarks>
    public Target Feature => _ => _
       .Description($"Starts a new feature development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {FeatureBranchSourceName}")
       .Requires(() => IsLocalBuild)
       .Requires(() => !GitRepository.IsOnFeatureBranch() || GitHasCleanWorkingCopy())
       .Executes(async () =>
       {
           if (!GitRepository.IsOnFeatureBranch())
           {
               Information("Enter the name of the feature. It will be used as the name of the feature/branch (leave empty to exit) :");
               AskBranchNameAndSwitchToIt(FeatureBranchPrefix, sourceBranch: FeatureBranchSourceName);
               Information("Good bye !");
           }
           else
           {
               await FinishFeature();
           }
       });

    /// <summary>
    /// Asks the user for a branch name
    /// </summary>
    /// <param name="branchNamePrefix">A prefix to preprend in front of the user branch name</param>
    /// <param name="sourceBranch">Branch from which a new branch will be created</param>
    protected void AskBranchNameAndSwitchToIt(string branchNamePrefix, string sourceBranch)
    {
        string featureName;
        bool exitCreatingFeature;
        do
        {
            featureName = (Name ?? Console.ReadLine() ?? string.Empty).Trim()
                                                            .Trim('/');

            switch (featureName)
            {
                case string name when !string.IsNullOrWhiteSpace(name):
                    {
                        string branchName = $"{branchNamePrefix}/{featureName.Slugify()}";
                        Information($"The branch '{{BranchName}}' will be created.{Environment.NewLine}Confirm ? (Y/N) ", branchName);

                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                Information($"{Environment.NewLine}Checking out branch '{branchName}' from '{sourceBranch}'");

                                Checkout(branchName, start: sourceBranch);

                                Information($"{Environment.NewLine}'{branchName}' created successfully");
                                exitCreatingFeature = true;
                                break;

                            default:
                                Information($"{Environment.NewLine}Exiting {nameof(Feature)} task.");
                                exitCreatingFeature = true;
                                break;
                        }
                    }
                    break;
                default:
                    Information("Exiting task.");
                    exitCreatingFeature = true;
                    break;
            }
        } while (string.IsNullOrWhiteSpace(featureName) && !exitCreatingFeature);
    }

    /// <summary>
    /// Starts a new branch from the branch named <paramref name="start"/>
    /// </summary>
    /// <param name="branch">Name of the branch to create</param>
    /// <param name="start">Name of the branch to use as starting branch.</param>
    protected void Checkout(string branch, string start)
    {
        bool hasCleanWorkingCopy = GitHasCleanWorkingCopy();

        if (!hasCleanWorkingCopy && AutoStash)
        {
            Git("stash");
        }
        Git($"checkout -b {branch} {start}");

        if (!hasCleanWorkingCopy && AutoStash)
        {
            Git("stash pop");
        }
    }

    /// <summary>
    /// Merges a feature branch back to <see cref="FeatureBranchSourceName"/>
    /// </summary>
    ValueTask FinishFeature() => ValueTask.CompletedTask;

    /// <summary>
    /// Merges a hotfix branch back to <see cref="HotfixBranchSourceName"/>
    /// </summary>
    ValueTask FinishHotfix() => ValueTask.CompletedTask;
}
