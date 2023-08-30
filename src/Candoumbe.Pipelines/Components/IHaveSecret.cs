using Nuke.Common;
using Nuke.Common.Tooling;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
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
    /// Name of the profile used to store secrets
    /// </summary>
    [Parameter("Name of the profile where the current set of secrets")]
    string Profile => TryGetValue(() => Profile);

    /// <summary>
    /// Manage secrets so that pipelines can be runned locally"
    /// </summary>
    public Target ManageSecrets => _ => _
        .Description("Manage secrets that can be used when running build locally")
        .Requires(() => IsLocalBuild)
        .Executes(() =>
        {
            Arguments args = new Arguments();
            args.Add($"tool run nuke :secrets");

            if (string.IsNullOrWhiteSpace(Profile))
            {
                Information("No profile set. Parameters will be set for the default profile");
            }
            else
            {
                args.Add(Profile);
            }

            DotNet(args.RenderForExecution());
        });
}
