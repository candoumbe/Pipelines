
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
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--add-host</c> via <see cref="PodmanBuildSettings.AddHost"/></li><li><c>--all-platforms</c> via <see cref="PodmanBuildSettings.AllPlatforms"/></li><li><c>--annotation</c> via <see cref="PodmanBuildSettings.Annotation"/></li><li><c>--arch</c> via <see cref="PodmanBuildSettings.Arch"/></li><li><c>--authfile</c> via <see cref="PodmanBuildSettings.AuthFile"/></li><li><c>--build-arg</c> via <see cref="PodmanBuildSettings.BuildArg"/></li><li><c>--build-arg-file</c> via <see cref="PodmanBuildSettings.BuildArgFile"/></li><li><c>--build-context</c> via <see cref="PodmanBuildSettings.BuildContext"/></li><li><c>--cache-from</c> via <see cref="PodmanBuildSettings.CacheFrom"/></li><li><c>--cache-to</c> via <see cref="PodmanBuildSettings.CacheTo"/></li><li><c>--cache-ttl</c> via <see cref="PodmanBuildSettings.CacheTtl"/></li><li><c>--cap-add</c> via <see cref="PodmanBuildSettings.CapAdd"/></li><li><c>--cap-drop</c> via <see cref="PodmanBuildSettings.CapDrop"/></li><li><c>--cert-dir</c> via <see cref="PodmanBuildSettings.CertDir"/></li><li><c>--cgroup-parent</c> via <see cref="PodmanBuildSettings.CgroupParent"/></li><li><c>--cgroupns</c> via <see cref="PodmanBuildSettings.Cgroupns"/></li><li><c>--compat-volumes</c> via <see cref="PodmanBuildSettings.CompatVolumes"/></li><li><c>--compress</c> via <see cref="PodmanBuildSettings.Compress"/></li><li><c>--cpp-flag</c> via <see cref="PodmanBuildSettings.CppFlag"/></li><li><c>--cpu-period</c> via <see cref="PodmanBuildSettings.CpuPeriod"/></li><li><c>--cpu-quota</c> via <see cref="PodmanBuildSettings.CpuQuota"/></li><li><c>--cpu-shares</c> via <see cref="PodmanBuildSettings.CpuShares"/></li><li><c>--cpuset-cpus</c> via <see cref="PodmanBuildSettings.CpusetCpus"/></li><li><c>--cpuset-mems</c> via <see cref="PodmanBuildSettings.CpusetMems"/></li><li><c>--created-annotation</c> via <see cref="PodmanBuildSettings.CreatedAnnotation"/></li><li><c>--creds</c> via <see cref="PodmanBuildSettings.Creds"/></li><li><c>--cw</c> via <see cref="PodmanBuildSettings.Cw"/></li><li><c>--description-key</c> via <see cref="PodmanBuildSettings.DescriptionKey"/></li><li><c>--device</c> via <see cref="PodmanBuildSettings.Device"/></li><li><c>--disable-compression</c> via <see cref="PodmanBuildSettings.DisableCompression"/></li><li><c>--disable-content-trust</c> via <see cref="PodmanBuildSettings.DisableContentTrust"/></li><li><c>--dns</c> via <see cref="PodmanBuildSettings.Dns"/></li><li><c>--dns-option</c> via <see cref="PodmanBuildSettings.DnsOption"/></li><li><c>--dns-search</c> via <see cref="PodmanBuildSettings.DnsSearch"/></li><li><c>--env</c> via <see cref="PodmanBuildSettings.Env"/></li><li><c>--file</c> via <see cref="PodmanBuildSettings.File"/></li><li><c>--force-rm</c> via <see cref="PodmanBuildSettings.ForceRm"/></li><li><c>--format</c> via <see cref="PodmanBuildSettings.Format"/></li><li><c>--from</c> via <see cref="PodmanBuildSettings.From"/></li><li><c>--group-add</c> via <see cref="PodmanBuildSettings.GroupAdd"/></li><li><c>--help</c> via <see cref="PodmanBuildSettings.Help"/></li><li><c>--hooks-dir</c> via <see cref="PodmanBuildSettings.HooksDir"/></li><li><c>--http-proxy</c> via <see cref="PodmanBuildSettings.HttpProxy"/></li><li><c>--identity-label</c> via <see cref="PodmanBuildSettings.IdentityLabel"/></li><li><c>--ignore-file</c> via <see cref="PodmanBuildSettings.IgnoreFile"/></li><li><c>--iidfile</c> via <see cref="PodmanBuildSettings.Iidfile"/></li><li><c>--inherit-annotations</c> via <see cref="PodmanBuildSettings.InheritAnnotations"/></li><li><c>--inherit-labels</c> via <see cref="PodmanBuildSettings.InheritLabels"/></li><li><c>--ipc</c> via <see cref="PodmanBuildSettings.Ipc"/></li><li><c>--isolation</c> via <see cref="PodmanBuildSettings.Isolation"/></li><li><c>--jobs</c> via <see cref="PodmanBuildSettings.Jobs"/></li><li><c>--label</c> via <see cref="PodmanBuildSettings.Label"/></li><li><c>--layer-label</c> via <see cref="PodmanBuildSettings.LayerLabel"/></li><li><c>--layers</c> via <see cref="PodmanBuildSettings.Layers"/></li><li><c>--log-split</c> via <see cref="PodmanBuildSettings.LogSplit"/></li><li><c>--logfile</c> via <see cref="PodmanBuildSettings.LogFile"/></li></ul></remarks>
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
    /// <summary><p>Add an image <i>annotation</i> (e.g. annotation=value) to the image metadata. Can be used multiple times.</p><p>Note: this information is not present in Docker image formats, so it is discarded when writing images in Docker formats.</p></summary>
    [Argument(Format = "--annotation={key}={value}")] public IReadOnlyDictionary<string, string> Annotation => Get<IReadOnlyDictionary<string, string>>(() => Annotation);
    /// <summary><p>Set the architecture of the image to be built, and that of the base image to be pulled, if the build uses one, to the provided value instead of using the architecture of the build host. Unless overridden, subsequent lookups of the same image in the local storage matches this architecture, regardless of the host. (Examples: arm, arm64, 386, amd64, ppc64le, s390x)</p></summary>
    [Argument(Format = "--arch={value}")] public string Arch => Get<string>(() => Arch);
    /// <summary><p>Path of the authentication file. Default is ${XDG_RUNTIME_DIR}/containers/auth.json on Linux, and <c>$HOME/.config/containers/auth.json</c> on Windows/macOS. The file is created by <see href='https://docs.podman.io/en/latest/markdown/podman-login.1.html'>podman login</see>. If the authorization state is not found there, <c>$HOME/.docker/config.json</c> is checked, which is set using <strong>docker login</strong>.</p></summary>
    [Argument(Format = "--authfile={value}")] public Nuke.Common.IO.AbsolutePath AuthFile => Get<Nuke.Common.IO.AbsolutePath>(() => AuthFile);
    /// <summary><p>Specifies a build argument and its value, which is interpolated in instructions read from the Containerfiles in the same way that environment variables are, but which are not added to environment variable list in the resulting image’s configuration.</p></summary>
    [Argument(Format = "--build-arg={key}={value}")] public IReadOnlyDictionary<string, string> BuildArg => Get<IReadOnlyDictionary<string, string>>(() => BuildArg);
    /// <summary><p>Specifies a file containing lines of build arguments of the form <c>arg=value</c>. The suggested file name is argfile.conf.</p><p>Comment lines beginning with <c>#</c> are ignored, along with blank lines. All others must be of the <c>arg=value</c> format passed to <c>--build-arg</c>.</p><p>If several arguments are provided via the <c>--build-arg-file</c> and <c>--build-arg</c> options, the build arguments are merged across all of the provided files and command line arguments.</p><p>Any file provided in a <c>--build-arg-file</c> option is read before the arguments supplied via the <c>--build-arg</c> option.</p><p>When a given argument name is specified several times, the last instance is the one that is passed to the resulting builds. This means <c>--build-arg</c> values always override those in a <c>--build-arg-file</c>.</p></summary>
    [Argument(Format = "--build-arg-file={value}")] public Nuke.Common.IO.AbsolutePath BuildArgFile => Get<Nuke.Common.IO.AbsolutePath>(() => BuildArgFile);
    /// <summary><p>Specify an additional build context using its short name and its location. Additional build contexts can be referenced in the same manner as we access different stages in COPY instruction.</p></summary>
    [Argument(Format = "--build-context={value}")] public string BuildContext => Get<string>(() => BuildContext);
    /// <summary><p>Repository to utilize as a potential cache source. When specified, Buildah tries to look for cache images in the specified repository and attempts to pull cache images instead of actually executing the build steps locally. Buildah only attempts to pull previously cached images if they are considered as valid cache hits.</p><p>Use the <c>--cache-to</c> option to populate a remote repository with cache content.</p><p>Note: <c>--cache-from</c> option is ignored unless <c>--layers</c> is specified.</p></summary>
    [Argument(Format = "--cache-from={value}")] public string CacheFrom => Get<string>(() => CacheFrom);
    /// <summary><p>Set this flag to specify a remote repository that is used to store cache images. Buildah attempts to push newly built cache image to the remote repository.</p><p>Note: Use the <c>--cache-from</c> option in order to use cache content in a remote repository.</p><p>Note: <c>--cache-to</c> option is ignored unless <c>--layers</c> is specified.</p></summary>
    [Argument(Format = "--cache-to={value}")] public string CacheTo => Get<string>(() => CacheTo);
    /// <summary><p>Limit the use of cached images to only consider images with created timestamps less than duration ago. For example if <c>--cache-ttl=1h</c> is specified, Buildah considers intermediate cache images which are created under the duration of one hour, and intermediate cache images outside this duration is ignored.</p><p>Note: Setting <c>--cache-ttl=0</c> manually is equivalent to using <c>--no-cache</c> in the implementation since this means that the user does not want to use cache at all.</p></summary>
    [Argument(Format = "--cache-ttl={value}")] public string CacheTtl => Get<string>(() => CacheTtl);
    /// <summary><p>When executing <c>RUN</c> instructions, run the command specified in the instruction with the specified capability added to its capability set. Certain capabilities are granted by default; this option can be used to add more.</p></summary>
    [Argument(Format = "--cap-add={value}", Separator = ",")] public IEnumerable<string> CapAdd => Get<IEnumerable<string>>(() => CapAdd);
    /// <summary><p>When executing <c>RUN</c> instructions, run the command specified in the instruction with the specified capability removed from its capability set. The <c>CAP_CHOWN</c>, <c>CAP_DAC_OVERRIDE</c>, <c>CAP_FOWNER</c>, <c>CAP_FSETID</c>, <c>CAP_KILL</c>, <c>CAP_NET_BIND_SERVICE</c>, <c>CAP_SETFCAP</c>, <c>CAP_SETGID</c>, <c>CAP_SETPCAP</c>, and <c>CAP_SETUID</c> capabilities are granted by default; this option can be used to remove them.</p><p>If a capability is specified to both the <strong>--cap-add</strong> and <strong>--cap-drop</strong> options, it is dropped, regardless of the order in which the options were given.</p></summary>
    [Argument(Format = "--cap-drop={value}")] public IEnumerable<string> CapDrop => Get<IEnumerable<string>>(() => CapDrop);
    /// <summary><p>Use certificates at <i>path</i> (*.crt, *.cert, *.key) to connect to the registry. (Default: /etc/containers/certs.d) For details, see <strong><see href='https://github.com/containers/image/blob/main/docs/containers-certs.d.5.md'>containers-certs.d(5)</see></strong>. (This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines).</p></summary>
    [Argument(Format = "--cert-dir={value}")] public Nuke.Common.IO.AbsolutePath CertDir => Get<Nuke.Common.IO.AbsolutePath>(() => CertDir);
    /// <summary><p>Path to cgroups under which the cgroup for the container is created. If the path is not absolute, the path is considered to be relative to the cgroups path of the init process. Cgroups are created if they do not already exist.</p></summary>
    [Argument(Format = "--cgroup-parent={value}")] public Nuke.Common.IO.AbsolutePath CgroupParent => Get<Nuke.Common.IO.AbsolutePath>(() => CgroupParent);
    /// <summary><p>Sets the configuration for cgroup namespaces when handling RUN instructions. The configured value can be “” (the empty string) or “private” to indicate that a new cgroup namespace is created, or it can be “host” to indicate that the cgroup namespace in which buildah itself is being run is reused.</p></summary>
    [Argument(Format = "--cgroupns={value}")] public string Cgroupns => Get<string>(() => Cgroupns);
    /// <summary><p>Handle directories marked using the <c>VOLUME</c> instruction (both in this build, and those inherited from base images) such that their contents can only be modified by <c>ADD</c> and <c>COPY</c> instructions. Any changes made in those locations by RUN instructions will be reverted. Before the introduction of this option, this behavior was the default, but it is now disabled by default.</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--compat-volumes")] public bool? CompatVolumes => Get<bool?>(() => CompatVolumes);
    /// <summary><p>This option is added to be aligned with other containers CLIs. Podman doesn’t communicate with a daemon or a remote server. Thus, compressing the data before sending it is irrelevant to Podman. (This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines).</p></summary>
    [Argument(Format = "--compress")] public bool? Compress => Get<bool?>(() => Compress);
    /// <summary><p>Set additional flags to pass to the C Preprocessor cpp(1). Containerfiles ending with a “.in” suffix is preprocessed via cpp(1). This option can be used to pass additional flags to cpp.Note: You can also set default CPPFLAGS by setting the BUILDAH_CPPFLAGS environment variable (e.g., export BUILDAH_CPPFLAGS=”-DDEBUG”).</p></summary>
    [Argument(Format = "--cpp-flag={value}")] public string CppFlag => Get<string>(() => CppFlag);
    /// <summary><p>Set the CPU period for the Completely Fair Scheduler (CFS), which is a duration in microseconds. Once the container’s CPU quota is used up, it will not be scheduled to run until the current period ends. Defaults to <c>100_000</c> microseconds.</p><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, see https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error.</p><p>This option is not supported on cgroups V1 rootless systems.</p><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, <see href='https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error'/>.</p></summary>
    [Argument(Format = "--cpu-period={value}")] public int? CpuPeriod => Get<int?>(() => CpuPeriod);
    /// <summary><p>Limit the CPU Completely Fair Scheduler (CFS) quota.</p><p>Limit the container’s CPU usage. By default, containers run with the full CPU resource. The limit is a number in microseconds. If a number is provided, the container is allowed to use that much CPU time until the CPU period ends (controllable via <strong>--cpu-period</strong>).</p></summary>
    [Argument(Format = "--cpu-quota={value}")] public int? CpuQuota => Get<int?>(() => CpuQuota);
    /// <summary><p>CPU shares (relative weight).</p><p>By default, all containers get the same proportion of CPU cycles. This proportion can be modified by changing the container’s CPU share weighting relative to the combined weight of all the running containers. Default weight is <c>1024</c>.</p><p>The proportion only applies when CPU-intensive processes are running. When tasks in one container are idle, other containers can use the left-over CPU time. The actual amount of CPU time varies depending on the number of containers running on the system.</p><p>For example, consider three containers, one has a cpu-share of 1024 and two others have a cpu-share setting of 512. When processes in all three containers attempt to use 100% of CPU, the first container receives 50% of the total CPU time. If a fourth container is added with a cpu-share of 1024, the first container only gets 33% of the CPU. The remaining containers receive 16.5%, 16.5% and 33% of the CPU.</p><p>On a multi-core system, the shares of CPU time are distributed over all CPU cores. Even if a container is limited to less than 100% of CPU time, it can use 100% of each individual CPU core.</p><p>For example, consider a system with more than three cores. If the container C0 is started with --cpu-shares=512 running one process, and another container C1 with --cpu-shares=1024 running two processes, this can result in the following division of CPU shares:</p><list type= 'table' >  <listheader>  <term>PID</term>  <term>Container</term>  <term>CPU</term>  <term>CPU Share</term>  </listheader>  <item>  <term>100</term>  <description>  <list type= 'bullet' >  <item>Container: C0</item>  <item>CPU: 0</item>  <item>CPU Share: 100% of CPU0</item>  </list>  </description>  </item>  <item>  <term>101</term>  <description>  <list type= 'bullet' >  <item>Container: C1</item>  <item>CPU: 1</item>  <item>CPU Share: 100% of CPU1</item>  </list>  </description>  </item>  <item>  <term>102</term>  <description>  <list type= 'bullet' >  <item>Container: C1</item>  <item>CPU: 2</item>  <item>CPU Share: 100% of CPU2</item>  </list>  </description>  </item>  </list><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, see https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--cpu-shares={value}")] public decimal? CpuShares => Get<decimal?>(() => CpuShares);
    /// <summary><p>CPUs in which to allow execution. Can be specified as a comma-separated list (e.g. 0,1), as a range (e.g. 0-3), or any combination thereof (e.g. 0-3,7,11-15).</p><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, <see href='https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error' />.</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--cpuset-cpus={value}", Separator = ",")] public IEnumerable<string> CpusetCpus => Get<IEnumerable<string>>(() => CpusetCpus);
    /// <summary><p>Memory nodes (MEMs) in which to allow execution (0-3, 0,1). Only effective on NUMA systems.</p><p>If there are four memory nodes on the system (0-3), use <strong>--cpuset-mems=0,1</strong> then processes in the container only uses memory from the first two memory nodes.</p><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, <see href='https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error'/>.</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--cpuset-mems={value}", Separator = ",")] public IEnumerable<string> CpusetMems => Get<IEnumerable<string>>(() => CpusetMems);
    /// <summary><p>Add an image annotation (see also --annotation) to the image metadata setting <c>'org.opencontainers.image.created'</c> to the current time, or to the datestamp specified to the <c>--source-date-epoch</c> or <c>--timestamp</c> flag, if either was used. If <see langword='false'/>, no such annotation will be present in the written image.</p><p>Note: this information is not present in Docker image formats, so it is discarded when writing images in Docker formats.</p></summary>
    [Argument(Format = "--created-annotation")] public bool? CreatedAnnotation => Get<bool?>(() => CreatedAnnotation);
    /// <summary><p>The [username[:password]] to use to authenticate with the registry, if required. If one or both values are not supplied, a command line prompt appears and the value can be entered. The password is entered without <c>echo</c></p><p>Note that the specified credentials are only used to authenticate against target registries. They are not used for mirrors or when the registry gets rewritten (see containers-registries.conf(5)); to authenticate against those consider using a containers-auth.json(5) file.</p></summary>
    [Argument(Format = "--creds={value}", Secret = true)] public string Creds => Get<string>(() => Creds);
    /// <summary><p>Produce an image suitable for use as a confidential workload running in a trusted execution environment (TEE) using krun (i.e., <i>crun</i> built with the libkrun feature enabled and invoked as <i>krun</i>). Instead of the conventional contents, the root filesystem of the image will contain an encrypted disk image and configuration information for krun.</p><p>The value for options is a comma-separated list of <c>key=value</c> pairs, supplying configuration information which is needed for producing the additional data which will be included in the container image.</p><p>Recognized keys are:<list type='bullet'><item><term><i>attestation_url</i></term><description>The location of a key broker / attestation server. If a value is specified, the new image’s workload ID, along with the passphrase used to encrypt the disk image, will be registered with the server, and the server’s location will be stored in the container image. At run-time, krun is expected to contact the server to retrieve the passphrase using the workload ID, which is also stored in the container image. If no value is specified, a <i>passphrase</i> value must be specified.</description></item><item><term>cpus</term><description> The number of virtual CPUs which the image expects to be run with at run-time. If not specified, a default value will be supplied.</description></item><item><term>firmware_library</term><description>The location of the libkrunfw-sev shared library. If not specified, buildah checks for its presence in a number of hard-coded locations.</description></item><item><term><i>memory</i></term><description>The amount of memory which the image expects to be run with at run-time, as a number of megabytes. If not specified, a default value will be supplied.</description></item><item><term>passphrase</term><description>The passphrase to use to encrypt the disk image which will be included in the container image. If no value is specified, but an <i>attestation_url</i> value is specified, a randomly-generated passphrase will be used. The authors recommend setting an attestation_url but not a passphrase.</description></item><item><term><i>slop</i></term><description>Extra space to allocate for the disk image compared to the size of the container image’s contents, expressed either as a percentage (..%) or a size value (bytes, or larger units if suffixes like KB or MB are present), or a sum of two or more such specifications. If not specified, buildah guesses that 25% more space than the contents will be enough, but this option is provided in case its guess is wrong.</description></item><item><term><i>type</i></term><description>The type of trusted execution environment (TEE) which the image should be marked for use with. Accepted values are “SEV” (AMD Secure Encrypted Virtualization - Encrypted State) and “SNP” (AMD Secure Encrypted Virtualization - Secure Nested Paging). If not specified, defaults to “SNP”.</description></item><item><term>workload_id</term><description>A workload identifier which will be recorded in the container image, to be used at run-time for retrieving the passphrase which was used to encrypt the disk image. If not specified, a semi-random value will be derived from the base image’s image ID.</description></item></list></p><p>This option is not supported on the remote client, including Mac and Windows (excluding WSL2) machines.</p></summary>
    [Argument(Format = "--cw={value}", Separator = ",")] public IReadOnlyDictionary<string, string> Cw => Get<IReadOnlyDictionary<string, string>>(() => Cw);
    /// <summary><p>The [key[:passphrase]] to be used for decryption of images. Key can point to keys and/or certificates. Decryption is tried with all keys. If the key is protected by a passphrase, it is required to be passed in the argument and omitted otherwise.</p></summary>
    [Argument(Format = "--description-key={value}", Secret = true)] public string DescriptionKey => Get<string>(() => DescriptionKey);
    /// <summary><p>Add a host device to the container. Optional permissions parameter can be used to specify device permissions by combining <strong>r</strong> for read, <strong>w</strong> for write, and <strong>m</strong> for <strong>mknod</strong>(2).</p><p><example>--device=/dev/sdc:/dev/xvdc:rwm</example></p><p>Note: if <i>host-device</i> is a symbolic link then it is resolved first. The container only stores the major and minor numbers of the host device.</p><p>Podman may load kernel modules required for using the specified device. The devices that Podman loads modules for when necessary are: /dev/fuse.</p><p>In rootless mode, the new device is bind mounted in the container from the host rather than Podman creating it within the container space. Because the bind mount retains its SELinux label on SELinux systems, the container can get permission denied when accessing the mounted device. Modify SELinux settings to allow containers to use all device labels via the following command:</p><p>$ sudo setsebool -P container_use_devices=true</p><p>Note: if the user only has access rights via a group, accessing the device from inside a rootless container fails. The <see href='https://github.com/containers/crun/tree/main/crun.1.md'>crun(1)</see> runtime offers a workaround for this by adding the option <strong>--annotation run.oci.keep_original_groups=1</strong>.</p></summary>
    [Argument(Format = "--device={value}")] public string Device => Get<string>(() => Device);
    /// <summary><p>Don’t compress filesystem layers when building the image unless it is required by the location where the image is being written. This is the default setting, because image layers are compressed automatically when they are pushed to registries, and images being written to local storage only need to be decompressed again to be stored. Compression can be forced in all cases by specifying <strong>--disable-compression=false</strong>.</p></summary>
    [Argument(Format = "--disable-compression")] public bool? DisableCompression => Get<bool?>(() => DisableCompression);
    /// <summary><p>This is a Docker-specific option to disable image verification to a container registry and is not supported by Podman. This option is a NOOP and provided solely for scripting compatibility.</p></summary>
    [Argument(Format = "--disable-content-trust")] public bool? DisableContentTrust => Get<bool?>(() => DisableContentTrust);
    /// <summary><p>Set custom DNS servers.</p><p>This option can be used to override the DNS configuration passed to the container. Typically this is necessary when the host DNS configuration is invalid for the container (e.g., <strong>127.0.0.1</strong>). When this is the case the <c>--dns</c> flag is necessary for every run.</p><p>The special value none can be specified to disable creation of /etc/resolv.conf in the container by Podman. The /etc/resolv.conf file in the image is then used without changes.</p><p>The special value none can be specified to disable creation of /etc/resolv.conf in the container by Podman. The <i>/etc/resolv.conf</i> file in the image is then used without changes.</p><p>This option cannot be combined with <c>--network</c> that is set to <c>none</c>.</p><p>Note: this option takes effect only during <i>RUN</i> instructions in the build. It does not affect <i>/etc/resolv.conf</i> in the final image.</p></summary>
    [Argument(Format = "--dns={value}", Separator = ",")] public System.Net.IPAddress Dns => Get<System.Net.IPAddress>(() => Dns);
    /// <summary><p>Set custom DNS options to be used during the build.</p></summary>
    [Argument(Format = "--dns-option={value}")] public string DnsOption => Get<string>(() => DnsOption);
    /// <summary><p>Set custom DNS search domains to be used during the build.</p></summary>
    [Argument(Format = "--dns-search={value}", Separator = ",")] public string DnsSearch => Get<string>(() => DnsSearch);
    /// <summary><p>Add a value (e.g. <c>env=value</c>) to the built image. Can be used multiple times. If neither <c>=</c> nor a <i>value</i> are specified, but env is set in the current environment, the value from the current environment is added to the image. To remove an environment variable from the built image, use the <c>--unsetenv</c> option.</p></summary>
    [Argument(Format = "--env={value}", Separator = ",")] public string Env => Get<string>(() => Env);
    /// <summary><p>Specifies a Containerfile which contains instructions for building the image, either a local file or an <strong>http</strong> or <strong>https</strong> URL. If more than one Containerfile is specified, <strong>FROM</strong> instructions are only be accepted from the last specified file.</p><p>If a build context is not specified, and at least one Containerfile is a local file, the directory in which it resides is used as the build context.</p><p>Specifying the option <c>-f -</c> causes the Containerfile contents to be read from stdin.</p></summary>
    [Argument(Format = "--file={value}")] public string File => Get<string>(() => File);
    /// <summary><p>Always remove intermediate containers after a build, even if the build fails (default <see langword='true'/>).</p></summary>
    [Argument(Format = "--force-rm")] public bool? ForceRm => Get<bool?>(() => ForceRm);
    /// <summary><p>Control the format for the built image’s manifest and configuration data. Recognized formats include <i>oci</i> (OCI image-spec v1.0, the default) and <i>docker</i> (version 2, using schema format 2 for the manifest).</p><p>Note: You can also override the default format by setting the BUILDAH_FORMAT environment variable. <c>export BUILDAH_FORMAT=docker</c></p></summary>
    [Argument(Format = "--format={value}")] public string Format => Get<string>(() => Format);
    /// <summary><p>Overrides the first FROM instruction within the Containerfile. If there are multiple FROM instructions in a Containerfile, only the first is changed.</p><p>With the remote podman client, not all container transports work as expected. For example, oci-archive:/x.tar references /x.tar on the remote machine instead of on the client. When using podman remote clients it is best to restrict use to <i>containers-storage</i>, and <i>docker:// transports</i>.</p></summary>
    [Argument(Format = "--from={value}")] public string From => Get<string>(() => From);
    /// <summary><p>Assign additional groups to the primary user running within the container process.</p><p><list type='bullet'><item><term><c>keep-groups</c></term><description>is a special flag that tells Podman to keep the supplementary group access.</description></item></list></p><p>Allows container to use the user’s supplementary group access. If file systems or devices are only accessible by the rootless user’s group, this flag tells the OCI runtime to pass the group access into the container. Currently only available with the crun OCI runtime. Note: <c>keep-groups</c> is exclusive, other groups cannot be specified with this flag. (Not available for remote commands, including Mac and Windows (excluding WSL2) machines).</p></summary>
    [Argument(Format = "--group-add={value}")] public string GroupAdd => Get<string>(() => GroupAdd);
    /// <summary>Print usage statement</summary>
    [Argument(Format = "--help")] public bool? Help => Get<bool?>(() => Help);
    /// <summary><p>Each *.json file in the path configures a hook for buildah build containers. For more details on the syntax of the JSON files and the semantics of hook injection. Buildah currently support both the 1.0.0 and 0.1.0 hook schemas, although the 0.1.0 schema is deprecated.</p><p>This option may be set multiple times; paths from later options have higher precedence.</p><p>For the annotation conditions, buildah uses any annotations set in the generated OCI configuration.</p><p>For the bind-mount conditions, only mounts explicitly requested by the caller via --volume are considered. Bind mounts that buildah inserts by default (e.g. /dev/shm) are not considered.</p><p>If --hooks-dir is unset for root callers, Buildah currently defaults to /usr/share/containers/oci/hooks.d and /etc/containers/oci/hooks.d in order of increasing precedence. Using these defaults is deprecated. Migrate to explicitly setting --hooks-dir.</p></summary>
    [Argument(Format = "--hooks-dir={value}")] public Nuke.Common.IO.AbsolutePath HooksDir => Get<Nuke.Common.IO.AbsolutePath>(() => HooksDir);
    /// <summary><p>By default proxy environment variables are passed into the container if set for the Podman process. This can be disabled by setting the value to <see langword='false'/>. The environment variables passed in include <strong>http_proxy</strong>, <strong>https_proxy</strong>, <strong>ftp_proxy</strong>, <strong>no_proxy</strong>, and also the upper case versions of those. This option is only needed when the host system must use a proxy but the container does not use any proxy. Proxy environment variables specified for the container in any other way overrides the values that have been passed through from the host. (Other ways to specify the proxy for the container include passing the values with the <strong>--env</strong> flag, or hard coding the proxy environment at container build time.) When used with the remote client it uses the proxy environment variables that are set on the server process.</p><p>Defaults to <see langword='true'/>.</p></summary>
    [Argument(Format = "--http-proxy={value}")] public bool? HttpProxy => Get<bool?>(() => HttpProxy);
    /// <summary><p>Adds default identity label io.buildah.version if set. (default <see langword='true'/>).</p></summary>
    [Argument(Format = "--identity-label={value}")] public bool? IdentityLabel => Get<bool?>(() => IdentityLabel);
    /// <summary><p>Path to an alternative <c>.containerignore</c> file.</p></summary>
    [Argument(Format = "--ignore-file={value}")] public Nuke.Common.IO.AbsolutePath IgnoreFile => Get<Nuke.Common.IO.AbsolutePath>(() => IgnoreFile);
    /// <summary><p>Write the built image’s ID to the file. When <c>--platform</c> is specified more than once, attempting to use this option triggers an error.</p></summary>
    [Argument(Format = "--iidfile={value}")] public string Iidfile => Get<string>(() => Iidfile);
    /// <summary><p>Inherit the annotations from the base image or base stages. (default <see langword='true'/>). Use cases which set this flag to <see langword='false'/> may need to do the same for the <c>--created-annotation</c> flag.</p></summary>
    [Argument(Format = "--inherit-annotations={value}")] public bool? InheritAnnotations => Get<bool?>(() => InheritAnnotations);
    /// <summary><p>Inherit the labels from the base image or base stages. (default <see langword='true'/>).</p></summary>
    [Argument(Format = "--inherit-labels={value}")] public bool? InheritLabels => Get<bool?>(() => InheritLabels);
    /// <summary><p>Sets the configuration for IPC namespaces when handling <c>RUN</c> instructions. The configured value can be “” (the empty string) or “container” to indicate that a new IPC namespace is created, or it can be “host” to indicate that the IPC namespace in which <c>podman</c> itself is being run is reused, or it can be the path to an IPC namespace which is already in use by another process.</p></summary>
    [Argument(Format = "--ipc={value}")] public string Ipc => Get<string>(() => Ipc);
    /// <summary><p>Controls what type of isolation is used for running processes as part of <c>RUN</c> instructions. Recognized types include <i>oci</i> (OCI-compatible runtime, the default), <i>rootless</i> (OCI-compatible runtime invoked using a modified configuration and its --rootless option enabled, with <c>--no-new-keyring</c> <c>--no-pivot</c> added to its create invocation, with network and UTS namespaces disabled, and IPC, PID, and user namespaces enabled; the default for unprivileged users), and chroot (an internal wrapper that leans more toward chroot(1) than container technology).</p><p>Note: You can also override the default isolation type by setting the BUILDAH_ISOLATION environment variable. <c>export BUILDAH_ISOLATION=oci</c>.</p></summary>
    [Argument(Format = "--isolation={value}")] public string Isolation => Get<string>(() => Isolation);
    /// <summary><p>Run up to N concurrent stages in parallel. If the number of jobs is greater than 1, stdin is read from <i>/dev/null</i>. If 0 is specified, then there is no limit in the number of jobs that run in parallel.</p></summary>
    [Argument(Format = "--jobs={value}")] public uint? Jobs => Get<uint?>(() => Jobs);
    /// <summary><p>Add an image label (e.g. label=value) to the image metadata. Can be used multiple times.</p><p>Users can set a special LABEL <strong>io.containers.capabilities</strong>=<strong>CAP1</strong>,<strong>CAP2</strong>,<strong>CAP3</strong> in a Containerfile that specifies the list of Linux capabilities required for the container to run properly. This label specified in a container image tells Podman to run the container with just these capabilities. Podman launches the container with just the specified capabilities, as long as this list of capabilities is a subset of the default list.</p><p>If the specified capabilities are not in the default set, Podman prints an error message and runs the container with the default capabilities.</p></summary>
    [Argument(Format = "--label={value}", Separator = ",")] public IEnumerable<string> Label => Get<IEnumerable<string>>(() => Label);
    /// <summary><p>Add an intermediate image <i>label</i> (e.g. label=<c>value</c>) to the intermediate image metadata. It can be used multiple times.</p><p>If label is named, but neither <c>=</c> nor a <c>value</c> is provided, then the <i>label</i> is set to an empty value.</p></summary>
    [Argument(Format = "--layer-label={value}")] public string LayerLabel => Get<string>(() => LayerLabel);
    /// <summary><p>Cache intermediate images during the build process (Default is <see langword='true'/>).</p><p>Note: You can also override the default value of layers by setting the BUILDAH_LAYERS environment variable. <c>export BUILDAH_LAYERS=true</c></p></summary>
    [Argument(Format = "--layers")] public bool? Layers => Get<bool?>(() => Layers);
    /// <summary><p>Log output which is sent to standard output and standard error to the specified file instead of to standard output and standard error. This option is not supported on the remote client, including Mac and Windows (excluding WSL2) machines.</p></summary>
    [Argument(Format = "--logfile={value}")] public string LogFile => Get<string>(() => LogFile);
    /// <summary><p>If <c>--logfile</c> and <c>--platform</c> are specified, the <c>--logsplit</c> option allows end-users to split the log file for each platform into different files in the following format: <c>${logfile}_${platform-os}_${platform-arch}</c>. This option is not supported on the remote client, including Mac and Windows (excluding WSL2) machines.</p></summary>
    [Argument(Format = "--log-split={value}")] public bool? LogSplit => Get<bool?>(() => LogSplit);
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
    #region Annotation
    /// <inheritdoc cref="PodmanBuildSettings.Annotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Annotation))]
    public static T SetAnnotation<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Annotation, v));
    /// <inheritdoc cref="PodmanBuildSettings.Annotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Annotation))]
    public static T ResetAnnotation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Annotation));
    #endregion
    #region Arch
    /// <inheritdoc cref="PodmanBuildSettings.Arch"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Arch))]
    public static T SetArch<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Arch, v));
    /// <inheritdoc cref="PodmanBuildSettings.Arch"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Arch))]
    public static T ResetArch<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Arch));
    #endregion
    #region AuthFile
    /// <inheritdoc cref="PodmanBuildSettings.AuthFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AuthFile))]
    public static T SetAuthFile<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.AuthFile, v));
    /// <inheritdoc cref="PodmanBuildSettings.AuthFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.AuthFile))]
    public static T ResetAuthFile<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.AuthFile));
    #endregion
    #region BuildArg
    /// <inheritdoc cref="PodmanBuildSettings.BuildArg"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.BuildArg))]
    public static T SetBuildArg<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.BuildArg, v));
    /// <inheritdoc cref="PodmanBuildSettings.BuildArg"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.BuildArg))]
    public static T ResetBuildArg<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.BuildArg));
    #endregion
    #region BuildArgFile
    /// <inheritdoc cref="PodmanBuildSettings.BuildArgFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.BuildArgFile))]
    public static T SetBuildArgFile<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.BuildArgFile, v));
    /// <inheritdoc cref="PodmanBuildSettings.BuildArgFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.BuildArgFile))]
    public static T ResetBuildArgFile<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.BuildArgFile));
    #endregion
    #region BuildContext
    /// <inheritdoc cref="PodmanBuildSettings.BuildContext"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.BuildContext))]
    public static T SetBuildContext<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.BuildContext, v));
    /// <inheritdoc cref="PodmanBuildSettings.BuildContext"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.BuildContext))]
    public static T ResetBuildContext<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.BuildContext));
    #endregion
    #region CacheFrom
    /// <inheritdoc cref="PodmanBuildSettings.CacheFrom"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CacheFrom))]
    public static T SetCacheFrom<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CacheFrom, v));
    /// <inheritdoc cref="PodmanBuildSettings.CacheFrom"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CacheFrom))]
    public static T ResetCacheFrom<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CacheFrom));
    #endregion
    #region CacheTo
    /// <inheritdoc cref="PodmanBuildSettings.CacheTo"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CacheTo))]
    public static T SetCacheTo<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CacheTo, v));
    /// <inheritdoc cref="PodmanBuildSettings.CacheTo"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CacheTo))]
    public static T ResetCacheTo<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CacheTo));
    #endregion
    #region CacheTtl
    /// <inheritdoc cref="PodmanBuildSettings.CacheTtl"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CacheTtl))]
    public static T SetCacheTtl<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CacheTtl, v));
    /// <inheritdoc cref="PodmanBuildSettings.CacheTtl"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CacheTtl))]
    public static T ResetCacheTtl<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CacheTtl));
    #endregion
    #region CapAdd
    /// <inheritdoc cref="PodmanBuildSettings.CapAdd"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CapAdd))]
    public static T SetCapAdd<T>(this T o, IEnumerable<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CapAdd, v));
    /// <inheritdoc cref="PodmanBuildSettings.CapAdd"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CapAdd))]
    public static T ResetCapAdd<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CapAdd));
    #endregion
    #region CapDrop
    /// <inheritdoc cref="PodmanBuildSettings.CapDrop"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CapDrop))]
    public static T SetCapDrop<T>(this T o, IEnumerable<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CapDrop, v));
    /// <inheritdoc cref="PodmanBuildSettings.CapDrop"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CapDrop))]
    public static T ResetCapDrop<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CapDrop));
    #endregion
    #region CertDir
    /// <inheritdoc cref="PodmanBuildSettings.CertDir"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CertDir))]
    public static T SetCertDir<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CertDir, v));
    /// <inheritdoc cref="PodmanBuildSettings.CertDir"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CertDir))]
    public static T ResetCertDir<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CertDir));
    #endregion
    #region CgroupParent
    /// <inheritdoc cref="PodmanBuildSettings.CgroupParent"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CgroupParent))]
    public static T SetCgroupParent<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CgroupParent, v));
    /// <inheritdoc cref="PodmanBuildSettings.CgroupParent"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CgroupParent))]
    public static T ResetCgroupParent<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CgroupParent));
    #endregion
    #region Cgroupns
    /// <inheritdoc cref="PodmanBuildSettings.Cgroupns"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Cgroupns))]
    public static T SetCgroupns<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Cgroupns, v));
    /// <inheritdoc cref="PodmanBuildSettings.Cgroupns"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Cgroupns))]
    public static T ResetCgroupns<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Cgroupns));
    #endregion
    #region CompatVolumes
    /// <inheritdoc cref="PodmanBuildSettings.CompatVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CompatVolumes))]
    public static T SetCompatVolumes<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CompatVolumes, v));
    /// <inheritdoc cref="PodmanBuildSettings.CompatVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CompatVolumes))]
    public static T ResetCompatVolumes<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CompatVolumes));
    /// <inheritdoc cref="PodmanBuildSettings.CompatVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CompatVolumes))]
    public static T EnableCompatVolumes<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CompatVolumes, true));
    /// <inheritdoc cref="PodmanBuildSettings.CompatVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CompatVolumes))]
    public static T DisableCompatVolumes<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CompatVolumes, false));
    /// <inheritdoc cref="PodmanBuildSettings.CompatVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CompatVolumes))]
    public static T ToggleCompatVolumes<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CompatVolumes, !o.CompatVolumes));
    #endregion
    #region Compress
    /// <inheritdoc cref="PodmanBuildSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Compress))]
    public static T SetCompress<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Compress, v));
    /// <inheritdoc cref="PodmanBuildSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Compress))]
    public static T ResetCompress<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Compress));
    /// <inheritdoc cref="PodmanBuildSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Compress))]
    public static T EnableCompress<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Compress, true));
    /// <inheritdoc cref="PodmanBuildSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Compress))]
    public static T DisableCompress<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Compress, false));
    /// <inheritdoc cref="PodmanBuildSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Compress))]
    public static T ToggleCompress<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Compress, !o.Compress));
    #endregion
    #region CppFlag
    /// <inheritdoc cref="PodmanBuildSettings.CppFlag"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CppFlag))]
    public static T SetCppFlag<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CppFlag, v));
    /// <inheritdoc cref="PodmanBuildSettings.CppFlag"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CppFlag))]
    public static T ResetCppFlag<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CppFlag));
    #endregion
    #region CpuPeriod
    /// <inheritdoc cref="PodmanBuildSettings.CpuPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpuPeriod))]
    public static T SetCpuPeriod<T>(this T o, int? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CpuPeriod, v));
    /// <inheritdoc cref="PodmanBuildSettings.CpuPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpuPeriod))]
    public static T ResetCpuPeriod<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CpuPeriod));
    #endregion
    #region CpuQuota
    /// <inheritdoc cref="PodmanBuildSettings.CpuQuota"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpuQuota))]
    public static T SetCpuQuota<T>(this T o, int? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CpuQuota, v));
    /// <inheritdoc cref="PodmanBuildSettings.CpuQuota"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpuQuota))]
    public static T ResetCpuQuota<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CpuQuota));
    #endregion
    #region CpuShares
    /// <inheritdoc cref="PodmanBuildSettings.CpuShares"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpuShares))]
    public static T SetCpuShares<T>(this T o, decimal? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CpuShares, v));
    /// <inheritdoc cref="PodmanBuildSettings.CpuShares"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpuShares))]
    public static T ResetCpuShares<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CpuShares));
    #endregion
    #region CpusetCpus
    /// <inheritdoc cref="PodmanBuildSettings.CpusetCpus"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpusetCpus))]
    public static T SetCpusetCpus<T>(this T o, IEnumerable<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CpusetCpus, v));
    /// <inheritdoc cref="PodmanBuildSettings.CpusetCpus"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpusetCpus))]
    public static T ResetCpusetCpus<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CpusetCpus));
    #endregion
    #region CpusetMems
    /// <inheritdoc cref="PodmanBuildSettings.CpusetMems"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpusetMems))]
    public static T SetCpusetMems<T>(this T o, IEnumerable<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CpusetMems, v));
    /// <inheritdoc cref="PodmanBuildSettings.CpusetMems"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CpusetMems))]
    public static T ResetCpusetMems<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CpusetMems));
    #endregion
    #region CreatedAnnotation
    /// <inheritdoc cref="PodmanBuildSettings.CreatedAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CreatedAnnotation))]
    public static T SetCreatedAnnotation<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CreatedAnnotation, v));
    /// <inheritdoc cref="PodmanBuildSettings.CreatedAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CreatedAnnotation))]
    public static T ResetCreatedAnnotation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.CreatedAnnotation));
    /// <inheritdoc cref="PodmanBuildSettings.CreatedAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CreatedAnnotation))]
    public static T EnableCreatedAnnotation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CreatedAnnotation, true));
    /// <inheritdoc cref="PodmanBuildSettings.CreatedAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CreatedAnnotation))]
    public static T DisableCreatedAnnotation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CreatedAnnotation, false));
    /// <inheritdoc cref="PodmanBuildSettings.CreatedAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.CreatedAnnotation))]
    public static T ToggleCreatedAnnotation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.CreatedAnnotation, !o.CreatedAnnotation));
    #endregion
    #region Creds
    /// <inheritdoc cref="PodmanBuildSettings.Creds"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Creds))]
    public static T SetCreds<T>(this T o, [Secret] string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Creds, v));
    /// <inheritdoc cref="PodmanBuildSettings.Creds"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Creds))]
    public static T ResetCreds<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Creds));
    #endregion
    #region Cw
    /// <inheritdoc cref="PodmanBuildSettings.Cw"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Cw))]
    public static T SetCw<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Cw, v));
    /// <inheritdoc cref="PodmanBuildSettings.Cw"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Cw))]
    public static T ResetCw<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Cw));
    #endregion
    #region DescriptionKey
    /// <inheritdoc cref="PodmanBuildSettings.DescriptionKey"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DescriptionKey))]
    public static T SetDescriptionKey<T>(this T o, [Secret] string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DescriptionKey, v));
    /// <inheritdoc cref="PodmanBuildSettings.DescriptionKey"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DescriptionKey))]
    public static T ResetDescriptionKey<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.DescriptionKey));
    #endregion
    #region Device
    /// <inheritdoc cref="PodmanBuildSettings.Device"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Device))]
    public static T SetDevice<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Device, v));
    /// <inheritdoc cref="PodmanBuildSettings.Device"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Device))]
    public static T ResetDevice<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Device));
    #endregion
    #region DisableCompression
    /// <inheritdoc cref="PodmanBuildSettings.DisableCompression"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableCompression))]
    public static T SetDisableCompression<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableCompression, v));
    /// <inheritdoc cref="PodmanBuildSettings.DisableCompression"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableCompression))]
    public static T ResetDisableCompression<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.DisableCompression));
    /// <inheritdoc cref="PodmanBuildSettings.DisableCompression"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableCompression))]
    public static T EnableDisableCompression<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableCompression, true));
    /// <inheritdoc cref="PodmanBuildSettings.DisableCompression"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableCompression))]
    public static T DisableDisableCompression<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableCompression, false));
    /// <inheritdoc cref="PodmanBuildSettings.DisableCompression"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableCompression))]
    public static T ToggleDisableCompression<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableCompression, !o.DisableCompression));
    #endregion
    #region DisableContentTrust
    /// <inheritdoc cref="PodmanBuildSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableContentTrust))]
    public static T SetDisableContentTrust<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, v));
    /// <inheritdoc cref="PodmanBuildSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableContentTrust))]
    public static T ResetDisableContentTrust<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.DisableContentTrust));
    /// <inheritdoc cref="PodmanBuildSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableContentTrust))]
    public static T EnableDisableContentTrust<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, true));
    /// <inheritdoc cref="PodmanBuildSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableContentTrust))]
    public static T DisableDisableContentTrust<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, false));
    /// <inheritdoc cref="PodmanBuildSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DisableContentTrust))]
    public static T ToggleDisableContentTrust<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, !o.DisableContentTrust));
    #endregion
    #region Dns
    /// <inheritdoc cref="PodmanBuildSettings.Dns"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Dns))]
    public static T SetDns<T>(this T o, System.Net.IPAddress v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Dns, v));
    /// <inheritdoc cref="PodmanBuildSettings.Dns"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Dns))]
    public static T ResetDns<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Dns));
    #endregion
    #region DnsOption
    /// <inheritdoc cref="PodmanBuildSettings.DnsOption"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DnsOption))]
    public static T SetDnsOption<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DnsOption, v));
    /// <inheritdoc cref="PodmanBuildSettings.DnsOption"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DnsOption))]
    public static T ResetDnsOption<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.DnsOption));
    #endregion
    #region DnsSearch
    /// <inheritdoc cref="PodmanBuildSettings.DnsSearch"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DnsSearch))]
    public static T SetDnsSearch<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.DnsSearch, v));
    /// <inheritdoc cref="PodmanBuildSettings.DnsSearch"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.DnsSearch))]
    public static T ResetDnsSearch<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.DnsSearch));
    #endregion
    #region Env
    /// <inheritdoc cref="PodmanBuildSettings.Env"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Env))]
    public static T SetEnv<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Env, v));
    /// <inheritdoc cref="PodmanBuildSettings.Env"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Env))]
    public static T ResetEnv<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Env));
    #endregion
    #region File
    /// <inheritdoc cref="PodmanBuildSettings.File"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.File))]
    public static T SetFile<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.File, v));
    /// <inheritdoc cref="PodmanBuildSettings.File"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.File))]
    public static T ResetFile<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.File));
    #endregion
    #region ForceRm
    /// <inheritdoc cref="PodmanBuildSettings.ForceRm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ForceRm))]
    public static T SetForceRm<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.ForceRm, v));
    /// <inheritdoc cref="PodmanBuildSettings.ForceRm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ForceRm))]
    public static T ResetForceRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.ForceRm));
    /// <inheritdoc cref="PodmanBuildSettings.ForceRm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ForceRm))]
    public static T EnableForceRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.ForceRm, true));
    /// <inheritdoc cref="PodmanBuildSettings.ForceRm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ForceRm))]
    public static T DisableForceRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.ForceRm, false));
    /// <inheritdoc cref="PodmanBuildSettings.ForceRm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ForceRm))]
    public static T ToggleForceRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.ForceRm, !o.ForceRm));
    #endregion
    #region Format
    /// <inheritdoc cref="PodmanBuildSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Format))]
    public static T SetFormat<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Format, v));
    /// <inheritdoc cref="PodmanBuildSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Format))]
    public static T ResetFormat<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Format));
    #endregion
    #region From
    /// <inheritdoc cref="PodmanBuildSettings.From"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.From))]
    public static T SetFrom<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.From, v));
    /// <inheritdoc cref="PodmanBuildSettings.From"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.From))]
    public static T ResetFrom<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.From));
    #endregion
    #region GroupAdd
    /// <inheritdoc cref="PodmanBuildSettings.GroupAdd"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.GroupAdd))]
    public static T SetGroupAdd<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.GroupAdd, v));
    /// <inheritdoc cref="PodmanBuildSettings.GroupAdd"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.GroupAdd))]
    public static T ResetGroupAdd<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.GroupAdd));
    #endregion
    #region Help
    /// <inheritdoc cref="PodmanBuildSettings.Help"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Help))]
    public static T SetHelp<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Help, v));
    /// <inheritdoc cref="PodmanBuildSettings.Help"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Help))]
    public static T ResetHelp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Help));
    /// <inheritdoc cref="PodmanBuildSettings.Help"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Help))]
    public static T EnableHelp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Help, true));
    /// <inheritdoc cref="PodmanBuildSettings.Help"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Help))]
    public static T DisableHelp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Help, false));
    /// <inheritdoc cref="PodmanBuildSettings.Help"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Help))]
    public static T ToggleHelp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Help, !o.Help));
    #endregion
    #region HooksDir
    /// <inheritdoc cref="PodmanBuildSettings.HooksDir"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HooksDir))]
    public static T SetHooksDir<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.HooksDir, v));
    /// <inheritdoc cref="PodmanBuildSettings.HooksDir"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HooksDir))]
    public static T ResetHooksDir<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.HooksDir));
    #endregion
    #region HttpProxy
    /// <inheritdoc cref="PodmanBuildSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HttpProxy))]
    public static T SetHttpProxy<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.HttpProxy, v));
    /// <inheritdoc cref="PodmanBuildSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HttpProxy))]
    public static T ResetHttpProxy<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.HttpProxy));
    /// <inheritdoc cref="PodmanBuildSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HttpProxy))]
    public static T EnableHttpProxy<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.HttpProxy, true));
    /// <inheritdoc cref="PodmanBuildSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HttpProxy))]
    public static T DisableHttpProxy<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.HttpProxy, false));
    /// <inheritdoc cref="PodmanBuildSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.HttpProxy))]
    public static T ToggleHttpProxy<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.HttpProxy, !o.HttpProxy));
    #endregion
    #region IdentityLabel
    /// <inheritdoc cref="PodmanBuildSettings.IdentityLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IdentityLabel))]
    public static T SetIdentityLabel<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.IdentityLabel, v));
    /// <inheritdoc cref="PodmanBuildSettings.IdentityLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IdentityLabel))]
    public static T ResetIdentityLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.IdentityLabel));
    /// <inheritdoc cref="PodmanBuildSettings.IdentityLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IdentityLabel))]
    public static T EnableIdentityLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.IdentityLabel, true));
    /// <inheritdoc cref="PodmanBuildSettings.IdentityLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IdentityLabel))]
    public static T DisableIdentityLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.IdentityLabel, false));
    /// <inheritdoc cref="PodmanBuildSettings.IdentityLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IdentityLabel))]
    public static T ToggleIdentityLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.IdentityLabel, !o.IdentityLabel));
    #endregion
    #region IgnoreFile
    /// <inheritdoc cref="PodmanBuildSettings.IgnoreFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IgnoreFile))]
    public static T SetIgnoreFile<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.IgnoreFile, v));
    /// <inheritdoc cref="PodmanBuildSettings.IgnoreFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.IgnoreFile))]
    public static T ResetIgnoreFile<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.IgnoreFile));
    #endregion
    #region Iidfile
    /// <inheritdoc cref="PodmanBuildSettings.Iidfile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Iidfile))]
    public static T SetIidfile<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Iidfile, v));
    /// <inheritdoc cref="PodmanBuildSettings.Iidfile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Iidfile))]
    public static T ResetIidfile<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Iidfile));
    #endregion
    #region InheritAnnotations
    /// <inheritdoc cref="PodmanBuildSettings.InheritAnnotations"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritAnnotations))]
    public static T SetInheritAnnotations<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritAnnotations, v));
    /// <inheritdoc cref="PodmanBuildSettings.InheritAnnotations"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritAnnotations))]
    public static T ResetInheritAnnotations<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.InheritAnnotations));
    /// <inheritdoc cref="PodmanBuildSettings.InheritAnnotations"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritAnnotations))]
    public static T EnableInheritAnnotations<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritAnnotations, true));
    /// <inheritdoc cref="PodmanBuildSettings.InheritAnnotations"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritAnnotations))]
    public static T DisableInheritAnnotations<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritAnnotations, false));
    /// <inheritdoc cref="PodmanBuildSettings.InheritAnnotations"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritAnnotations))]
    public static T ToggleInheritAnnotations<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritAnnotations, !o.InheritAnnotations));
    #endregion
    #region InheritLabels
    /// <inheritdoc cref="PodmanBuildSettings.InheritLabels"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritLabels))]
    public static T SetInheritLabels<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritLabels, v));
    /// <inheritdoc cref="PodmanBuildSettings.InheritLabels"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritLabels))]
    public static T ResetInheritLabels<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.InheritLabels));
    /// <inheritdoc cref="PodmanBuildSettings.InheritLabels"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritLabels))]
    public static T EnableInheritLabels<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritLabels, true));
    /// <inheritdoc cref="PodmanBuildSettings.InheritLabels"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritLabels))]
    public static T DisableInheritLabels<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritLabels, false));
    /// <inheritdoc cref="PodmanBuildSettings.InheritLabels"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.InheritLabels))]
    public static T ToggleInheritLabels<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.InheritLabels, !o.InheritLabels));
    #endregion
    #region Ipc
    /// <inheritdoc cref="PodmanBuildSettings.Ipc"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Ipc))]
    public static T SetIpc<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Ipc, v));
    /// <inheritdoc cref="PodmanBuildSettings.Ipc"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Ipc))]
    public static T ResetIpc<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Ipc));
    #endregion
    #region Isolation
    /// <inheritdoc cref="PodmanBuildSettings.Isolation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Isolation))]
    public static T SetIsolation<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Isolation, v));
    /// <inheritdoc cref="PodmanBuildSettings.Isolation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Isolation))]
    public static T ResetIsolation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Isolation));
    #endregion
    #region Jobs
    /// <inheritdoc cref="PodmanBuildSettings.Jobs"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Jobs))]
    public static T SetJobs<T>(this T o, uint? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Jobs, v));
    /// <inheritdoc cref="PodmanBuildSettings.Jobs"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Jobs))]
    public static T ResetJobs<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Jobs));
    #endregion
    #region Label
    /// <inheritdoc cref="PodmanBuildSettings.Label"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Label))]
    public static T SetLabel<T>(this T o, IEnumerable<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Label, v));
    /// <inheritdoc cref="PodmanBuildSettings.Label"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Label))]
    public static T ResetLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Label));
    #endregion
    #region LayerLabel
    /// <inheritdoc cref="PodmanBuildSettings.LayerLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LayerLabel))]
    public static T SetLayerLabel<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.LayerLabel, v));
    /// <inheritdoc cref="PodmanBuildSettings.LayerLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LayerLabel))]
    public static T ResetLayerLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.LayerLabel));
    #endregion
    #region Layers
    /// <inheritdoc cref="PodmanBuildSettings.Layers"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Layers))]
    public static T SetLayers<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Layers, v));
    /// <inheritdoc cref="PodmanBuildSettings.Layers"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Layers))]
    public static T ResetLayers<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Layers));
    /// <inheritdoc cref="PodmanBuildSettings.Layers"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Layers))]
    public static T EnableLayers<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Layers, true));
    /// <inheritdoc cref="PodmanBuildSettings.Layers"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Layers))]
    public static T DisableLayers<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Layers, false));
    /// <inheritdoc cref="PodmanBuildSettings.Layers"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Layers))]
    public static T ToggleLayers<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Layers, !o.Layers));
    #endregion
    #region LogFile
    /// <inheritdoc cref="PodmanBuildSettings.LogFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogFile))]
    public static T SetLogFile<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.LogFile, v));
    /// <inheritdoc cref="PodmanBuildSettings.LogFile"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogFile))]
    public static T ResetLogFile<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.LogFile));
    #endregion
    #region LogSplit
    /// <inheritdoc cref="PodmanBuildSettings.LogSplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogSplit))]
    public static T SetLogSplit<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.LogSplit, v));
    /// <inheritdoc cref="PodmanBuildSettings.LogSplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogSplit))]
    public static T ResetLogSplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.LogSplit));
    /// <inheritdoc cref="PodmanBuildSettings.LogSplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogSplit))]
    public static T EnableLogSplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.LogSplit, true));
    /// <inheritdoc cref="PodmanBuildSettings.LogSplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogSplit))]
    public static T DisableLogSplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.LogSplit, false));
    /// <inheritdoc cref="PodmanBuildSettings.LogSplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.LogSplit))]
    public static T ToggleLogSplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.LogSplit, !o.LogSplit));
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
