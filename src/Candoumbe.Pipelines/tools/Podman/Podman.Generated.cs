
using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Candoumbe.Pipelines.Tools.Podman;

/// <summary><p>Podman is a tool for managing OCI containers and pods. Podman provides a Docker-compatible CLI (Docker command line interface) to manage OCI containers and pods.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public partial class PodmanTasks : ToolTasks
{
    public static string PodmanPath { get => new PodmanTasks().GetToolPathInternal(); set => new PodmanTasks().SetToolPath(value); }
    /// <summary><p>Podman is a tool for managing OCI containers and pods. Podman provides a Docker-compatible CLI (Docker command line interface) to manage OCI containers and pods.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> Podman(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new PodmanTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Podman is a tool for managing OCI containers and pods. Podman provides a Docker-compatible CLI (Docker command line interface) to manage OCI containers and pods.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--detach-keys</c> via <see cref="PodmanAttachSettings.DetachKeys"/></li><li><c>--latest}</c> via <see cref="PodmanAttachSettings.Latest"/></li><li><c>--no-stdin</c> via <see cref="PodmanAttachSettings.NoStdin"/></li><li><c>--sig-proxy</c> via <see cref="PodmanAttachSettings.SigProxy"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanAttach(PodmanAttachSettings options = null) => new PodmanTasks().Run<PodmanAttachSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanAttach(Candoumbe.Pipelines.Tools.Podman.PodmanAttachSettings)"/>
    public static IReadOnlyCollection<Output> PodmanAttach(Configure<PodmanAttachSettings> configurator) => new PodmanTasks().Run<PodmanAttachSettings>(configurator.Invoke(new PodmanAttachSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanAttach(Candoumbe.Pipelines.Tools.Podman.PodmanAttachSettings)"/>
    public static IEnumerable<(PodmanAttachSettings Settings, IReadOnlyCollection<Output> Output)> PodmanAttach(CombinatorialConfigure<PodmanAttachSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanAttach, degreeOfParallelism, completeOnFailure);
    /// <summary><p><p>Pulls down new container images and restarts containers configured for auto updates. To make use of auto updates, the container or Kubernetes workloads must run inside a systemd unit. After a successful update of an image, the containers using the image get updated by restarting the systemd units they run in. Please refer to podman-systemd.unit(5) on how to run Podman under systemd.</p>To configure a container for auto updates, it must be created with the io.containers.autoupdate label or the AutoUpdate field in podman-systemd.unit(5) with one of the following two values:</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--auth-file</c> via <see cref="PodmanAutoUpdateSettings.AuthFile"/></li><li><c>--dry-run</c> via <see cref="PodmanAutoUpdateSettings.DryRun"/></li><li><c>--format</c> via <see cref="PodmanAutoUpdateSettings.Format"/></li><li><c>--rollback</c> via <see cref="PodmanAutoUpdateSettings.Rollback"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanAutoUpdate(PodmanAutoUpdateSettings options = null) => new PodmanTasks().Run<PodmanAutoUpdateSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanAutoUpdate(Candoumbe.Pipelines.Tools.Podman.PodmanAutoUpdateSettings)"/>
    public static IReadOnlyCollection<Output> PodmanAutoUpdate(Configure<PodmanAutoUpdateSettings> configurator) => new PodmanTasks().Run<PodmanAutoUpdateSettings>(configurator.Invoke(new PodmanAutoUpdateSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanAutoUpdate(Candoumbe.Pipelines.Tools.Podman.PodmanAutoUpdateSettings)"/>
    public static IEnumerable<(PodmanAutoUpdateSettings Settings, IReadOnlyCollection<Output> Output)> PodmanAutoUpdate(CombinatorialConfigure<PodmanAutoUpdateSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanAutoUpdate, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Podman is a tool for managing OCI containers and pods. Podman provides a Docker-compatible CLI (Docker command line interface) to manage OCI containers and pods.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--all-tags</c> via <see cref="PodmanPsSettings.AllTags"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanPs(PodmanPsSettings options = null) => new PodmanTasks().Run<PodmanPsSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanPs(Candoumbe.Pipelines.Tools.Podman.PodmanPsSettings)"/>
    public static IReadOnlyCollection<Output> PodmanPs(Configure<PodmanPsSettings> configurator) => new PodmanTasks().Run<PodmanPsSettings>(configurator.Invoke(new PodmanPsSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanPs(Candoumbe.Pipelines.Tools.Podman.PodmanPsSettings)"/>
    public static IEnumerable<(PodmanPsSettings Settings, IReadOnlyCollection<Output> Output)> PodmanPs(CombinatorialConfigure<PodmanPsSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanPs, degreeOfParallelism, completeOnFailure);
}
#region PodmanAttachSettings
/// <inheritdoc cref="PodmanTasks.PodmanAttach(Candoumbe.Pipelines.Tools.Podman.PodmanAttachSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanAttach), Arguments = "attach")]
public partial class PodmanAttachSettings : ToolOptions
{
    /// <summary>Specify the key sequence for detaching a container. Format is a single character <c>[a-Z]</c> or one or more ctrl-&lt;value&gt; characters where &lt;value&gt; is one of: <c>a-z</c>, @, ^, [, , or _.<p>Specifying <c>“”</c> disables this feature. The default is <c>ctrl-p,ctrl-q..</c></p></summary>
    [Argument(Format = "--detach-keys={value}", Separator = ",")] public IReadOnlyCollection<string> DetachKeys => Get<IReadOnlyCollection<string>>(() => DetachKeys);
    /// <summary>Instead of providing the container name or ID, use the last created container. <p>Note: the last started container can be from other users of Podman on the host machine. (This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines)</p></summary>
    [Argument(Format = "--latest}")] public bool? Latest => Get<bool?>(() => Latest);
    /// <summary>Do not attach STDIN. Default is <c>false</c>.</summary>
    [Argument(Format = "--no-stdin")] public bool? NoStdin => Get<bool?>(() => NoStdin);
    /// <summary>Proxy received signals to the container. <c>SIGCHLD</c>, <c>SIGURG</c>, <c>SIGSTOP</c>, and <c>SIGKILL</c> are not proxied. Default is <c>true</c>.</summary>
    [Argument(Format = "--sig-proxy")] public bool? SigProxy => Get<bool?>(() => SigProxy);
}
#endregion
#region PodmanAutoUpdateSettings
/// <inheritdoc cref="PodmanTasks.PodmanAutoUpdate(Candoumbe.Pipelines.Tools.Podman.PodmanAutoUpdateSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanAutoUpdate), Arguments = "auto-update")]
public partial class PodmanAutoUpdateSettings : ToolOptions
{
    /// <summary><p>Path of the authentication file. Default is <c>${XDG_RUNTIME_DIR}/containers/auth.json</c> on Linux, and <c>$HOME/.config/containers/auth.json</c> on Windows/macOS. The file is created by <see href='https://docs.podman.io/en/latest/markdown/podman-login.1.html'>podman login</see>. If the authorization state is not found there, <c>$HOME/.docker/config.json</c> is checked, which is set using docker login.</p><p>Note: There is also the option to override the default path of the authentication file by setting the <c>REGISTRY_AUTH_FILE</c> environment variable. This can be done with export <c>REGISTRY_AUTH_FILE=path</c>.</p><p>Alternatively, the <c>io.containers.autoupdate.authfile</c> container label can be configured. In that case, Podman will use the specified label’s value instead.</p>></summary>
    [Argument(Format = "--auth-file={value}")] public Nuke.Common.IO.AbsolutePath AuthFile => Get<Nuke.Common.IO.AbsolutePath>(() => AuthFile);
    /// <summary>Check for the availability of new images but do not perform any pull operation or restart any service or container. The UPDATED field indicates the availability of a new image with 'pending'.</summary>
    [Argument(Format = "--dry-run")] public bool? DryRun => Get<bool?>(() => DryRun);
    /// <summary>Change the default output format. This can be of a supported type like ‘json’ or a Go template</summary>
    [Argument(Format = "--format={value}")] public string Format => Get<string>(() => Format);
    /// <summary><p>If restarting a systemd unit after updating the image has failed, rollback to using the previous image and restart the unit another time. Default is <see langword='true'/>.</p></summary>
    [Argument(Format = "--rollback")] public bool? Rollback => Get<bool?>(() => Rollback);
}
#endregion
#region PodmanPsSettings
/// <inheritdoc cref="PodmanTasks.PodmanPs(Candoumbe.Pipelines.Tools.Podman.PodmanPsSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanPs), Arguments = "ps")]
public partial class PodmanPsSettings : ToolOptions
{
    /// <summary>All tagged images in the repository are pulled.</summary>
    [Argument(Format = "--all-tags")] public bool? AllTags => Get<bool?>(() => AllTags);
}
#endregion
#region PodmanAttachSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanAttach(Candoumbe.Pipelines.Tools.Podman.PodmanAttachSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanAttachSettingsExtensions
{
    #region DetachKeys
    /// <inheritdoc cref="PodmanAttachSettings.DetachKeys"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.DetachKeys))]
    public static T SetDetachKeys<T>(this T o, IReadOnlyCollection<string> v) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.DetachKeys, v));
    /// <inheritdoc cref="PodmanAttachSettings.DetachKeys"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.DetachKeys))]
    public static T ResetDetachKeys<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Remove(() => o.DetachKeys));
    #endregion
    #region Latest
    /// <inheritdoc cref="PodmanAttachSettings.Latest"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.Latest))]
    public static T SetLatest<T>(this T o, bool? v) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.Latest, v));
    /// <inheritdoc cref="PodmanAttachSettings.Latest"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.Latest))]
    public static T ResetLatest<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Remove(() => o.Latest));
    /// <inheritdoc cref="PodmanAttachSettings.Latest"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.Latest))]
    public static T EnableLatest<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.Latest, true));
    /// <inheritdoc cref="PodmanAttachSettings.Latest"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.Latest))]
    public static T DisableLatest<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.Latest, false));
    /// <inheritdoc cref="PodmanAttachSettings.Latest"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.Latest))]
    public static T ToggleLatest<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.Latest, !o.Latest));
    #endregion
    #region NoStdin
    /// <inheritdoc cref="PodmanAttachSettings.NoStdin"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.NoStdin))]
    public static T SetNoStdin<T>(this T o, bool? v) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.NoStdin, v));
    /// <inheritdoc cref="PodmanAttachSettings.NoStdin"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.NoStdin))]
    public static T ResetNoStdin<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Remove(() => o.NoStdin));
    /// <inheritdoc cref="PodmanAttachSettings.NoStdin"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.NoStdin))]
    public static T EnableNoStdin<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.NoStdin, true));
    /// <inheritdoc cref="PodmanAttachSettings.NoStdin"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.NoStdin))]
    public static T DisableNoStdin<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.NoStdin, false));
    /// <inheritdoc cref="PodmanAttachSettings.NoStdin"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.NoStdin))]
    public static T ToggleNoStdin<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.NoStdin, !o.NoStdin));
    #endregion
    #region SigProxy
    /// <inheritdoc cref="PodmanAttachSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.SigProxy))]
    public static T SetSigProxy<T>(this T o, bool? v) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.SigProxy, v));
    /// <inheritdoc cref="PodmanAttachSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.SigProxy))]
    public static T ResetSigProxy<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Remove(() => o.SigProxy));
    /// <inheritdoc cref="PodmanAttachSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.SigProxy))]
    public static T EnableSigProxy<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.SigProxy, true));
    /// <inheritdoc cref="PodmanAttachSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.SigProxy))]
    public static T DisableSigProxy<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.SigProxy, false));
    /// <inheritdoc cref="PodmanAttachSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanAttachSettings), Property = nameof(PodmanAttachSettings.SigProxy))]
    public static T ToggleSigProxy<T>(this T o) where T : PodmanAttachSettings => o.Modify(b => b.Set(() => o.SigProxy, !o.SigProxy));
    #endregion
}
#endregion
#region PodmanAutoUpdateSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanAutoUpdate(Candoumbe.Pipelines.Tools.Podman.PodmanAutoUpdateSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanAutoUpdateSettingsExtensions
{
    #region AuthFile
    /// <inheritdoc cref="PodmanAutoUpdateSettings.AuthFile"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.AuthFile))]
    public static T SetAuthFile<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.AuthFile, v));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.AuthFile"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.AuthFile))]
    public static T ResetAuthFile<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Remove(() => o.AuthFile));
    #endregion
    #region DryRun
    /// <inheritdoc cref="PodmanAutoUpdateSettings.DryRun"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.DryRun))]
    public static T SetDryRun<T>(this T o, bool? v) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.DryRun, v));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.DryRun"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.DryRun))]
    public static T ResetDryRun<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Remove(() => o.DryRun));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.DryRun"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.DryRun))]
    public static T EnableDryRun<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.DryRun, true));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.DryRun"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.DryRun))]
    public static T DisableDryRun<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.DryRun, false));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.DryRun"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.DryRun))]
    public static T ToggleDryRun<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.DryRun, !o.DryRun));
    #endregion
    #region Format
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Format))]
    public static T SetFormat<T>(this T o, string v) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.Format, v));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Format))]
    public static T ResetFormat<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Remove(() => o.Format));
    #endregion
    #region Rollback
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Rollback"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Rollback))]
    public static T SetRollback<T>(this T o, bool? v) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.Rollback, v));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Rollback"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Rollback))]
    public static T ResetRollback<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Remove(() => o.Rollback));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Rollback"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Rollback))]
    public static T EnableRollback<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.Rollback, true));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Rollback"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Rollback))]
    public static T DisableRollback<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.Rollback, false));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.Rollback"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.Rollback))]
    public static T ToggleRollback<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.Rollback, !o.Rollback));
    #endregion
}
#endregion
#region PodmanPsSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanPs(Candoumbe.Pipelines.Tools.Podman.PodmanPsSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanPsSettingsExtensions
{
    #region AllTags
    /// <inheritdoc cref="PodmanPsSettings.AllTags"/>
    [Pure] [Builder(Type = typeof(PodmanPsSettings), Property = nameof(PodmanPsSettings.AllTags))]
    public static T SetAllTags<T>(this T o, bool? v) where T : PodmanPsSettings => o.Modify(b => b.Set(() => o.AllTags, v));
    /// <inheritdoc cref="PodmanPsSettings.AllTags"/>
    [Pure] [Builder(Type = typeof(PodmanPsSettings), Property = nameof(PodmanPsSettings.AllTags))]
    public static T ResetAllTags<T>(this T o) where T : PodmanPsSettings => o.Modify(b => b.Remove(() => o.AllTags));
    /// <inheritdoc cref="PodmanPsSettings.AllTags"/>
    [Pure] [Builder(Type = typeof(PodmanPsSettings), Property = nameof(PodmanPsSettings.AllTags))]
    public static T EnableAllTags<T>(this T o) where T : PodmanPsSettings => o.Modify(b => b.Set(() => o.AllTags, true));
    /// <inheritdoc cref="PodmanPsSettings.AllTags"/>
    [Pure] [Builder(Type = typeof(PodmanPsSettings), Property = nameof(PodmanPsSettings.AllTags))]
    public static T DisableAllTags<T>(this T o) where T : PodmanPsSettings => o.Modify(b => b.Set(() => o.AllTags, false));
    /// <inheritdoc cref="PodmanPsSettings.AllTags"/>
    [Pure] [Builder(Type = typeof(PodmanPsSettings), Property = nameof(PodmanPsSettings.AllTags))]
    public static T ToggleAllTags<T>(this T o) where T : PodmanPsSettings => o.Modify(b => b.Set(() => o.AllTags, !o.AllTags));
    #endregion
}
#endregion
