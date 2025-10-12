using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace Candoumbe.Pipelines.Components.GitHub;

/// <summary>
/// Generates GitHub workflows.
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

            foreach (AbsolutePath workflow in workflowsDirectory.GlobFiles("*.yml"))
            {
                string workflowName = workflow.NameWithoutExtension;
                DotNetTasks.DotNet($"nuke --generate-configuration GitHubActions_{workflowName} --host GitHubActions");
            }
        });
}