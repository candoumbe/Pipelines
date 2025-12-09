
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
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--add-host</c> via <see cref="PodmanBuildSettings.AddHost"/></li><li><c>--all-platforms</c> via <see cref="PodmanBuildSettings.AllPlatforms"/></li><li><c>--annotation</c> via <see cref="PodmanBuildSettings.Annotation"/></li><li><c>--arch</c> via <see cref="PodmanBuildSettings.Arch"/></li><li><c>--authfile</c> via <see cref="PodmanBuildSettings.AuthFile"/></li><li><c>--build-arg</c> via <see cref="PodmanBuildSettings.BuildArg"/></li><li><c>--build-arg-file</c> via <see cref="PodmanBuildSettings.BuildArgFile"/></li><li><c>--build-context</c> via <see cref="PodmanBuildSettings.BuildContext"/></li><li><c>--cache-from</c> via <see cref="PodmanBuildSettings.CacheFrom"/></li><li><c>--cache-to</c> via <see cref="PodmanBuildSettings.CacheTo"/></li><li><c>--cache-ttl</c> via <see cref="PodmanBuildSettings.CacheTtl"/></li><li><c>--cap-add</c> via <see cref="PodmanBuildSettings.CapAdd"/></li><li><c>--cap-drop</c> via <see cref="PodmanBuildSettings.CapDrop"/></li><li><c>--cert-dir</c> via <see cref="PodmanBuildSettings.CertDir"/></li><li><c>--cgroup-parent</c> via <see cref="PodmanBuildSettings.CgroupParent"/></li><li><c>--cgroupns</c> via <see cref="PodmanBuildSettings.Cgroupns"/></li><li><c>--compat-volumes</c> via <see cref="PodmanBuildSettings.CompatVolumes"/></li><li><c>--compress</c> via <see cref="PodmanBuildSettings.Compress"/></li><li><c>--cpp-flag</c> via <see cref="PodmanBuildSettings.CppFlag"/></li><li><c>--cpu-period</c> via <see cref="PodmanBuildSettings.CpuPeriod"/></li><li><c>--cpu-quota</c> via <see cref="PodmanBuildSettings.CpuQuota"/></li><li><c>--cpu-shares</c> via <see cref="PodmanBuildSettings.CpuShares"/></li><li><c>--cpuset-cpus</c> via <see cref="PodmanBuildSettings.CpusetCpus"/></li><li><c>--cpuset-mems</c> via <see cref="PodmanBuildSettings.CpusetMems"/></li><li><c>--created-annotation</c> via <see cref="PodmanBuildSettings.CreatedAnnotation"/></li><li><c>--creds</c> via <see cref="PodmanBuildSettings.Creds"/></li><li><c>--cw</c> via <see cref="PodmanBuildSettings.Cw"/></li><li><c>--description-key</c> via <see cref="PodmanBuildSettings.DescriptionKey"/></li><li><c>--device</c> via <see cref="PodmanBuildSettings.Device"/></li><li><c>--disable-compression</c> via <see cref="PodmanBuildSettings.DisableCompression"/></li><li><c>--disable-content-trust</c> via <see cref="PodmanBuildSettings.DisableContentTrust"/></li><li><c>--dns</c> via <see cref="PodmanBuildSettings.Dns"/></li><li><c>--dns-option</c> via <see cref="PodmanBuildSettings.DnsOption"/></li><li><c>--dns-search</c> via <see cref="PodmanBuildSettings.DnsSearch"/></li><li><c>--env</c> via <see cref="PodmanBuildSettings.Env"/></li><li><c>--file</c> via <see cref="PodmanBuildSettings.File"/></li><li><c>--force-rm</c> via <see cref="PodmanBuildSettings.ForceRm"/></li><li><c>--format</c> via <see cref="PodmanBuildSettings.Format"/></li><li><c>--from</c> via <see cref="PodmanBuildSettings.From"/></li><li><c>--group-add</c> via <see cref="PodmanBuildSettings.GroupAdd"/></li><li><c>--help</c> via <see cref="PodmanBuildSettings.Help"/></li><li><c>--hooks-dir</c> via <see cref="PodmanBuildSettings.HooksDir"/></li><li><c>--http-proxy</c> via <see cref="PodmanBuildSettings.HttpProxy"/></li><li><c>--identity-label</c> via <see cref="PodmanBuildSettings.IdentityLabel"/></li><li><c>--ignore-file</c> via <see cref="PodmanBuildSettings.IgnoreFile"/></li><li><c>--iidfile</c> via <see cref="PodmanBuildSettings.Iidfile"/></li><li><c>--inherit-annotations</c> via <see cref="PodmanBuildSettings.InheritAnnotations"/></li><li><c>--inherit-labels</c> via <see cref="PodmanBuildSettings.InheritLabels"/></li><li><c>--ipc</c> via <see cref="PodmanBuildSettings.Ipc"/></li><li><c>--isolation</c> via <see cref="PodmanBuildSettings.Isolation"/></li><li><c>--jobs</c> via <see cref="PodmanBuildSettings.Jobs"/></li><li><c>--label</c> via <see cref="PodmanBuildSettings.Label"/></li><li><c>--layer-label</c> via <see cref="PodmanBuildSettings.LayerLabel"/></li><li><c>--layers</c> via <see cref="PodmanBuildSettings.Layers"/></li><li><c>--logfile</c> via <see cref="PodmanBuildSettings.LogFile"/></li><li><c>--logsplit</c> via <see cref="PodmanBuildSettings.Logsplit"/></li><li><c>--manifest</c> via <see cref="PodmanBuildSettings.Manifest"/></li><li><c>--memory</c> via <see cref="PodmanBuildSettings.Memory"/></li><li><c>--memory-swap</c> via <see cref="PodmanBuildSettings.MemorySwap"/></li><li><c>--network</c> via <see cref="PodmanBuildSettings.Network"/></li><li><c>--no-cache</c> via <see cref="PodmanBuildSettings.NoCache"/></li><li><c>--no-hostname</c> via <see cref="PodmanBuildSettings.NoHostname"/></li><li><c>--no-hosts</c> via <see cref="PodmanBuildSettings.NoHosts"/></li><li><c>--omit-history</c> via <see cref="PodmanBuildSettings.OmitHistory"/></li><li><c>--os</c> via <see cref="PodmanBuildSettings.Os"/></li><li><c>--os-feature</c> via <see cref="PodmanBuildSettings.OsFeature"/></li><li><c>--os-version</c> via <see cref="PodmanBuildSettings.OsVersion"/></li><li><c>--output</c> via <see cref="PodmanBuildSettings.Output"/></li><li><c>--pid</c> via <see cref="PodmanBuildSettings.Pid"/></li><li><c>--platform</c> via <see cref="PodmanBuildSettings.Platform"/></li><li><c>--pull</c> via <see cref="PodmanBuildSettings.Pull"/></li><li><c>--quiet</c> via <see cref="PodmanBuildSettings.Quiet"/></li><li><c>--retry</c> via <see cref="PodmanBuildSettings.Retry"/></li><li><c>--retry-delay</c> via <see cref="PodmanBuildSettings.RetryDelay"/></li><li><c>--rewrite-timestamp</c> via <see cref="PodmanBuildSettings.RewriteTimestamp"/></li><li><c>--rm</c> via <see cref="PodmanBuildSettings.Rm"/></li><li><c>--runtime</c> via <see cref="PodmanBuildSettings.Runtime"/></li><li><c>--runtime-flag</c> via <see cref="PodmanBuildSettings.RuntimeFlag"/></li><li><c>--sbom</c> via <see cref="PodmanBuildSettings.Sbom"/></li><li><c>--sbom-image-output</c> via <see cref="PodmanBuildSettings.SbomImpageOutput"/></li><li><c>--sbom-merge-strategy</c> via <see cref="PodmanBuildSettings.SbomMergeStrategy"/></li><li><c>--sbom-output</c> via <see cref="PodmanBuildSettings.SbomOutput"/></li><li><c>--sbom-purl-output</c> via <see cref="PodmanBuildSettings.SbomPurlOutput"/></li><li><c>--sbom-scanner-command</c> via <see cref="PodmanBuildSettings.SbomScannerCommand"/></li><li><c>--sbom-scanner-image</c> via <see cref="PodmanBuildSettings.SbomScannerImage"/></li><li><c>--secret</c> via <see cref="PodmanBuildSettings.Secret"/></li><li><c>--security-opt</c> via <see cref="PodmanBuildSettings.SecurityOpt"/></li><li><c>--shm-size</c> via <see cref="PodmanBuildSettings.ShmSize"/></li><li><c>--sign-by</c> via <see cref="PodmanBuildSettings.SignBy"/></li><li><c>--skip-unused-stages</c> via <see cref="PodmanBuildSettings.SkipUnusedStages"/></li><li><c>--source-date-epoch</c> via <see cref="PodmanBuildSettings.SourceDateEpoch"/></li><li><c>--squash</c> via <see cref="PodmanBuildSettings.Squash"/></li><li><c>--squash-all</c> via <see cref="PodmanBuildSettings.SquashAll"/></li><li><c>--ssh</c> via <see cref="PodmanBuildSettings.Ssh"/></li><li><c>--stdin</c> via <see cref="PodmanBuildSettings.Stdin"/></li><li><c>--tag</c> via <see cref="PodmanBuildSettings.Tag"/></li><li><c>--target</c> via <see cref="PodmanBuildSettings.Target"/></li><li><c>--timestamp</c> via <see cref="PodmanBuildSettings.Timestamp"/></li><li><c>--tls-verify</c> via <see cref="PodmanBuildSettings.TlsVerify"/></li><li><c>--ulimit</c> via <see cref="PodmanBuildSettings.Ulimit"/></li><li><c>--unsetannotation</c> via <see cref="PodmanBuildSettings.UnsetAnnotation"/></li><li><c>--unsetenv</c> via <see cref="PodmanBuildSettings.UnsetEnv"/></li><li><c>--unsetlabel</c> via <see cref="PodmanBuildSettings.UnsetLabel"/></li><li><c>--userns</c> via <see cref="PodmanBuildSettings.Userns"/></li><li><c>--userns-gid-map</c> via <see cref="PodmanBuildSettings.UsernsGidMap"/></li><li><c>--userns-uid-map</c> via <see cref="PodmanBuildSettings.UsernsGidMapGroup"/></li><li><c>--userns-uid-map</c> via <see cref="PodmanBuildSettings.UsernsUidMap"/></li><li><c>--userns-uid-map-user</c> via <see cref="PodmanBuildSettings.UsernsUidMapUser"/></li><li><c>--uts</c> via <see cref="PodmanBuildSettings.Uts"/></li><li><c>--variant</c> via <see cref="PodmanBuildSettings.Variant"/></li><li><c>--volume</c> via <see cref="PodmanBuildSettings.Volume"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanBuild(PodmanBuildSettings options = null) => new PodmanTasks().Run<PodmanBuildSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanBuild(Candoumbe.Pipelines.Tools.Podman.PodmanBuildSettings)"/>
    public static IReadOnlyCollection<Output> PodmanBuild(Configure<PodmanBuildSettings> configurator) => new PodmanTasks().Run<PodmanBuildSettings>(configurator.Invoke(new PodmanBuildSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanBuild(Candoumbe.Pipelines.Tools.Podman.PodmanBuildSettings)"/>
    public static IEnumerable<(PodmanBuildSettings Settings, IReadOnlyCollection<Output> Output)> PodmanBuild(CombinatorialConfigure<PodmanBuildSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanBuild, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Creates an image based on a changed container. The author of the image can be set using the <b>--author</b> OPTION. Various image instructions can be configured with the <b>--change</b> OPTION and a commit message can be set using the <b>--message</b> OPTION. The container and its processes aren’t paused while the image is committed. If this is not desired, the <b>--pause</b> OPTION can be set to <see langword="true"/>. When the commit is complete, Podman prints out the ID of the new image.</p><p>If <b>image</b> does not begin with a registry name component, <c>localhost</c> is added to the name. If <c>image</c> is not provided, the values for the <c>REPOSITORY</c> and <c>TAG</c> values of the created image is set to <c>&lt;none&gt;</c>.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--author</c> via <see cref="PodmanCommitSettings.Author"/></li><li><c>--change</c> via <see cref="PodmanCommitSettings.Change"/></li><li><c>--config</c> via <see cref="PodmanCommitSettings.Config"/></li><li><c>--format</c> via <see cref="PodmanCommitSettings.Format"/></li><li><c>--iidfile</c> via <see cref="PodmanCommitSettings.Iidfile"/></li><li><c>--include-volumes</c> via <see cref="PodmanCommitSettings.IncludeVolumes"/></li><li><c>--message</c> via <see cref="PodmanCommitSettings.Message"/></li><li><c>--pause</c> via <see cref="PodmanCommitSettings.Pause"/></li><li><c>--quiet</c> via <see cref="PodmanCommitSettings.Quiet"/></li><li><c>--squash</c> via <see cref="PodmanCommitSettings.Squash"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanCommit(PodmanCommitSettings options = null) => new PodmanTasks().Run<PodmanCommitSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanCommit(Candoumbe.Pipelines.Tools.Podman.PodmanCommitSettings)"/>
    public static IReadOnlyCollection<Output> PodmanCommit(Configure<PodmanCommitSettings> configurator) => new PodmanTasks().Run<PodmanCommitSettings>(configurator.Invoke(new PodmanCommitSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanCommit(Candoumbe.Pipelines.Tools.Podman.PodmanCommitSettings)"/>
    public static IEnumerable<(PodmanCommitSettings Settings, IReadOnlyCollection<Output> Output)> PodmanCommit(CombinatorialConfigure<PodmanCommitSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanCommit, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Copy files/folders between a container and the local filesystem</p> <p><b>podman cp</b> allows copying the contents of <b>src_path</b> to the <b>dest_path</b>. Files can be copied from a container to the local machine and vice versa or between two containers. If <c>-</c> is specified for either the <c> SRC_PATH</c> or <c>DEST_PATH</c>, one can also stream a tar archive from <c>STDIN</c> or to <c>STDOUT</c>.</p> <p>The containers can be either running or stopped and the <i>src_path</i> or <i>dest_path</i> can be a file or directory.</p> <p><i>IMPORTANT: <b>The podman cp</b> command assumes container paths are relative to the container’s root directory ( <c>/</c>), which means supplying the initial forward slash is optional and therefore sees <c> compassionate_darwin:/tmp/foo/myfile.txt</c> and <c>compassionate_darwin:tmp/foo/myfile.txt</c> as identical. </i></p> <p>Local machine paths can be an absolute or relative value. The command interprets a local machine’s relative paths as relative to the current working directory where <b>podman cp</b> is run.</p> <p>Assuming a path separator of <c>/</c>, a first argument of <b>src_path</b> and second argument of <b>dest_path</b>, the behavior is as follows: <p><b>src_path</b> specifies a file: <list type= 'bullet' > <item><b>dest_path</b> does not exist<list type= 'bullet' > <item>the file is saved to a file created at <b>dest_path</b> (note that the parent directory must exist).</item> </list> </item> <item><b>dest_path</b> exists and is a file<list type= 'bullet' > <item>the destination is overriden with source file 's contents.</item></list></item><item><b>dest_path</b> exists and is a directory<list type=' bullet '><item>the file is copied into this directory using the base name from <b>src_path</b></item></list></item></list></p></p><p><b>src_path</b> specifies a directory: <list type='bullet'><item><b>dest_path</b> does not exist <list type=' bullet'><item><b>dest_path</b> is created as a directory and the contents of the source directory are copied into this directory.</item></list></item><item><b>dest_path</b> exists and is a file<list type=' bullet '><item>Error condition: cannot copy a directory to a file</item></list></item><item><b>dest_path</b> exists and is a directory<list type='bullet'><item><b>src_path</b> ends with <c>/</c><list type='bullet'><item>the source directory is copied into this directory.</item></list></item><item><b>src_path</b> ends with <c>/.</c> (i.e. slash followed by dot)<list type='bullet'><item>the content of the source directory is copied into this directory</item></list></item></list></item></list></p><p>The command requires <b>src_path</b> and <b>dest_path</b> to exist according to the above rules.</p><p>If <b>src_path</b> is local and is a symbolic link, the symbolic target, is copied by default.</p><p>A <i>colon</i> ( : ) is used as a delimiter between a container and its path, it can also be used when specifying paths to a <b>src_path</b> or <b>dest_path</b> on a local machine, for example, <c>file:name.txt</c>.</p><p><i>IMPORTANT: while using a colon ( : ) in a local machine path, one must be explicit with a relative or absolute path, for example: <c>/path/to/file:name.txt</c> or <c>./file:name.txt</c>.</i></p><p>Using <c>-</c> as the <b>src_path</b> streams the contents of <c>STDIN</c> as a tar archive. The command extracts the content of the tar to the <c>DEST_PATH</c> in the container. In this case, <b>dest_path</b> must specify a directory. Using <c>-</c> as the <b>dest_path</b> streams the contents of the resource (can be a directory) as a tar archive to <c>STDOUT</c>.</p><p>Note that <c>podman cp</c> ignores permission errors when copying from a running rootless container. The TTY devices inside a rootless container are owned by the host’s root user and hence cannot be read inside the container’s user namespace.</p><p>Further note that <c>podman cp</c> does not support globbing (e.g., <c>cp dir/*.txt</c>). To copy multiple files from the host to the container use xargs(1) or find(1) (or similar tools for chaining commands) in conjunction with podman cp. To copy multiple files from the container to the host, use <c>podman mount CONTAINER</c> and operate on the returned mount point instead (see ALTERNATIVES below).</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--archive</c> via <see cref="PodmanCpSettings.Archive"/></li><li><c>--overwrite</c> via <see cref="PodmanCpSettings.Overwrite"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanCp(PodmanCpSettings options = null) => new PodmanTasks().Run<PodmanCpSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanCp(Candoumbe.Pipelines.Tools.Podman.PodmanCpSettings)"/>
    public static IReadOnlyCollection<Output> PodmanCp(Configure<PodmanCpSettings> configurator) => new PodmanTasks().Run<PodmanCpSettings>(configurator.Invoke(new PodmanCpSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanCp(Candoumbe.Pipelines.Tools.Podman.PodmanCpSettings)"/>
    public static IEnumerable<(PodmanCpSettings Settings, IReadOnlyCollection<Output> Output)> PodmanCp(CombinatorialConfigure<PodmanCpSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanCp, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Creates a writable container layer over the specified image and prepares it for running the specified command. The container ID is then printed to STDOUT. This is similar to <b>podman run -d</b> except the container is never started. Use the <b>podman start</b> <i>container</i> command to start the container at any point.</p><p>The initial status of the container created with <b>podman create</b> is ‘created’.</p><p>Default settings for flags are defined in containers.conf. Most settings for remote connections use the server’s <c>containers.conf</c>, except when documented in man pages.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--add-host</c> via <see cref="PodmanCreateSettings.AddHost"/></li><li><c>--annotation</c> via <see cref="PodmanCreateSettings.Annotation"/></li><li><c>--arch</c> via <see cref="PodmanCreateSettings.Arch"/></li><li><c>--attach</c> via <see cref="PodmanCreateSettings.Attach"/></li><li><c>--authfile</c> via <see cref="PodmanCreateSettings.AuthFile"/></li><li><c>--blkio-weight</c> via <see cref="PodmanCreateSettings.BlkioWeight"/></li><li><c>--blkio-weight-device</c> via <see cref="PodmanCreateSettings.BlkioWeightDevice"/></li><li><c>--cap-add</c> via <see cref="PodmanCreateSettings.CapAdd"/></li><li><c>--cap-drop</c> via <see cref="PodmanCreateSettings.CapDrop"/></li><li><c>--cgroup-conf</c> via <see cref="PodmanCreateSettings.CgroupConf"/></li><li><c>--cgroup-parent</c> via <see cref="PodmanCreateSettings.CgroupParent"/></li><li><c>--cgroupns</c> via <see cref="PodmanCreateSettings.CgroupNs"/></li><li><c>--cgroups</c> via <see cref="PodmanCreateSettings.Cgroups"/></li><li><c>--chrootdirs</c> via <see cref="PodmanCreateSettings.ChrootDirs"/></li><li><c>--cidfile</c> via <see cref="PodmanCreateSettings.CidFile"/></li><li><c>--conmon-pidfile</c> via <see cref="PodmanCreateSettings.ConmonPidFile"/></li><li><c>--cpu-period</c> via <see cref="PodmanCreateSettings.CpuPeriod"/></li><li><c>--cpu-quota</c> via <see cref="PodmanCreateSettings.CpuQuota"/></li><li><c>--cpu-rt-period</c> via <see cref="PodmanCreateSettings.CpuRtPeriod"/></li><li><c>--cpu-rt-runtime</c> via <see cref="PodmanCreateSettings.CpuRtRuntime"/></li><li><c>--cpuset-cpus</c> via <see cref="PodmanCreateSettings.CpusetCpus"/></li><li><c>--cpuset-mems</c> via <see cref="PodmanCreateSettings.CpusetMems"/></li><li><c>--decryption-key</c> via <see cref="PodmanCreateSettings.DecryptionKey"/></li><li><c>--device</c> via <see cref="PodmanCreateSettings.Device"/></li><li><c>--device-cgroup-rule</c> via <see cref="PodmanCreateSettings.DeviceCgroupRule"/></li><li><c>--device-read-bps</c> via <see cref="PodmanCreateSettings.DeviceReadBps"/></li><li><c>--device-read-iops</c> via <see cref="PodmanCreateSettings.DeviceReadIops"/></li><li><c>--device-write-bps</c> via <see cref="PodmanCreateSettings.DeviceWriteBps"/></li><li><c>--device-write-iops</c> via <see cref="PodmanCreateSettings.DeviceWriteIops"/></li><li><c>--disable-content-trust</c> via <see cref="PodmanCreateSettings.DisableContentTrust"/></li><li><c>--dns</c> via <see cref="PodmanCreateSettings.Dns"/></li><li><c>--dns-option</c> via <see cref="PodmanCreateSettings.DnsOption"/></li><li><c>--dns-search</c> via <see cref="PodmanCreateSettings.DnsSearch"/></li><li><c>--entrypoint</c> via <see cref="PodmanCreateSettings.EntryPoint"/></li><li><c>--env</c> via <see cref="PodmanCreateSettings.Env"/></li><li><c>--env-file</c> via <see cref="PodmanCreateSettings.EnvFile"/></li><li><c>--env-host</c> via <see cref="PodmanCreateSettings.EnvHost"/></li><li><c>--env-merge</c> via <see cref="PodmanCreateSettings.EnvMerge"/></li><li><c>--expose</c> via <see cref="PodmanCreateSettings.Expose"/></li><li><c>--gidmap</c> via <see cref="PodmanCreateSettings.GidMap"/></li><li><c>--gpus</c> via <see cref="PodmanCreateSettings.Gpus"/></li><li><c>--group-add</c> via <see cref="PodmanCreateSettings.GroupAdd"/></li><li><c>--group-entry</c> via <see cref="PodmanCreateSettings.GroupEntry"/></li><li><c>--health-cmd</c> via <see cref="PodmanCreateSettings.HealthCmd"/></li><li><c>--health-interval</c> via <see cref="PodmanCreateSettings.HealthInterval"/></li><li><c>--health-log-destination</c> via <see cref="PodmanCreateSettings.HealthLogDestination"/></li><li><c>--health-max-log-count</c> via <see cref="PodmanCreateSettings.HealthMaxLogCount"/></li><li><c>--health-max-log-size</c> via <see cref="PodmanCreateSettings.HealthMaxLogSize"/></li><li><c>--health-on-failure</c> via <see cref="PodmanCreateSettings.HealthOnFailure"/></li><li><c>--health-retries</c> via <see cref="PodmanCreateSettings.HealthRetries"/></li><li><c>--health-start-period</c> via <see cref="PodmanCreateSettings.HealthStartPeriod"/></li><li><c>--health-startup-cmd</c> via <see cref="PodmanCreateSettings.HealthStartupCmd"/></li><li><c>--health-startup-interval</c> via <see cref="PodmanCreateSettings.HealthStartupInterval"/></li><li><c>--health-startup-retries</c> via <see cref="PodmanCreateSettings.HealthStartupRetries"/></li><li><c>--health-startup-success</c> via <see cref="PodmanCreateSettings.HealthStartupSuccess"/></li><li><c>--health-startup-timeout</c> via <see cref="PodmanCreateSettings.HealthStartupTimeout"/></li><li><c>--health-timeout</c> via <see cref="PodmanCreateSettings.HealthTimeout"/></li><li><c>--hostname</c> via <see cref="PodmanCreateSettings.Hostname"/></li><li><c>--hostuser</c> via <see cref="PodmanCreateSettings.HostUser"/></li><li><c>--http-proxy</c> via <see cref="PodmanCreateSettings.HttpProxy"/></li><li><c>--image-volume</c> via <see cref="PodmanCreateSettings.ImageVolume"/></li><li><c>--init</c> via <see cref="PodmanCreateSettings.Init"/></li><li><c>--init-ctr</c> via <see cref="PodmanCreateSettings.InitCtr"/></li><li><c>--init-path</c> via <see cref="PodmanCreateSettings.InitPath"/></li><li><c>--interactive</c> via <see cref="PodmanCreateSettings.Interactive"/></li><li><c>--ip</c> via <see cref="PodmanCreateSettings.Ip"/></li><li><c>--ip6</c> via <see cref="PodmanCreateSettings.Ip6"/></li><li><c>--label</c> via <see cref="PodmanCreateSettings.Label"/></li><li><c>--log-driver</c> via <see cref="PodmanCreateSettings.LogDriver"/></li><li><c>--log-opt</c> via <see cref="PodmanCreateSettings.LogOpt"/></li><li><c>--mac-address</c> via <see cref="PodmanCreateSettings.MacAddress"/></li><li><c>--memory</c> via <see cref="PodmanCreateSettings.Memory"/></li><li><c>--memory-swap</c> via <see cref="PodmanCreateSettings.MemorySwap"/></li><li><c>--mount</c> via <see cref="PodmanCreateSettings.Mount"/></li><li><c>--name</c> via <see cref="PodmanCreateSettings.Name"/></li><li><c>--network</c> via <see cref="PodmanCreateSettings.Network"/></li><li><c>--network-alias</c> via <see cref="PodmanCreateSettings.NetworkAlias"/></li><li><c>--no-hosts</c> via <see cref="PodmanCreateSettings.NoHosts"/></li><li><c>--oom-kill-disable</c> via <see cref="PodmanCreateSettings.OomKillDisable"/></li><li><c>--oom-score-adj</c> via <see cref="PodmanCreateSettings.OomScoreAdj"/></li><li><c>--pid</c> via <see cref="PodmanCreateSettings.Pid"/></li><li><c>--pids-limit</c> via <see cref="PodmanCreateSettings.PidsLimit"/></li><li><c>--pod</c> via <see cref="PodmanCreateSettings.Pod"/></li><li><c>--pod-id-file</c> via <see cref="PodmanCreateSettings.PodIdFile"/></li><li><c>--preserve-fds</c> via <see cref="PodmanCreateSettings.PreserveFds"/></li><li><c>--privileged</c> via <see cref="PodmanCreateSettings.Privileged"/></li><li><c>--publish</c> via <see cref="PodmanCreateSettings.Publish"/></li><li><c>--publish-all</c> via <see cref="PodmanCreateSettings.PublishAll"/></li><li><c>--read-only</c> via <see cref="PodmanCreateSettings.ReadOnly"/></li><li><c>--read-only-tmpfs</c> via <see cref="PodmanCreateSettings.ReadOnlyTmpfs"/></li><li><c>--replace</c> via <see cref="PodmanCreateSettings.Replace"/></li><li><c>--restart</c> via <see cref="PodmanCreateSettings.Restart"/></li><li><c>--rm</c> via <see cref="PodmanCreateSettings.Rm"/></li><li><c>--security-opt</c> via <see cref="PodmanCreateSettings.SecurityOpt"/></li><li><c>--shm-size</c> via <see cref="PodmanCreateSettings.ShmSize"/></li><li><c>--sig-proxy</c> via <see cref="PodmanCreateSettings.SigProxy"/></li><li><c>--stop-signal</c> via <see cref="PodmanCreateSettings.StopSignal"/></li><li><c>--stop-timeout</c> via <see cref="PodmanCreateSettings.StopTimeout"/></li><li><c>--subgidname</c> via <see cref="PodmanCreateSettings.Subgidname"/></li><li><c>--subuidname</c> via <see cref="PodmanCreateSettings.Subuidname"/></li><li><c>--sysctl</c> via <see cref="PodmanCreateSettings.Sysctl"/></li><li><c>--tmpfs</c> via <see cref="PodmanCreateSettings.Tmpfs"/></li><li><c>--tty</c> via <see cref="PodmanCreateSettings.Tty"/></li><li><c>--ulimit</c> via <see cref="PodmanCreateSettings.Ulimit"/></li><li><c>--user</c> via <see cref="PodmanCreateSettings.User"/></li><li><c>--userns</c> via <see cref="PodmanCreateSettings.Userns"/></li><li><c>--uts</c> via <see cref="PodmanCreateSettings.Uts"/></li><li><c>--volume</c> via <see cref="PodmanCreateSettings.Volume"/></li><li><c>--volumes-from</c> via <see cref="PodmanCreateSettings.VolumesFrom"/></li><li><c>--workdir</c> via <see cref="PodmanCreateSettings.Workdir"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanCreate(PodmanCreateSettings options = null) => new PodmanTasks().Run<PodmanCreateSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanCreate(Candoumbe.Pipelines.Tools.Podman.PodmanCreateSettings)"/>
    public static IReadOnlyCollection<Output> PodmanCreate(Configure<PodmanCreateSettings> configurator) => new PodmanTasks().Run<PodmanCreateSettings>(configurator.Invoke(new PodmanCreateSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanCreate(Candoumbe.Pipelines.Tools.Podman.PodmanCreateSettings)"/>
    public static IEnumerable<(PodmanCreateSettings Settings, IReadOnlyCollection<Output> Output)> PodmanCreate(CombinatorialConfigure<PodmanCreateSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanCreate, degreeOfParallelism, completeOnFailure);
    /// <summary><p><p>Displays changes on a container or image’s filesystem. The container or image is compared to its parent layer or the second argument when given.</p><p>The output is prefixed with the following symbols:</p><list type='table'><listheader><term>Symbol</term><description>Description</description></listheader><item><term>A</term><description>A file or a directory was added</description></item><item>D<term>D</term><description>A file or a directory was deleted</description></item><item><term>C</term><description>A file or a directory was changed</description></item></list></p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p></remarks>
    public static IReadOnlyCollection<Output> PodmanDiff(PodmanDiffSettings options = null) => new PodmanTasks().Run<PodmanDiffSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanDiff(Candoumbe.Pipelines.Tools.Podman.PodmanDiffSettings)"/>
    public static IReadOnlyCollection<Output> PodmanDiff(Configure<PodmanDiffSettings> configurator) => new PodmanTasks().Run<PodmanDiffSettings>(configurator.Invoke(new PodmanDiffSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanDiff(Candoumbe.Pipelines.Tools.Podman.PodmanDiffSettings)"/>
    public static IEnumerable<(PodmanDiffSettings Settings, IReadOnlyCollection<Output> Output)> PodmanDiff(CombinatorialConfigure<PodmanDiffSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanDiff, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Podman is a tool for managing OCI containers and pods. Podman provides a Docker-compatible CLI (Docker command line interface) to manage OCI containers and pods.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--all-tags</c> via <see cref="PodmanPsSettings.AllTags"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanPs(PodmanPsSettings options = null) => new PodmanTasks().Run<PodmanPsSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanPs(Candoumbe.Pipelines.Tools.Podman.PodmanPsSettings)"/>
    public static IReadOnlyCollection<Output> PodmanPs(Configure<PodmanPsSettings> configurator) => new PodmanTasks().Run<PodmanPsSettings>(configurator.Invoke(new PodmanPsSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanPs(Candoumbe.Pipelines.Tools.Podman.PodmanPsSettings)"/>
    public static IEnumerable<(PodmanPsSettings Settings, IReadOnlyCollection<Output> Output)> PodmanPs(CombinatorialConfigure<PodmanPsSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanPs, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Podman is a tool for managing OCI containers and pods. Podman provides a Docker-compatible CLI (Docker command line interface) to manage OCI containers and pods.</p><p>For more details, visit the <a href="https://docs.podman.io/en/latest/Commands.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--authfile</c> via <see cref="PodmanImagePullSettings.Authfile"/></li><li><c>--cert-dir</c> via <see cref="PodmanImagePullSettings.CertDir"/></li><li><c>--compress</c> via <see cref="PodmanImagePullSettings.Compress"/></li><li><c>--compression-format</c> via <see cref="PodmanImagePullSettings.CompressionFormat"/></li><li><c>--compression-level</c> via <see cref="PodmanImagePullSettings.CompressionLevel"/></li><li><c>--creds</c> via <see cref="PodmanImagePullSettings.Creds"/></li><li><c>--digestfile</c> via <see cref="PodmanImagePullSettings.Digestfile"/></li><li><c>--disable-content-trust</c> via <see cref="PodmanImagePullSettings.DisableContentTrust"/></li><li><c>--encrypt-layer</c> via <see cref="PodmanImagePullSettings.EncryptLayer"/></li><li><c>--encryption-key</c> via <see cref="PodmanImagePullSettings.EncryptionKey"/></li><li><c>--force-compression</c> via <see cref="PodmanImagePullSettings.ForceCompression"/></li><li><c>--format</c> via <see cref="PodmanImagePullSettings.Format"/></li><li><c>--quiet</c> via <see cref="PodmanImagePullSettings.Quiet"/></li><li><c>--remove-signatures</c> via <see cref="PodmanImagePullSettings.RemoveSignatures"/></li><li><c>--retry</c> via <see cref="PodmanImagePullSettings.Retry"/></li><li><c>--retry-delay</c> via <see cref="PodmanImagePullSettings.RetryDelay"/></li><li><c>--sign-by</c> via <see cref="PodmanImagePullSettings.SignBy"/></li><li><c>--sign-by-sigstore</c> via <see cref="PodmanImagePullSettings.SignBySigstore"/></li><li><c>--sign-by-sigstore-private-key</c> via <see cref="PodmanImagePullSettings.SignBySigstorePrivateKey"/></li><li><c>--sign-by-sq-fingerprint</c> via <see cref="PodmanImagePullSettings.SignBySqFingerprint"/></li><li><c>--sign-passphrase-file</c> via <see cref="PodmanImagePullSettings.SignPassphraseFile"/></li><li><c>--tls-verify</c> via <see cref="PodmanImagePullSettings.TlsVerify"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PodmanImagePull(PodmanImagePullSettings options = null) => new PodmanTasks().Run<PodmanImagePullSettings>(options);
    /// <inheritdoc cref="PodmanTasks.PodmanImagePull(Candoumbe.Pipelines.Tools.Podman.PodmanImagePullSettings)"/>
    public static IReadOnlyCollection<Output> PodmanImagePull(Configure<PodmanImagePullSettings> configurator) => new PodmanTasks().Run<PodmanImagePullSettings>(configurator.Invoke(new PodmanImagePullSettings()));
    /// <inheritdoc cref="PodmanTasks.PodmanImagePull(Candoumbe.Pipelines.Tools.Podman.PodmanImagePullSettings)"/>
    public static IEnumerable<(PodmanImagePullSettings Settings, IReadOnlyCollection<Output> Output)> PodmanImagePull(CombinatorialConfigure<PodmanImagePullSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PodmanImagePull, degreeOfParallelism, completeOnFailure);
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
    /// <summary>Instead of building for a set of platforms specified using the <b>--platform</b> option, inspect the build’s base images, and build for all of the platforms for which they are all available. Stages that use scratch as a starting point can not be inspected, so at least one non-scratch stage must be present for detection to work usefully.</summary>
    [Argument(Format = "--all-platforms")] public bool? AllPlatforms => Get<bool?>(() => AllPlatforms);
    /// <summary><p>Add an image <i>annotation</i> (e.g. annotation=value) to the image metadata. Can be used multiple times.</p><p>Note: this information is not present in Docker image formats, so it is discarded when writing images in Docker formats.</p></summary>
    [Argument(Format = "--annotation={key}={value}")] public IReadOnlyDictionary<string, string> Annotation => Get<IReadOnlyDictionary<string, string>>(() => Annotation);
    /// <summary><p>Set the architecture of the image to be built, and that of the base image to be pulled, if the build uses one, to the provided value instead of using the architecture of the build host. Unless overridden, subsequent lookups of the same image in the local storage matches this architecture, regardless of the host. (Examples: arm, arm64, 386, amd64, ppc64le, s390x)</p></summary>
    [Argument(Format = "--arch={value}")] public string Arch => Get<string>(() => Arch);
    /// <summary><p>Path of the authentication file. Default is ${XDG_RUNTIME_DIR}/containers/auth.json on Linux, and <c>$HOME/.config/containers/auth.json</c> on Windows/macOS. The file is created by <see href='https://docs.podman.io/en/latest/markdown/podman-login.1.html'>podman login</see>. If the authorization state is not found there, <c>$HOME/.docker/config.json</c> is checked, which is set using <b>docker login</b>.</p></summary>
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
    /// <summary><p>When executing <c>RUN</c> instructions, run the command specified in the instruction with the specified capability removed from its capability set. The <c>CAP_CHOWN</c>, <c>CAP_DAC_OVERRIDE</c>, <c>CAP_FOWNER</c>, <c>CAP_FSETID</c>, <c>CAP_KILL</c>, <c>CAP_NET_BIND_SERVICE</c>, <c>CAP_SETFCAP</c>, <c>CAP_SETGID</c>, <c>CAP_SETPCAP</c>, and <c>CAP_SETUID</c> capabilities are granted by default; this option can be used to remove them.</p><p>If a capability is specified to both the <b>--cap-add</b> and <b>--cap-drop</b> options, it is dropped, regardless of the order in which the options were given.</p></summary>
    [Argument(Format = "--cap-drop={value}")] public IEnumerable<string> CapDrop => Get<IEnumerable<string>>(() => CapDrop);
    /// <summary><p>Use certificates at <i>path</i> (*.crt, *.cert, *.key) to connect to the registry. (Default: /etc/containers/certs.d) For details, see <b><see href='https://github.com/containers/image/blob/main/docs/containers-certs.d.5.md'>containers-certs.d(5)</see></b>. (This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines).</p></summary>
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
    /// <summary><p>Limit the CPU Completely Fair Scheduler (CFS) quota.</p><p>Limit the container’s CPU usage. By default, containers run with the full CPU resource. The limit is a number in microseconds. If a number is provided, the container is allowed to use that much CPU time until the CPU period ends (controllable via <b>--cpu-period</b>).</p></summary>
    [Argument(Format = "--cpu-quota={value}")] public int? CpuQuota => Get<int?>(() => CpuQuota);
    /// <summary><p>CPU shares (relative weight).</p><p>By default, all containers get the same proportion of CPU cycles. This proportion can be modified by changing the container’s CPU share weighting relative to the combined weight of all the running containers. Default weight is <c>1024</c>.</p><p>The proportion only applies when CPU-intensive processes are running. When tasks in one container are idle, other containers can use the left-over CPU time. The actual amount of CPU time varies depending on the number of containers running on the system.</p><p>For example, consider three containers, one has a cpu-share of 1024 and two others have a cpu-share setting of 512. When processes in all three containers attempt to use 100% of CPU, the first container receives 50% of the total CPU time. If a fourth container is added with a cpu-share of 1024, the first container only gets 33% of the CPU. The remaining containers receive 16.5%, 16.5% and 33% of the CPU.</p><p>On a multi-core system, the shares of CPU time are distributed over all CPU cores. Even if a container is limited to less than 100% of CPU time, it can use 100% of each individual CPU core.</p><p>For example, consider a system with more than three cores. If the container C0 is started with --cpu-shares=512 running one process, and another container C1 with --cpu-shares=1024 running two processes, this can result in the following division of CPU shares:</p><list type= 'table' >  <listheader>  <term>PID</term>  <term>Container</term>  <term>CPU</term>  <term>CPU Share</term>  </listheader>  <item>  <term>100</term>  <description>  <list type= 'bullet' >  <item>Container: C0</item>  <item>CPU: 0</item>  <item>CPU Share: 100% of CPU0</item>  </list>  </description>  </item>  <item>  <term>101</term>  <description>  <list type= 'bullet' >  <item>Container: C1</item>  <item>CPU: 1</item>  <item>CPU Share: 100% of CPU1</item>  </list>  </description>  </item>  <item>  <term>102</term>  <description>  <list type= 'bullet' >  <item>Container: C1</item>  <item>CPU: 2</item>  <item>CPU Share: 100% of CPU2</item>  </list>  </description>  </item>  </list><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, see https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--cpu-shares={value}")] public decimal? CpuShares => Get<decimal?>(() => CpuShares);
    /// <summary><p>CPUs in which to allow execution. Can be specified as a comma-separated list (e.g. 0,1), as a range (e.g. 0-3), or any combination thereof (e.g. 0-3,7,11-15).</p><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, <see href='https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error' />.</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--cpuset-cpus={value}", Separator = ",")] public IEnumerable<string> CpusetCpus => Get<IEnumerable<string>>(() => CpusetCpus);
    /// <summary><p>Memory nodes (MEMs) in which to allow execution (0-3, 0,1). Only effective on NUMA systems.</p><p>If there are four memory nodes on the system (0-3), use <b>--cpuset-mems=0,1</b> then processes in the container only uses memory from the first two memory nodes.</p><p>On some systems, changing the resource limits may not be allowed for non-root users. For more details, <see href='https://github.com/containers/podman/blob/main/troubleshooting.md#26-running-containers-with-resource-limits-fails-with-a-permissions-error'/>.</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--cpuset-mems={value}", Separator = ",")] public IEnumerable<string> CpusetMems => Get<IEnumerable<string>>(() => CpusetMems);
    /// <summary><p>Add an image annotation (see also --annotation) to the image metadata setting <c>'org.opencontainers.image.created'</c> to the current time, or to the datestamp specified to the <c>--source-date-epoch</c> or <c>--timestamp</c> flag, if either was used. If <see langword='false'/>, no such annotation will be present in the written image.</p><p>Note: this information is not present in Docker image formats, so it is discarded when writing images in Docker formats.</p></summary>
    [Argument(Format = "--created-annotation")] public bool? CreatedAnnotation => Get<bool?>(() => CreatedAnnotation);
    /// <summary><p>The [username[:password]] to use to authenticate with the registry, if required. If one or both values are not supplied, a command line prompt appears and the value can be entered. The password is entered without <c>echo</c></p><p>Note that the specified credentials are only used to authenticate against target registries. They are not used for mirrors or when the registry gets rewritten (see containers-registries.conf(5)); to authenticate against those consider using a containers-auth.json(5) file.</p></summary>
    [Argument(Format = "--creds={value}", Secret = true)] public string Creds => Get<string>(() => Creds);
    /// <summary><p>Produce an image suitable for use as a confidential workload running in a trusted execution environment (TEE) using krun (i.e., <i>crun</i> built with the libkrun feature enabled and invoked as <i>krun</i>). Instead of the conventional contents, the root filesystem of the image will contain an encrypted disk image and configuration information for krun.</p><p>The value for options is a comma-separated list of <c>key=value</c> pairs, supplying configuration information which is needed for producing the additional data which will be included in the container image.</p><p>Recognized keys are:<list type='bullet'><item><term><i>attestation_url</i></term><description>The location of a key broker / attestation server. If a value is specified, the new image’s workload ID, along with the passphrase used to encrypt the disk image, will be registered with the server, and the server’s location will be stored in the container image. At run-time, krun is expected to contact the server to retrieve the passphrase using the workload ID, which is also stored in the container image. If no value is specified, a <i>passphrase</i> value must be specified.</description></item><item><term>cpus</term><description> The number of virtual CPUs which the image expects to be run with at run-time. If not specified, a default value will be supplied.</description></item><item><term>firmware_library</term><description>The location of the libkrunfw-sev shared library. If not specified, buildah checks for its presence in a number of hard-coded locations.</description></item><item><term><i>memory</i></term><description>The amount of memory which the image expects to be run with at run-time, as a number of megabytes. If not specified, a default value will be supplied.</description></item><item><term>passphrase</term><description>The passphrase to use to encrypt the disk image which will be included in the container image. If no value is specified, but an <i>attestation_url</i> value is specified, a randomly-generated passphrase will be used. The authors recommend setting an attestation_url but not a passphrase.</description></item><item><term><i>slop</i></term><description>Extra space to allocate for the disk image compared to the size of the container image’s contents, expressed either as a percentage (..%) or a size value (bytes, or larger units if suffixes like KB or MB are present), or a sum of two or more such specifications. If not specified, buildah guesses that 25% more space than the contents will be enough, but this option is provided in case its guess is wrong.</description></item><item><term><i>type</i></term><description>The type of trusted execution environment (TEE) which the image should be marked for use with. Accepted values are “SEV” (AMD Secure Encrypted Virtualization - Encrypted State) and “SNP” (AMD Secure Encrypted Virtualization - Secure Nested Paging). If not specified, defaults to “SNP”.</description></item><item><term>workload_id</term><description>A workload identifier which will be recorded in the container image, to be used at run-time for retrieving the passphrase which was used to encrypt the disk image. If not specified, a semi-random value will be derived from the base image’s image ID.</description></item></list></p><p>This option is not supported on the remote client, including Mac and Windows (excluding WSL2) machines.</p></summary>
    [Argument(Format = "--cw={value}", Separator = ",")] public IReadOnlyDictionary<string, string> Cw => Get<IReadOnlyDictionary<string, string>>(() => Cw);
    /// <summary><p>The [key[:passphrase]] to be used for decryption of images. Key can point to keys and/or certificates. Decryption is tried with all keys. If the key is protected by a passphrase, it is required to be passed in the argument and omitted otherwise.</p></summary>
    [Argument(Format = "--description-key={value}", Secret = true)] public string DescriptionKey => Get<string>(() => DescriptionKey);
    /// <summary><p>Add a host device to the container. Optional permissions parameter can be used to specify device permissions by combining <b>r</b> for read, <b>w</b> for write, and <b>m</b> for <b>mknod</b>(2).</p><p><example>--device=/dev/sdc:/dev/xvdc:rwm</example></p><p>Note: if <i>host-device</i> is a symbolic link then it is resolved first. The container only stores the major and minor numbers of the host device.</p><p>Podman may load kernel modules required for using the specified device. The devices that Podman loads modules for when necessary are: /dev/fuse.</p><p>In rootless mode, the new device is bind mounted in the container from the host rather than Podman creating it within the container space. Because the bind mount retains its SELinux label on SELinux systems, the container can get permission denied when accessing the mounted device. Modify SELinux settings to allow containers to use all device labels via the following command:</p><p>$ sudo setsebool -P container_use_devices=true</p><p>Note: if the user only has access rights via a group, accessing the device from inside a rootless container fails. The <see href='https://github.com/containers/crun/tree/main/crun.1.md'>crun(1)</see> runtime offers a workaround for this by adding the option <b>--annotation run.oci.keep_original_groups=1</b>.</p></summary>
    [Argument(Format = "--device={value}")] public string Device => Get<string>(() => Device);
    /// <summary><p>Don’t compress filesystem layers when building the image unless it is required by the location where the image is being written. This is the default setting, because image layers are compressed automatically when they are pushed to registries, and images being written to local storage only need to be decompressed again to be stored. Compression can be forced in all cases by specifying <b>--disable-compression=false</b>.</p></summary>
    [Argument(Format = "--disable-compression")] public bool? DisableCompression => Get<bool?>(() => DisableCompression);
    /// <summary><p>This is a Docker-specific option to disable image verification to a container registry and is not supported by Podman. This option is a NOOP and provided solely for scripting compatibility.</p></summary>
    [Argument(Format = "--disable-content-trust")] public bool? DisableContentTrust => Get<bool?>(() => DisableContentTrust);
    /// <summary><p>Set custom DNS servers.</p><p>This option can be used to override the DNS configuration passed to the container. Typically this is necessary when the host DNS configuration is invalid for the container (e.g., <b>127.0.0.1</b>). When this is the case the <c>--dns</c> flag is necessary for every run.</p><p>The special value none can be specified to disable creation of /etc/resolv.conf in the container by Podman. The /etc/resolv.conf file in the image is then used without changes.</p><p>The special value none can be specified to disable creation of /etc/resolv.conf in the container by Podman. The <i>/etc/resolv.conf</i> file in the image is then used without changes.</p><p>This option cannot be combined with <c>--network</c> that is set to <c>none</c>.</p><p>Note: this option takes effect only during <i>RUN</i> instructions in the build. It does not affect <i>/etc/resolv.conf</i> in the final image.</p></summary>
    [Argument(Format = "--dns={value}", Separator = ",")] public System.Net.IPAddress Dns => Get<System.Net.IPAddress>(() => Dns);
    /// <summary><p>Set custom DNS options to be used during the build.</p></summary>
    [Argument(Format = "--dns-option={value}")] public string DnsOption => Get<string>(() => DnsOption);
    /// <summary><p>Set custom DNS search domains to be used during the build.</p></summary>
    [Argument(Format = "--dns-search={value}", Separator = ",")] public string DnsSearch => Get<string>(() => DnsSearch);
    /// <summary><p>Add a value (e.g. <c>env=value</c>) to the built image. Can be used multiple times. If neither <c>=</c> nor a <i>value</i> are specified, but env is set in the current environment, the value from the current environment is added to the image. To remove an environment variable from the built image, use the <c>--unsetenv</c> option.</p></summary>
    [Argument(Format = "--env={value}", Separator = ",")] public string Env => Get<string>(() => Env);
    /// <summary><p>Specifies a Containerfile which contains instructions for building the image, either a local file or an <b>http</b> or <b>https</b> URL. If more than one Containerfile is specified, <b>FROM</b> instructions are only be accepted from the last specified file.</p><p>If a build context is not specified, and at least one Containerfile is a local file, the directory in which it resides is used as the build context.</p><p>Specifying the option <c>-f -</c> causes the Containerfile contents to be read from stdin.</p></summary>
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
    /// <summary><p>By default proxy environment variables are passed into the container if set for the Podman process. This can be disabled by setting the value to <see langword='false'/>. The environment variables passed in include <b>http_proxy</b>, <b>https_proxy</b>, <b>ftp_proxy</b>, <b>no_proxy</b>, and also the upper case versions of those. This option is only needed when the host system must use a proxy but the container does not use any proxy. Proxy environment variables specified for the container in any other way overrides the values that have been passed through from the host. (Other ways to specify the proxy for the container include passing the values with the <b>--env</b> flag, or hard coding the proxy environment at container build time.) When used with the remote client it uses the proxy environment variables that are set on the server process.</p><p>Defaults to <see langword='true'/>.</p></summary>
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
    /// <summary><p>Add an image label (e.g. label=value) to the image metadata. Can be used multiple times.</p><p>Users can set a special LABEL <b>io.containers.capabilities</b>=<b>CAP1</b>,<b>CAP2</b>,<b>CAP3</b> in a Containerfile that specifies the list of Linux capabilities required for the container to run properly. This label specified in a container image tells Podman to run the container with just these capabilities. Podman launches the container with just the specified capabilities, as long as this list of capabilities is a subset of the default list.</p><p>If the specified capabilities are not in the default set, Podman prints an error message and runs the container with the default capabilities.</p></summary>
    [Argument(Format = "--label={value}", Separator = ",")] public IEnumerable<string> Label => Get<IEnumerable<string>>(() => Label);
    /// <summary><p>Add an intermediate image <i>label</i> (e.g. label=<c>value</c>) to the intermediate image metadata. It can be used multiple times.</p><p>If label is named, but neither <c>=</c> nor a <c>value</c> is provided, then the <i>label</i> is set to an empty value.</p></summary>
    [Argument(Format = "--layer-label={value}")] public string LayerLabel => Get<string>(() => LayerLabel);
    /// <summary><p>Cache intermediate images during the build process (Default is <see langword='true'/>).</p><p>Note: You can also override the default value of layers by setting the BUILDAH_LAYERS environment variable. <c>export BUILDAH_LAYERS=true</c></p></summary>
    [Argument(Format = "--layers")] public bool? Layers => Get<bool?>(() => Layers);
    /// <summary><p>Log output which is sent to standard output and standard error to the specified file instead of to standard output and standard error. This option is not supported on the remote client, including Mac and Windows (excluding WSL2) machines.</p></summary>
    [Argument(Format = "--logfile={value}")] public string LogFile => Get<string>(() => LogFile);
    /// <summary><p>If <c>--logfile</c> and <c>--platform</c> are specified, the <c>--logsplit</c> option allows end-users to split the log file for each platform into different files in the following format: <c>${logfile}_${platform-os}_${platform-arch}</c>. This option is not supported on the remote client, including Mac and Windows (excluding WSL2) machines.</p></summary>
    [Argument(Format = "--logsplit={value}")] public bool? Logsplit => Get<bool?>(() => Logsplit);
    /// <summary><p>Name of the manifest list to which the image is added. Creates the manifest list if it does not exist. This option is useful for building multi architecture images.</p></summary>
    [Argument(Format = "--manifest={value}")] public string Manifest => Get<string>(() => Manifest);
    /// <summary><p>Memory limit. A <i>unit</i> can be <b>b</b> (bytes), <b>k</b> (kibibytes), <b>m</b> (mebibytes), or <b>g</b> (gibibytes).</p><p>Allows the memory available to a container to be constrained. If the host supports swap memory, then the <b>-m</b> memory setting can be larger than physical RAM. If a limit of 0 is specified (not using <b>-m</b>), the container’s memory is not limited. The actual limit may be rounded up to a multiple of the operating system’s page size (the value is very large, that’s millions of trillions).</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--memory={value}")] public string Memory => Get<string>(() => Memory);
    /// <summary><p>A limit value equal to memory plus swap. A <i>unit</i> can be <b>b</b> (bytes), <b>k</b> (kibibytes), <b>m</b> (mebibytes), or <b>g</b> (gibibytes).</p><p>Must be used with the <b>-m</b> (<b>--memory</b>) flag. The argument value must be larger than that of <b>-m</b> (<b>--memory</b>) By default, it is set to double the value of <b>--memory</b>.</p><p>Set number to <c>-1</c> to enable unlimited swap.</p><p>This option is not supported on cgroups V1 rootless systems.</p></summary>
    [Argument(Format = "--memory-swap={value}")] public string MemorySwap => Get<string>(() => MemorySwap);
    /// <summary><p>Sets the configuration for network namespaces when handling <c>RUN</c> instructions.</p><p>Valid <i>mode</i> are:<list type='bullet'><item><term><b>none</b> : </term><description>no networking</description></item><item><term><b>host</b> : </term><description>use the Podman host network stack. Note: the host mode gives the container full access to local system services such as D-bus and is therefore considered insecure.</description></item><item><term><b>ns</b>:<i>path</i> : </term><description>path to a network namespace to join.</description></item><item><term><b>private</b> : </term><description>create a new namespace for the container (default)</description></item><item><term><b>&lt;network name | ID&gt;</b></term><description>Join the network with the given name or ID, e.g. use <c>--network mynet</c> to join the network with the name <c>mynet</c>. Only supported for rootful users.</description></item><item><term><b>slirp4netns[:OPTIONS,…]</b></term><description>use <b>slirp4netns(1)</b> to create a user network stack. It is possible to specify these additional options, they can also be set with <c>network_cmd_options</c> in containers.conf:<list type='bullet'><item><term><b>allow_host_loopback</b>=<b>true</b>|<b>false</b> : </term><description>Allow slirp4netns to reach the host loopback IP (default is 10.0.2.2 or the second IP from slirp4netns cidr subnet when changed, see the cidr option below). The default is <see langword='false'/></description></item><item><term><b>mtu=MTU</b> : </term><description>Specify the MTU to use for this network. (Default is <c>65520</c>)</description></item><item><term><b>cidr=CIDR</b> : </term><description>Specify ip range to use for this network. (Default is <c>10.0.2.0/24</c>)</description></item><item><term><b>enable_ipv6</b>=<b>true</b>|<b>false</b> : </term><description>Enable IPv6. Default is <see langword='true'/>. (Required for <c>outbound_addr6</c>).</description></item><item><term><b>outbound_addr</b>=<b>INTERFACE</b> : </term><description>Specify the outbound interface slirps binds to (ipv4 traffic only).</description></item><item><term><b>outbound_addr</b>=<b>IPv4</b> : </term><description>Specify the outbound IPv4 slirps binds to.</description></item><item><term><b>outbound_addr6</b>=<b>INTERFACE</b> : </term><description>Specify the outbound interface slirps binds to (ipv6 traffic only).</description></item><item><term><b>outbound_addr6</b>=<b>INTERFACE</b> : </term><description>Specify the outbound ipv6 slirps binds to.</description></item></list></description></item><item><term><b>pasta[:OPTIONS,...]</b> : </term><description>use <b>pasta</b>(1) to create a user-mode networking stack. This is the default for rootless containers and only supported in rootless mode.<p>By default, IPv4 and IPv6 addresses and routes, as well as the pod interface name, are copied from the host. If port forwarding isn’t configured, ports are forwarded dynamically as services are bound on either side (init namespace or container namespace). Port forwarding preserves the original source IP address. Options described in pasta(1) can be specified as comma-separated arguments.</p><p>In terms of pasta(1) options, <b>--config-net</b> is given by default, in order to configure networking when the container is started, and <b>--no-map-gw</b> is also assumed by default, to avoid direct access from container to host using the gateway address. The latter can be overridden by passing <b>--map-gw</b> in the pasta-specific options (despite not being an actual pasta(1) option). Also, <b>-t none</b> and <b>-u none</b> are passed to disable automatic port forwarding based on bound ports. Similarly, <b>-T none</b> and <b>-U none</b> are given to disable the same functionality from container to host. Some examples:<list type='bullet'><item><term><b>pasta:-map-gw</b> : </term><description>Allow the container to directly reach the host using the gateway address.</description></item><item><term><b>pasta:--mtu,1500</b> : </term><description>Specify a 1500 bytes MTU for the <i>tap</i> interface in the container.</description></item><item><term><b>pasta:--ipv4-only,-a,10.0.2.0,-n,24,-g,10.0.2.2,--dns-forward,10.0.2.3,-m,1500,--no-ndp,--no-dhcpv6,--no-dhcp</b>, </term><description>equivalent to default slirp4netns(1) options: disable IPv6, assign <c>10.0.2.0/24</c> to the <c>tap0</c> interface in the container, with gateway <c>10.0.2.3</c>, enable DNS forwarder reachable at <c>10.0.2.3</c>, set MTU to 1500 bytes, disable NDP, DHCPv6 and DHCP support.</description></item><item><term><b>pasta:-I,tap0,--ipv4-only,-a,10.0.2.0,-n,24,-g,10.0.2.2,--dns-forward,10.0.2.3,--no-ndp,--no-dhcpv6,--no-dhcp</b>, </term><description> equivalent to default slirp4netns(1) options with Podman overrides: same as above, but leave the MTU to 65520 bytes.</description></item><item><term><b>pasta:-t,auto,-u,auto,-T,auto,-U,auto</b> : </term><description>enable automatic port forwarding based on observed bound ports from both host and container sides.</description></item><item><term><b>pasta:-T,5201</b> : </term><description>enable forwarding of TCP port 5201 from container to host, using the loopback interface instead of the tap interface for improved performance.</description></item></list></p></description></item></list></p></summary>
    [Argument(Format = "--network={value}")] public string Network => Get<string>(() => Network);
    /// <summary><p>Do not use existing cached images for the container build. Build from the start with a new set of cached layers.</p></summary>
    [Argument(Format = "--no-cache")] public bool? NoCache => Get<bool?>(() => NoCache);
    /// <summary><p>Do not create the <i>/etc/hostname</i> file in the container.</p><p>By default, Podman manages the <i>/etc/hostname</i> file, adding the container’s own hostname. When the <b>--no-hostname</b> option is set, the image’s <i>/etc/hostname</i> will be preserved unmodified if it exists.</p></summary>
    [Argument(Format = "--no-hostname")] public bool? NoHostname => Get<bool?>(() => NoHostname);
    /// <summary><p>Do not modify the <i>/etc/hosts</i> file in the container.</p><p>Podman assumes control over the container’s <c>/etc/hosts</c> file by default and adds entries for the container’s name (see <b>--name</b> option) and hostname (see <b>--hostname</b> option), the internal <c>host.containers.internal</c> and <c>host.docker.internal</c> hosts, as well as any hostname added using the <b>--add-host</b> option. Refer to the <b>--add-host</b> option for details. Passing <b>--no-hosts</b> disables this, so that the image’s <c>/etc/hosts</c> file is kept unmodified. The same can be achieved globally by setting <c>no_hosts=true</c> in <c>containers.conf</c>.</p><p>This options conflicts with <b>--add-host</b>.</p></summary>
    [Argument(Format = "--no-hosts")] public bool? NoHosts => Get<bool?>(() => NoHosts);
    /// <summary><p>Omit build history information in the built image. (default <see langword="false"/>).</p><p>This option is useful for the cases where end users explicitly want to set <c>--omit-history</c> to omit the optional <c>History</c> from built images or when working with images built using build tools that do not include <c>History</c> information in their images.</p></summary>
    [Argument(Format = "--omit-history")] public bool? OmitHistory => Get<bool?>(() => OmitHistory);
    /// <summary><p>Set the OS of the image to be built, and that of the base image to be pulled, if the build uses one, instead of using the current operating system of the build host. Unless overridden, subsequent lookups of the same image in the local storage matches this OS, regardless of the host.</p></summary>
    [Argument(Format = "--os={value}")] public string Os => Get<string>(() => Os);
    /// <summary><p>Set the name of a required operating system feature for the image which is built. By default, if the image is not based on scratch, the base image’s required OS feature list is kept, if the base image specified any. This option is typically only meaningful when the image’s OS is Windows.</p><p>If feature has a trailing <c>-</c>, then the feature is removed from the set of required features which is listed in the image.</p></summary>
    [Argument(Format = "--os-feature={value}")] public string OsFeature => Get<string>(() => OsFeature);
    /// <summary><p>Set the exact required operating system version for the image which is built. By default, if the image is not based on scratch, the base image’s required OS version is kept, if the base image specified one. This option is typically only meaningful when the image’s OS is Windows, and is typically set in Windows base images, so using this option is usually unnecessary.</p></summary>
    [Argument(Format = "--os-version={value}")] public string OsVersion => Get<string>(() => OsVersion);
    /// <summary><p>Output destination (format: type=local,dest=path)</p><p>The --output (or -o) option extends the default behavior of building a container image by allowing users to export the contents of the image as files on the local filesystem, which can be useful for generating local binaries, code generation, etc. (This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines)</p><p>The value for --output is a comma-separated sequence of key=value pairs, defining the output type and options.</p><p>Supported <i>keys</i> are:<list type='bullet'><item><term><b>dest</b>:</term><description>Destination path for exported output. Valid values is absolute or relative path, <c>-</c> means the standard output</description></item><item><term><b>type</b>:</term><description>Defines the type of output to be used. Valid values is documented below.</description></item></list></p><p>Valid <i>type</i> values are:</p><list type='bullet'><item><term><b>local</b>:</term><description>write the resulting build files to a directory on the client side.</description></item><item><term><b>tar</b>:</term><description>write the resulting files as a single tarball (.<i>tar</i>).</description></item></list><p>If no type is specified, the value defaults to <b>local</b>. Alternatively, instead of a comma-separated sequence, the value of <b>--output</b> can be just a destination (in the dest format) (e.g. <c>--output some-path</c>, <c>--output -</c>) where <c>--output some-path</c> is treated as if <b>type=local</b> and <c>--output -</c> is treated as if <b>type=tar</b>.</p></summary>
    [Argument(Format = "--output={value}")] public string Output => Get<string>(() => Output);
    /// <summary><p>Sets the configuration for PID namespaces when handling <c>RUN</c> instructions.The configured value can be <c>""</c> (the empty string) or <c>"container"</c> to indicate that a new PID namespace is created, or it can be <c>"host"</c> to indicate that the PID namespace in which <c>podman</c> itself is being run is reused, or it can be the path to a PID namespace which is already in use by another process.</p></summary>
    [Argument(Format = "--pid={value}")] public string Pid => Get<string>(() => Pid);
    /// <summary><p>Set the <i>os/arch</i> of the built image (and its base image, when using one) to the provided value instead of using the current operating system and architecture of the host (for example <c>linux/arm</c>). Unless overridden, subsequent lookups of the same image in the local storage matches this platform, regardless of the host.</p><p>If <c>--platform</c> is set, then the values of the <c>--arch</c>, <c>--os</c>, and <c>--variant</c> options is overridden.</p><p>The <c>--platform</c> option can be specified more than once, or given a comma-separated list of values as its argument. When more than one platform is specified, the <c>--manifest</c> option is used instead of the <c>--tag</c> option.</p><p>Os/arch pairs are those used by the Go Programming Language. In several cases the <i>arch</i> value for a platform differs from one produced by other tools such as the <c>arch</c> command. Valid OS and architecture name combinations are listed as values for $GOOS and $GOARCH at <see href="https://golang.org/doc/install/source#environment"/>, and can also be found by running <c>go tool dist list</c>.</p><p>While <c>podman build</c> is happy to use base images and build images for any platform that exists, <c>RUN</c> instructions are unable to succeed without the help of emulation provided by packages like <c>qemu-user-static</c>.</p></summary>
    [Argument(Format = "--platform={value}")] public string Platform => Get<string>(() => Platform);
    /// <summary><p>Pull image policy.</p><list type='bullet'><item><term>always</term><description>Always pull the image and throw an error if the pull fails.</description></item><item><term>missing</term><description>Onlu pull the image when it does not exist in the local containers storage. Throw an error if no image is found and the pull fails.</description></item><item><term>never</term><description>Never pull the image but use the one form the local containers storage. Throw an error when no image is found.</description></item><item><term>newer</term><description>Pull if the image on the registry is newer than the one in the local containers storage. An image is considered to be newer when the digests are different. Comparing the time stamps is prone to errors. Pull errors are suppressed if a local image was found.</description></item></list></summary>
    [Argument(Format = "--pull={value}")] public PullPolicy Pull => Get<PullPolicy>(() => Pull);
    /// <summary><p>Suppress output messages which indicate which instruction is being processed, and of progress when pulling images from a registry, and when writing the output image.</p></summary>
    [Argument(Format = "--quiet")] public bool? Quiet => Get<bool?>(() => Quiet);
    /// <summary><p>Number of times to retry pulling or pushing images between the registry and local storage in case of failure. Default is <c>3</c>.</p></summary>
    [Argument(Format = "--retry={value}")] public uint? Retry => Get<uint?>(() => Retry);
    /// <summary><p>Duration of delay between retry attempts when pulling or pushing images between the registry and local storage in case of failure. The default is to start at two seconds and then exponentially back off. The delay is used when this value is set, and no exponential back off occurs.</p></summary>
    [Argument(Format = "--retry-delay={value}")] public uint? RetryDelay => Get<uint?>(() => RetryDelay);
    /// <summary><p>When generating new layers for the image, ensure that no newly added content bears a timestamp later than the value used by the <b>--source-date-epoch</b> flag, if one was provided, by replacing any timestamps which are later than that value, with that value.</p></summary>
    [Argument(Format = "--rewrite-timestamp")] public bool? RewriteTimestamp => Get<bool?>(() => RewriteTimestamp);
    /// <summary><p>Remove intermediate containers after a successful build (default <see langword="true"/>).</p></summary>
    [Argument(Format = "--rm")] public bool? Rm => Get<bool?>(() => Rm);
    /// <summary><p>The path to an alternate OCI-compatible runtime, which is used to run commands specified by the <b>RUN</b> instruction.</p><p>Note: You can also override the default runtime by setting the BUILDAH_RUNTIME environment variable. <c>export BUILDAH_RUNTIME=/usr/local/bin/runc</c>.</p></summary>
    [Argument(Format = "--runtime={value}")] public Nuke.Common.IO.AbsolutePath Runtime => Get<Nuke.Common.IO.AbsolutePath>(() => Runtime);
    /// <summary><p>Adds global flags for the container runtime. To list the supported flags, please consult the manpages of the selected container runtime.</p><p>Default runtime flags can be added in containers.conf.</p><p>Note: Do not pass the leading -- to the flag. To pass the runc flag --log-format json to buildah build, the option given is --runtime-flag log-format=json.</p></summary>
    [Argument(Format = "--runtime-flag={value}")] public string RuntimeFlag => Get<string>(() => RuntimeFlag);
    /// <summary><p>Generate SBOMs (Software Bills Of Materials) for the output image by scanning the working container and build contexts using the named combination of scanner image, scanner commands, and merge strategy. Must be specified with one or more of <b>--sbom-image-output</b>, <b>--sbom-image-purl-output</b>, <b>--sbom-output</b>, and <b>--sbom-purl-output</b>. Recognized presets, and the set of options which they equate to:<list type='bullet'><item><term>"syft", "syft-cyclonedx"</term><description>--sbom-scanner-image=ghcr.io/anchore/syft --sbom-scanner-command=”/syft scan -q dir:{ROOTFS} --output cyclonedx-json={OUTPUT}” --sbom-scanner-command=”/syft scan -q dir:{CONTEXT} --output cyclonedx-json={OUTPUT}” --sbom-merge-strategy=merge-cyclonedx-by-component-name-and-version</description></item><item><term>"syft-spdx"</term><description>--sbom-scanner-image=ghcr.io/anchore/syft --sbom-scanner-command=”/syft scan -q dir:{ROOTFS} --output spdx-json={OUTPUT}” --sbom-scanner-command=”/syft scan -q dir:{CONTEXT} --output spdx-json={OUTPUT}” --sbom-merge-strategy=merge-spdx-by-package-name-and-versioninfo</description></item><item><term>"trivy", "trivy-cyclonedx"</term><description>--sbom-scanner-image=ghcr.io/aquasecurity/trivy --sbom-scanner-command=”trivy filesystem -q {ROOTFS} --format cyclonedx --output {OUTPUT}” --sbom-scanner-command=”trivy filesystem -q {CONTEXT} --format cyclonedx --output {OUTPUT}” --sbom-merge-strategy=merge-cyclonedx-by-component-name-and-version</description></item><item><term>"trivy-spdx"</term><description>--sbom-scanner-image=ghcr.io/aquasecurity/trivy --sbom-scanner-command=”trivy filesystem -q {ROOTFS} --format spdx-json --output {OUTPUT}” --sbom-scanner-command=”trivy filesystem -q {CONTEXT} --format spdx-json --output {OUTPUT}” --sbom-merge-strategy=merge-spdx-by-package-name-and-versioninfo</description></item></list></p></summary>
    [Argument(Format = "--sbom={value}")] public SbomPreset Sbom => Get<SbomPreset>(() => Sbom);
    /// <summary>When generating SBOMs, store the generated SBOM in the specified path in the output image.There is no default.</summary>
    [Argument(Format = "--sbom-image-output={value}")] public Nuke.Common.IO.AbsolutePath SbomImpageOutput => Get<Nuke.Common.IO.AbsolutePath>(() => SbomImpageOutput);
    /// <summary><p>If more than one <b>--sbom-scanner-command</b> value is being used, use the specified method to merge the output from later commands with output from earlier commands. Recognized values include:<list type='bullet'><item><term>cat</term><description>Concatenate the files.</description></item><item><term>merge-cyclonedx-by-component-name-and-version</term><description>Merge the “component” fields of JSON documents, ignoring values from documents when the combination of their “name” and “version” values is already present. Documents are processed in the order in which they are generated, which is the order in which the commands that generate them were specified.</description></item><item><term>merge-spdx-by-package-name-and-versioninfo</term><description>Merge the “package” fields of JSON documents, ignoring values from documents when the combination of their “name” and “versionInfo” values is already present. Documents are processed in the order in which they are generated, which is the order in which the commands that generate them were specified.</description></item></list></p></summary>
    [Argument(Format = "--sbom-merge-strategy={value}")] public SbomMergeStrategy SbomMergeStrategy => Get<SbomMergeStrategy>(() => SbomMergeStrategy);
    /// <summary><p>When generating SBOMs, store the generated SBOM in the named file on the local filesystem. There is no default.</p></summary>
    [Argument(Format = "--sbom-output={value}")] public string SbomOutput => Get<string>(() => SbomOutput);
    /// <summary>When generating SBOMs, scan them for PURL (<see href='https://github.com/package-url/purl-spec/blob/master/PURL-SPECIFICATION.rst'>package URL</see>) information, and save a list of found PURLs to the named file in the local filesystem. There is no default.</summary>
    [Argument(Format = "--sbom-purl-output={value}")] public string SbomPurlOutput => Get<string>(() => SbomPurlOutput);
    /// <summary>Generate SBOMs by running the specified command from the scanner image. If multiple commands are specified, they are run in the order in which they are specified. These text substitutions are performed:<list type='bullet'><item><term>{ROOTFS}</term><description>The root of the build image's filesystem, bind mounted.</description></item><item><term>{CONTEXT}</term><description>The build context and additional build contexts, bind mounted</description></item><item><term>{OUTPUT}</term><description>The name of a temporary output file, to be read and merged with others or copied elsewhere.</description></item></list></summary>
    [Argument(Format = "--sbom-scanner-command={value}")] public string SbomScannerCommand => Get<string>(() => SbomScannerCommand);
    /// <summary>Generate SBOMs using the specified scanner image.</summary>
    [Argument(Format = "--sbom-scanner-image={value}")] public string SbomScannerImage => Get<string>(() => SbomScannerImage);
    /// <summary><p>Pass secret information to be used in the Containerfile for building images in a safe way that will not end up stored in the final image, or be seen in other stages. The value of the secret will be read from an environment variable or file named by the “id” option, or named by the “src” option if it is specified, or from an environment variable specified by the “env” option. See <see href="https://docs.podman.io/en/latest/markdown/podman-build.1.html#examples">EXAMPLES</see>. The secret will be mounted in the container at <c>/run/secrets/id</c> by default.</p><p>To later use the secret, use the <c>--mount</c> flag in a <c>RUN</c> instruction within <c>ContainerFile</c> : <code>RUN --mount=type=secret,id=mysecret cat /run/secrets/mysecret</code>The location of the secret in the container can be overriden using the "target" , "dst", or "destination" option in the RUN <c>--mount</c> flag.<code>RUN --mount=type=secret,id=mysecret,target=/run/secrets/myothersecret cat /run/secrets/myothersecret</code>Note: changing the contents of secret files will not trigger a rebuild of layers that use said secrets.</p></summary>
    [Argument(Format = "--secret={value}", Secret = true)] public IReadOnlyDictionary<string, string> Secret => Get<IReadOnlyDictionary<string, string>>(() => Secret);
    /// <summary>Security options : <list type='bullet'><item><term><c>apparmor=unconfined</c></term><description>Turn off apparmor confinement for the container</description></item><item><term><c>apparmor=alternate-profile</c></term><description>Set the apparmor confinement profile for the container</description></item><item><term><c>label=user:USER</c></term><description>Set the label user for the container processes</description></item><item><term><c>label=role:ROLE</c></term><description>Set the label role for the container processes</description></item><item><term><c>label=type:TYPE</c></term><description>Set the label process type for the container processes</description></item><item><term><c>label=level:LEVEL</c></term><description>Set the label level for the container processes</description></item><item><term><c>label=filetype:TYPE</c></term><description>Set the label file type for the container processes</description></item><item><term><c>label=disable</c></term><description>Turn off label separation for container</description></item><item><term><c>no-new-privileges</c></term><description>Disable container processes for gaining additional privileges</description></item><item><term><c>seccomp=unconfined</c></term><description>Turn off seccomp confinement for the container</description></item><item><term><c>seccomp=profile.json</c></term><description>JSON file to be used as the seccomp filter for the container</description></item></list></summary>
    [Argument(Format = "--security-opt={value}")] public SecurityOptions SecurityOpt => Get<SecurityOptions>(() => SecurityOpt);
    /// <summary>Size of <c>/dev/shm</c>. A unit can be <b>b</b> (bytes), <b>k</b> (kibibytes), <b>m</b> (mebibytes), or <b>g</b> (gibibytes). If the unit is omitted, the system uses bytes. If the size is omitted, the default is <b>64m</b>. When size is <b>0</b>, there is no limit on the amount of memory used for IPC by the container. This option conflicts with <b>--ipc</b>=<b>host</b></summary>
    [Argument(Format = "--shm-size={value}")] public string ShmSize => Get<string>(() => ShmSize);
    /// <summary>Sign the image using a GPG key with the specified FINGERPRINT. (This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines,)</summary>
    [Argument(Format = "--sign-by={value}")] public string SignBy => Get<string>(() => SignBy);
    /// <summary>Skip stages in multi-stage builds which don’t affect the target stage. (Default: <see langword="true"/>).</summary>
    [Argument(Format = "--skip-unused-stages")] public bool? SkipUnusedStages => Get<bool?>(() => SkipUnusedStages);
    /// <summary><p>Set the “created” timestamp for the built image to this number of seconds since the epoch (Unix time 0, i.e., 00:00:00 UTC on 1 January 1970) (default is to use the value set in the <c>SOURCE_DATE_EPOCH</c> environment variable, or the current time if it is not set).</p><p>The “created” timestamp is written into the image’s configuration and manifest when the image is committed, so running the same build two different times will ordinarily produce images with different sha256 hashes, even if no other changes were made to the Containerfile and build context.</p><p>When this flag is set, a <c>SOURCE_DATE_EPOCH</c> build arg will provide its value for a stage in which it is declared.</p><p>When this flag is set, the image configuration’s “created” timestamp is always set to the time specified, which should allow for identical images to be built at different times using the same set of inputs.</p><p>When this flag is set, output written as specified to the <b>--output</b> flag will bear exactly the specified timestamp.</p><p>Conflicts with the similar <b>--timestamp</b> flag, which also sets its specified time on the contents of new layers.</p></summary>
    [Argument(Format = "--source-date-epoch={value}")] public uint? SourceDateEpoch => Get<uint?>(() => SourceDateEpoch);
    /// <summary>Squash all of the image’s new layers into a single new layer; any preexisting layers are not squashed.</summary>
    [Argument(Format = "--squash")] public bool? Squash => Get<bool?>(() => Squash);
    /// <summary>Squash all of the new image’s layers (including those inherited from a base image) into a single new layer.</summary>
    [Argument(Format = "--squash-all")] public bool? SquashAll => Get<bool?>(() => SquashAll);
    /// <summary><p>SSH agent socket or keys to expose to the build. The socket path can be left empty to use the value of <c>default=$SSH_AUTH_SOCK</c>.</p><p>To later use the ssh agent, use the <c>--mount</c> option in a <c>RUN</c> instruction within a Containerfile:<code>RUN --mount=type=ssh,id=id mycmd</code></p></summary>
    [Argument(Format = "--ssh={value}")] public string Ssh => Get<string>(() => Ssh);
    /// <summary>Pass stdin into the RUN containers. Sometime commands being RUN within a Containerfile want to request information from the user. For example apt asking for a confirmation for install. Use --stdin to be able to interact from the terminal during the build.</summary>
    [Argument(Format = "--stdin")] public bool? Stdin => Get<bool?>(() => Stdin);
    /// <summary>Specifies the name which is assigned to the resulting image if the build process completes successfully. If <i>imageName</i> does not include a registry name, the registry name <i>localhost</i> is prepended to the image name.</summary>
    [Argument(Format = "--tag={value}")] public string Tag => Get<string>(() => Tag);
    /// <summary>Set the target build stage to build. When building a Containerfile with multiple build stages, --target can be used to specify an intermediate build stage by name as the final stage for the resulting image. Commands after the target stage is skipped.</summary>
    [Argument(Format = "--target={value}")] public string Target => Get<string>(() => Target);
    /// <summary><p>Set the create timestamp to seconds since epoch to allow for deterministic builds (defaults to current time). By default, the created timestamp is changed and written into the image manifest with every commit, causing the image’s sha256 hash to be different even if the sources are exactly the same otherwise. When --timestamp is set, the created timestamp is always set to the time specified and therefore not changed, allowing the image’s sha256 hash to remain the same. All files committed to the layers of the image is created with the timestamp.</p><p>If the only instruction in a Containerfile is FROM, this flag has no effect.</p></summary>
    [Argument(Format = "--timestamp={value}")] public uint? Timestamp => Get<uint?>(() => Timestamp);
    /// <summary>Require HTTPS and verify certificates when contacting registries (default: <see langword="true"/>). If explicitly set to <see langword="true"/>, TLS verification is used. If set to <see langword="false"/>, TLS verification is not used. If not specified, TLS verification is used unless the target registry is listed as an insecure registry in <see href="https://github.com/containers/image/blob/main/docs/containers-registries.conf.5.md">containers-registries.conf(5)</see></summary>
    [Argument(Format = "--tls-verify={value}")] public bool? TlsVerify => Get<bool?>(() => TlsVerify);
    /// <summary>Specifies resource limits to apply to processes launched when processing RUN instructions. This option can be specified multiple times. Recognized resource types include: “core”: maximum core dump size (ulimit -c) “cpu”: maximum CPU time (ulimit -t) “data”: maximum size of a process’s data segment (ulimit -d) “fsize”: maximum size of new files (ulimit -f) “locks”: maximum number of file locks (ulimit -x) “memlock”: maximum amount of locked memory (ulimit -l) “msgqueue”: maximum amount of data in message queues (ulimit -q) “nice”: niceness adjustment (nice -n, ulimit -e) “nofile”: maximum number of open files (ulimit -n) “nproc”: maximum number of processes (ulimit -u) “rss”: maximum size of a process’s (ulimit -m) “rtprio”: maximum real-time scheduling priority (ulimit -r) “rttime”: maximum amount of real-time execution between blocking syscalls “sigpending”: maximum number of pending signals (ulimit -i) “stack”: maximum stack size (ulimit -s).</summary>
    [Argument(Format = "--ulimit={value}")] public string Ulimit => Get<string>(() => Ulimit);
    /// <summary>Unset the image annotation, causing the annotation not to be inherited from the base image.</summary>
    [Argument(Format = "--unsetannotation={value}")] public string UnsetAnnotation => Get<string>(() => UnsetAnnotation);
    /// <summary>Unset environment variables from the final image.</summary>
    [Argument(Format = "--unsetenv={value}")] public string UnsetEnv => Get<string>(() => UnsetEnv);
    /// <summary>Unset the image label, causing the label not to be inherited from the base image.</summary>
    [Argument(Format = "--unsetlabel={value}")] public string UnsetLabel => Get<string>(() => UnsetLabel);
    /// <summary>Sets the configuration for user namespaces when handling <c>RUN</c> instructions. The configured value can be “” (the empty string) or “container” to indicate that a new user namespace is created, it can be “host” to indicate that the user namespace in which podman itself is being run is reused, or it can be the path to a user namespace which is already in use by another process.</summary>
    [Argument(Format = "--userns={value}")] public string Userns => Get<string>(() => Userns);
    /// <summary><p>Directly specifies a GID mapping to be used to set ownership, at the filesystem level, on the working container’s contents. Commands run when handling RUN instructions defaults to being run in their own user namespaces, configured using the UID and GID maps.</p><p>Entries in this map take the form of one or more triples of a starting in-container GID, a corresponding starting host-level GID, and the number of consecutive IDs which the map entry represents.</p><p>This option overrides the remap-gids setting in the options section of /etc/containers/storage.conf.</p><p>If this option is not specified, but a global --userns-gid-map setting is supplied, settings from the global option is used.</p><p>If none of --userns-uid-map-user, --userns-gid-map-group, or --userns-gid-map are specified, but --userns-uid-map is specified, the GID map is set to use the same numeric values as the UID map.</p></summary>
    [Argument(Format = "--userns-gid-map={value}")] public IReadOnlyDictionary<string, string> UsernsGidMap => Get<IReadOnlyDictionary<string, string>>(() => UsernsGidMap);
    /// <summary><p>Specifies that a GID mapping to be used to set ownership, at the filesystem level, on the working container’s contents, can be found in entries in the <c>/etc/subgid</c> file which correspond to the specified group. Commands run when handling RUN instructions defaults to being run in their own user namespaces, configured using the UID and GID maps. If --userns-uid-map-user is specified, but --userns-gid-map-group is not specified, <c>podman</c> assumes that the specified user name is also a suitable group name to use as the default setting for this option.</p><p>NOTE: When this option is specified by a rootless user, the specified mappings are relative to the rootless user namespace in the container, rather than being relative to the host as it is when run rootful.</p></summary>
    [Argument(Format = "--userns-uid-map={value}")] public IReadOnlyDictionary<string, string> UsernsGidMapGroup => Get<IReadOnlyDictionary<string, string>>(() => UsernsGidMapGroup);
    /// <summary><p>Directly specifies a UID mapping to be used to set ownership, at the filesystem level, on the working container’s contents. Commands run when handling <c>RUN</c> instructions defaults to being run in their own user namespaces, configured using the UID and GID maps.</p><p>Entries in this map take the form of one or more triples of a starting in-container UID, a corresponding starting host-level UID, and the number of consecutive IDs which the map entry represents.</p><p>This option overrides the <i>remap-uids</i> setting in the <i>options</i> section of /etc/containers/storage.conf.</p><p>If this option is not specified, but a global --userns-uid-map setting is supplied, settings from the global option is used.</p><p>If none of --userns-uid-map-user, --userns-gid-map-group, or --userns-gid-map are specified, but --userns-gid-map is specified, the UID map is set to use the same numeric values as the GID map.</p></summary>
    [Argument(Format = "--userns-uid-map={value}")] public IReadOnlyDictionary<string, string> UsernsUidMap => Get<IReadOnlyDictionary<string, string>>(() => UsernsUidMap);
    /// <summary><p>Specifies that a UID mapping to be used to set ownership, at the filesystem level, on the working container’s contents, can be found in entries in the <c>/etc/subuid</c> file which correspond to the specified user. Commands run when handling <c>RUN</c> instructions defaults to being run in their own user namespaces, configured using the UID and GID maps. If --userns-gid-map-group is specified, but --userns-uid-map-user is not specified, <c>podman</c> assumes that the specified user name is also a suitable group name to use as the default setting for this option.</p><p>NOTE: When this option is specified by a rootless user, the specified mappings are relative to the rootless user namespace in the container, rather than being relative to the host as it is when run rootful.</p></summary>
    [Argument(Format = "--userns-uid-map-user={value}")] public string UsernsUidMapUser => Get<string>(() => UsernsUidMapUser);
    /// <summary>Sets the configuration for UTS namespaces when handling RUN instructions. The configured value can be “” (the empty string) or “container” to indicate that a new UTS namespace to be created, or it can be “host” to indicate that the UTS namespace in which <c>podman</c> itself is being run is reused, or it can be the path to a UTS namespace which is already in use by another process.</summary>
    [Argument(Format = "--uts={value}")] public string Uts => Get<string>(() => Uts);
    /// <summary><p>Set the architecture variant of the image to be built, and that of the base image to be pulled, if the build uses one, to the provided value instead of using the architecture variant of the build host.</p>></summary>
    [Argument(Format = "--variant={value}")] public string Variant => Get<string>(() => Variant);
    /// <summary><p>Mount a host directory into containers when executing RUN instructions during the build.</p><p>The OPTIONS are a comma-separated list and can be one or more of:</p><list type='bullet'><item><term>[rw|ro]</term></item><item><term>[z|Z|O]</term></item><item><term>[U]</term></item><item><term>[[r]shared|[r]slave|[r]private](<see href="https://docs.podman.io/en/latest/markdown/podman-build.1.html#Footnote1">1</see>)</term></item></list><p>The <c>CONTAINER-DIR</c> must be an absolute path such as <c>/src/docs</c>. The <c>HOST-DIR</c> must be an absolute path as well. Podman bind-mounts the <c>HOST-DIR</c> to the specified path when processing <c>RUN</c> instructions.</p><p>You can specify multiple -v options to mount one or more mounts.</p><p>You can add the :ro or :rw suffix to a volume to mount it read-only or read-write mode, respectively. By default, the volumes are mounted read-write. See examples.</p><p>Chowning Volume Mounts</p><p>By default, Podman does not change the owner and group of source volume directories mounted. When running using user namespaces, the UID and GID inside the namespace may correspond to another UID and GID on the host.</p><p>The <c>:U</c> suffix tells Podman to use the correct host UID and GID based on the UID and GID within the namespace, to change recursively the owner and group of the source volume.</p><p><b>Warning</b> use with caution since this modifies the host filesystem.</p><p>Labeling Volume Mounts</p><p>Labeling systems like SELinux require that proper labels are placed on volume content mounted into a container. Without a label, the security system might prevent the processes running inside the container from using the content. By default, Podman does not change the labels set by the OS.</p><p>To change a label in the container context, add one of these two suffixes <c>:z</c> or <c>:Z</c> to the volume mount. These suffixes tell Podman to relabel file objects on the shared volumes. The <c>z</c> option tells Podman that two containers share the volume content. As a result, Podman labels the content with a shared content label. Shared volume labels allow all containers to read/write content. The <c>Z</c> option tells Podman to label the content with a private unshared label. Only the current container can use a private volume.</p><p>Note: Do not relabel system files and directories. Relabeling system content might cause other confined services on the host machine to fail. For these types of containers, disabling SELinux separation is recommended. The option <c>--security-opt label=disable</c> disables SELinux separation for the container. For example, if a user wanted to volume mount their entire home directory into the build containers, they need to disable SELinux separation.</p><code>$ podman build --security-opt label=disable -v $HOME:/home/user </code><p>Overlay Volume Mounts</p><p>The <c>:O</c> flag tells Podman to mount the directory from the host as a temporary storage using the Overlay file system. The <c>RUN</c> command containers are allowed to modify contents within the mountpoint and are stored in the container storage in a separate directory. In Overlay FS terms the source directory is the lower, and the container storage directory is the upper. Modifications to the mount point are destroyed when the <c>RUN</c> command finishes executing, similar to a tmpfs mount point.</p><p>Any subsequent execution of <c>RUN</c> commands sees the original source directory content, any changes from previous RUN commands no longer exists.</p><p>One use case of the <c>overlay</c> mount is sharing the package cache from the host into the container to allow speeding up builds.</p><p>Note:<list type='bullet'><item>Overlay mounts are not currently supported in rootless mode.</item><item>The <c>O</c> flag is not allowed to be specified with the <c>Z</c> or <c>z</c> flags. Content mounted into the container is labeled with the private label. On SELinux systems, labels in the source directory needs to be readable by the container label. If not, SELinux container separation must be disabled for the container to work.</item><item>Modification of the directory volume mounted into the container with an overlay mount can cause unexpected failures. Do not modify the directory until the container finishes running.</item></list></p><p>By default bind mounted volumes are <c>private</c>. That means any mounts done inside containers are not be visible on the host and vice versa. This behavior can be changed by specifying a volume mount propagation property.</p><p>When the mount propagation policy is set to <c>shared</c>, any mounts completed inside the container on that volume is visible to both the host and container. When the mount propagation policy is set to <c>slave</c>, one way mount propagation is enabled and any mounts completed on the host for that volume is visible only inside of the container. To control the mount propagation property of volume use the <c>:[r]shared</c>, <c>:[r]slave</c> or <c>:[r]private propagation</c> flag. For mount propagation to work on the source mount point (mount point where source dir is mounted on) has to have the right propagation properties. For shared volumes, the source mount point has to be shared. And for slave volumes, the source mount has to be either shared or slave(<see href='https://docs.podman.io/en/latest/markdown/podman-build.1.html#Footnote1'>notes</see>).</p><p>Use <c>df &lt;source-dir&gt;</c> to determine the source mount and then use <c>findmnt -o TARGET,PROPAGATION &lt;source-mount-dir&gt;</c> to determine propagation properties of source mount, if <c>findmnt</c> utility is not available, the source mount point can be determined by looking at the mount entry in <c>/proc/self/mountinfo</c>. Look at optional <c>fields</c> and see if any propagation properties are specified. <c>shared:X</c> means the mount is shared, <c>master:X</c> means the mount is <c>slave</c> and if nothing is there that means the mount is <c>private</c>. <see href='https://docs.podman.io/en/latest/markdown/podman-build.1.html#Footnote1'>[1]</see></p><p>To change propagation properties of a mount point use the <c>mount</c> command. For example, to bind mount the source directory <c>/foo</c> do <c>mount --bind /foo /foo</c> and <c>mount --make-private --make-shared /foo</c>. This converts <c>/foo</c> into a shared mount point. The propagation properties of the source mount can be changed directly. For instance if <c>/</c> is the source mount for <c>/foo</c>, then use <c>mount --make-shared /</c> to convert <c>/</c> into a shared mount.</p></summary>
    [Argument(Format = "--volume={value}")] public IReadOnlyList<string> Volume => Get<IReadOnlyList<string>>(() => Volume);
}
#endregion
#region PodmanCommitSettings
/// <inheritdoc cref="PodmanTasks.PodmanCommit(Candoumbe.Pipelines.Tools.Podman.PodmanCommitSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanCommit), Arguments = "commit")]
public partial class PodmanCommitSettings : ToolOptions
{
    /// <summary>Set the author field for the generated image.</summary>
    [Argument(Format = "--author={value}")] public string Author => Get<string>(() => Author);
    /// <summary>Apply the following possible instructions to the created image :<list type='bullet'><item><i>CMD</i></item><item><i>ENTRYPOINT</i></item><item><i>ENV</i></item><item><i>EXPOSE</i></item><item><i>LABEL</i></item><item><i>ONBUILD</i></item><item><i>STOPSIGNAL</i></item><item><i>USER</i></item><item><i>VOLUME</i></item><item><i>WORKDIR</i></item></list><p>Can be set multiple times</p></summary>
    [Argument(Format = "--change={value}")] public IReadOnlyList<(InstructionType, string)> Change => Get<IReadOnlyList<(InstructionType, string)>>(() => Change);
    /// <summary>Merge the container configuration from the specified file into the configuration for the image as it is being committed. The file contents should be a JSON-encoded version of a Schema2Config structure, which is defined at <see href='https://github.com/containers/image/blob/v5.29.0/manifest/docker_schema2.go#L67'/>.</summary>
    [Argument(Format = "--config={value}")] public string Config => Get<string>(() => Config);
    /// <summary>Set the format of the image manifest and metadata. The currently supported formats are oci and docker.<br />The default is <b>oci</b>.</summary>
    [Argument(Format = "--format={value}")] public FormatType Format => Get<FormatType>(() => Format);
    /// <summary>Write the image ID to the file.</summary>
    [Argument(Format = "--iidfile={value}")] public string Iidfile => Get<string>(() => Iidfile);
    /// <summary>Include in the committed image any volumes added to the container by the <b>--volume</b> or <b>--mount</b> OPTIONS to the <b><see href='https://docs.podman.io/en/latest/markdown/podman-create.1.html'>podman create</see></b> and <b><see href='https://docs.podman.io/en/latest/markdown/podman-run.1.html'>podman run</see></b> commands.<br />The default is <see langword='false'/>.</summary>
    [Argument(Format = "--include-volumes={value}")] public bool? IncludeVolumes => Get<bool?>(() => IncludeVolumes);
    /// <summary>Set the commit message for the commmitted image.<br /><i>IMPORTANT : The message field is not supported in oc format</i> </summary>
    [Argument(Format = "--message={value}")] public string Message => Get<string>(() => Message);
    /// <summary>Pause the container when creating an image.<br /> The default is <see langword='false'/>.</summary>
    [Argument(Format = "--pause={value}")] public bool? Pause => Get<bool?>(() => Pause);
    /// <summary>Suppresses output.<br />The default is <see langword='false'/>.</summary>
    [Argument(Format = "--quiet")] public bool? Quiet => Get<bool?>(() => Quiet);
    /// <summary>Squash newly built layers into a single new layer.<br />The default is <see langword='false'/>.</summary>
    [Argument(Format = "--squash")] public bool? Squash => Get<bool?>(() => Squash);
}
#endregion
#region PodmanCpSettings
/// <inheritdoc cref="PodmanTasks.PodmanCp(Candoumbe.Pipelines.Tools.Podman.PodmanCpSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanCp), Arguments = "cp")]
public partial class PodmanCpSettings : ToolOptions
{
    /// <summary>Archive mode (copy all UID/GID information). When set to true, files copied to a container have changed ownership to the primary UID/GID of the container. When set to false, maintain UID/GID from archive sources instead of changing them to the primary UID/GID of the destination container. The default is <see langword='true' />.</summary>
    [Argument(Format = "--archive")] public bool? Archive => Get<bool?>(() => Archive);
    /// <summary>Allow directories to be overwritten with non-directories and vice versa. By default, <c>podman cp</c> errors out when attempting to overwrite, for instance, a regular file with a directory.</summary>
    [Argument(Format = "--overwrite")] public bool? Overwrite => Get<bool?>(() => Overwrite);
}
#endregion
#region PodmanCreateSettings
/// <inheritdoc cref="PodmanTasks.PodmanCreate(Candoumbe.Pipelines.Tools.Podman.PodmanCreateSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanCreate), Arguments = "create")]
public partial class PodmanCreateSettings : ToolOptions
{
    /// <summary><p>Add a custom host-to-IP mapping to the container’s <c>/etc/hosts</c> file.<p>The option takes one or multiple semicolon-separated hostnames to be mapped to a single IPv4 or IPv6 address, separated by a colon. It can also be used to overwrite the IP addresses of hostnames Podman adds to <c>/etc/hosts</c> by default (also see the <c>--name</c> and <c>--hostname</c> options). This option can be specified multiple times to add additional mappings to <c>/etc/hosts</c>. It conflicts with the <c>--no-hosts</c> option and conflicts with <c>no_hosts=true</c> in <c>containers.conf</c>.</p><p>Instead of an IP address, the special flag <c>host-gateway</c> can be given. This resolves to an IP address the container can use to connect to the host. The IP address chosen depends on your network setup, thus there’s no guarantee that Podman can determine the <c>host-gateway</c> address automatically, which will then cause Podman to fail with an error message. You can overwrite this IP address using the <c>host_containers_internal_ip</c> option in <c>containers.conf</c>.</p></p></summary>
    [Argument(Format = "--add-host={value}", Separator = ";")] public IReadOnlyList<string> AddHost => Get<IReadOnlyList<string>>(() => AddHost);
    /// <summary><p>Add an annotation to the container.<p>This option can be set multiple times.</p></p></summary>
    [Argument(Format = "--annotation={value}")] public IReadOnlyDictionary<string, string> Annotation => Get<IReadOnlyDictionary<string, string>>(() => Annotation);
    /// <summary><p>Override the architecture, defaults to hosts, of the image to be pulled.<p>For example, <c>arm</c>. Unless overridden, subsequent lookups of the same image in the local storage matches this architecture, regardless of the host.</p></p></summary>
    [Argument(Format = "--arch={value}")] public string Arch => Get<string>(() => Arch);
    /// <summary><p>Attach to STDIN, STDOUT or STDERR.<p>In foreground mode (the default when <c>-d</c> is not specified), <c>podman run</c> can start the process in the container and attach the console to the process’s standard input, output, and error. It can even pretend to be a TTY (this is what most command-line executables expect) and pass along signals. The <c>-a</c> option can be set for each of <c>stdin</c>, <c>stdout</c>, and <c>stderr</c>.</p></p></summary>
    [Argument(Format = "--attach={value}")] public AttachType Attach => Get<AttachType>(() => Attach);
    /// <summary><p>Path of the authentication file.<p>Default is <c>${XDG_RUNTIME_DIR}/containers/auth.json</c> on Linux, and <c>$HOME/.config/containers/auth.json</c> on Windows/macOS. The file is created by <see href="https://docs.podman.io/en/latest/markdown/podman-login.1.html">podman login</see>. If the authorization state is not found there, <c>$HOME/.docker/config.json</c> is checked, which is set using <c>docker login</c>.</p><p>Note: There is also the option to override the default path of the authentication file by setting the <c>REGISTRY_AUTH_FILE</c> environment variable. This can be done with <c>export REGISTRY_AUTH_FILE=path</c>.</p></p></summary>
    [Argument(Format = "--authfile={value}")] public string AuthFile => Get<string>(() => AuthFile);
    /// <summary><p>Block IO relative weight.<p>The <c>weight</c> is a value between <c>10</c> and <c>1000</c>.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--blkio-weight={value}")] public int? BlkioWeight => Get<int?>(() => BlkioWeight);
    /// <summary><p>Block IO relative device weight.</p></summary>
    [Argument(Format = "--blkio-weight-device={value}")] public IReadOnlyList<string> BlkioWeightDevice => Get<IReadOnlyList<string>>(() => BlkioWeightDevice);
    /// <summary><p>Add Linux capabilities.<p>Granting additional capabilities increases the privileges of the processes running inside the container and potentially allow it to break out of confinement. Capabilities like <c>CAP_SYS_ADMIN</c>, <c>CAP_SYS_PTRACE</c>, <c>CAP_MKNOD</c> and <c>CAP_SYS_MODULE</c> are particularly dangerous when they are not used within a user namespace. Please refer to <c>user_namespaces(7)</c> for a more detailed explanation of the interaction between user namespaces and capabilities.</p><p>Before adding any capability, review its security implications and ensure it is really necessary for the container’s functionality. See <c>capabilities(7)</c> for more information.</p></p></summary>
    [Argument(Format = "--cap-add={value}")] public IReadOnlyList<string> CapAdd => Get<IReadOnlyList<string>>(() => CapAdd);
    /// <summary><p>Drop Linux capabilities.</p></summary>
    [Argument(Format = "--cap-drop={value}")] public IReadOnlyList<string> CapDrop => Get<IReadOnlyList<string>>(() => CapDrop);
    /// <summary><p>When running on cgroup v2, specify the cgroup file to write to and its value.<p>For example <c>--cgroup-conf=memory.high=1073741824</c> sets the memory.high limit to 1GB.</p></p></summary>
    [Argument(Format = "--cgroup-conf={value}")] public IReadOnlyDictionary<string, string> CgroupConf => Get<IReadOnlyDictionary<string, string>>(() => CgroupConf);
    /// <summary><p>Path to cgroups under which the cgroup for the container is created.<p>If the path is not absolute, the path is considered to be relative to the cgroups path of the init process. Cgroups are created if they do not already exist.</p></p></summary>
    [Argument(Format = "--cgroup-parent={value}")] public Nuke.Common.IO.AbsolutePath CgroupParent => Get<Nuke.Common.IO.AbsolutePath>(() => CgroupParent);
    /// <summary><p>Set the cgroup namespace mode for the container.<p>Possible values: <c>host</c> (use the host’s cgroup namespace inside the container), <c>container:id</c> (join the namespace of the specified container), <c>private</c> (create a new cgroup namespace), <c>ns:path</c> (join the namespace at the specified path).</p><p>If the host uses cgroups v1, the default is set to <c>host</c>. On cgroups v2, the default is <c>private</c>.</p></p></summary>
    [Argument(Format = "--cgroupns={value}")] public string CgroupNs => Get<string>(() => CgroupNs);
    /// <summary><p>Determines whether the container creates cgroups.<p>Default is <c>enabled</c>.</p><p>The <c>enabled</c> option creates a new cgroup under the cgroup-parent. The <c>disabled</c> option forces the container to not create cgroups, and thus conflicts with cgroup options (<c>--cgroupns</c> and <c>--cgroup-parent</c>). The <c>no-conmon</c> option disables a new cgroup only for the <c>conmon</c> process. The <c>split</c> option splits the current cgroup in two sub-cgroups: one for conmon and one for the container payload. It is not possible to set <c>--cgroup-parent</c> with <c>split</c>.</p></p></summary>
    [Argument(Format = "--cgroups={value}")] public string Cgroups => Get<string>(() => Cgroups);
    /// <summary><p>Path to a directory inside the container that is treated as a <c>chroot</c> directory.<p>Any Podman managed file (e.g., <c>/etc/resolv.conf</c>, <c>/etc/hosts</c>, <c>/etc/hostname</c>) that is mounted into the root directory is mounted into that location as well. Multiple directories are separated with a comma.</p></p></summary>
    [Argument(Format = "--chrootdirs={value}")] public IReadOnlyList<string> ChrootDirs => Get<IReadOnlyList<string>>(() => ChrootDirs);
    /// <summary><p>Write the container ID to value. The file is removed along with the container, except when used with podman --remote run on detached containers.</p></summary>
    [Argument(Format = "--cidfile={value}", Separator = ",")] public IReadOnlyList<Nuke.Common.IO.AbsolutePath> CidFile => Get<IReadOnlyList<Nuke.Common.IO.AbsolutePath>>(() => CidFile);
    /// <summary><p>Write the pid of the <c>conmon</c> process to a file.<p>As <c>conmon</c> runs in a separate process than Podman, this is necessary when using systemd to restart Podman containers.</p><p>This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--conmon-pidfile={value}")] public string ConmonPidFile => Get<string>(() => ConmonPidFile);
    /// <summary><p>Set the CPU period for the Completely Fair Scheduler (CFS), which is a duration in microseconds.<p>Once the container’s CPU quota is used up, it will not be scheduled to run until the current period ends. Defaults to 100000 microseconds.</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--cpu-period={value}")] public int? CpuPeriod => Get<int?>(() => CpuPeriod);
    /// <summary><p>Limit the CPU Completely Fair Scheduler (CFS) quota.<p>Limit the container’s CPU usage. By default, containers run with the full CPU resource. The limit is a number in microseconds. If a number is provided, the container is allowed to use that much CPU time until the CPU period ends (controllable via <c>--cpu-period</c>).</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--cpu-quota={value}")] public int? CpuQuota => Get<int?>(() => CpuQuota);
    /// <summary><p>Limit the CPU real-time period in microseconds.<p>Limit the container’s Real Time CPU usage. This option tells the kernel to restrict the container’s Real Time CPU usage to the period specified.</p><p>This option is only supported on cgroups V1 rootful systems.</p></p></summary>
    [Argument(Format = "--cpu-rt-period={value}")] public int? CpuRtPeriod => Get<int?>(() => CpuRtPeriod);
    /// <summary><p>Limit the CPU real-time runtime in microseconds.<p>Limit the containers Real Time CPU usage. This option tells the kernel to limit the amount of time in a given CPU period Real Time tasks may consume. Ex: Period of 1,000,000us and Runtime of 950,000us means that this container can consume 95% of available CPU and leave the remaining 5% to normal priority tasks.</p><p>The sum of all runtimes across containers cannot exceed the amount allotted to the parent cgroup.</p><p>This option is only supported on cgroups V1 rootful systems.</p></p></summary>
    [Argument(Format = "--cpu-rt-runtime={value}")] public int? CpuRtRuntime => Get<int?>(() => CpuRtRuntime);
    /// <summary><p>CPUs in which to allow execution.<p>Can be specified as a comma-separated list (e.g. <c>0,1</c>), as a range (e.g. <c>0-3</c>), or any combination thereof (e.g. <c>0-3,7,11-15</c>).</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--cpuset-cpus={value}")] public string CpusetCpus => Get<string>(() => CpusetCpus);
    /// <summary><p>Memory nodes (MEMs) in which to allow execution (0-3, 0,1).<p>Only effective on NUMA systems.</p><p>If there are four memory nodes on the system (0-3), use <c>--cpuset-mems=0,1</c> then processes in the container only uses memory from the first two memory nodes.</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--cpuset-mems={value}")] public string CpusetMems => Get<string>(() => CpusetMems);
    /// <summary><p>The <c>[key[:passphrase]]</c> to be used for decryption of images.<p>Key can point to keys and/or certificates. Decryption is tried with all keys. If the key is protected by a passphrase, it is required to be passed in the argument and omitted otherwise.</p></p></summary>
    [Argument(Format = "--decryption-key={value}")] public IReadOnlyList<string> DecryptionKey => Get<IReadOnlyList<string>>(() => DecryptionKey);
    /// <summary><p>Add a host device to the container.<p>Optional value parameter can be used to specify device permissions by combining <c>r</c> for read, <c>w</c> for write, and <c>m</c> for <c>mknod(2)</c>.</p><p>Example: <c>--device=/dev/sdc:/dev/xvdc:rwm</c>.</p><p>Note: if <c>host-device</c> is a symbolic link then it is resolved first. The container only stores the major and minor numbers of the host device.</p><p>Podman may load kernel modules required for using the specified device. The devices that Podman loads modules for when necessary are: <c>/dev/fuse</c>.</p><p>In rootless mode, the new device is bind mounted in the container from the host rather than Podman creating it within the container space. Because the bind mount retains its SELinux label on SELinux systems, the container can get permission denied when accessing the mounted device. Modify SELinux settings to allow containers to use all device labels via the following command: <c>sudo setsebool -P container_use_devices=true</c>.</p><p>Note: if the user only has access rights via a group, accessing the device from inside a rootless container fails. Use the <c>--group-add keep-groups</c> flag to pass the user’s supplementary group access into the container.</p></p></summary>
    [Argument(Format = "--device={value}")] public IReadOnlyList<string> Device => Get<IReadOnlyList<string>>(() => Device);
    /// <summary><p>Add a rule to the cgroup allowed devices list.<p>The rule is expected to be in the format specified in the Linux kernel documentation: <c>type</c> can be <c>a</c> (all), <c>c</c> (char), or <c>b</c> (block); <c>major</c> and <c>minor</c> can be a number or <c>*</c>; <c>mode</c> is a composition of <c>r</c> (read), <c>w</c> (write), and <c>m</c> (mknod(2)).</p></p></summary>
    [Argument(Format = "--device-cgroup-rule={value}")] public IReadOnlyList<string> DeviceCgroupRule => Get<IReadOnlyList<string>>(() => DeviceCgroupRule);
    /// <summary><p>Limit read rate (in bytes per second) from a device.<p>Example: <c>--device-read-bps=/dev/sda:1mb</c>.</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--device-read-bps={value}")] public IReadOnlyList<string> DeviceReadBps => Get<IReadOnlyList<string>>(() => DeviceReadBps);
    /// <summary><p>Limit read rate (in IO operations per second) from a device.<p>Example: <c>--device-read-iops=/dev/sda:1000</c>.</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--device-read-iops={value}")] public IReadOnlyList<string> DeviceReadIops => Get<IReadOnlyList<string>>(() => DeviceReadIops);
    /// <summary><p>Limit write rate (in bytes per second) to a device.<p>Example: <c>--device-write-bps=/dev/sda:1mb</c>.</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--device-write-bps={value}")] public IReadOnlyList<string> DeviceWriteBps => Get<IReadOnlyList<string>>(() => DeviceWriteBps);
    /// <summary><p>Limit write rate (in IO operations per second) to a device.<p>Example: <c>--device-write-iops=/dev/sda:1000</c>.</p><p>On some systems, changing the resource limits may not be allowed for non-root users.</p><p>This option is not supported on cgroups V1 rootless systems.</p></p></summary>
    [Argument(Format = "--device-write-iops={value}")] public IReadOnlyList<string> DeviceWriteIops => Get<IReadOnlyList<string>>(() => DeviceWriteIops);
    /// <summary><p>This is a Docker-specific option to disable image verification to a container registry and is not supported by Podman.<p>This option is a NOOP and provided solely for scripting compatibility.</p></p></summary>
    [Argument(Format = "--disable-content-trust")] public bool? DisableContentTrust => Get<bool?>(() => DisableContentTrust);
    /// <summary><p>Set custom DNS servers.<p>This option can be used to override the DNS configuration passed to the container. Typically this is necessary when the host DNS configuration is invalid for the container (e.g., <c>127.0.0.1</c>). When this is the case the <c>--dns</c> flag is necessary for every run.</p><p>The special value <c>none</c> can be specified to disable creation of <c>/etc/resolv.conf</c> in the container by Podman. The <c>/etc/resolv.conf</c> file in the image is then used without changes.</p><p>Note that <c>ipaddr</c> may be added directly to the container’s <c>/etc/resolv.conf</c>. This is not guaranteed though. For example, passing a custom network whose <c>dns_enabled</c> is set to <c>true</c> to <c>--network</c> will result in <c>/etc/resolv.conf</c> only referring to the aardvark-dns server. aardvark-dns then forwards to the supplied <c>ipaddr</c> for all non-container name queries.</p><p>This option cannot be combined with <c>--network</c> that is set to <c>none</c> or <c>container:id</c>.</p></p></summary>
    [Argument(Format = "--dns={value}")] public IReadOnlyList<string> Dns => Get<IReadOnlyList<string>>(() => Dns);
    /// <summary><p>Set custom DNS options.<p>Invalid if using <c>--dns-option</c> with <c>--network</c> that is set to <c>none</c> or <c>container:id</c>.</p></p></summary>
    [Argument(Format = "--dns-option={value}")] public IReadOnlyList<string> DnsOption => Get<IReadOnlyList<string>>(() => DnsOption);
    /// <summary><p>Set custom DNS search domains.<p>Invalid if using <c>--dns-search</c> with <c>--network</c> that is set to <c>none</c> or <c>container:id</c>. Use <c>--dns-search=.</c> to remove the search domain.</p></p></summary>
    [Argument(Format = "--dns-search={value}")] public IReadOnlyList<string> DnsSearch => Get<IReadOnlyList<string>>(() => DnsSearch);
    /// <summary><p>Override the default ENTRYPOINT from the image.<p>The ENTRYPOINT of an image is similar to a COMMAND because it specifies what executable to run when the container starts, but it is (purposely) more difficult to override. The ENTRYPOINT gives a container its default nature or behavior. When the ENTRYPOINT is set, the container runs as if it were that binary, complete with default options. More options can be passed in via the COMMAND. But, if a user wants to run something else inside the container, the <c>--entrypoint</c> option allows a new ENTRYPOINT to be specified.</p><p>Specify multi option commands in the form of a JSON string.</p></p></summary>
    [Argument(Format = "--entrypoint={value}")] public IReadOnlyList<string> EntryPoint => Get<IReadOnlyList<string>>(() => EntryPoint);
    /// <summary><p>Set environment variables.<p>This option allows arbitrary environment variables that are available for the process to be launched inside of the container. If an environment variable is specified without a value, Podman checks the host environment for a value and set the variable only if it is set on the host. As a special case, if an environment variable ending in <c>*</c> is specified without a value, Podman searches the host environment for variables starting with the prefix and adds those variables to the container.</p></p></summary>
    [Argument(Format = "--env={value}")] public IReadOnlyList<string> Env => Get<IReadOnlyList<string>>(() => Env);
    /// <summary><p>Read in a line-delimited file of environment variables.</p></summary>
    [Argument(Format = "--env-file={value}")] public string EnvFile => Get<string>(() => EnvFile);
    /// <summary><p>Use host environment inside of the container.<p>See Environment note below for precedence. This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--env-host")] public bool? EnvHost => Get<bool?>(() => EnvHost);
    /// <summary><p>Preprocess default environment variables for the containers.<p>For example if image contains environment variable <c>hello=world</c> user can preprocess it using <c>--env-merge hello=${hello}-some</c> so new value is <c>hello=world-some</c>.</p><p>Please note that if the environment variable <c>hello</c> is not present in the image, then it’ll be replaced by an empty string and so using <c>--env-merge hello=${hello}-some</c> would result in the new value of <c>hello=-some</c>, notice the leading <c>-</c> delimiter.</p></p></summary>
    [Argument(Format = "--env-merge={value}")] public IReadOnlyList<string> EnvMerge => Get<IReadOnlyList<string>>(() => EnvMerge);
    /// <summary><p>Expose a port or a range of ports (e.g. <c>--expose=3300-3310</c>).<p>The protocol can be <c>tcp</c>, <c>udp</c> or <c>sctp</c> and if not given <c>tcp</c> is assumed. This option matches the EXPOSE instruction for image builds and has no effect on the actual networking rules unless <c>-P/--publish-all</c> is used to forward to all exposed ports from random host ports. To forward specific ports from the host into the container use the <c>-p/--publish</c> option instead.</p></p></summary>
    [Argument(Format = "--expose={value}")] public IReadOnlyList<string> Expose => Get<IReadOnlyList<string>>(() => Expose);
    /// <summary><p>Run the container in a new user namespace using the supplied GID mapping.<p>This option conflicts with the <c>--userns</c> and <c>--subgidname</c> options. This option provides a way to map host GIDs to container GIDs in the same way as <c>--uidmap</c> maps host UIDs to container UIDs. For details see <c>--uidmap</c>.</p><p>Note: the <c>--gidmap</c> option cannot be called in conjunction with the <c>--pod</c> option as a gidmap cannot be set on the container level when in a pod.</p></p></summary>
    [Argument(Format = "--gidmap={value}")] public IReadOnlyList<string> GidMap => Get<IReadOnlyList<string>>(() => GidMap);
    /// <summary><p>GPU devices to add to the container (‘all’ to pass all GPUs).<p>Currently only Nvidia devices are supported.</p></p></summary>
    [Argument(Format = "--gpus={value}")] public string Gpus => Get<string>(() => Gpus);
    /// <summary><p>Assign additional groups to the primary user running within the container process.<p><c>keep-groups</c> is a special flag that tells Podman to keep the supplementary group access.</p><p>Allows container to use the user’s supplementary group access. If file systems or devices are only accessible by the rootless user’s group, this flag tells the OCI runtime to pass the group access into the container. Currently only available with the <c>crun</c> OCI runtime. Note: <c>keep-groups</c> is exclusive, other groups cannot be specified with this flag. This option is not available for remote commands, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--group-add={value}")] public IReadOnlyList<string> GroupAdd => Get<IReadOnlyList<string>>(() => GroupAdd);
    /// <summary><p>Customize the entry that is written to the <c>/etc/group</c> file within the container when <c>--user</c> is used.<p>The variables <c>$GROUPNAME</c>, <c>$GID</c>, and <c>$USERLIST</c> are automatically replaced with their value at runtime if present.</p></p></summary>
    [Argument(Format = "--group-entry={value}")] public string GroupEntry => Get<string>(() => GroupEntry);
    /// <summary><p>Set or alter a healthcheck command for a container.<p>The command is a command to be executed inside the container that determines the container health. The command is required for other healthcheck options to be applied. A value of <c>none</c> disables existing healthchecks.</p><p>Multiple options can be passed in the form of a JSON array; otherwise, the command is interpreted as an argument to <c>/bin/sh -c</c>.</p></p></summary>
    [Argument(Format = "--health-cmd={value}")] public IReadOnlyList<string> HealthCmd => Get<IReadOnlyList<string>>(() => HealthCmd);
    /// <summary><p>Set an interval for the healthchecks.<p>An <c>interval</c> of <c>disable</c> results in no automatic timer setup. The default is <c>30s</c>.</p></p></summary>
    [Argument(Format = "--health-interval={value}")] public string HealthInterval => Get<string>(() => HealthInterval);
    /// <summary><p>Set the destination of the HealthCheck log.<p>Possible values: <c>local</c> (HealthCheck logs are stored in overlay containers), <c>directory</c> (creates a log file named <c>&lt;container-ID&gt;-healthcheck.log</c> with HealthCheck logs in the specified directory), <c>events_logger</c> (the log will be written with logging mechanism set by events_logger).</p></p></summary>
    [Argument(Format = "--health-log-destination={value}")] public string HealthLogDestination => Get<string>(() => HealthLogDestination);
    /// <summary><p>Set maximum number of attempts in the HealthCheck log file.<p>‘0’ value means an infinite number of attempts in the log file. Default: 5 attempts.</p></p></summary>
    [Argument(Format = "--health-max-log-count={value}")] public int? HealthMaxLogCount => Get<int?>(() => HealthMaxLogCount);
    /// <summary><p>Set maximum length in characters of stored HealthCheck log.<p>‘0’ value means an infinite log length. Default: 500 characters.</p></p></summary>
    [Argument(Format = "--health-max-log-size={value}")] public int? HealthMaxLogSize => Get<int?>(() => HealthMaxLogSize);
    /// <summary><p>Action to take once the container transitions to an unhealthy state.<p>Possible values: <c>none</c> (take no action), <c>kill</c> (kill the container), <c>restart</c> (restart the container; do not combine the <c>restart</c> action with the <c>--restart</c> flag), <c>stop</c> (stop the container).</p></p></summary>
    [Argument(Format = "--health-on-failure={value}")] public string HealthOnFailure => Get<string>(() => HealthOnFailure);
    /// <summary><p>The number of retries allowed before a healthcheck is considered to be unhealthy.<p>The default value is <c>3</c>.</p></p></summary>
    [Argument(Format = "--health-retries={value}")] public int? HealthRetries => Get<int?>(() => HealthRetries);
    /// <summary><p>The initialization time needed for a container to bootstrap.<p>The value can be expressed in time format like <c>2m3s</c>. The default value is <c>0s</c>.</p><p>Note: The health check command is executed as soon as a container is started, if the health check is successful the container’s health state will be updated to <c>healthy</c>. However, if the health check fails, the health state will stay as <c>starting</c> until either the health check is successful or until the <c>--health-start-period</c> time is over. If the health check command fails after the <c>--health-start-period</c> time is over, the health state will be updated to <c>unhealthy</c>.</p></p></summary>
    [Argument(Format = "--health-start-period={value}")] public string HealthStartPeriod => Get<string>(() => HealthStartPeriod);
    /// <summary><p>Set a startup healthcheck command for a container.<p>This command is executed inside the container and is used to gate the regular healthcheck. When the startup command succeeds, the regular healthcheck begins and the startup healthcheck ceases. Optionally, if the command fails for a set number of attempts, the container is restarted. A startup healthcheck can be used to ensure that containers with an extended startup period are not marked as unhealthy until they are fully started. Startup healthchecks can only be used when a regular healthcheck (from the container’s image or the <c>--health-cmd</c> option) is also set.</p></p></summary>
    [Argument(Format = "--health-startup-cmd={value}")] public IReadOnlyList<string> HealthStartupCmd => Get<IReadOnlyList<string>>(() => HealthStartupCmd);
    /// <summary><p>Set an interval for the startup healthcheck.<p>An <c>interval</c> of <c>disable</c> results in no automatic timer setup. The default is <c>30s</c>.</p></p></summary>
    [Argument(Format = "--health-startup-interval={value}")] public string HealthStartupInterval => Get<string>(() => HealthStartupInterval);
    /// <summary><p>The number of attempts allowed before the startup healthcheck restarts the container.<p>If set to <c>0</c>, the container is never restarted. The default is <c>0</c>.</p></p></summary>
    [Argument(Format = "--health-startup-retries={value}")] public int? HealthStartupRetries => Get<int?>(() => HealthStartupRetries);
    /// <summary><p>The number of successful runs required before the startup healthcheck succeeds and the regular healthcheck begins.<p>A value of <c>0</c> means that any success begins the regular healthcheck. The default is <c>0</c>.</p></p></summary>
    [Argument(Format = "--health-startup-success={value}")] public int? HealthStartupSuccess => Get<int?>(() => HealthStartupSuccess);
    /// <summary><p>The maximum time a startup healthcheck command has to complete before it is marked as failed.<p>The value can be expressed in a time format like <c>2m3s</c>. The default value is <c>30s</c>.</p></p></summary>
    [Argument(Format = "--health-startup-timeout={value}")] public string HealthStartupTimeout => Get<string>(() => HealthStartupTimeout);
    /// <summary><p>The maximum time allowed to complete the healthcheck before an interval is considered failed.<p>Like start-period, the value can be expressed in a time format such as <c>1m22s</c>. The default value is <c>30s</c>.</p></p></summary>
    [Argument(Format = "--health-timeout={value}")] public string HealthTimeout => Get<string>(() => HealthTimeout);
    /// <summary><p>Sets the container host name that is available inside the container.<p>Can only be used with a private UTS namespace <c>--uts=private</c> (default). If <c>--pod</c> is specified and the pod shares the UTS namespace (default) the pod’s hostname is used.</p></p></summary>
    [Argument(Format = "--hostname={value}")] public string Hostname => Get<string>(() => Hostname);
    /// <summary><p>Add a user account to <c>/etc/passwd</c> from the host to the container.<p>The Username or UID must exist on the host system.</p></p></summary>
    [Argument(Format = "--hostuser={value}")] public string HostUser => Get<string>(() => HostUser);
    /// <summary><p>By default proxy environment variables are passed into the container if set for the Podman process.<p>This can be disabled by setting the value to <c>false</c>. The environment variables passed in include <c>http_proxy</c>, <c>https_proxy</c>, <c>ftp_proxy</c>, <c>no_proxy</c>, and also the upper case versions of those. This option is only needed when the host system must use a proxy but the container does not use any proxy. Proxy environment variables specified for the container in any other way overrides the values that have been passed through from the host. When used with the remote client it uses the proxy environment variables that are set on the server process.</p><p>Defaults to <c>true</c>.</p></p></summary>
    [Argument(Format = "--http-proxy={value}")] public bool? HttpProxy => Get<bool?>(() => HttpProxy);
    /// <summary><p>Tells Podman how to handle the builtin image volumes.<p>Possible values: <c>bind</c> (an anonymous named volume is created and mounted into the container), <c>tmpfs</c> (the volume is mounted onto the container as a tmpfs, which allows the users to create content that disappears when the container is stopped), <c>ignore</c> (all volumes are just ignored and no action is taken).</p></p></summary>
    [Argument(Format = "--image-volume={value}")] public string ImageVolume => Get<string>(() => ImageVolume);
    /// <summary><p>Run an init inside the container that forwards signals and reaps processes.<p>The container-init binary is mounted at <c>/run/podman-init</c>. Mounting over <c>/run</c> breaks container execution.</p></p></summary>
    [Argument(Format = "--init")] public bool? Init => Get<bool?>(() => Init);
    /// <summary><p>When using pods, create an init style container.<p>Valid values: <c>always</c> (the container runs with each and every <c>pod start</c>), <c>once</c> (the container only runs once when the pod is started and then the container is removed).</p><p>Init containers are only run on pod <c>start</c>. Restarting a pod does not execute any init containers. Furthermore, init containers can only be created in a pod when that pod is not running.</p></p></summary>
    [Argument(Format = "--init-ctr={value}")] public string InitCtr => Get<string>(() => InitCtr);
    /// <summary><p>Path to the container-init binary.</p></summary>
    [Argument(Format = "--init-path={value}")] public string InitPath => Get<string>(() => InitPath);
    /// <summary><p>When set to <c>true</c>, keep stdin open even if not attached.<p>The default is <c>false</c>.</p></p></summary>
    [Argument(Format = "--interactive")] public bool? Interactive => Get<bool?>(() => Interactive);
    /// <summary><p>Specify a static IPv4 address for the container.<p>Example: <c>10.88.64.128</c>. This option can only be used if the container is joined to only a single network - i.e., <c>--network=network-name</c> is used at most once - and if the container is not joining another container’s network namespace via <c>--network=container:id</c>. The address must be within the network’s IP address pool (default <c>10.88.0.0/16</c>).</p><p>To specify multiple static IP addresses per container, set multiple networks using the <c>--network</c> option with a static IP address specified for each using the <c>ip</c> mode for that option.</p></p></summary>
    [Argument(Format = "--ip={value}")] public string Ip => Get<string>(() => Ip);
    /// <summary><p>Specify a static IPv6 address for the container.</p></summary>
    [Argument(Format = "--ip6={value}")] public string Ip6 => Get<string>(() => Ip6);
    /// <summary><p>Add metadata to a container.<p>Example: <c>--label com.example.key=value</c>.</p></p></summary>
    [Argument(Format = "--label={value}")] public IReadOnlyList<string> Label => Get<IReadOnlyList<string>>(() => Label);
    /// <summary><p>Logging driver for the container.<p>Possible values: <c>k8s-file</c>, <c>journald</c>, <c>json-file</c>, <c>none</c>.</p></p></summary>
    [Argument(Format = "--log-driver={value}")] public string LogDriver => Get<string>(() => LogDriver);
    /// <summary><p>Log driver options.</p></summary>
    [Argument(Format = "--log-opt={value}")] public IReadOnlyList<string> LogOpt => Get<IReadOnlyList<string>>(() => LogOpt);
    /// <summary><p>Container MAC address.<p>Example: <c>92:d0:c6:0a:29:33</c>.</p></p></summary>
    [Argument(Format = "--mac-address={value}")] public string MacAddress => Get<string>(() => MacAddress);
    /// <summary><p>Memory limit.<p>Format: <c>&lt;number&gt;[&lt;unit&gt;]</c>, where unit = b (bytes), k (kilobytes), m (megabytes), or g (gigabytes).</p></p></summary>
    [Argument(Format = "--memory={value}")] public string Memory => Get<string>(() => Memory);
    /// <summary><p>Swap limit equal to memory plus swap.<p><c>0</c> to disable swap.</p></p></summary>
    [Argument(Format = "--memory-swap={value}")] public string MemorySwap => Get<string>(() => MemorySwap);
    /// <summary><p>Attach a filesystem mount to the container.</p></summary>
    [Argument(Format = "--mount={value}")] public IReadOnlyList<string> Mount => Get<IReadOnlyList<string>>(() => Mount);
    /// <summary><p>Assign a name to the container.</p></summary>
    [Argument(Format = "--name={value}")] public string Name => Get<string>(() => Name);
    /// <summary><p>Connect a container to a network.<p>Possible values: <c>bridge</c>, <c>host</c>, <c>none</c>, <c>container:id</c>.</p></p></summary>
    [Argument(Format = "--network={value}")] public IReadOnlyList<string> Network => Get<IReadOnlyList<string>>(() => Network);
    /// <summary><p>Add network-scoped alias for the container.</p></summary>
    [Argument(Format = "--network-alias={value}")] public IReadOnlyList<string> NetworkAlias => Get<IReadOnlyList<string>>(() => NetworkAlias);
    /// <summary><p>Do not create <c>/etc/hosts</c> for the container.</p></summary>
    [Argument(Format = "--no-hosts")] public bool? NoHosts => Get<bool?>(() => NoHosts);
    /// <summary><p>Disable OOM Killer for the container.</p></summary>
    [Argument(Format = "--oom-kill-disable")] public bool? OomKillDisable => Get<bool?>(() => OomKillDisable);
    /// <summary><p>Tune the host’s OOM preferences for the container.</p></summary>
    [Argument(Format = "--oom-score-adj={value}")] public int? OomScoreAdj => Get<int?>(() => OomScoreAdj);
    /// <summary><p>Set the PID namespace mode for the container.<p>Possible values: <c>container:id</c> (reuses another container’s PID namespace), <c>host</c> (use the host’s PID namespace inside the container), <c>private</c> (create a new PID namespace).</p></p></summary>
    [Argument(Format = "--pid={value}")] public string Pid => Get<string>(() => Pid);
    /// <summary><p>Tune container pids limit.</p></summary>
    [Argument(Format = "--pids-limit={value}")] public int? PidsLimit => Get<int?>(() => PidsLimit);
    /// <summary><p>Run container in an existing pod.</p></summary>
    [Argument(Format = "--pod={value}")] public string Pod => Get<string>(() => Pod);
    /// <summary><p>Write the pod ID to the file.</p></summary>
    [Argument(Format = "--pod-id-file={value}")] public string PodIdFile => Get<string>(() => PodIdFile);
    /// <summary><p>Pass value additional file descriptors to the container.</p></summary>
    [Argument(Format = "--preserve-fds={value}")] public int? PreserveFds => Get<int?>(() => PreserveFds);
    /// <summary><p>Give extended privileges to this container.</p></summary>
    [Argument(Format = "--privileged")] public bool? Privileged => Get<bool?>(() => Privileged);
    /// <summary><p>Publish a container’s port to the host.<p>Format: <c>ip:hostPort:containerPort</c> or <c>hostPort:containerPort</c>.</p></p></summary>
    [Argument(Format = "--publish={value}")] public IReadOnlyList<string> Publish => Get<IReadOnlyList<string>>(() => Publish);
    /// <summary><p>Publish all exposed ports to random ports.</p></summary>
    [Argument(Format = "--publish-all")] public bool? PublishAll => Get<bool?>(() => PublishAll);
    /// <summary><p>Mount the container’s root filesystem as read only.</p></summary>
    [Argument(Format = "--read-only")] public bool? ReadOnly => Get<bool?>(() => ReadOnly);
    /// <summary><p>Mount a tmpfs on <c>/run</c>, <c>/tmp</c>, and <c>/var/tmp</c>.</p></summary>
    [Argument(Format = "--read-only-tmpfs")] public bool? ReadOnlyTmpfs => Get<bool?>(() => ReadOnlyTmpfs);
    /// <summary><p>If a container with the same name exists, replace it.</p></summary>
    [Argument(Format = "--replace")] public bool? Replace => Get<bool?>(() => Replace);
    /// <summary><p>Restart policy to apply when a container exits.<p>Possible values: <c>no</c>, <c>always</c>, <c>on-failure</c>, <c>unless-stopped</c>.</p></p></summary>
    [Argument(Format = "--restart={value}")] public string Restart => Get<string>(() => Restart);
    /// <summary><p>Remove the container and pod if it exists after it exits.</p></summary>
    [Argument(Format = "--rm")] public bool? Rm => Get<bool?>(() => Rm);
    /// <summary><p>Security Options.</p></summary>
    [Argument(Format = "--security-opt={value}")] public IReadOnlyList<string> SecurityOpt => Get<IReadOnlyList<string>>(() => SecurityOpt);
    /// <summary><p>Size of <c>/dev/shm</c>.</p></summary>
    [Argument(Format = "--shm-size={value}")] public string ShmSize => Get<string>(() => ShmSize);
    /// <summary><p>Proxy received signals to the process.</p></summary>
    [Argument(Format = "--sig-proxy={value}")] public bool? SigProxy => Get<bool?>(() => SigProxy);
    /// <summary><p>Signal to stop a container.</p></summary>
    [Argument(Format = "--stop-signal={value}")] public string StopSignal => Get<string>(() => StopSignal);
    /// <summary><p>Timeout (in seconds) to stop a container.</p></summary>
    [Argument(Format = "--stop-timeout={value}")] public int? StopTimeout => Get<int?>(() => StopTimeout);
    /// <summary><p>Name of range listed in <c>/etc/subgid</c>.</p></summary>
    [Argument(Format = "--subgidname={value}")] public string Subgidname => Get<string>(() => Subgidname);
    /// <summary><p>Name of range listed in <c>/etc/subuid</c>.</p></summary>
    [Argument(Format = "--subuidname={value}")] public string Subuidname => Get<string>(() => Subuidname);
    /// <summary><p>Sysctl options.</p></summary>
    [Argument(Format = "--sysctl={value}")] public IReadOnlyList<string> Sysctl => Get<IReadOnlyList<string>>(() => Sysctl);
    /// <summary><p>Mount a tmpfs.</p></summary>
    [Argument(Format = "--tmpfs={value}")] public IReadOnlyList<string> Tmpfs => Get<IReadOnlyList<string>>(() => Tmpfs);
    /// <summary><p>Allocate a pseudo-TTY.</p></summary>
    [Argument(Format = "--tty")] public bool? Tty => Get<bool?>(() => Tty);
    /// <summary><p>Ulimit options.</p></summary>
    [Argument(Format = "--ulimit={value}")] public IReadOnlyList<string> Ulimit => Get<IReadOnlyList<string>>(() => Ulimit);
    /// <summary><p>Username or UID.<p>Format: <c>&lt;name|uid&gt;[:&lt;group|gid&gt;]</c>.</p></p></summary>
    [Argument(Format = "--user={value}")] public string User => Get<string>(() => User);
    /// <summary><p>Set the user namespace mode for the container.<p>Possible values: <c>host</c> (use the host’s user namespace inside the container), <c>container:id</c> (join the namespace of the specified container), <c>private</c> (create a new user namespace), <c>ns:path</c> (join the namespace at the specified path).</p></p></summary>
    [Argument(Format = "--userns={value}")] public string Userns => Get<string>(() => Userns);
    /// <summary><p>Set the UTS namespace mode for the container.<p>Possible values: <c>host</c> (use the host’s UTS namespace inside the container), <c>container:id</c> (join the namespace of the specified container), <c>private</c> (create a new UTS namespace), <c>ns:path</c> (join the namespace at the specified path).</p></p></summary>
    [Argument(Format = "--uts={value}")] public string Uts => Get<string>(() => Uts);
    /// <summary><p>Bind mount a volume.</p></summary>
    [Argument(Format = "--volume={value}")] public IReadOnlyList<string> Volume => Get<IReadOnlyList<string>>(() => Volume);
    /// <summary><p>Mount volumes from the specified container(s).</p></summary>
    [Argument(Format = "--volumes-from={value}")] public IReadOnlyList<string> VolumesFrom => Get<IReadOnlyList<string>>(() => VolumesFrom);
    /// <summary><p>Working directory inside the container.</p></summary>
    [Argument(Format = "--workdir={value}")] public string Workdir => Get<string>(() => Workdir);
}
#endregion
#region PodmanDiffSettings
/// <inheritdoc cref="PodmanTasks.PodmanDiff(Candoumbe.Pipelines.Tools.Podman.PodmanDiffSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanDiff), Arguments = "diff")]
public partial class PodmanDiffSettings : ToolOptions
{
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
#region PodmanImagePullSettings
/// <inheritdoc cref="PodmanTasks.PodmanImagePull(Candoumbe.Pipelines.Tools.Podman.PodmanImagePullSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PodmanTasks), Command = nameof(PodmanTasks.PodmanImagePull), Arguments = "image pull")]
public partial class PodmanImagePullSettings : ToolOptions
{
    /// <summary><p>Path of the authentication file. Default is <c>${XDG_RUNTIME_DIR}/containers/auth.json</c> on Linux, and <c>$HOME/.config/containers/auth.json</c> on Windows/macOS.<p>The file is created by <see href="https://docs.podman.io/en/latest/markdown/podman-login.1.html">podman login</see>. If the authorization state is not found there, <c>$HOME/.docker/config.json</c> is checked, which is set using <c>docker login</c>.</p><p>Note: There is also the option to override the default path of the authentication file by setting the <c>REGISTRY_AUTH_FILE</c> environment variable.</p></p></summary>
    [Argument(Format = "--authfile={value}")] public Nuke.Common.IO.AbsolutePath Authfile => Get<Nuke.Common.IO.AbsolutePath>(() => Authfile);
    /// <summary><p>Use certificates at <i>path</i> (.crt, .cert, .key) to connect to the registry. (Default: <c>/etc/containers/certs.d</c>).<p>For details, see <see href="https://github.com/containers/image/blob/main/docs/containers-certs.d.5.md">containers-certs.d(5)</see>.</p><p>This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--cert-dir={value}")] public string CertDir => Get<string>(() => CertDir);
    /// <summary><p>Compress tarball image layers when pushing to a directory using the ‘dir’ transport. (default is same compression type, compressed or uncompressed, as source).<p>Note: This flag can only be set when using the <c>dir</c> transport.</p></p></summary>
    [Argument(Format = "--compress")] public bool? Compress => Get<bool?>(() => Compress);
    /// <summary><p>Specifies the compression format to use. Supported values are: <list type="bullet"><item><c>gzip</c></item><item><c>zstd</c></item><item><c>zstd:chunked</c></item></list>The default is <c>gzip</c> unless overridden in the containers.conf file.<p><c>zstd:chunked</c> is incompatible with encrypting images, and will be treated as <c>zstd</c> with a warning in that case.</p></p></summary>
    [Argument(Format = "--compression-format={value}")] public CompressionFormatType CompressionFormat => Get<CompressionFormatType>(() => CompressionFormat);
    /// <summary><p>Specifies the compression level to use. The value is specific to the compression algorithm used, e.g. for zstd the accepted values are in the range 1-20 (inclusive) with a default of 3, while for gzip it is 1-9 (inclusive) and has a default of 5.</p></summary>
    [Argument(Format = "--compression-level={value}")] public int? CompressionLevel => Get<int?>(() => CompressionLevel);
    /// <summary><p>The <c>[username[:password]]</c> to use to authenticate with the registry, if required.<p>If one or both values are not supplied, a command line prompt appears and the value can be entered. The password is entered without echo.</p><p>Note that the specified credentials are only used to authenticate against target registries. They are not used for mirrors or when the registry gets rewritten; to authenticate against those consider using a <see href="https://github.com/containers/image/blob/main/docs/containers-auth.json.5.md">containers-auth.json(5)</see> file.</p></p></summary>
    [Argument(Format = "--creds={value}")] public string Creds => Get<string>(() => Creds);
    /// <summary><p>After copying the image, write the digest of the resulting image to the file.</p></summary>
    [Argument(Format = "--digestfile={value}")] public string Digestfile => Get<string>(() => Digestfile);
    /// <summary><p>This is a Docker-specific option to disable image verification to a container registry and is not supported by Podman. This option is a NOOP and provided solely for scripting compatibility.</p></summary>
    [Argument(Format = "--disable-content-trust")] public bool? DisableContentTrust => Get<bool?>(() => DisableContentTrust);
    /// <summary><p>Layer(s) to encrypt: 0-indexed layer indices with support for negative indexing (e.g. 0 is the first layer, -1 is the last layer). If not defined, encrypts all layers if encryption-key flag is specified.</p></summary>
    [Argument(Format = "--encrypt-layer={value}")] public string EncryptLayer => Get<string>(() => EncryptLayer);
    /// <summary><p>The <c>[protocol:keyfile]</c> specifies the encryption protocol, which can be JWE (RFC7516), PGP (RFC4880), and PKCS7 (RFC2315) and the key material required for image encryption. For instance, <c>jwe:/path/to/key.pem</c> or <c>pgp:admin@example.com</c> or <c>pkcs7:/path/to/x509-file</c>.</p></summary>
    [Argument(Format = "--encryption-key={value}")] public string EncryptionKey => Get<string>(() => EncryptionKey);
    /// <summary><p>If set, push uses the specified compression algorithm even if the destination contains a differently-compressed variant already.<p>Defaults to <c>true</c> if <c>--compression-format</c> is explicitly specified on the command-line, <c>false</c> otherwise.</p></p></summary>
    [Argument(Format = "--force-compression")] public bool? ForceCompression => Get<bool?>(() => ForceCompression);
    /// <summary><p>Manifest Type (<c>oci</c>, <c>v2s2</c>, or <c>v2s1</c>) to use when pushing an image.</p></summary>
    [Argument(Format = "--format={value}")] public ManifestType Format => Get<ManifestType>(() => Format);
    /// <summary><p>When writing the output image, suppress progress output.</p></summary>
    [Argument(Format = "--quiet")] public bool? Quiet => Get<bool?>(() => Quiet);
    /// <summary><p>Discard any pre-existing signatures in the image.</p></summary>
    [Argument(Format = "--remove-signatures")] public bool? RemoveSignatures => Get<bool?>(() => RemoveSignatures);
    /// <summary><p>Number of times to retry pulling or pushing images between the registry and local storage in case of failure. Default is <c>3</c>.</p></summary>
    [Argument(Format = "--retry={value}")] public int? Retry => Get<int?>(() => Retry);
    /// <summary><p>Duration of delay between retry attempts when pulling or pushing images between the registry and local storage in case of failure. The default is to start at two seconds and then exponentially back off. The delay is used when this value is set, and no exponential back off occurs.</p></summary>
    [Argument(Format = "--retry-delay={value}")] public string RetryDelay => Get<string>(() => RetryDelay);
    /// <summary><p>Add a “simple signing” signature at the destination using the specified key.<p>This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--sign-by={value}")] public string SignBy => Get<string>(() => SignBy);
    /// <summary><p>Add a sigstore signature based on further options specified in a container’s sigstore signing parameter file <c>param-file</c>.<p>See <see href="https://github.com/containers/image/blob/main/docs/containers-sigstore-signing-params.yaml.5.md">containers-sigstore-signing-params.yaml(5)</see> for details about the file format.</p></p></summary>
    [Argument(Format = "--sign-by-sigstore={value}")] public string SignBySigstore => Get<string>(() => SignBySigstore);
    /// <summary><p>Add a sigstore signature at the destination using a private key at the specified path.<p>This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--sign-by-sigstore-private-key={value}")] public Nuke.Common.IO.AbsolutePath SignBySigstorePrivateKey => Get<Nuke.Common.IO.AbsolutePath>(() => SignBySigstorePrivateKey);
    /// <summary><p>Add a “simple signing” signature using a Sequoia-PGP key with the specified fingerprint.<p>This option is not available with the remote Podman client, including Mac and Windows (excluding WSL2) machines.</p></p></summary>
    [Argument(Format = "--sign-by-sq-fingerprint={value}")] public string SignBySqFingerprint => Get<string>(() => SignBySqFingerprint);
    /// <summary><p>If signing the image (using <c>--sign-by</c>, <c>sign-by-sq-fingerprint</c> or <c>--sign-by-sigstore-private-key</c>), read the passphrase to use from the specified path.</p></summary>
    [Argument(Format = "--sign-passphrase-file={value}")] public Nuke.Common.IO.AbsolutePath SignPassphraseFile => Get<Nuke.Common.IO.AbsolutePath>(() => SignPassphraseFile);
    /// <summary><p>Require HTTPS and verify certificates when contacting registries (default: <c>true</c>).<p>If explicitly set to <c>true</c>, TLS verification is used. If set to <c>false</c>, TLS verification is not used. If not specified, TLS verification is used unless the target registry is listed as an insecure registry in <see href="https://github.com/containers/image/blob/main/docs/containers-registries.d.5.md">containers-registries.conf(5)</see>.</p></p></summary>
    [Argument(Format = "--tls-verify")] public bool? TlsVerify => Get<bool?>(() => TlsVerify);
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
    #region Logsplit
    /// <inheritdoc cref="PodmanBuildSettings.Logsplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Logsplit))]
    public static T SetLogsplit<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Logsplit, v));
    /// <inheritdoc cref="PodmanBuildSettings.Logsplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Logsplit))]
    public static T ResetLogsplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Logsplit));
    /// <inheritdoc cref="PodmanBuildSettings.Logsplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Logsplit))]
    public static T EnableLogsplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Logsplit, true));
    /// <inheritdoc cref="PodmanBuildSettings.Logsplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Logsplit))]
    public static T DisableLogsplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Logsplit, false));
    /// <inheritdoc cref="PodmanBuildSettings.Logsplit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Logsplit))]
    public static T ToggleLogsplit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Logsplit, !o.Logsplit));
    #endregion
    #region Manifest
    /// <inheritdoc cref="PodmanBuildSettings.Manifest"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Manifest))]
    public static T SetManifest<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Manifest, v));
    /// <inheritdoc cref="PodmanBuildSettings.Manifest"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Manifest))]
    public static T ResetManifest<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Manifest));
    #endregion
    #region Memory
    /// <inheritdoc cref="PodmanBuildSettings.Memory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Memory))]
    public static T SetMemory<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Memory, v));
    /// <inheritdoc cref="PodmanBuildSettings.Memory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Memory))]
    public static T ResetMemory<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Memory));
    #endregion
    #region MemorySwap
    /// <inheritdoc cref="PodmanBuildSettings.MemorySwap"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.MemorySwap))]
    public static T SetMemorySwap<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.MemorySwap, v));
    /// <inheritdoc cref="PodmanBuildSettings.MemorySwap"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.MemorySwap))]
    public static T ResetMemorySwap<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.MemorySwap));
    #endregion
    #region Network
    /// <inheritdoc cref="PodmanBuildSettings.Network"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Network))]
    public static T SetNetwork<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Network, v));
    /// <inheritdoc cref="PodmanBuildSettings.Network"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Network))]
    public static T ResetNetwork<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Network));
    #endregion
    #region NoCache
    /// <inheritdoc cref="PodmanBuildSettings.NoCache"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoCache))]
    public static T SetNoCache<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoCache, v));
    /// <inheritdoc cref="PodmanBuildSettings.NoCache"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoCache))]
    public static T ResetNoCache<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.NoCache));
    /// <inheritdoc cref="PodmanBuildSettings.NoCache"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoCache))]
    public static T EnableNoCache<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoCache, true));
    /// <inheritdoc cref="PodmanBuildSettings.NoCache"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoCache))]
    public static T DisableNoCache<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoCache, false));
    /// <inheritdoc cref="PodmanBuildSettings.NoCache"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoCache))]
    public static T ToggleNoCache<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoCache, !o.NoCache));
    #endregion
    #region NoHostname
    /// <inheritdoc cref="PodmanBuildSettings.NoHostname"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHostname))]
    public static T SetNoHostname<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHostname, v));
    /// <inheritdoc cref="PodmanBuildSettings.NoHostname"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHostname))]
    public static T ResetNoHostname<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.NoHostname));
    /// <inheritdoc cref="PodmanBuildSettings.NoHostname"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHostname))]
    public static T EnableNoHostname<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHostname, true));
    /// <inheritdoc cref="PodmanBuildSettings.NoHostname"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHostname))]
    public static T DisableNoHostname<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHostname, false));
    /// <inheritdoc cref="PodmanBuildSettings.NoHostname"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHostname))]
    public static T ToggleNoHostname<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHostname, !o.NoHostname));
    #endregion
    #region NoHosts
    /// <inheritdoc cref="PodmanBuildSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHosts))]
    public static T SetNoHosts<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHosts, v));
    /// <inheritdoc cref="PodmanBuildSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHosts))]
    public static T ResetNoHosts<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.NoHosts));
    /// <inheritdoc cref="PodmanBuildSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHosts))]
    public static T EnableNoHosts<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHosts, true));
    /// <inheritdoc cref="PodmanBuildSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHosts))]
    public static T DisableNoHosts<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHosts, false));
    /// <inheritdoc cref="PodmanBuildSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.NoHosts))]
    public static T ToggleNoHosts<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.NoHosts, !o.NoHosts));
    #endregion
    #region OmitHistory
    /// <inheritdoc cref="PodmanBuildSettings.OmitHistory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OmitHistory))]
    public static T SetOmitHistory<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.OmitHistory, v));
    /// <inheritdoc cref="PodmanBuildSettings.OmitHistory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OmitHistory))]
    public static T ResetOmitHistory<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.OmitHistory));
    /// <inheritdoc cref="PodmanBuildSettings.OmitHistory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OmitHistory))]
    public static T EnableOmitHistory<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.OmitHistory, true));
    /// <inheritdoc cref="PodmanBuildSettings.OmitHistory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OmitHistory))]
    public static T DisableOmitHistory<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.OmitHistory, false));
    /// <inheritdoc cref="PodmanBuildSettings.OmitHistory"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OmitHistory))]
    public static T ToggleOmitHistory<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.OmitHistory, !o.OmitHistory));
    #endregion
    #region Os
    /// <inheritdoc cref="PodmanBuildSettings.Os"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Os))]
    public static T SetOs<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Os, v));
    /// <inheritdoc cref="PodmanBuildSettings.Os"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Os))]
    public static T ResetOs<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Os));
    #endregion
    #region OsFeature
    /// <inheritdoc cref="PodmanBuildSettings.OsFeature"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OsFeature))]
    public static T SetOsFeature<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.OsFeature, v));
    /// <inheritdoc cref="PodmanBuildSettings.OsFeature"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OsFeature))]
    public static T ResetOsFeature<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.OsFeature));
    #endregion
    #region OsVersion
    /// <inheritdoc cref="PodmanBuildSettings.OsVersion"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OsVersion))]
    public static T SetOsVersion<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.OsVersion, v));
    /// <inheritdoc cref="PodmanBuildSettings.OsVersion"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.OsVersion))]
    public static T ResetOsVersion<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.OsVersion));
    #endregion
    #region Output
    /// <inheritdoc cref="PodmanBuildSettings.Output"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Output))]
    public static T SetOutput<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Output, v));
    /// <inheritdoc cref="PodmanBuildSettings.Output"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Output))]
    public static T ResetOutput<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Output));
    #endregion
    #region Pid
    /// <inheritdoc cref="PodmanBuildSettings.Pid"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Pid))]
    public static T SetPid<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Pid, v));
    /// <inheritdoc cref="PodmanBuildSettings.Pid"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Pid))]
    public static T ResetPid<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Pid));
    #endregion
    #region Platform
    /// <inheritdoc cref="PodmanBuildSettings.Platform"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Platform))]
    public static T SetPlatform<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Platform, v));
    /// <inheritdoc cref="PodmanBuildSettings.Platform"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Platform))]
    public static T ResetPlatform<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Platform));
    #endregion
    #region Pull
    /// <inheritdoc cref="PodmanBuildSettings.Pull"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Pull))]
    public static T SetPull<T>(this T o, PullPolicy v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Pull, v));
    /// <inheritdoc cref="PodmanBuildSettings.Pull"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Pull))]
    public static T ResetPull<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Pull));
    #endregion
    #region Quiet
    /// <inheritdoc cref="PodmanBuildSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Quiet))]
    public static T SetQuiet<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Quiet, v));
    /// <inheritdoc cref="PodmanBuildSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Quiet))]
    public static T ResetQuiet<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Quiet));
    /// <inheritdoc cref="PodmanBuildSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Quiet))]
    public static T EnableQuiet<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Quiet, true));
    /// <inheritdoc cref="PodmanBuildSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Quiet))]
    public static T DisableQuiet<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Quiet, false));
    /// <inheritdoc cref="PodmanBuildSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Quiet))]
    public static T ToggleQuiet<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Quiet, !o.Quiet));
    #endregion
    #region Retry
    /// <inheritdoc cref="PodmanBuildSettings.Retry"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Retry))]
    public static T SetRetry<T>(this T o, uint? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Retry, v));
    /// <inheritdoc cref="PodmanBuildSettings.Retry"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Retry))]
    public static T ResetRetry<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Retry));
    #endregion
    #region RetryDelay
    /// <inheritdoc cref="PodmanBuildSettings.RetryDelay"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RetryDelay))]
    public static T SetRetryDelay<T>(this T o, uint? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.RetryDelay, v));
    /// <inheritdoc cref="PodmanBuildSettings.RetryDelay"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RetryDelay))]
    public static T ResetRetryDelay<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.RetryDelay));
    #endregion
    #region RewriteTimestamp
    /// <inheritdoc cref="PodmanBuildSettings.RewriteTimestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RewriteTimestamp))]
    public static T SetRewriteTimestamp<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.RewriteTimestamp, v));
    /// <inheritdoc cref="PodmanBuildSettings.RewriteTimestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RewriteTimestamp))]
    public static T ResetRewriteTimestamp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.RewriteTimestamp));
    /// <inheritdoc cref="PodmanBuildSettings.RewriteTimestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RewriteTimestamp))]
    public static T EnableRewriteTimestamp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.RewriteTimestamp, true));
    /// <inheritdoc cref="PodmanBuildSettings.RewriteTimestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RewriteTimestamp))]
    public static T DisableRewriteTimestamp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.RewriteTimestamp, false));
    /// <inheritdoc cref="PodmanBuildSettings.RewriteTimestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RewriteTimestamp))]
    public static T ToggleRewriteTimestamp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.RewriteTimestamp, !o.RewriteTimestamp));
    #endregion
    #region Rm
    /// <inheritdoc cref="PodmanBuildSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Rm))]
    public static T SetRm<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Rm, v));
    /// <inheritdoc cref="PodmanBuildSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Rm))]
    public static T ResetRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Rm));
    /// <inheritdoc cref="PodmanBuildSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Rm))]
    public static T EnableRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Rm, true));
    /// <inheritdoc cref="PodmanBuildSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Rm))]
    public static T DisableRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Rm, false));
    /// <inheritdoc cref="PodmanBuildSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Rm))]
    public static T ToggleRm<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Rm, !o.Rm));
    #endregion
    #region Runtime
    /// <inheritdoc cref="PodmanBuildSettings.Runtime"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Runtime))]
    public static T SetRuntime<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Runtime, v));
    /// <inheritdoc cref="PodmanBuildSettings.Runtime"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Runtime))]
    public static T ResetRuntime<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Runtime));
    #endregion
    #region RuntimeFlag
    /// <inheritdoc cref="PodmanBuildSettings.RuntimeFlag"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RuntimeFlag))]
    public static T SetRuntimeFlag<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.RuntimeFlag, v));
    /// <inheritdoc cref="PodmanBuildSettings.RuntimeFlag"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.RuntimeFlag))]
    public static T ResetRuntimeFlag<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.RuntimeFlag));
    #endregion
    #region Sbom
    /// <inheritdoc cref="PodmanBuildSettings.Sbom"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Sbom))]
    public static T SetSbom<T>(this T o, SbomPreset v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Sbom, v));
    /// <inheritdoc cref="PodmanBuildSettings.Sbom"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Sbom))]
    public static T ResetSbom<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Sbom));
    #endregion
    #region SbomImpageOutput
    /// <inheritdoc cref="PodmanBuildSettings.SbomImpageOutput"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomImpageOutput))]
    public static T SetSbomImpageOutput<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SbomImpageOutput, v));
    /// <inheritdoc cref="PodmanBuildSettings.SbomImpageOutput"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomImpageOutput))]
    public static T ResetSbomImpageOutput<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SbomImpageOutput));
    #endregion
    #region SbomMergeStrategy
    /// <inheritdoc cref="PodmanBuildSettings.SbomMergeStrategy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomMergeStrategy))]
    public static T SetSbomMergeStrategy<T>(this T o, SbomMergeStrategy v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SbomMergeStrategy, v));
    /// <inheritdoc cref="PodmanBuildSettings.SbomMergeStrategy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomMergeStrategy))]
    public static T ResetSbomMergeStrategy<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SbomMergeStrategy));
    #endregion
    #region SbomOutput
    /// <inheritdoc cref="PodmanBuildSettings.SbomOutput"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomOutput))]
    public static T SetSbomOutput<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SbomOutput, v));
    /// <inheritdoc cref="PodmanBuildSettings.SbomOutput"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomOutput))]
    public static T ResetSbomOutput<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SbomOutput));
    #endregion
    #region SbomPurlOutput
    /// <inheritdoc cref="PodmanBuildSettings.SbomPurlOutput"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomPurlOutput))]
    public static T SetSbomPurlOutput<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SbomPurlOutput, v));
    /// <inheritdoc cref="PodmanBuildSettings.SbomPurlOutput"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomPurlOutput))]
    public static T ResetSbomPurlOutput<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SbomPurlOutput));
    #endregion
    #region SbomScannerCommand
    /// <inheritdoc cref="PodmanBuildSettings.SbomScannerCommand"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomScannerCommand))]
    public static T SetSbomScannerCommand<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SbomScannerCommand, v));
    /// <inheritdoc cref="PodmanBuildSettings.SbomScannerCommand"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomScannerCommand))]
    public static T ResetSbomScannerCommand<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SbomScannerCommand));
    #endregion
    #region SbomScannerImage
    /// <inheritdoc cref="PodmanBuildSettings.SbomScannerImage"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomScannerImage))]
    public static T SetSbomScannerImage<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SbomScannerImage, v));
    /// <inheritdoc cref="PodmanBuildSettings.SbomScannerImage"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SbomScannerImage))]
    public static T ResetSbomScannerImage<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SbomScannerImage));
    #endregion
    #region Secret
    /// <inheritdoc cref="PodmanBuildSettings.Secret"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Secret))]
    public static T SetSecret<T>(this T o, [Secret] IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Secret, v));
    /// <inheritdoc cref="PodmanBuildSettings.Secret"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Secret))]
    public static T ResetSecret<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Secret));
    #endregion
    #region SecurityOpt
    /// <inheritdoc cref="PodmanBuildSettings.SecurityOpt"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SecurityOpt))]
    public static T SetSecurityOpt<T>(this T o, SecurityOptions v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SecurityOpt, v));
    /// <inheritdoc cref="PodmanBuildSettings.SecurityOpt"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SecurityOpt))]
    public static T ResetSecurityOpt<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SecurityOpt));
    #endregion
    #region ShmSize
    /// <inheritdoc cref="PodmanBuildSettings.ShmSize"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ShmSize))]
    public static T SetShmSize<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.ShmSize, v));
    /// <inheritdoc cref="PodmanBuildSettings.ShmSize"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.ShmSize))]
    public static T ResetShmSize<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.ShmSize));
    #endregion
    #region SignBy
    /// <inheritdoc cref="PodmanBuildSettings.SignBy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SignBy))]
    public static T SetSignBy<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SignBy, v));
    /// <inheritdoc cref="PodmanBuildSettings.SignBy"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SignBy))]
    public static T ResetSignBy<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SignBy));
    #endregion
    #region SkipUnusedStages
    /// <inheritdoc cref="PodmanBuildSettings.SkipUnusedStages"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SkipUnusedStages))]
    public static T SetSkipUnusedStages<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SkipUnusedStages, v));
    /// <inheritdoc cref="PodmanBuildSettings.SkipUnusedStages"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SkipUnusedStages))]
    public static T ResetSkipUnusedStages<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SkipUnusedStages));
    /// <inheritdoc cref="PodmanBuildSettings.SkipUnusedStages"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SkipUnusedStages))]
    public static T EnableSkipUnusedStages<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SkipUnusedStages, true));
    /// <inheritdoc cref="PodmanBuildSettings.SkipUnusedStages"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SkipUnusedStages))]
    public static T DisableSkipUnusedStages<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SkipUnusedStages, false));
    /// <inheritdoc cref="PodmanBuildSettings.SkipUnusedStages"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SkipUnusedStages))]
    public static T ToggleSkipUnusedStages<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SkipUnusedStages, !o.SkipUnusedStages));
    #endregion
    #region SourceDateEpoch
    /// <inheritdoc cref="PodmanBuildSettings.SourceDateEpoch"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SourceDateEpoch))]
    public static T SetSourceDateEpoch<T>(this T o, uint? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SourceDateEpoch, v));
    /// <inheritdoc cref="PodmanBuildSettings.SourceDateEpoch"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SourceDateEpoch))]
    public static T ResetSourceDateEpoch<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SourceDateEpoch));
    #endregion
    #region Squash
    /// <inheritdoc cref="PodmanBuildSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Squash))]
    public static T SetSquash<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Squash, v));
    /// <inheritdoc cref="PodmanBuildSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Squash))]
    public static T ResetSquash<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Squash));
    /// <inheritdoc cref="PodmanBuildSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Squash))]
    public static T EnableSquash<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Squash, true));
    /// <inheritdoc cref="PodmanBuildSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Squash))]
    public static T DisableSquash<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Squash, false));
    /// <inheritdoc cref="PodmanBuildSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Squash))]
    public static T ToggleSquash<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Squash, !o.Squash));
    #endregion
    #region SquashAll
    /// <inheritdoc cref="PodmanBuildSettings.SquashAll"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SquashAll))]
    public static T SetSquashAll<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SquashAll, v));
    /// <inheritdoc cref="PodmanBuildSettings.SquashAll"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SquashAll))]
    public static T ResetSquashAll<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.SquashAll));
    /// <inheritdoc cref="PodmanBuildSettings.SquashAll"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SquashAll))]
    public static T EnableSquashAll<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SquashAll, true));
    /// <inheritdoc cref="PodmanBuildSettings.SquashAll"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SquashAll))]
    public static T DisableSquashAll<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SquashAll, false));
    /// <inheritdoc cref="PodmanBuildSettings.SquashAll"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.SquashAll))]
    public static T ToggleSquashAll<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.SquashAll, !o.SquashAll));
    #endregion
    #region Ssh
    /// <inheritdoc cref="PodmanBuildSettings.Ssh"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Ssh))]
    public static T SetSsh<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Ssh, v));
    /// <inheritdoc cref="PodmanBuildSettings.Ssh"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Ssh))]
    public static T ResetSsh<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Ssh));
    #endregion
    #region Stdin
    /// <inheritdoc cref="PodmanBuildSettings.Stdin"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Stdin))]
    public static T SetStdin<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Stdin, v));
    /// <inheritdoc cref="PodmanBuildSettings.Stdin"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Stdin))]
    public static T ResetStdin<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Stdin));
    /// <inheritdoc cref="PodmanBuildSettings.Stdin"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Stdin))]
    public static T EnableStdin<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Stdin, true));
    /// <inheritdoc cref="PodmanBuildSettings.Stdin"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Stdin))]
    public static T DisableStdin<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Stdin, false));
    /// <inheritdoc cref="PodmanBuildSettings.Stdin"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Stdin))]
    public static T ToggleStdin<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Stdin, !o.Stdin));
    #endregion
    #region Tag
    /// <inheritdoc cref="PodmanBuildSettings.Tag"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Tag))]
    public static T SetTag<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Tag, v));
    /// <inheritdoc cref="PodmanBuildSettings.Tag"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Tag))]
    public static T ResetTag<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Tag));
    #endregion
    #region Target
    /// <inheritdoc cref="PodmanBuildSettings.Target"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Target))]
    public static T SetTarget<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Target, v));
    /// <inheritdoc cref="PodmanBuildSettings.Target"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Target))]
    public static T ResetTarget<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Target));
    #endregion
    #region Timestamp
    /// <inheritdoc cref="PodmanBuildSettings.Timestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Timestamp))]
    public static T SetTimestamp<T>(this T o, uint? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Timestamp, v));
    /// <inheritdoc cref="PodmanBuildSettings.Timestamp"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Timestamp))]
    public static T ResetTimestamp<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Timestamp));
    #endregion
    #region TlsVerify
    /// <inheritdoc cref="PodmanBuildSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.TlsVerify))]
    public static T SetTlsVerify<T>(this T o, bool? v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.TlsVerify, v));
    /// <inheritdoc cref="PodmanBuildSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.TlsVerify))]
    public static T ResetTlsVerify<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.TlsVerify));
    /// <inheritdoc cref="PodmanBuildSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.TlsVerify))]
    public static T EnableTlsVerify<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.TlsVerify, true));
    /// <inheritdoc cref="PodmanBuildSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.TlsVerify))]
    public static T DisableTlsVerify<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.TlsVerify, false));
    /// <inheritdoc cref="PodmanBuildSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.TlsVerify))]
    public static T ToggleTlsVerify<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.TlsVerify, !o.TlsVerify));
    #endregion
    #region Ulimit
    /// <inheritdoc cref="PodmanBuildSettings.Ulimit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Ulimit))]
    public static T SetUlimit<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Ulimit, v));
    /// <inheritdoc cref="PodmanBuildSettings.Ulimit"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Ulimit))]
    public static T ResetUlimit<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Ulimit));
    #endregion
    #region UnsetAnnotation
    /// <inheritdoc cref="PodmanBuildSettings.UnsetAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UnsetAnnotation))]
    public static T SetUnsetAnnotation<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UnsetAnnotation, v));
    /// <inheritdoc cref="PodmanBuildSettings.UnsetAnnotation"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UnsetAnnotation))]
    public static T ResetUnsetAnnotation<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UnsetAnnotation));
    #endregion
    #region UnsetEnv
    /// <inheritdoc cref="PodmanBuildSettings.UnsetEnv"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UnsetEnv))]
    public static T SetUnsetEnv<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UnsetEnv, v));
    /// <inheritdoc cref="PodmanBuildSettings.UnsetEnv"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UnsetEnv))]
    public static T ResetUnsetEnv<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UnsetEnv));
    #endregion
    #region UnsetLabel
    /// <inheritdoc cref="PodmanBuildSettings.UnsetLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UnsetLabel))]
    public static T SetUnsetLabel<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UnsetLabel, v));
    /// <inheritdoc cref="PodmanBuildSettings.UnsetLabel"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UnsetLabel))]
    public static T ResetUnsetLabel<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UnsetLabel));
    #endregion
    #region Userns
    /// <inheritdoc cref="PodmanBuildSettings.Userns"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Userns))]
    public static T SetUserns<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Userns, v));
    /// <inheritdoc cref="PodmanBuildSettings.Userns"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Userns))]
    public static T ResetUserns<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Userns));
    #endregion
    #region UsernsGidMap
    /// <inheritdoc cref="PodmanBuildSettings.UsernsGidMap"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsGidMap))]
    public static T SetUsernsGidMap<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UsernsGidMap, v));
    /// <inheritdoc cref="PodmanBuildSettings.UsernsGidMap"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsGidMap))]
    public static T ResetUsernsGidMap<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UsernsGidMap));
    #endregion
    #region UsernsGidMapGroup
    /// <inheritdoc cref="PodmanBuildSettings.UsernsGidMapGroup"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsGidMapGroup))]
    public static T SetUsernsGidMapGroup<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UsernsGidMapGroup, v));
    /// <inheritdoc cref="PodmanBuildSettings.UsernsGidMapGroup"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsGidMapGroup))]
    public static T ResetUsernsGidMapGroup<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UsernsGidMapGroup));
    #endregion
    #region UsernsUidMap
    /// <inheritdoc cref="PodmanBuildSettings.UsernsUidMap"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsUidMap))]
    public static T SetUsernsUidMap<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UsernsUidMap, v));
    /// <inheritdoc cref="PodmanBuildSettings.UsernsUidMap"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsUidMap))]
    public static T ResetUsernsUidMap<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UsernsUidMap));
    #endregion
    #region UsernsUidMapUser
    /// <inheritdoc cref="PodmanBuildSettings.UsernsUidMapUser"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsUidMapUser))]
    public static T SetUsernsUidMapUser<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.UsernsUidMapUser, v));
    /// <inheritdoc cref="PodmanBuildSettings.UsernsUidMapUser"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.UsernsUidMapUser))]
    public static T ResetUsernsUidMapUser<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.UsernsUidMapUser));
    #endregion
    #region Uts
    /// <inheritdoc cref="PodmanBuildSettings.Uts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Uts))]
    public static T SetUts<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Uts, v));
    /// <inheritdoc cref="PodmanBuildSettings.Uts"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Uts))]
    public static T ResetUts<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Uts));
    #endregion
    #region Variant
    /// <inheritdoc cref="PodmanBuildSettings.Variant"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Variant))]
    public static T SetVariant<T>(this T o, string v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Variant, v));
    /// <inheritdoc cref="PodmanBuildSettings.Variant"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Variant))]
    public static T ResetVariant<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Variant));
    #endregion
    #region Volume
    /// <inheritdoc cref="PodmanBuildSettings.Volume"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Volume))]
    public static T SetVolume<T>(this T o, IReadOnlyList<string> v) where T : PodmanBuildSettings => o.Modify(b => b.Set(() => o.Volume, v));
    /// <inheritdoc cref="PodmanBuildSettings.Volume"/>
    [Pure] [Builder(Type = typeof(PodmanBuildSettings), Property = nameof(PodmanBuildSettings.Volume))]
    public static T ResetVolume<T>(this T o) where T : PodmanBuildSettings => o.Modify(b => b.Remove(() => o.Volume));
    #endregion
}
#endregion
#region PodmanCommitSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanCommit(Candoumbe.Pipelines.Tools.Podman.PodmanCommitSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanCommitSettingsExtensions
{
    #region Author
    /// <inheritdoc cref="PodmanCommitSettings.Author"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Author))]
    public static T SetAuthor<T>(this T o, string v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Author, v));
    /// <inheritdoc cref="PodmanCommitSettings.Author"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Author))]
    public static T ResetAuthor<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Author));
    #endregion
    #region Change
    /// <inheritdoc cref="PodmanCommitSettings.Change"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Change))]
    public static T SetChange<T>(this T o, IReadOnlyList<(InstructionType, string)> v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Change, v));
    /// <inheritdoc cref="PodmanCommitSettings.Change"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Change))]
    public static T ResetChange<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Change));
    #endregion
    #region Config
    /// <inheritdoc cref="PodmanCommitSettings.Config"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Config))]
    public static T SetConfig<T>(this T o, string v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Config, v));
    /// <inheritdoc cref="PodmanCommitSettings.Config"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Config))]
    public static T ResetConfig<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Config));
    #endregion
    #region Format
    /// <inheritdoc cref="PodmanCommitSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Format))]
    public static T SetFormat<T>(this T o, FormatType v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Format, v));
    /// <inheritdoc cref="PodmanCommitSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Format))]
    public static T ResetFormat<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Format));
    #endregion
    #region Iidfile
    /// <inheritdoc cref="PodmanCommitSettings.Iidfile"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Iidfile))]
    public static T SetIidfile<T>(this T o, string v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Iidfile, v));
    /// <inheritdoc cref="PodmanCommitSettings.Iidfile"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Iidfile))]
    public static T ResetIidfile<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Iidfile));
    #endregion
    #region IncludeVolumes
    /// <inheritdoc cref="PodmanCommitSettings.IncludeVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.IncludeVolumes))]
    public static T SetIncludeVolumes<T>(this T o, bool? v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.IncludeVolumes, v));
    /// <inheritdoc cref="PodmanCommitSettings.IncludeVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.IncludeVolumes))]
    public static T ResetIncludeVolumes<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.IncludeVolumes));
    /// <inheritdoc cref="PodmanCommitSettings.IncludeVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.IncludeVolumes))]
    public static T EnableIncludeVolumes<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.IncludeVolumes, true));
    /// <inheritdoc cref="PodmanCommitSettings.IncludeVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.IncludeVolumes))]
    public static T DisableIncludeVolumes<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.IncludeVolumes, false));
    /// <inheritdoc cref="PodmanCommitSettings.IncludeVolumes"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.IncludeVolumes))]
    public static T ToggleIncludeVolumes<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.IncludeVolumes, !o.IncludeVolumes));
    #endregion
    #region Message
    /// <inheritdoc cref="PodmanCommitSettings.Message"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Message))]
    public static T SetMessage<T>(this T o, string v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Message, v));
    /// <inheritdoc cref="PodmanCommitSettings.Message"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Message))]
    public static T ResetMessage<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Message));
    #endregion
    #region Pause
    /// <inheritdoc cref="PodmanCommitSettings.Pause"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Pause))]
    public static T SetPause<T>(this T o, bool? v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Pause, v));
    /// <inheritdoc cref="PodmanCommitSettings.Pause"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Pause))]
    public static T ResetPause<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Pause));
    /// <inheritdoc cref="PodmanCommitSettings.Pause"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Pause))]
    public static T EnablePause<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Pause, true));
    /// <inheritdoc cref="PodmanCommitSettings.Pause"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Pause))]
    public static T DisablePause<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Pause, false));
    /// <inheritdoc cref="PodmanCommitSettings.Pause"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Pause))]
    public static T TogglePause<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Pause, !o.Pause));
    #endregion
    #region Quiet
    /// <inheritdoc cref="PodmanCommitSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Quiet))]
    public static T SetQuiet<T>(this T o, bool? v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Quiet, v));
    /// <inheritdoc cref="PodmanCommitSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Quiet))]
    public static T ResetQuiet<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Quiet));
    /// <inheritdoc cref="PodmanCommitSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Quiet))]
    public static T EnableQuiet<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Quiet, true));
    /// <inheritdoc cref="PodmanCommitSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Quiet))]
    public static T DisableQuiet<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Quiet, false));
    /// <inheritdoc cref="PodmanCommitSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Quiet))]
    public static T ToggleQuiet<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Quiet, !o.Quiet));
    #endregion
    #region Squash
    /// <inheritdoc cref="PodmanCommitSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Squash))]
    public static T SetSquash<T>(this T o, bool? v) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Squash, v));
    /// <inheritdoc cref="PodmanCommitSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Squash))]
    public static T ResetSquash<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Remove(() => o.Squash));
    /// <inheritdoc cref="PodmanCommitSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Squash))]
    public static T EnableSquash<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Squash, true));
    /// <inheritdoc cref="PodmanCommitSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Squash))]
    public static T DisableSquash<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Squash, false));
    /// <inheritdoc cref="PodmanCommitSettings.Squash"/>
    [Pure] [Builder(Type = typeof(PodmanCommitSettings), Property = nameof(PodmanCommitSettings.Squash))]
    public static T ToggleSquash<T>(this T o) where T : PodmanCommitSettings => o.Modify(b => b.Set(() => o.Squash, !o.Squash));
    #endregion
}
#endregion
#region PodmanCpSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanCp(Candoumbe.Pipelines.Tools.Podman.PodmanCpSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanCpSettingsExtensions
{
    #region Archive
    /// <inheritdoc cref="PodmanCpSettings.Archive"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Archive))]
    public static T SetArchive<T>(this T o, bool? v) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Archive, v));
    /// <inheritdoc cref="PodmanCpSettings.Archive"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Archive))]
    public static T ResetArchive<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Remove(() => o.Archive));
    /// <inheritdoc cref="PodmanCpSettings.Archive"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Archive))]
    public static T EnableArchive<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Archive, true));
    /// <inheritdoc cref="PodmanCpSettings.Archive"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Archive))]
    public static T DisableArchive<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Archive, false));
    /// <inheritdoc cref="PodmanCpSettings.Archive"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Archive))]
    public static T ToggleArchive<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Archive, !o.Archive));
    #endregion
    #region Overwrite
    /// <inheritdoc cref="PodmanCpSettings.Overwrite"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Overwrite))]
    public static T SetOverwrite<T>(this T o, bool? v) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Overwrite, v));
    /// <inheritdoc cref="PodmanCpSettings.Overwrite"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Overwrite))]
    public static T ResetOverwrite<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Remove(() => o.Overwrite));
    /// <inheritdoc cref="PodmanCpSettings.Overwrite"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Overwrite))]
    public static T EnableOverwrite<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Overwrite, true));
    /// <inheritdoc cref="PodmanCpSettings.Overwrite"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Overwrite))]
    public static T DisableOverwrite<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Overwrite, false));
    /// <inheritdoc cref="PodmanCpSettings.Overwrite"/>
    [Pure] [Builder(Type = typeof(PodmanCpSettings), Property = nameof(PodmanCpSettings.Overwrite))]
    public static T ToggleOverwrite<T>(this T o) where T : PodmanCpSettings => o.Modify(b => b.Set(() => o.Overwrite, !o.Overwrite));
    #endregion
}
#endregion
#region PodmanCreateSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanCreate(Candoumbe.Pipelines.Tools.Podman.PodmanCreateSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanCreateSettingsExtensions
{
    #region AddHost
    /// <inheritdoc cref="PodmanCreateSettings.AddHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.AddHost))]
    public static T SetAddHost<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.AddHost, v));
    /// <inheritdoc cref="PodmanCreateSettings.AddHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.AddHost))]
    public static T ResetAddHost<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.AddHost));
    #endregion
    #region Annotation
    /// <inheritdoc cref="PodmanCreateSettings.Annotation"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Annotation))]
    public static T SetAnnotation<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Annotation, v));
    /// <inheritdoc cref="PodmanCreateSettings.Annotation"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Annotation))]
    public static T ResetAnnotation<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Annotation));
    #endregion
    #region Arch
    /// <inheritdoc cref="PodmanCreateSettings.Arch"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Arch))]
    public static T SetArch<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Arch, v));
    /// <inheritdoc cref="PodmanCreateSettings.Arch"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Arch))]
    public static T ResetArch<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Arch));
    #endregion
    #region Attach
    /// <inheritdoc cref="PodmanCreateSettings.Attach"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Attach))]
    public static T SetAttach<T>(this T o, AttachType v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Attach, v));
    /// <inheritdoc cref="PodmanCreateSettings.Attach"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Attach))]
    public static T ResetAttach<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Attach));
    #endregion
    #region AuthFile
    /// <inheritdoc cref="PodmanCreateSettings.AuthFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.AuthFile))]
    public static T SetAuthFile<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.AuthFile, v));
    /// <inheritdoc cref="PodmanCreateSettings.AuthFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.AuthFile))]
    public static T ResetAuthFile<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.AuthFile));
    #endregion
    #region BlkioWeight
    /// <inheritdoc cref="PodmanCreateSettings.BlkioWeight"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.BlkioWeight))]
    public static T SetBlkioWeight<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.BlkioWeight, v));
    /// <inheritdoc cref="PodmanCreateSettings.BlkioWeight"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.BlkioWeight))]
    public static T ResetBlkioWeight<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.BlkioWeight));
    #endregion
    #region BlkioWeightDevice
    /// <inheritdoc cref="PodmanCreateSettings.BlkioWeightDevice"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.BlkioWeightDevice))]
    public static T SetBlkioWeightDevice<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.BlkioWeightDevice, v));
    /// <inheritdoc cref="PodmanCreateSettings.BlkioWeightDevice"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.BlkioWeightDevice))]
    public static T ResetBlkioWeightDevice<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.BlkioWeightDevice));
    #endregion
    #region CapAdd
    /// <inheritdoc cref="PodmanCreateSettings.CapAdd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CapAdd))]
    public static T SetCapAdd<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CapAdd, v));
    /// <inheritdoc cref="PodmanCreateSettings.CapAdd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CapAdd))]
    public static T ResetCapAdd<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CapAdd));
    #endregion
    #region CapDrop
    /// <inheritdoc cref="PodmanCreateSettings.CapDrop"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CapDrop))]
    public static T SetCapDrop<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CapDrop, v));
    /// <inheritdoc cref="PodmanCreateSettings.CapDrop"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CapDrop))]
    public static T ResetCapDrop<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CapDrop));
    #endregion
    #region CgroupConf
    /// <inheritdoc cref="PodmanCreateSettings.CgroupConf"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CgroupConf))]
    public static T SetCgroupConf<T>(this T o, IReadOnlyDictionary<string, string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CgroupConf, v));
    /// <inheritdoc cref="PodmanCreateSettings.CgroupConf"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CgroupConf))]
    public static T ResetCgroupConf<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CgroupConf));
    #endregion
    #region CgroupParent
    /// <inheritdoc cref="PodmanCreateSettings.CgroupParent"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CgroupParent))]
    public static T SetCgroupParent<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CgroupParent, v));
    /// <inheritdoc cref="PodmanCreateSettings.CgroupParent"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CgroupParent))]
    public static T ResetCgroupParent<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CgroupParent));
    #endregion
    #region CgroupNs
    /// <inheritdoc cref="PodmanCreateSettings.CgroupNs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CgroupNs))]
    public static T SetCgroupNs<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CgroupNs, v));
    /// <inheritdoc cref="PodmanCreateSettings.CgroupNs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CgroupNs))]
    public static T ResetCgroupNs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CgroupNs));
    #endregion
    #region Cgroups
    /// <inheritdoc cref="PodmanCreateSettings.Cgroups"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Cgroups))]
    public static T SetCgroups<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Cgroups, v));
    /// <inheritdoc cref="PodmanCreateSettings.Cgroups"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Cgroups))]
    public static T ResetCgroups<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Cgroups));
    #endregion
    #region ChrootDirs
    /// <inheritdoc cref="PodmanCreateSettings.ChrootDirs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ChrootDirs))]
    public static T SetChrootDirs<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ChrootDirs, v));
    /// <inheritdoc cref="PodmanCreateSettings.ChrootDirs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ChrootDirs))]
    public static T ResetChrootDirs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.ChrootDirs));
    #endregion
    #region CidFile
    /// <inheritdoc cref="PodmanCreateSettings.CidFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CidFile))]
    public static T SetCidFile<T>(this T o, IReadOnlyList<Nuke.Common.IO.AbsolutePath> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CidFile, v));
    /// <inheritdoc cref="PodmanCreateSettings.CidFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CidFile))]
    public static T ResetCidFile<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CidFile));
    #endregion
    #region ConmonPidFile
    /// <inheritdoc cref="PodmanCreateSettings.ConmonPidFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ConmonPidFile))]
    public static T SetConmonPidFile<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ConmonPidFile, v));
    /// <inheritdoc cref="PodmanCreateSettings.ConmonPidFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ConmonPidFile))]
    public static T ResetConmonPidFile<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.ConmonPidFile));
    #endregion
    #region CpuPeriod
    /// <inheritdoc cref="PodmanCreateSettings.CpuPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuPeriod))]
    public static T SetCpuPeriod<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CpuPeriod, v));
    /// <inheritdoc cref="PodmanCreateSettings.CpuPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuPeriod))]
    public static T ResetCpuPeriod<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CpuPeriod));
    #endregion
    #region CpuQuota
    /// <inheritdoc cref="PodmanCreateSettings.CpuQuota"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuQuota))]
    public static T SetCpuQuota<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CpuQuota, v));
    /// <inheritdoc cref="PodmanCreateSettings.CpuQuota"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuQuota))]
    public static T ResetCpuQuota<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CpuQuota));
    #endregion
    #region CpuRtPeriod
    /// <inheritdoc cref="PodmanCreateSettings.CpuRtPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuRtPeriod))]
    public static T SetCpuRtPeriod<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CpuRtPeriod, v));
    /// <inheritdoc cref="PodmanCreateSettings.CpuRtPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuRtPeriod))]
    public static T ResetCpuRtPeriod<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CpuRtPeriod));
    #endregion
    #region CpuRtRuntime
    /// <inheritdoc cref="PodmanCreateSettings.CpuRtRuntime"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuRtRuntime))]
    public static T SetCpuRtRuntime<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CpuRtRuntime, v));
    /// <inheritdoc cref="PodmanCreateSettings.CpuRtRuntime"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpuRtRuntime))]
    public static T ResetCpuRtRuntime<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CpuRtRuntime));
    #endregion
    #region CpusetCpus
    /// <inheritdoc cref="PodmanCreateSettings.CpusetCpus"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpusetCpus))]
    public static T SetCpusetCpus<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CpusetCpus, v));
    /// <inheritdoc cref="PodmanCreateSettings.CpusetCpus"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpusetCpus))]
    public static T ResetCpusetCpus<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CpusetCpus));
    #endregion
    #region CpusetMems
    /// <inheritdoc cref="PodmanCreateSettings.CpusetMems"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpusetMems))]
    public static T SetCpusetMems<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.CpusetMems, v));
    /// <inheritdoc cref="PodmanCreateSettings.CpusetMems"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.CpusetMems))]
    public static T ResetCpusetMems<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.CpusetMems));
    #endregion
    #region DecryptionKey
    /// <inheritdoc cref="PodmanCreateSettings.DecryptionKey"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DecryptionKey))]
    public static T SetDecryptionKey<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DecryptionKey, v));
    /// <inheritdoc cref="PodmanCreateSettings.DecryptionKey"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DecryptionKey))]
    public static T ResetDecryptionKey<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DecryptionKey));
    #endregion
    #region Device
    /// <inheritdoc cref="PodmanCreateSettings.Device"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Device))]
    public static T SetDevice<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Device, v));
    /// <inheritdoc cref="PodmanCreateSettings.Device"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Device))]
    public static T ResetDevice<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Device));
    #endregion
    #region DeviceCgroupRule
    /// <inheritdoc cref="PodmanCreateSettings.DeviceCgroupRule"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceCgroupRule))]
    public static T SetDeviceCgroupRule<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DeviceCgroupRule, v));
    /// <inheritdoc cref="PodmanCreateSettings.DeviceCgroupRule"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceCgroupRule))]
    public static T ResetDeviceCgroupRule<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DeviceCgroupRule));
    #endregion
    #region DeviceReadBps
    /// <inheritdoc cref="PodmanCreateSettings.DeviceReadBps"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceReadBps))]
    public static T SetDeviceReadBps<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DeviceReadBps, v));
    /// <inheritdoc cref="PodmanCreateSettings.DeviceReadBps"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceReadBps))]
    public static T ResetDeviceReadBps<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DeviceReadBps));
    #endregion
    #region DeviceReadIops
    /// <inheritdoc cref="PodmanCreateSettings.DeviceReadIops"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceReadIops))]
    public static T SetDeviceReadIops<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DeviceReadIops, v));
    /// <inheritdoc cref="PodmanCreateSettings.DeviceReadIops"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceReadIops))]
    public static T ResetDeviceReadIops<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DeviceReadIops));
    #endregion
    #region DeviceWriteBps
    /// <inheritdoc cref="PodmanCreateSettings.DeviceWriteBps"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceWriteBps))]
    public static T SetDeviceWriteBps<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DeviceWriteBps, v));
    /// <inheritdoc cref="PodmanCreateSettings.DeviceWriteBps"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceWriteBps))]
    public static T ResetDeviceWriteBps<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DeviceWriteBps));
    #endregion
    #region DeviceWriteIops
    /// <inheritdoc cref="PodmanCreateSettings.DeviceWriteIops"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceWriteIops))]
    public static T SetDeviceWriteIops<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DeviceWriteIops, v));
    /// <inheritdoc cref="PodmanCreateSettings.DeviceWriteIops"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DeviceWriteIops))]
    public static T ResetDeviceWriteIops<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DeviceWriteIops));
    #endregion
    #region DisableContentTrust
    /// <inheritdoc cref="PodmanCreateSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DisableContentTrust))]
    public static T SetDisableContentTrust<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, v));
    /// <inheritdoc cref="PodmanCreateSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DisableContentTrust))]
    public static T ResetDisableContentTrust<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DisableContentTrust));
    /// <inheritdoc cref="PodmanCreateSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DisableContentTrust))]
    public static T EnableDisableContentTrust<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, true));
    /// <inheritdoc cref="PodmanCreateSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DisableContentTrust))]
    public static T DisableDisableContentTrust<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, false));
    /// <inheritdoc cref="PodmanCreateSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DisableContentTrust))]
    public static T ToggleDisableContentTrust<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, !o.DisableContentTrust));
    #endregion
    #region Dns
    /// <inheritdoc cref="PodmanCreateSettings.Dns"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Dns))]
    public static T SetDns<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Dns, v));
    /// <inheritdoc cref="PodmanCreateSettings.Dns"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Dns))]
    public static T ResetDns<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Dns));
    #endregion
    #region DnsOption
    /// <inheritdoc cref="PodmanCreateSettings.DnsOption"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DnsOption))]
    public static T SetDnsOption<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DnsOption, v));
    /// <inheritdoc cref="PodmanCreateSettings.DnsOption"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DnsOption))]
    public static T ResetDnsOption<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DnsOption));
    #endregion
    #region DnsSearch
    /// <inheritdoc cref="PodmanCreateSettings.DnsSearch"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DnsSearch))]
    public static T SetDnsSearch<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.DnsSearch, v));
    /// <inheritdoc cref="PodmanCreateSettings.DnsSearch"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.DnsSearch))]
    public static T ResetDnsSearch<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.DnsSearch));
    #endregion
    #region EntryPoint
    /// <inheritdoc cref="PodmanCreateSettings.EntryPoint"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EntryPoint))]
    public static T SetEntryPoint<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EntryPoint, v));
    /// <inheritdoc cref="PodmanCreateSettings.EntryPoint"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EntryPoint))]
    public static T ResetEntryPoint<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.EntryPoint));
    #endregion
    #region Env
    /// <inheritdoc cref="PodmanCreateSettings.Env"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Env))]
    public static T SetEnv<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Env, v));
    /// <inheritdoc cref="PodmanCreateSettings.Env"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Env))]
    public static T ResetEnv<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Env));
    #endregion
    #region EnvFile
    /// <inheritdoc cref="PodmanCreateSettings.EnvFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvFile))]
    public static T SetEnvFile<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EnvFile, v));
    /// <inheritdoc cref="PodmanCreateSettings.EnvFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvFile))]
    public static T ResetEnvFile<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.EnvFile));
    #endregion
    #region EnvHost
    /// <inheritdoc cref="PodmanCreateSettings.EnvHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvHost))]
    public static T SetEnvHost<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EnvHost, v));
    /// <inheritdoc cref="PodmanCreateSettings.EnvHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvHost))]
    public static T ResetEnvHost<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.EnvHost));
    /// <inheritdoc cref="PodmanCreateSettings.EnvHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvHost))]
    public static T EnableEnvHost<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EnvHost, true));
    /// <inheritdoc cref="PodmanCreateSettings.EnvHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvHost))]
    public static T DisableEnvHost<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EnvHost, false));
    /// <inheritdoc cref="PodmanCreateSettings.EnvHost"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvHost))]
    public static T ToggleEnvHost<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EnvHost, !o.EnvHost));
    #endregion
    #region EnvMerge
    /// <inheritdoc cref="PodmanCreateSettings.EnvMerge"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvMerge))]
    public static T SetEnvMerge<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.EnvMerge, v));
    /// <inheritdoc cref="PodmanCreateSettings.EnvMerge"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.EnvMerge))]
    public static T ResetEnvMerge<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.EnvMerge));
    #endregion
    #region Expose
    /// <inheritdoc cref="PodmanCreateSettings.Expose"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Expose))]
    public static T SetExpose<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Expose, v));
    /// <inheritdoc cref="PodmanCreateSettings.Expose"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Expose))]
    public static T ResetExpose<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Expose));
    #endregion
    #region GidMap
    /// <inheritdoc cref="PodmanCreateSettings.GidMap"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.GidMap))]
    public static T SetGidMap<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.GidMap, v));
    /// <inheritdoc cref="PodmanCreateSettings.GidMap"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.GidMap))]
    public static T ResetGidMap<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.GidMap));
    #endregion
    #region Gpus
    /// <inheritdoc cref="PodmanCreateSettings.Gpus"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Gpus))]
    public static T SetGpus<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Gpus, v));
    /// <inheritdoc cref="PodmanCreateSettings.Gpus"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Gpus))]
    public static T ResetGpus<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Gpus));
    #endregion
    #region GroupAdd
    /// <inheritdoc cref="PodmanCreateSettings.GroupAdd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.GroupAdd))]
    public static T SetGroupAdd<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.GroupAdd, v));
    /// <inheritdoc cref="PodmanCreateSettings.GroupAdd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.GroupAdd))]
    public static T ResetGroupAdd<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.GroupAdd));
    #endregion
    #region GroupEntry
    /// <inheritdoc cref="PodmanCreateSettings.GroupEntry"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.GroupEntry))]
    public static T SetGroupEntry<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.GroupEntry, v));
    /// <inheritdoc cref="PodmanCreateSettings.GroupEntry"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.GroupEntry))]
    public static T ResetGroupEntry<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.GroupEntry));
    #endregion
    #region HealthCmd
    /// <inheritdoc cref="PodmanCreateSettings.HealthCmd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthCmd))]
    public static T SetHealthCmd<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthCmd, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthCmd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthCmd))]
    public static T ResetHealthCmd<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthCmd));
    #endregion
    #region HealthInterval
    /// <inheritdoc cref="PodmanCreateSettings.HealthInterval"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthInterval))]
    public static T SetHealthInterval<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthInterval, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthInterval"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthInterval))]
    public static T ResetHealthInterval<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthInterval));
    #endregion
    #region HealthLogDestination
    /// <inheritdoc cref="PodmanCreateSettings.HealthLogDestination"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthLogDestination))]
    public static T SetHealthLogDestination<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthLogDestination, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthLogDestination"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthLogDestination))]
    public static T ResetHealthLogDestination<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthLogDestination));
    #endregion
    #region HealthMaxLogCount
    /// <inheritdoc cref="PodmanCreateSettings.HealthMaxLogCount"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthMaxLogCount))]
    public static T SetHealthMaxLogCount<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthMaxLogCount, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthMaxLogCount"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthMaxLogCount))]
    public static T ResetHealthMaxLogCount<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthMaxLogCount));
    #endregion
    #region HealthMaxLogSize
    /// <inheritdoc cref="PodmanCreateSettings.HealthMaxLogSize"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthMaxLogSize))]
    public static T SetHealthMaxLogSize<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthMaxLogSize, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthMaxLogSize"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthMaxLogSize))]
    public static T ResetHealthMaxLogSize<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthMaxLogSize));
    #endregion
    #region HealthOnFailure
    /// <inheritdoc cref="PodmanCreateSettings.HealthOnFailure"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthOnFailure))]
    public static T SetHealthOnFailure<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthOnFailure, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthOnFailure"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthOnFailure))]
    public static T ResetHealthOnFailure<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthOnFailure));
    #endregion
    #region HealthRetries
    /// <inheritdoc cref="PodmanCreateSettings.HealthRetries"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthRetries))]
    public static T SetHealthRetries<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthRetries, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthRetries"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthRetries))]
    public static T ResetHealthRetries<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthRetries));
    #endregion
    #region HealthStartPeriod
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartPeriod))]
    public static T SetHealthStartPeriod<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthStartPeriod, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartPeriod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartPeriod))]
    public static T ResetHealthStartPeriod<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthStartPeriod));
    #endregion
    #region HealthStartupCmd
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupCmd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupCmd))]
    public static T SetHealthStartupCmd<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthStartupCmd, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupCmd"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupCmd))]
    public static T ResetHealthStartupCmd<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthStartupCmd));
    #endregion
    #region HealthStartupInterval
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupInterval"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupInterval))]
    public static T SetHealthStartupInterval<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthStartupInterval, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupInterval"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupInterval))]
    public static T ResetHealthStartupInterval<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthStartupInterval));
    #endregion
    #region HealthStartupRetries
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupRetries"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupRetries))]
    public static T SetHealthStartupRetries<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthStartupRetries, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupRetries"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupRetries))]
    public static T ResetHealthStartupRetries<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthStartupRetries));
    #endregion
    #region HealthStartupSuccess
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupSuccess"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupSuccess))]
    public static T SetHealthStartupSuccess<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthStartupSuccess, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupSuccess"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupSuccess))]
    public static T ResetHealthStartupSuccess<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthStartupSuccess));
    #endregion
    #region HealthStartupTimeout
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupTimeout"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupTimeout))]
    public static T SetHealthStartupTimeout<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthStartupTimeout, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthStartupTimeout"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthStartupTimeout))]
    public static T ResetHealthStartupTimeout<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthStartupTimeout));
    #endregion
    #region HealthTimeout
    /// <inheritdoc cref="PodmanCreateSettings.HealthTimeout"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthTimeout))]
    public static T SetHealthTimeout<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HealthTimeout, v));
    /// <inheritdoc cref="PodmanCreateSettings.HealthTimeout"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HealthTimeout))]
    public static T ResetHealthTimeout<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HealthTimeout));
    #endregion
    #region Hostname
    /// <inheritdoc cref="PodmanCreateSettings.Hostname"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Hostname))]
    public static T SetHostname<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Hostname, v));
    /// <inheritdoc cref="PodmanCreateSettings.Hostname"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Hostname))]
    public static T ResetHostname<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Hostname));
    #endregion
    #region HostUser
    /// <inheritdoc cref="PodmanCreateSettings.HostUser"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HostUser))]
    public static T SetHostUser<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HostUser, v));
    /// <inheritdoc cref="PodmanCreateSettings.HostUser"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HostUser))]
    public static T ResetHostUser<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HostUser));
    #endregion
    #region HttpProxy
    /// <inheritdoc cref="PodmanCreateSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HttpProxy))]
    public static T SetHttpProxy<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HttpProxy, v));
    /// <inheritdoc cref="PodmanCreateSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HttpProxy))]
    public static T ResetHttpProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.HttpProxy));
    /// <inheritdoc cref="PodmanCreateSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HttpProxy))]
    public static T EnableHttpProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HttpProxy, true));
    /// <inheritdoc cref="PodmanCreateSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HttpProxy))]
    public static T DisableHttpProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HttpProxy, false));
    /// <inheritdoc cref="PodmanCreateSettings.HttpProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.HttpProxy))]
    public static T ToggleHttpProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.HttpProxy, !o.HttpProxy));
    #endregion
    #region ImageVolume
    /// <inheritdoc cref="PodmanCreateSettings.ImageVolume"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ImageVolume))]
    public static T SetImageVolume<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ImageVolume, v));
    /// <inheritdoc cref="PodmanCreateSettings.ImageVolume"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ImageVolume))]
    public static T ResetImageVolume<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.ImageVolume));
    #endregion
    #region Init
    /// <inheritdoc cref="PodmanCreateSettings.Init"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Init))]
    public static T SetInit<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Init, v));
    /// <inheritdoc cref="PodmanCreateSettings.Init"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Init))]
    public static T ResetInit<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Init));
    /// <inheritdoc cref="PodmanCreateSettings.Init"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Init))]
    public static T EnableInit<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Init, true));
    /// <inheritdoc cref="PodmanCreateSettings.Init"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Init))]
    public static T DisableInit<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Init, false));
    /// <inheritdoc cref="PodmanCreateSettings.Init"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Init))]
    public static T ToggleInit<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Init, !o.Init));
    #endregion
    #region InitCtr
    /// <inheritdoc cref="PodmanCreateSettings.InitCtr"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.InitCtr))]
    public static T SetInitCtr<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.InitCtr, v));
    /// <inheritdoc cref="PodmanCreateSettings.InitCtr"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.InitCtr))]
    public static T ResetInitCtr<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.InitCtr));
    #endregion
    #region InitPath
    /// <inheritdoc cref="PodmanCreateSettings.InitPath"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.InitPath))]
    public static T SetInitPath<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.InitPath, v));
    /// <inheritdoc cref="PodmanCreateSettings.InitPath"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.InitPath))]
    public static T ResetInitPath<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.InitPath));
    #endregion
    #region Interactive
    /// <inheritdoc cref="PodmanCreateSettings.Interactive"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Interactive))]
    public static T SetInteractive<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Interactive, v));
    /// <inheritdoc cref="PodmanCreateSettings.Interactive"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Interactive))]
    public static T ResetInteractive<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Interactive));
    /// <inheritdoc cref="PodmanCreateSettings.Interactive"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Interactive))]
    public static T EnableInteractive<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Interactive, true));
    /// <inheritdoc cref="PodmanCreateSettings.Interactive"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Interactive))]
    public static T DisableInteractive<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Interactive, false));
    /// <inheritdoc cref="PodmanCreateSettings.Interactive"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Interactive))]
    public static T ToggleInteractive<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Interactive, !o.Interactive));
    #endregion
    #region Ip
    /// <inheritdoc cref="PodmanCreateSettings.Ip"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Ip))]
    public static T SetIp<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Ip, v));
    /// <inheritdoc cref="PodmanCreateSettings.Ip"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Ip))]
    public static T ResetIp<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Ip));
    #endregion
    #region Ip6
    /// <inheritdoc cref="PodmanCreateSettings.Ip6"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Ip6))]
    public static T SetIp6<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Ip6, v));
    /// <inheritdoc cref="PodmanCreateSettings.Ip6"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Ip6))]
    public static T ResetIp6<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Ip6));
    #endregion
    #region Label
    /// <inheritdoc cref="PodmanCreateSettings.Label"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Label))]
    public static T SetLabel<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Label, v));
    /// <inheritdoc cref="PodmanCreateSettings.Label"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Label))]
    public static T ResetLabel<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Label));
    #endregion
    #region LogDriver
    /// <inheritdoc cref="PodmanCreateSettings.LogDriver"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.LogDriver))]
    public static T SetLogDriver<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.LogDriver, v));
    /// <inheritdoc cref="PodmanCreateSettings.LogDriver"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.LogDriver))]
    public static T ResetLogDriver<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.LogDriver));
    #endregion
    #region LogOpt
    /// <inheritdoc cref="PodmanCreateSettings.LogOpt"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.LogOpt))]
    public static T SetLogOpt<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.LogOpt, v));
    /// <inheritdoc cref="PodmanCreateSettings.LogOpt"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.LogOpt))]
    public static T ResetLogOpt<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.LogOpt));
    #endregion
    #region MacAddress
    /// <inheritdoc cref="PodmanCreateSettings.MacAddress"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.MacAddress))]
    public static T SetMacAddress<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.MacAddress, v));
    /// <inheritdoc cref="PodmanCreateSettings.MacAddress"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.MacAddress))]
    public static T ResetMacAddress<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.MacAddress));
    #endregion
    #region Memory
    /// <inheritdoc cref="PodmanCreateSettings.Memory"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Memory))]
    public static T SetMemory<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Memory, v));
    /// <inheritdoc cref="PodmanCreateSettings.Memory"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Memory))]
    public static T ResetMemory<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Memory));
    #endregion
    #region MemorySwap
    /// <inheritdoc cref="PodmanCreateSettings.MemorySwap"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.MemorySwap))]
    public static T SetMemorySwap<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.MemorySwap, v));
    /// <inheritdoc cref="PodmanCreateSettings.MemorySwap"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.MemorySwap))]
    public static T ResetMemorySwap<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.MemorySwap));
    #endregion
    #region Mount
    /// <inheritdoc cref="PodmanCreateSettings.Mount"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Mount))]
    public static T SetMount<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Mount, v));
    /// <inheritdoc cref="PodmanCreateSettings.Mount"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Mount))]
    public static T ResetMount<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Mount));
    #endregion
    #region Name
    /// <inheritdoc cref="PodmanCreateSettings.Name"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Name))]
    public static T SetName<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Name, v));
    /// <inheritdoc cref="PodmanCreateSettings.Name"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Name))]
    public static T ResetName<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Name));
    #endregion
    #region Network
    /// <inheritdoc cref="PodmanCreateSettings.Network"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Network))]
    public static T SetNetwork<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Network, v));
    /// <inheritdoc cref="PodmanCreateSettings.Network"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Network))]
    public static T ResetNetwork<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Network));
    #endregion
    #region NetworkAlias
    /// <inheritdoc cref="PodmanCreateSettings.NetworkAlias"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NetworkAlias))]
    public static T SetNetworkAlias<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.NetworkAlias, v));
    /// <inheritdoc cref="PodmanCreateSettings.NetworkAlias"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NetworkAlias))]
    public static T ResetNetworkAlias<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.NetworkAlias));
    #endregion
    #region NoHosts
    /// <inheritdoc cref="PodmanCreateSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NoHosts))]
    public static T SetNoHosts<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.NoHosts, v));
    /// <inheritdoc cref="PodmanCreateSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NoHosts))]
    public static T ResetNoHosts<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.NoHosts));
    /// <inheritdoc cref="PodmanCreateSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NoHosts))]
    public static T EnableNoHosts<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.NoHosts, true));
    /// <inheritdoc cref="PodmanCreateSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NoHosts))]
    public static T DisableNoHosts<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.NoHosts, false));
    /// <inheritdoc cref="PodmanCreateSettings.NoHosts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.NoHosts))]
    public static T ToggleNoHosts<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.NoHosts, !o.NoHosts));
    #endregion
    #region OomKillDisable
    /// <inheritdoc cref="PodmanCreateSettings.OomKillDisable"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomKillDisable))]
    public static T SetOomKillDisable<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.OomKillDisable, v));
    /// <inheritdoc cref="PodmanCreateSettings.OomKillDisable"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomKillDisable))]
    public static T ResetOomKillDisable<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.OomKillDisable));
    /// <inheritdoc cref="PodmanCreateSettings.OomKillDisable"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomKillDisable))]
    public static T EnableOomKillDisable<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.OomKillDisable, true));
    /// <inheritdoc cref="PodmanCreateSettings.OomKillDisable"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomKillDisable))]
    public static T DisableOomKillDisable<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.OomKillDisable, false));
    /// <inheritdoc cref="PodmanCreateSettings.OomKillDisable"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomKillDisable))]
    public static T ToggleOomKillDisable<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.OomKillDisable, !o.OomKillDisable));
    #endregion
    #region OomScoreAdj
    /// <inheritdoc cref="PodmanCreateSettings.OomScoreAdj"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomScoreAdj))]
    public static T SetOomScoreAdj<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.OomScoreAdj, v));
    /// <inheritdoc cref="PodmanCreateSettings.OomScoreAdj"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.OomScoreAdj))]
    public static T ResetOomScoreAdj<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.OomScoreAdj));
    #endregion
    #region Pid
    /// <inheritdoc cref="PodmanCreateSettings.Pid"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Pid))]
    public static T SetPid<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Pid, v));
    /// <inheritdoc cref="PodmanCreateSettings.Pid"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Pid))]
    public static T ResetPid<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Pid));
    #endregion
    #region PidsLimit
    /// <inheritdoc cref="PodmanCreateSettings.PidsLimit"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PidsLimit))]
    public static T SetPidsLimit<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PidsLimit, v));
    /// <inheritdoc cref="PodmanCreateSettings.PidsLimit"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PidsLimit))]
    public static T ResetPidsLimit<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.PidsLimit));
    #endregion
    #region Pod
    /// <inheritdoc cref="PodmanCreateSettings.Pod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Pod))]
    public static T SetPod<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Pod, v));
    /// <inheritdoc cref="PodmanCreateSettings.Pod"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Pod))]
    public static T ResetPod<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Pod));
    #endregion
    #region PodIdFile
    /// <inheritdoc cref="PodmanCreateSettings.PodIdFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PodIdFile))]
    public static T SetPodIdFile<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PodIdFile, v));
    /// <inheritdoc cref="PodmanCreateSettings.PodIdFile"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PodIdFile))]
    public static T ResetPodIdFile<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.PodIdFile));
    #endregion
    #region PreserveFds
    /// <inheritdoc cref="PodmanCreateSettings.PreserveFds"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PreserveFds))]
    public static T SetPreserveFds<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PreserveFds, v));
    /// <inheritdoc cref="PodmanCreateSettings.PreserveFds"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PreserveFds))]
    public static T ResetPreserveFds<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.PreserveFds));
    #endregion
    #region Privileged
    /// <inheritdoc cref="PodmanCreateSettings.Privileged"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Privileged))]
    public static T SetPrivileged<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Privileged, v));
    /// <inheritdoc cref="PodmanCreateSettings.Privileged"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Privileged))]
    public static T ResetPrivileged<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Privileged));
    /// <inheritdoc cref="PodmanCreateSettings.Privileged"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Privileged))]
    public static T EnablePrivileged<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Privileged, true));
    /// <inheritdoc cref="PodmanCreateSettings.Privileged"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Privileged))]
    public static T DisablePrivileged<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Privileged, false));
    /// <inheritdoc cref="PodmanCreateSettings.Privileged"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Privileged))]
    public static T TogglePrivileged<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Privileged, !o.Privileged));
    #endregion
    #region Publish
    /// <inheritdoc cref="PodmanCreateSettings.Publish"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Publish))]
    public static T SetPublish<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Publish, v));
    /// <inheritdoc cref="PodmanCreateSettings.Publish"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Publish))]
    public static T ResetPublish<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Publish));
    #endregion
    #region PublishAll
    /// <inheritdoc cref="PodmanCreateSettings.PublishAll"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PublishAll))]
    public static T SetPublishAll<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PublishAll, v));
    /// <inheritdoc cref="PodmanCreateSettings.PublishAll"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PublishAll))]
    public static T ResetPublishAll<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.PublishAll));
    /// <inheritdoc cref="PodmanCreateSettings.PublishAll"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PublishAll))]
    public static T EnablePublishAll<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PublishAll, true));
    /// <inheritdoc cref="PodmanCreateSettings.PublishAll"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PublishAll))]
    public static T DisablePublishAll<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PublishAll, false));
    /// <inheritdoc cref="PodmanCreateSettings.PublishAll"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.PublishAll))]
    public static T TogglePublishAll<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.PublishAll, !o.PublishAll));
    #endregion
    #region ReadOnly
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnly"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnly))]
    public static T SetReadOnly<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnly, v));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnly"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnly))]
    public static T ResetReadOnly<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.ReadOnly));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnly"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnly))]
    public static T EnableReadOnly<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnly, true));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnly"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnly))]
    public static T DisableReadOnly<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnly, false));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnly"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnly))]
    public static T ToggleReadOnly<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnly, !o.ReadOnly));
    #endregion
    #region ReadOnlyTmpfs
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnlyTmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnlyTmpfs))]
    public static T SetReadOnlyTmpfs<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnlyTmpfs, v));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnlyTmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnlyTmpfs))]
    public static T ResetReadOnlyTmpfs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.ReadOnlyTmpfs));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnlyTmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnlyTmpfs))]
    public static T EnableReadOnlyTmpfs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnlyTmpfs, true));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnlyTmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnlyTmpfs))]
    public static T DisableReadOnlyTmpfs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnlyTmpfs, false));
    /// <inheritdoc cref="PodmanCreateSettings.ReadOnlyTmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ReadOnlyTmpfs))]
    public static T ToggleReadOnlyTmpfs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ReadOnlyTmpfs, !o.ReadOnlyTmpfs));
    #endregion
    #region Replace
    /// <inheritdoc cref="PodmanCreateSettings.Replace"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Replace))]
    public static T SetReplace<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Replace, v));
    /// <inheritdoc cref="PodmanCreateSettings.Replace"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Replace))]
    public static T ResetReplace<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Replace));
    /// <inheritdoc cref="PodmanCreateSettings.Replace"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Replace))]
    public static T EnableReplace<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Replace, true));
    /// <inheritdoc cref="PodmanCreateSettings.Replace"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Replace))]
    public static T DisableReplace<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Replace, false));
    /// <inheritdoc cref="PodmanCreateSettings.Replace"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Replace))]
    public static T ToggleReplace<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Replace, !o.Replace));
    #endregion
    #region Restart
    /// <inheritdoc cref="PodmanCreateSettings.Restart"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Restart))]
    public static T SetRestart<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Restart, v));
    /// <inheritdoc cref="PodmanCreateSettings.Restart"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Restart))]
    public static T ResetRestart<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Restart));
    #endregion
    #region Rm
    /// <inheritdoc cref="PodmanCreateSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Rm))]
    public static T SetRm<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Rm, v));
    /// <inheritdoc cref="PodmanCreateSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Rm))]
    public static T ResetRm<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Rm));
    /// <inheritdoc cref="PodmanCreateSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Rm))]
    public static T EnableRm<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Rm, true));
    /// <inheritdoc cref="PodmanCreateSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Rm))]
    public static T DisableRm<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Rm, false));
    /// <inheritdoc cref="PodmanCreateSettings.Rm"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Rm))]
    public static T ToggleRm<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Rm, !o.Rm));
    #endregion
    #region SecurityOpt
    /// <inheritdoc cref="PodmanCreateSettings.SecurityOpt"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SecurityOpt))]
    public static T SetSecurityOpt<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.SecurityOpt, v));
    /// <inheritdoc cref="PodmanCreateSettings.SecurityOpt"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SecurityOpt))]
    public static T ResetSecurityOpt<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.SecurityOpt));
    #endregion
    #region ShmSize
    /// <inheritdoc cref="PodmanCreateSettings.ShmSize"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ShmSize))]
    public static T SetShmSize<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.ShmSize, v));
    /// <inheritdoc cref="PodmanCreateSettings.ShmSize"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.ShmSize))]
    public static T ResetShmSize<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.ShmSize));
    #endregion
    #region SigProxy
    /// <inheritdoc cref="PodmanCreateSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SigProxy))]
    public static T SetSigProxy<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.SigProxy, v));
    /// <inheritdoc cref="PodmanCreateSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SigProxy))]
    public static T ResetSigProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.SigProxy));
    /// <inheritdoc cref="PodmanCreateSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SigProxy))]
    public static T EnableSigProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.SigProxy, true));
    /// <inheritdoc cref="PodmanCreateSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SigProxy))]
    public static T DisableSigProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.SigProxy, false));
    /// <inheritdoc cref="PodmanCreateSettings.SigProxy"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.SigProxy))]
    public static T ToggleSigProxy<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.SigProxy, !o.SigProxy));
    #endregion
    #region StopSignal
    /// <inheritdoc cref="PodmanCreateSettings.StopSignal"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.StopSignal))]
    public static T SetStopSignal<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.StopSignal, v));
    /// <inheritdoc cref="PodmanCreateSettings.StopSignal"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.StopSignal))]
    public static T ResetStopSignal<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.StopSignal));
    #endregion
    #region StopTimeout
    /// <inheritdoc cref="PodmanCreateSettings.StopTimeout"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.StopTimeout))]
    public static T SetStopTimeout<T>(this T o, int? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.StopTimeout, v));
    /// <inheritdoc cref="PodmanCreateSettings.StopTimeout"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.StopTimeout))]
    public static T ResetStopTimeout<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.StopTimeout));
    #endregion
    #region Subgidname
    /// <inheritdoc cref="PodmanCreateSettings.Subgidname"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Subgidname))]
    public static T SetSubgidname<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Subgidname, v));
    /// <inheritdoc cref="PodmanCreateSettings.Subgidname"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Subgidname))]
    public static T ResetSubgidname<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Subgidname));
    #endregion
    #region Subuidname
    /// <inheritdoc cref="PodmanCreateSettings.Subuidname"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Subuidname))]
    public static T SetSubuidname<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Subuidname, v));
    /// <inheritdoc cref="PodmanCreateSettings.Subuidname"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Subuidname))]
    public static T ResetSubuidname<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Subuidname));
    #endregion
    #region Sysctl
    /// <inheritdoc cref="PodmanCreateSettings.Sysctl"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Sysctl))]
    public static T SetSysctl<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Sysctl, v));
    /// <inheritdoc cref="PodmanCreateSettings.Sysctl"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Sysctl))]
    public static T ResetSysctl<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Sysctl));
    #endregion
    #region Tmpfs
    /// <inheritdoc cref="PodmanCreateSettings.Tmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tmpfs))]
    public static T SetTmpfs<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Tmpfs, v));
    /// <inheritdoc cref="PodmanCreateSettings.Tmpfs"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tmpfs))]
    public static T ResetTmpfs<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Tmpfs));
    #endregion
    #region Tty
    /// <inheritdoc cref="PodmanCreateSettings.Tty"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tty))]
    public static T SetTty<T>(this T o, bool? v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Tty, v));
    /// <inheritdoc cref="PodmanCreateSettings.Tty"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tty))]
    public static T ResetTty<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Tty));
    /// <inheritdoc cref="PodmanCreateSettings.Tty"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tty))]
    public static T EnableTty<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Tty, true));
    /// <inheritdoc cref="PodmanCreateSettings.Tty"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tty))]
    public static T DisableTty<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Tty, false));
    /// <inheritdoc cref="PodmanCreateSettings.Tty"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Tty))]
    public static T ToggleTty<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Tty, !o.Tty));
    #endregion
    #region Ulimit
    /// <inheritdoc cref="PodmanCreateSettings.Ulimit"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Ulimit))]
    public static T SetUlimit<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Ulimit, v));
    /// <inheritdoc cref="PodmanCreateSettings.Ulimit"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Ulimit))]
    public static T ResetUlimit<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Ulimit));
    #endregion
    #region User
    /// <inheritdoc cref="PodmanCreateSettings.User"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.User))]
    public static T SetUser<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.User, v));
    /// <inheritdoc cref="PodmanCreateSettings.User"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.User))]
    public static T ResetUser<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.User));
    #endregion
    #region Userns
    /// <inheritdoc cref="PodmanCreateSettings.Userns"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Userns))]
    public static T SetUserns<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Userns, v));
    /// <inheritdoc cref="PodmanCreateSettings.Userns"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Userns))]
    public static T ResetUserns<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Userns));
    #endregion
    #region Uts
    /// <inheritdoc cref="PodmanCreateSettings.Uts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Uts))]
    public static T SetUts<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Uts, v));
    /// <inheritdoc cref="PodmanCreateSettings.Uts"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Uts))]
    public static T ResetUts<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Uts));
    #endregion
    #region Volume
    /// <inheritdoc cref="PodmanCreateSettings.Volume"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Volume))]
    public static T SetVolume<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Volume, v));
    /// <inheritdoc cref="PodmanCreateSettings.Volume"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Volume))]
    public static T ResetVolume<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Volume));
    #endregion
    #region VolumesFrom
    /// <inheritdoc cref="PodmanCreateSettings.VolumesFrom"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.VolumesFrom))]
    public static T SetVolumesFrom<T>(this T o, IReadOnlyList<string> v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.VolumesFrom, v));
    /// <inheritdoc cref="PodmanCreateSettings.VolumesFrom"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.VolumesFrom))]
    public static T ResetVolumesFrom<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.VolumesFrom));
    #endregion
    #region Workdir
    /// <inheritdoc cref="PodmanCreateSettings.Workdir"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Workdir))]
    public static T SetWorkdir<T>(this T o, string v) where T : PodmanCreateSettings => o.Modify(b => b.Set(() => o.Workdir, v));
    /// <inheritdoc cref="PodmanCreateSettings.Workdir"/>
    [Pure] [Builder(Type = typeof(PodmanCreateSettings), Property = nameof(PodmanCreateSettings.Workdir))]
    public static T ResetWorkdir<T>(this T o) where T : PodmanCreateSettings => o.Modify(b => b.Remove(() => o.Workdir));
    #endregion
}
#endregion
#region PodmanDiffSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanDiff(Candoumbe.Pipelines.Tools.Podman.PodmanDiffSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanDiffSettingsExtensions
{
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
#region PodmanImagePullSettingsExtensions
/// <inheritdoc cref="PodmanTasks.PodmanImagePull(Candoumbe.Pipelines.Tools.Podman.PodmanImagePullSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PodmanImagePullSettingsExtensions
{
    #region Authfile
    /// <inheritdoc cref="PodmanImagePullSettings.Authfile"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Authfile))]
    public static T SetAuthfile<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Authfile, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Authfile"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Authfile))]
    public static T ResetAuthfile<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Authfile));
    #endregion
    #region CertDir
    /// <inheritdoc cref="PodmanImagePullSettings.CertDir"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.CertDir))]
    public static T SetCertDir<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.CertDir, v));
    /// <inheritdoc cref="PodmanImagePullSettings.CertDir"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.CertDir))]
    public static T ResetCertDir<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.CertDir));
    #endregion
    #region Compress
    /// <inheritdoc cref="PodmanImagePullSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Compress))]
    public static T SetCompress<T>(this T o, bool? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Compress, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Compress))]
    public static T ResetCompress<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Compress));
    /// <inheritdoc cref="PodmanImagePullSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Compress))]
    public static T EnableCompress<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Compress, true));
    /// <inheritdoc cref="PodmanImagePullSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Compress))]
    public static T DisableCompress<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Compress, false));
    /// <inheritdoc cref="PodmanImagePullSettings.Compress"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Compress))]
    public static T ToggleCompress<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Compress, !o.Compress));
    #endregion
    #region CompressionFormat
    /// <inheritdoc cref="PodmanImagePullSettings.CompressionFormat"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.CompressionFormat))]
    public static T SetCompressionFormat<T>(this T o, CompressionFormatType v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.CompressionFormat, v));
    /// <inheritdoc cref="PodmanImagePullSettings.CompressionFormat"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.CompressionFormat))]
    public static T ResetCompressionFormat<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.CompressionFormat));
    #endregion
    #region CompressionLevel
    /// <inheritdoc cref="PodmanImagePullSettings.CompressionLevel"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.CompressionLevel))]
    public static T SetCompressionLevel<T>(this T o, int? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.CompressionLevel, v));
    /// <inheritdoc cref="PodmanImagePullSettings.CompressionLevel"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.CompressionLevel))]
    public static T ResetCompressionLevel<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.CompressionLevel));
    #endregion
    #region Creds
    /// <inheritdoc cref="PodmanImagePullSettings.Creds"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Creds))]
    public static T SetCreds<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Creds, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Creds"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Creds))]
    public static T ResetCreds<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Creds));
    #endregion
    #region Digestfile
    /// <inheritdoc cref="PodmanImagePullSettings.Digestfile"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Digestfile))]
    public static T SetDigestfile<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Digestfile, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Digestfile"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Digestfile))]
    public static T ResetDigestfile<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Digestfile));
    #endregion
    #region DisableContentTrust
    /// <inheritdoc cref="PodmanImagePullSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.DisableContentTrust))]
    public static T SetDisableContentTrust<T>(this T o, bool? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, v));
    /// <inheritdoc cref="PodmanImagePullSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.DisableContentTrust))]
    public static T ResetDisableContentTrust<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.DisableContentTrust));
    /// <inheritdoc cref="PodmanImagePullSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.DisableContentTrust))]
    public static T EnableDisableContentTrust<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, true));
    /// <inheritdoc cref="PodmanImagePullSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.DisableContentTrust))]
    public static T DisableDisableContentTrust<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, false));
    /// <inheritdoc cref="PodmanImagePullSettings.DisableContentTrust"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.DisableContentTrust))]
    public static T ToggleDisableContentTrust<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.DisableContentTrust, !o.DisableContentTrust));
    #endregion
    #region EncryptLayer
    /// <inheritdoc cref="PodmanImagePullSettings.EncryptLayer"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.EncryptLayer))]
    public static T SetEncryptLayer<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.EncryptLayer, v));
    /// <inheritdoc cref="PodmanImagePullSettings.EncryptLayer"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.EncryptLayer))]
    public static T ResetEncryptLayer<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.EncryptLayer));
    #endregion
    #region EncryptionKey
    /// <inheritdoc cref="PodmanImagePullSettings.EncryptionKey"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.EncryptionKey))]
    public static T SetEncryptionKey<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.EncryptionKey, v));
    /// <inheritdoc cref="PodmanImagePullSettings.EncryptionKey"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.EncryptionKey))]
    public static T ResetEncryptionKey<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.EncryptionKey));
    #endregion
    #region ForceCompression
    /// <inheritdoc cref="PodmanImagePullSettings.ForceCompression"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.ForceCompression))]
    public static T SetForceCompression<T>(this T o, bool? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.ForceCompression, v));
    /// <inheritdoc cref="PodmanImagePullSettings.ForceCompression"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.ForceCompression))]
    public static T ResetForceCompression<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.ForceCompression));
    /// <inheritdoc cref="PodmanImagePullSettings.ForceCompression"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.ForceCompression))]
    public static T EnableForceCompression<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.ForceCompression, true));
    /// <inheritdoc cref="PodmanImagePullSettings.ForceCompression"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.ForceCompression))]
    public static T DisableForceCompression<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.ForceCompression, false));
    /// <inheritdoc cref="PodmanImagePullSettings.ForceCompression"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.ForceCompression))]
    public static T ToggleForceCompression<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.ForceCompression, !o.ForceCompression));
    #endregion
    #region Format
    /// <inheritdoc cref="PodmanImagePullSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Format))]
    public static T SetFormat<T>(this T o, ManifestType v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Format, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Format"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Format))]
    public static T ResetFormat<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Format));
    #endregion
    #region Quiet
    /// <inheritdoc cref="PodmanImagePullSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Quiet))]
    public static T SetQuiet<T>(this T o, bool? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Quiet, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Quiet))]
    public static T ResetQuiet<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Quiet));
    /// <inheritdoc cref="PodmanImagePullSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Quiet))]
    public static T EnableQuiet<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Quiet, true));
    /// <inheritdoc cref="PodmanImagePullSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Quiet))]
    public static T DisableQuiet<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Quiet, false));
    /// <inheritdoc cref="PodmanImagePullSettings.Quiet"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Quiet))]
    public static T ToggleQuiet<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Quiet, !o.Quiet));
    #endregion
    #region RemoveSignatures
    /// <inheritdoc cref="PodmanImagePullSettings.RemoveSignatures"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RemoveSignatures))]
    public static T SetRemoveSignatures<T>(this T o, bool? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.RemoveSignatures, v));
    /// <inheritdoc cref="PodmanImagePullSettings.RemoveSignatures"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RemoveSignatures))]
    public static T ResetRemoveSignatures<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.RemoveSignatures));
    /// <inheritdoc cref="PodmanImagePullSettings.RemoveSignatures"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RemoveSignatures))]
    public static T EnableRemoveSignatures<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.RemoveSignatures, true));
    /// <inheritdoc cref="PodmanImagePullSettings.RemoveSignatures"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RemoveSignatures))]
    public static T DisableRemoveSignatures<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.RemoveSignatures, false));
    /// <inheritdoc cref="PodmanImagePullSettings.RemoveSignatures"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RemoveSignatures))]
    public static T ToggleRemoveSignatures<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.RemoveSignatures, !o.RemoveSignatures));
    #endregion
    #region Retry
    /// <inheritdoc cref="PodmanImagePullSettings.Retry"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Retry))]
    public static T SetRetry<T>(this T o, int? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.Retry, v));
    /// <inheritdoc cref="PodmanImagePullSettings.Retry"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.Retry))]
    public static T ResetRetry<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.Retry));
    #endregion
    #region RetryDelay
    /// <inheritdoc cref="PodmanImagePullSettings.RetryDelay"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RetryDelay))]
    public static T SetRetryDelay<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.RetryDelay, v));
    /// <inheritdoc cref="PodmanImagePullSettings.RetryDelay"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.RetryDelay))]
    public static T ResetRetryDelay<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.RetryDelay));
    #endregion
    #region SignBy
    /// <inheritdoc cref="PodmanImagePullSettings.SignBy"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBy))]
    public static T SetSignBy<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.SignBy, v));
    /// <inheritdoc cref="PodmanImagePullSettings.SignBy"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBy))]
    public static T ResetSignBy<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.SignBy));
    #endregion
    #region SignBySigstore
    /// <inheritdoc cref="PodmanImagePullSettings.SignBySigstore"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBySigstore))]
    public static T SetSignBySigstore<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.SignBySigstore, v));
    /// <inheritdoc cref="PodmanImagePullSettings.SignBySigstore"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBySigstore))]
    public static T ResetSignBySigstore<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.SignBySigstore));
    #endregion
    #region SignBySigstorePrivateKey
    /// <inheritdoc cref="PodmanImagePullSettings.SignBySigstorePrivateKey"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBySigstorePrivateKey))]
    public static T SetSignBySigstorePrivateKey<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.SignBySigstorePrivateKey, v));
    /// <inheritdoc cref="PodmanImagePullSettings.SignBySigstorePrivateKey"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBySigstorePrivateKey))]
    public static T ResetSignBySigstorePrivateKey<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.SignBySigstorePrivateKey));
    #endregion
    #region SignBySqFingerprint
    /// <inheritdoc cref="PodmanImagePullSettings.SignBySqFingerprint"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBySqFingerprint))]
    public static T SetSignBySqFingerprint<T>(this T o, string v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.SignBySqFingerprint, v));
    /// <inheritdoc cref="PodmanImagePullSettings.SignBySqFingerprint"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignBySqFingerprint))]
    public static T ResetSignBySqFingerprint<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.SignBySqFingerprint));
    #endregion
    #region SignPassphraseFile
    /// <inheritdoc cref="PodmanImagePullSettings.SignPassphraseFile"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignPassphraseFile))]
    public static T SetSignPassphraseFile<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.SignPassphraseFile, v));
    /// <inheritdoc cref="PodmanImagePullSettings.SignPassphraseFile"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.SignPassphraseFile))]
    public static T ResetSignPassphraseFile<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.SignPassphraseFile));
    #endregion
    #region TlsVerify
    /// <inheritdoc cref="PodmanImagePullSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.TlsVerify))]
    public static T SetTlsVerify<T>(this T o, bool? v) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.TlsVerify, v));
    /// <inheritdoc cref="PodmanImagePullSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.TlsVerify))]
    public static T ResetTlsVerify<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Remove(() => o.TlsVerify));
    /// <inheritdoc cref="PodmanImagePullSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.TlsVerify))]
    public static T EnableTlsVerify<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.TlsVerify, true));
    /// <inheritdoc cref="PodmanImagePullSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.TlsVerify))]
    public static T DisableTlsVerify<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.TlsVerify, false));
    /// <inheritdoc cref="PodmanImagePullSettings.TlsVerify"/>
    [Pure] [Builder(Type = typeof(PodmanImagePullSettings), Property = nameof(PodmanImagePullSettings.TlsVerify))]
    public static T ToggleTlsVerify<T>(this T o) where T : PodmanImagePullSettings => o.Modify(b => b.Set(() => o.TlsVerify, !o.TlsVerify));
    #endregion
}
#endregion
#region PullPolicy
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PullPolicy>))]
public partial class PullPolicy : Enumeration
{
    public static PullPolicy always = (PullPolicy) "always";
    public static PullPolicy missing = (PullPolicy) "missing";
    public static PullPolicy never = (PullPolicy) "never";
    public static PullPolicy newer = (PullPolicy) "newer";
    public static implicit operator PullPolicy(string value)
    {
        return new PullPolicy { Value = value };
    }
}
#endregion
#region SbomPreset
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<SbomPreset>))]
public partial class SbomPreset : Enumeration
{
    public static SbomPreset syft = (SbomPreset) "syft";
    public static SbomPreset syft_cyclonedx = (SbomPreset) "syft-cyclonedx";
    public static SbomPreset syft_spdx = (SbomPreset) "syft-spdx";
    public static SbomPreset trivy = (SbomPreset) "trivy";
    public static SbomPreset trivy_cyclonedx = (SbomPreset) "trivy-cyclonedx";
    public static SbomPreset trivy_spdx = (SbomPreset) "trivy-spdx";
    public static implicit operator SbomPreset(string value)
    {
        return new SbomPreset { Value = value };
    }
}
#endregion
#region SbomMergeStrategy
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<SbomMergeStrategy>))]
public partial class SbomMergeStrategy : Enumeration
{
    public static SbomMergeStrategy cat = (SbomMergeStrategy) "cat";
    public static SbomMergeStrategy merge_cyclonedx_by_component_name_and_version = (SbomMergeStrategy) "merge-cyclonedx-by-component-name-and-version";
    public static SbomMergeStrategy merge_spdx_by_package_name_and_versioninfo = (SbomMergeStrategy) "merge-spdx-by-package-name-and-versioninfo";
    public static implicit operator SbomMergeStrategy(string value)
    {
        return new SbomMergeStrategy { Value = value };
    }
}
#endregion
#region SecurityOptions
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<SecurityOptions>))]
public partial class SecurityOptions : Enumeration
{
    public static SecurityOptions apparmor_unconfined = (SecurityOptions) "apparmor=unconfined";
    public static SecurityOptions apparmor_alternate_profile = (SecurityOptions) "apparmor=alternate-profile";
    public static SecurityOptions label_user_USER = (SecurityOptions) "label=user:USER";
    public static SecurityOptions label_role_ROLE = (SecurityOptions) "label=role:ROLE";
    public static SecurityOptions label_type_TYPE = (SecurityOptions) "label=type:TYPE";
    public static SecurityOptions label_level_LEVEL = (SecurityOptions) "label=level:LEVEL";
    public static SecurityOptions label_filetype_TYPE = (SecurityOptions) "label=filetype:TYPE";
    public static SecurityOptions label_disable = (SecurityOptions) "label=disable";
    public static SecurityOptions no_new_privileges = (SecurityOptions) "no-new-privileges";
    public static SecurityOptions seccomp_unconfined = (SecurityOptions) "seccomp=unconfined";
    public static SecurityOptions seccomp_profile_json = (SecurityOptions) "seccomp=profile.json";
    public static implicit operator SecurityOptions(string value)
    {
        return new SecurityOptions { Value = value };
    }
}
#endregion
#region FormatType
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<FormatType>))]
public partial class FormatType : Enumeration
{
    public static FormatType docker = (FormatType) "docker";
    public static FormatType oci = (FormatType) "oci";
    public static implicit operator FormatType(string value)
    {
        return new FormatType { Value = value };
    }
}
#endregion
#region InstructionType
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<InstructionType>))]
public partial class InstructionType : Enumeration
{
    public static InstructionType CMD = (InstructionType) "CMD";
    public static InstructionType ENTRYPOINT = (InstructionType) "ENTRYPOINT";
    public static InstructionType ENV = (InstructionType) "ENV";
    public static InstructionType EXPOSE = (InstructionType) "EXPOSE";
    public static InstructionType LABEL = (InstructionType) "LABEL";
    public static InstructionType ONBUILD = (InstructionType) "ONBUILD";
    public static InstructionType STOPSIGNAL = (InstructionType) "STOPSIGNAL";
    public static InstructionType USER = (InstructionType) "USER";
    public static InstructionType VOLUME = (InstructionType) "VOLUME";
    public static InstructionType WORKDIR = (InstructionType) "WORKDIR";
    public static implicit operator InstructionType(string value)
    {
        return new InstructionType { Value = value };
    }
}
#endregion
#region AttachType
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<AttachType>))]
public partial class AttachType : Enumeration
{
    public static AttachType stdin = (AttachType) "stdin";
    public static AttachType stdout = (AttachType) "stdout";
    public static AttachType stderr = (AttachType) "stderr";
    public static implicit operator AttachType(string value)
    {
        return new AttachType { Value = value };
    }
}
#endregion
#region CompressionFormatType
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<CompressionFormatType>))]
public partial class CompressionFormatType : Enumeration
{
    public static CompressionFormatType gzip = (CompressionFormatType) "gzip";
    public static CompressionFormatType zstd = (CompressionFormatType) "zstd";
    public static CompressionFormatType zstd_chunked = (CompressionFormatType) "zstd:chunked";
    public static implicit operator CompressionFormatType(string value)
    {
        return new CompressionFormatType { Value = value };
    }
}
#endregion
#region ManifestType
/// <summary>Used within <see cref="PodmanTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<ManifestType>))]
public partial class ManifestType : Enumeration
{
    public static ManifestType oci = (ManifestType) "oci";
    public static ManifestType v2s1 = (ManifestType) "v2s1";
    public static ManifestType v2s2 = (ManifestType) "v2s2";
    public static implicit operator ManifestType(string value)
    {
        return new ManifestType { Value = value };
    }
}
#endregion
