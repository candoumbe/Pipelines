using System.Collections.Generic;
using System.Linq;
using Fallout.Common;
using Fallout.Common.CI.GitHubActions;
using Fallout.Common.IO;
using Fallout.Common.Tools.DotNet;
using static Fallout.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// Component to regenerate GitHub workflows.
/// This component is useful when you want to update the GitHub workflows after modifying the build pipeline.
/// </summary>
public interface ICanRegenerateGitHubWorkflows : IRegenerateWorkflows
{
    /// <summary>
    /// Target that regenerates all workflows.
    /// </summary>
    Target RegenerateWorkflow => _ => _
        .OnlyWhenStatic(() => IsLocalBuild)
        .Description("Regenerates GitHub workflows")
        .Executes(() =>
        {
            AbsolutePath workflowsDirectory = RootDirectory / ".github" / "workflows";
            const string allChoice = "all";
            const string exitChoice = "exit";

            HashSet<string> workflowNames = [.. this.GetType().GetCustomAttributes(true).OfType<GitHubActionsAttribute>()
                .Select(attr => attr.IdPostfix)];

            string userChoice = PromptForChoice("Select the workflow to regenerate",
                                                  [.. workflowNames.Select(item => (item, item)).Concat([(allChoice, "Regenerate all workflows"), (exitChoice, "Exit")])]);

            if (userChoice == exitChoice)
            {
                Information("No workflow selected. Exiting.");
            }
            else if (userChoice == allChoice)
            {
                foreach (string workflow in workflowNames)
                {
                    Information("Workflow {WorkflowName} will be regenerated", workflow);
                    DotNetTasks.DotNet($"fallout --generate-configuration GitHubActions_{workflow} --host GitHubActions");
                }
            }
            else
            {
                Information("Workflow {WorkflowName} will be regenerated", userChoice);
                DotNetTasks.DotNet($"fallout --generate-configuration GitHubActions_{userChoice} --host GitHubActions");
            }
        });
}