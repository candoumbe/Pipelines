using System;
using System.IO;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.Workflows;

/// <summary>
/// Marker interface for git repository that support a specific workflow
/// </summary>
public interface IWorkflow : IHaveGitRepository, IHaveGitVersion, IHaveChangeLog, ICanSkipConfirmation
{
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
    /// Finalizes the change log so that its up to date for the release.
    /// </summary>
    public Target Changelog => _ => _
        .OnlyWhenStatic(() => GitRepository.IsOnReleaseBranch() || GitRepository.IsOnHotfixBranch())
        .Description("Finalizes the change log so that its up to date for the release.")
        .Executes(() =>
        {
            FinalizeChangelog(ChangeLogFile, GitVersion.MajorMinorPatch, GitRepository);

            if (IsLocalBuild && !Quiet)
            {
                Information("Please review CHANGELOG.md ({ChangeLogFile}) and press 'Y' to validate (any other key will cancel changes)...", ChangeLogFile);
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    CommitChangelogChanges(ChangeLogFile, GitVersion.MajorMinorPatch);
                }
            }
            else
            {
                CommitChangelogChanges(ChangeLogFile, GitVersion.MajorMinorPatch);
            }

            return;

            static void CommitChangelogChanges(in AbsolutePath changelogFilePath,in string version)
            {
                Information("Committing CHANGELOG.md ({ChangeLogFile})", changelogFilePath);
                Git($"add {changelogFilePath}");
                Git($"commit -m \"Finalize {Path.GetFileName(changelogFilePath)} for {version}\"");
                Information("CHANGELOG.md ({ChangeLogFile}) successfully commited", changelogFilePath);
            }
        });

    /// <summary>
    /// Asks the user for a branch name
    /// </summary>
    /// <param name="branchNamePrefix">A prefix to prepend in front of the user branch name</param>
    /// <param name="sourceBranch">Branch from which a new branch will be created</param>
    protected void AskBranchNameAndSwitchToIt(string branchNamePrefix, string sourceBranch)
    {
        if (!Quiet)
        {
            Information("Enter the name of the coldfix. It will be used as the name of the coldfix/branch (leave empty to exit) :");
            
            string featureName;
            bool exitCreatingFeature;
            do
            {
                featureName = ( Name ?? Console.ReadLine() ?? string.Empty ).Trim()
                    .Trim('/');

                switch (featureName)
                {
                    case { } when !string.IsNullOrWhiteSpace(featureName):
                    {
                        string branchName = ComputeSanitizedFeatureBranchName(branchNamePrefix, featureName);
                        Information(
                            $"The branch '{{BranchName}}' will be created.{Environment.NewLine}Confirm ? (Y/N) ",
                            branchName);

                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                CheckoutBranch(branchName, sourceBranch);
                                exitCreatingFeature = true;
                                break;

                            default:
                            {
                                Information($"{Environment.NewLine}Exiting task.");
                                exitCreatingFeature = true;
                                break;
                            }
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
        else
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                string featureBranchName = ComputeSanitizedFeatureBranchName(branchNamePrefix, Name.Trim('/'));
                CheckoutBranch(featureBranchName, sourceBranch);
            }
            else
            {
                Error(@"Missing ""--{ParameterName}"" parameter.", nameof(Name).Slugify());
            }
        }

        return;
        
        /*
         * Method for checking out a branch from a specified starting branch.
         *  Displays information about the process and the result.
         */ 
        void CheckoutBranch(in string branchName, in string startBranch)
        {
            Information($"{Environment.NewLine}Checking out branch '{branchName}' from '{startBranch}'");

            Checkout(branchName, start: startBranch);

            Information($"{Environment.NewLine}'{branchName}' created successfully");
        }

        /*
         * Compute a sanitized name of the desired branch name with the specified prefix
         */
        static string ComputeSanitizedFeatureBranchName(in string prefix, in string desiredBranchName) => $"{prefix}/{desiredBranchName.Slugify()}";
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
}