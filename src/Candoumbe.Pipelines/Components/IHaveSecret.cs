using Nuke.Common;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Utilities.ConsoleUtility;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Adds a target that manages secrets
/// </summary>
/// <remarks>
/// The default implementation requires nuke to be install locally
/// </remarks>
public interface IHaveSecret : INukeBuild
{
    /// <summary>
    /// Manage secrets so that pipelines can be runned locally"
    /// </summary>
    public Target ManageSecrets => _ => _
        .Description("Manage secrets that can be used when running build locally")
        .Requires(() => IsLocalBuild)
        .Executes(() =>
        {
            string profile = PromptForInput("Profile name", string.Empty);
            if (string.IsNullOrWhiteSpace(profile))
            {
                Information("No profile set. Parameters will be set for the default profile");
            }
            DotNet($"tool run nuke :secrets{(string.IsNullOrWhiteSpace(profile) ? string.Empty : $" {profile}")}");
        });
}
