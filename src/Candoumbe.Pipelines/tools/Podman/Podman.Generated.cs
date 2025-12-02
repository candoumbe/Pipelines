
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
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--auth-file</c> via <see cref="PodmanAutoUpdateSettings.AuthFile"/></li><li><c>--dry-run</c> via <see cref="PodmanAutoUpdateSettings.DryRun"/></li><li><c>--format</c> via <see cref="PodmanAutoUpdateSettings.Format"/></li><li><c>--rollback</c> via <see cref="PodmanAutoUpdateSettings.Rollback"/></li><li><c>--tls-verify</c> via <see cref="PodmanAutoUpdateSettings.TlsVerify"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanAutoUpdate(PodmanAutoUpdateSettings options = null) => new PodmanTasks().Run<PodmanAutoUpdateSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanAutoUpdate(Candoumbe.Pipelines.Tools.Podman.PodmanAutoUpdateSettings)"/>
    public static IReadOnlyCollection<Output> PodmanAutoUpdate(Configure<PodmanAutoUpdateSettings> configurator) => new PodmanTasks().Run<PodmanAutoUpdateSettings>(configurator.Invoke(new PodmanAutoUpdateSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanAutoUpdate(Candoumbe.Pipelines.Tools.Podman.PodmanAutoUpdateSettings)"/>
    public static IEnumerable<(PodmanAutoUpdateSettings Settings, IReadOnlyCollection<Output> Output)> PodmanAutoUpdate(CombinatorialConfigure<PodmanAutoUpdateSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanAutoUpdate, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Builds an image using instructions from one or more Containerfiles or Dockerfiles and a specified build context directory. A Containerfile uses the same syntax as a Dockerfile internally. For this document, a file referred to as a Containerfile can be a file named either ‘Containerfile’ or ‘Dockerfile’ exclusively. Any file that has additional extension attached will not be recognized by podman <c>build</c> . unless a <c>-f</c> flag is used to specify the file.</p><p>The build context directory can be specified as the http(s) URL of an archive, git repository or Containerfile.</p><p>When invoked with -f and a path to a Containerfile, with no explicit CONTEXT directory, Podman uses the Containerfile’s parent directory as its build context.</p><p>Containerfiles ending with a “.in” suffix are preprocessed via CPP(1). This can be useful to decompose Containerfiles into several reusable parts that can be used via CPP’s #include directive. Containerfiles ending in .in are restricted to no comment lines unless they are CPP commands. Note, a Containerfile.in file can still be used by other tools when manually preprocessing them via cpp <c>-E</c>.</p><p>When the URL is an archive, the contents of the URL is downloaded to a temporary location and extracted before execution.</p><p>When the URL is a Containerfile, the Containerfile is downloaded to a temporary location.</p><p>When a Git repository is set as the URL, the repository is cloned locally and then set as the context. A URL is treated as a Git repository if it has a git:// prefix or a .git suffix.</p><p>NOTE: podman build uses code sourced from the Buildah project to build container images. This Buildah code creates Buildah containers for the RUN options in container storage. In certain situations, when the podman build crashes or users kill the podman build process, these external containers can be left in container storage. Use the podman ps --all --external command to see these containers.</p><p><c>podman buildx build</c> command is an alias of podman build. Not all <c>buildx</c> build features are available in Podman. The <c>buildx</c> build option is provided for scripting compatibility.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--add-host</c> via <see cref="PodmanBuildSettings.AddHost"/></li><li><c>--all-platforms</c> via <see cref="PodmanBuildSettings.AllPlatforms"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanBuild(PodmanBuildSettings options = null) => new PodmanTasks().Run<PodmanBuildSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanBuild(Candoumbe.Pipelines.Tools.Podman.PodmanBuildSettings)"/>
    public static IReadOnlyCollection<Output> PodmanBuild(Configure<PodmanBuildSettings> configurator) => new PodmanTasks().Run<PodmanBuildSettings>(configurator.Invoke(new PodmanBuildSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanBuild(Candoumbe.Pipelines.Tools.Podman.PodmanBuildSettings)"/>
    public static IEnumerable<(PodmanBuildSettings Settings, IReadOnlyCollection<Output> Output)> PodmanBuild(CombinatorialConfigure<PodmanBuildSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanBuild, degreeOfParallelism, completeOnFailure);
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
    /// <summary><p>If restarting a systemd unit after updating the image has failed, rollback to using the previous image and restart the unit another time. Default is <see langword='true'/>.</p><p>Note that detecting if a systemd unit has failed is best done by the container sending the <c>READY</c> message via <c>SDNOTIFY</c>. This way, restarting the unit waits until having received the message or a timeout kicked in. Without that, restarting the systemd unit may succeed even if the container has failed shortly after.</p><p>Note that detecting if a systemd unit has failed is best done by the container sending the READY message via <c>SDNOTIFY</c>. This way, restarting the unit waits until having received the message or a timeout kicked in. Without that, restarting the systemd unit may succeed even if the container has failed shortly after.</p><p>For a container to send the READY message via SDNOTIFY it must be created with the <c>--sdnotify=container</c> option (see podman-run(1)). The application running inside the container can then execute systemd-notify --ready when ready or use the sdnotify bindings of the specific programming language (e.g., sd_notify(3)).</p></summary>
    [Argument(Format = "--rollback")] public bool? Rollback => Get<bool?>(() => Rollback);
    /// <summary>Require HTTPS and verify certificates when contacting registries (default: <see langword='true'/>). If explicitly set to <see langword='true'/>, TLS verification is used. If set to <see langword='false'/>, TLS verification is not used. If not specified, TLS verification is used unless the target registry is listed as an insecure registry in <see href='https://github.com/containers/image/blob/main/docs/containers-registries.conf.5.md'>containers-registries.conf(5)</see></summary>
    [Argument(Format = "--tls-verify")] public bool? TlsVerify => Get<bool?>(() => TlsVerify);
}
#endregion
#region PodmanBuildSettings
/// <inheritdoc cref="PodmanTasks.PodmanBuild(Candoumbe.Pipelines.Tools.Podman.PodmanBuildSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanBuild))]
public partial class PodmanBuildSettings : ToolOptions
{
    /// <summary><p>Add a custom host-to-IP mapping to the container’s <c>/etc/hosts</c> file.</p><p>Instead of an IP address, the special flag <c>host-gateway</c> can be given. This resolves to an IP address the container can use to connect to the host. The IP address chosen depends on your network setup, thus there’s no guarantee that Podman can determine the host-gateway address automatically, which will then cause Podman to fail with an error message. You can overwrite this IP address using the <i>host_containers_internal_ip</i> option in <i>containers.conf</i>.</p><p>The <i>host-gateway</i> address is also used by Podman to automatically add the <c>host.containers.internal</c> and <c>host.docker.internal</c> hostnames to <c>/etc/hosts</c>. You can prevent that by either giving the <c>--no-hosts</c> option, or by setting <i>host_containers_internal_ip=”none”</i> in <i>containers.conf</i>. If no host-gateway address was configured manually and Podman fails to determine the IP address automatically, Podman will silently skip adding these internal hostnames to /etc/hosts. If Podman is running in a virtual machine using podman machine (this includes Mac and Windows hosts), Podman will silently skip adding the internal hostnames to /etc/hosts, unless an IP address was configured manually; the internal hostnames are resolved by the gvproxy DNS resolver instead.</p><p>Podman will use the <c>/etc/hosts</c> file of the host as a basis by default, i.e. any hostname present in this file will also be present in the <c>/etc/hosts</c> file of the container. A different base file can be configured using the <i>base_hosts_file</i> config in <c>containers.conf</c>.</p></summary>
    [Argument(Format = "--add-host={value}", Separator = ";")] public IEnumerable<string> AddHost => Get<IEnumerable<string>>(() => AddHost);
    /// <summary>Instead of building for a set of platforms specified using the <strong>--platform</strong> option, inspect the build’s base images, and build for all of the platforms for which they are all available. Stages that use scratch as a starting point can not be inspected, so at least one non-scratch stage must be present for detection to work usefully.</summary>
    [Argument(Format = "--all-platforms")] public bool? AllPlatforms => Get<bool?>(() => AllPlatforms);
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
    #region TlsVerify
    /// <inheritdoc cref="PodmanAutoUpdateSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.TlsVerify))]
    public static T SetTlsVerify<T>(this T o, bool? v) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.TlsVerify, v));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.TlsVerify))]
    public static T ResetTlsVerify<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Remove(() => o.TlsVerify));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.TlsVerify))]
    public static T EnableTlsVerify<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.TlsVerify, true));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.TlsVerify))]
    public static T DisableTlsVerify<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.TlsVerify, false));
    /// <inheritdoc cref="PodmanAutoUpdateSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanAutoUpdateSettings), Property = nameof(PodmanAutoUpdateSettings.TlsVerify))]
    public static T ToggleTlsVerify<T>(this T o) where T : PodmanAutoUpdateSettings => o.Modify(b => b.Set(() => o.TlsVerify, !o.TlsVerify));
    #endregion
}
#endregion
#region PodmanBuildSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanBuild(Candoumbe.Pipelines.Tools.Podman.PodmanBuildSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanBuildSettingsExtensions
{
    #region AddHost
    /// <inheritdoc cref="PodmanBuildSettings.AddHost"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AddHost))]
    public static T SetAddHost<T>(this T o, IEnumerable<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.AddHost, v));
    /// <inheritdoc cref="PodmanBuildSettings.AddHost"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AddHost))]
    public static T ResetAddHost<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.AddHost));
    #endregion
    #region AllPlatforms
    /// <inheritdoc cref="PodmanBuildSettings.AllPlatforms"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AllPlatforms))]
    public static T SetAllPlatforms<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.AllPlatforms, v));
    /// <inheritdoc cref="PodmanBuildSettings.AllPlatforms"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AllPlatforms))]
    public static T ResetAllPlatforms<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.AllPlatforms));
    /// <inheritdoc cref="PodmanBuildSettings.AllPlatforms"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AllPlatforms))]
    public static T EnableAllPlatforms<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.AllPlatforms, true));
    /// <inheritdoc cref="PodmanBuildSettings.AllPlatforms"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AllPlatforms))]
    public static T DisableAllPlatforms<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.AllPlatforms, false));
    /// <inheritdoc cref="PodmanBuildSettings.AllPlatforms"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AllPlatforms))]
    public static T ToggleAllPlatforms<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.AllPlatforms, !o.AllPlatforms));
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
