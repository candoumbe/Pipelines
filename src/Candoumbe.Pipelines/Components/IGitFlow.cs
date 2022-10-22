using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;

using System;
using System.Collections.Generic;
using System.IO;

using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.GitVersion.GitVersionTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a build as supporting the <see href="https://datasift.github.io/gitflow/IntroducingGitFlow.html">git flow</see> workflow.
/// </summary>
/// <remarks>
/// This interface will provide several ready to use targets to effectively manages this workflow even when the underlying "git" command does not support the gitflow verbs.
/// </remarks>
public interface IGitFlow : IHaveGitRepository, IHaveChangeLog, IHaveGitVersion
{
    /// <summary>
    /// Name of the main branch
    /// </summary>
    public const string MainBranchName = "main";

    /// <summary>
    /// Name of the develop branch
    /// </summary>
    public const string DevelopBranch = "develop";

    /// <summary>
    /// Name of the release branch
    /// </summary>
    public const string ReleaseBranch = "release";

    /// <summary>
    /// Prefix used to name feature branches
    /// </summary>
    public string FeatureBranchPrefix => "feature";

    /// <summary>
    /// Prefix used to name hotfix branches
    /// </summary>
    public string HotfixBranchPrefix => "hotfix";

    /// <summary>
    /// Prefix used to name coldfix branches
    /// </summary>
    public string ColdfixBranchPrefix => "coldfix";

    /// <summary>
    /// Prefix used to name release branches
    /// </summary>
    public string ReleaseBranchPrefix => "release";

    /// <summary>
    /// Name of the branch to create
    /// </summary>
    [Parameter("Name of the branch to create")]
    string Name => TryGetValue(() => Name);

    /// <summary>
    /// Hint to create a major release.
    /// </summary>
    /// <remarks>
    /// The value of this property is only taken into account when running <see cref="Release"/> target.
    /// </remarks>
    [Parameter("Hint to create a major release.")]
    bool Major => false;

    /// <summary>
    /// Indicates if any changes should be stashed automatically prior to switching branch (Default : true
    /// </summary>
    [Parameter("Indicates if any changes should be stashed automatically prior to switching branch (Default : true)")]
    bool AutoStash => true;

    /// <summary>
    /// Finalizes the change log so that its up to date for the release.
    /// </summary>
    public Target Changelog => _ => _
        .Requires(() => IsLocalBuild)
        .OnlyWhenStatic(() => GitRepository.IsOnReleaseBranch() || GitRepository.IsOnHotfixBranch())
        .Description("Finalizes the change log so that its up to date for the release.")
        .Executes(() =>
        {
            FinalizeChangelog(ChangeLogFile, GitVersion.MajorMinorPatch, GitRepository);
            Information("Please review CHANGELOG.md ({ChangeLogFile}) and press 'Y' to validate (any other key will cancel changes)...", ChangeLogFile);
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Y)
            {
                Git($"add {ChangeLogFile}");
                Git($"commit -m \"Finalize {Path.GetFileName(ChangeLogFile)} for {GitVersion.MajorMinorPatch}\"");
            }
        });
    /// <summary>
    /// Starts a new feature development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {DevelopBranch}
    /// </summary>
    /// <remarks>
    /// This target will instead ends a feature if the current branch is a feature/* branch with no pending changes.
    /// </remarks>
    public Target Feature => _ => _
       .Description($"Starts a new feature development by creating the associated branch {FeatureBranchPrefix}/{{feature-name}} from {DevelopBranch}")
       .Requires(() => IsLocalBuild)
       .Requires(() => !GitRepository.IsOnFeatureBranch() || GitHasCleanWorkingCopy())
       .Executes(() =>
       {
           if (!GitRepository.IsOnFeatureBranch())
           {
               Information("Enter the name of the feature. It will be used as the name of the feature/branch (leave empty to exit) :");
               AskBranchNameAndSwitchToIt(FeatureBranchPrefix, DevelopBranch);
               Information("Good bye !");
           }
           else
           {
               FinishFeature();
           }
       });

    /// <summary>
    /// Asks the user for a branch name
    /// </summary>
    /// <param name="branchNamePrefix">A prefix to preprend in front of the user branch name</param>
    /// <param name="sourceBranch">Branch from which a new branch will be created</param>
    private void AskBranchNameAndSwitchToIt(string branchNamePrefix, string sourceBranch)
    {
        string featureName;
        bool exitCreatingFeature = false;
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
    /// Creates a new release branch from the develop branch.
    /// </summary>
    /// <remarks>
    /// A major release can be created by setting <see cref="Major"/> to <see langword="true"/>
    /// </remarks>
    public Target Release => _ => _
        .DependsOn(Changelog)
        .Description($"Starts a new {ReleaseBranchPrefix}/{{version}} from {DevelopBranch}")
        .Requires(() => !GitRepository.IsOnReleaseBranch() || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            if (!GitRepository.IsOnReleaseBranch())
            {
                Checkout($"{ReleaseBranchPrefix}/{GitVersion.MajorMinorPatch}", start: DevelopBranch);
            }
            else
            {
                FinishReleaseOrHotfix();
            }
        });

    /// <summary>
    /// Creates a new hotfix branch from the main branch.
    /// </summary>
    /// <remarks>
    /// A major release can be created by setting <see cref="Major"/> to <see langword="true"/>
    /// </remarks>
    public Target Hotfix => _ => _
        .DependsOn(Changelog)
        .Description($"Starts a new hotfix branch '{HotfixBranchPrefix}/*' from {MainBranchName}")
        .Requires(() => !GitRepository.IsOnHotfixBranch() || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            (GitVersion mainBranchVersion, IReadOnlyCollection<Output> _) = GitVersion(s => s
                .SetFramework("net5.0")
                .SetUrl(RootDirectory)
                .SetBranch(MainBranchName)
                .DisableProcessLogOutput());

            if (!GitRepository.IsOnHotfixBranch())
            {
                Checkout($"{HotfixBranchPrefix}/{mainBranchVersion.Major}.{mainBranchVersion.Minor}.{mainBranchVersion.Patch + 1}", start: MainBranchName);
            }
            else
            {
                FinishReleaseOrHotfix();
            }
        });

    /// <summary>
    /// Creates a new coldfix branch from the develop branch.
    /// </summary>
    public Target Coldfix => _ => _
        .Description($"Starts a new coldfix development by creating the associated '{ColdfixBranchPrefix}/{{name}}' from {DevelopBranch}")
        .Requires(() => IsLocalBuild)
        .Requires(() => !GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*", true) || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            if (!GitRepository.Branch.Like($"{ColdfixBranchPrefix}/*"))
            {
                Information("Enter the name of the coldfix. It will be used as the name of the coldfix/branch (leave empty to exit) :");
                AskBranchNameAndSwitchToIt(ColdfixBranchPrefix, DevelopBranch);
                Information($"{EnvironmentInfo.NewLine}Good bye !");
            }
            else
            {
                FinishColdfix();
            }
        });

    /// <summary>
    /// Merge a coldfix/* branch back to the develop branch
    /// </summary>
    private void FinishColdfix() => FinishFeature();

    private void Checkout(string branch, string start)
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

    private void FinishReleaseOrHotfix()
    {
        Git($"checkout {MainBranchName}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");
        Git($"tag {MajorMinorPatchVersion}");

        Git($"checkout {DevelopBranch}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        Git($"branch -D {GitRepository.Branch}");

        Git($"push origin --follow-tags {MainBranchName} {DevelopBranch} {MajorMinorPatchVersion}");
    }

    private void FinishFeature()
    {
        Git($"rebase {DevelopBranch}");
        Git($"checkout {DevelopBranch}");
        Git("pull");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        Git($"branch -D {GitRepository.Branch}");
        Git($"push origin {DevelopBranch}");
    }
}