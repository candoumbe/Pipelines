
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

namespace Candoumbe.Pipelines.Tools;

/// <summary><p>Stryker.NET offers you mutation testing for your .NET Core and .NET Framework projects. It allows you to test your tests by temporarily inserting bugs. Stryker.NET is installed using NuGet.  New to Stryker.NET? Begin with our guide on <see href='https://stryker-mutator.io/docs/stryker-net/Getting-started'>getting started</see></p><p>For more details, visit the <a href="https://stryker-mutator.io/docs/stryker-net/configuration">official website</a>.</p></summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[NuGetTool(Id = PackageId, Executable = PackageExecutable)]
public partial class StrykerTasks : ToolTasks, IRequireNuGetPackage
{
    public static string StrykerPath { get => new StrykerTasks().GetToolPathInternal(); set => new StrykerTasks().SetToolPath(value); }
    public const string PackageId = "dotnet-stryker";
    public const string PackageExecutable = "Stryker.CLI.dll";
    /// <summary><p>Stryker.NET offers you mutation testing for your .NET Core and .NET Framework projects. It allows you to test your tests by temporarily inserting bugs. Stryker.NET is installed using NuGet.  New to Stryker.NET? Begin with our guide on <see href='https://stryker-mutator.io/docs/stryker-net/Getting-started'>getting started</see></p><p>For more details, visit the <a href="https://stryker-mutator.io/docs/stryker-net/configuration">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> Stryker(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new StrykerTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Stryker.NET offers you mutation testing for your .NET Core and .NET Framework projects. It allows you to test your tests by temporarily inserting bugs. Stryker.NET is installed using NuGet.  New to Stryker.NET? Begin with our guide on <see href='https://stryker-mutator.io/docs/stryker-net/Getting-started'>getting started</see></p><p>For more details, visit the <a href="https://stryker-mutator.io/docs/stryker-net/configuration">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--break-at</c> via <see cref="StrykerSettings.BreakAt"/></li><li><c>--break-on-initial-test-failure</c> via <see cref="StrykerSettings.BreakOnInitialTestFailure"/></li><li><c>--concurrency</c> via <see cref="StrykerSettings.Concurrency"/></li><li><c>--config-file</c> via <see cref="StrykerSettings.ConfigFile"/></li><li><c>--configuration</c> via <see cref="StrykerSettings.Configuration"/></li><li><c>--dashboard-api-key</c> via <see cref="StrykerSettings.DashboardApiKey"/></li><li><c>--dev-mode</c> via <see cref="StrykerSettings.DevMode"/></li><li><c>--disable-bail</c> via <see cref="StrykerSettings.DisableBail"/></li><li><c>--log-to-file</c> via <see cref="StrykerSettings.LogToFile"/></li><li><c>--msbuild-path</c> via <see cref="StrykerSettings.MsBuildPath"/></li><li><c>--mutate</c> via <see cref="StrykerSettings.Mutate"/></li><li><c>--mutation-level</c> via <see cref="StrykerSettings.MutationLevel"/></li><li><c>--open-report</c> via <see cref="StrykerSettings.OpenReport"/></li><li><c>--output</c> via <see cref="StrykerSettings.Output"/></li><li><c>--project</c> via <see cref="StrykerSettings.Project"/></li><li><c>--reporter</c> via <see cref="StrykerSettings.Reporters"/></li><li><c>--since</c> via <see cref="StrykerSettings.Since"/></li><li><c>--solution</c> via <see cref="StrykerSettings.Solution"/></li><li><c>--target-framework</c> via <see cref="StrykerSettings.TargetFramework"/></li><li><c>--test-project</c> via <see cref="StrykerSettings.TestProjects"/></li><li><c>--threshold-high</c> via <see cref="StrykerSettings.ThresholdHigh"/></li><li><c>--threshold-low</c> via <see cref="StrykerSettings.ThresholdLow"/></li><li><c>--verbosity</c> via <see cref="StrykerSettings.Verbosity"/></li><li><c>--version</c> via <see cref="StrykerSettings.ProjectInfoVersion"/></li><li><c>--with-baseline</c> via <see cref="StrykerSettings.WithBaseline"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> Stryker(StrykerSettings options = null) => new StrykerTasks().Run<StrykerSettings>(options);
    /// <inheritdoc cref="StrykerTasks.Stryker(Candoumbe.Pipelines.Tools.StrykerSettings)"/>
    public static IReadOnlyCollection<Output> Stryker(Configure<StrykerSettings> configurator) => new StrykerTasks().Run<StrykerSettings>(configurator.Invoke(new StrykerSettings()));
    /// <inheritdoc cref="StrykerTasks.Stryker(Candoumbe.Pipelines.Tools.StrykerSettings)"/>
    public static IEnumerable<(StrykerSettings Settings, IReadOnlyCollection<Output> Output)> Stryker(CombinatorialConfigure<StrykerSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(Stryker, degreeOfParallelism, completeOnFailure);
}
#region StrykerSettings
/// <inheritdoc cref="StrykerTasks.Stryker(Candoumbe.Pipelines.Tools.StrykerSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(StrykerTasks), Command = nameof(StrykerTasks.Stryker))]
public partial class StrykerSettings : ToolOptions
{
    /// <summary>Path / Name of the configuration file. You can specify a custom path to the config file. For example if you want to add the stryker config section to your appsettings file. The section should still be called <c>stryker-config</c>.</summary>
    [Argument(Format = "--config-file {value}")] public string ConfigFile => Get<string>(() => ConfigFile);
    /// <summary>The solution path can be supplied to help with dependency resolution. If stryker is ran from the solution file location the solution file will be analyzed and all projects in the solution will be tested by stryker.</summary>
    [Argument(Format = "--solution {value}")] public Nuke.Common.IO.AbsolutePath Solution => Get<Nuke.Common.IO.AbsolutePath>(() => Solution);
    /// <summary>The project file name is required when your test project has more than one project reference. Stryker can currently mutate one project under test for 1..N test projects but not 1..N projects under test for one test project.<br /><i>Do not pass a path to this option. Pass the project file <strong>name</strong> as it appears in your test project's references</i></summary>
    [Argument(Format = "--project {value}")] public string Project => Get<string>(() => Project);
    /// <summary>When you have multiple test projects covering one project under test you may specify all relevant test projects in the config file. You must run stryker from the project under test instead of the test project directory when using multiple test projects.</summary>
    [Argument(Format = "--test-project {value}")] public IReadOnlyList<string> TestProjects => Get<List<string>>(() => TestProjects);
    /// <summary><para>With <c>mutate</c> you configure the subset of files to use for mutation testing. Only source files part of your project will be taken into account. When this option is not specified the whole project will be mutated. You can add an <c>!</c> in front of the pattern to exclude instead of include matching files. This can be used to for example ignore generated files while mutating.</para><para>When only exclude patterns are provided, all files will be included that do not match any exclude pattern. If both include and exclude patterns are provided, only the files that match an include pattern but not also an exclude pattern will be included. The order of the patterns is irrelevant.</para><para>The patterns support <see href="https://en.wikipedia.org/wiki/Glob_(programming)">globbing syntax</see> to allow wildcards.</para><para><b>Example</b> :</para><table><thead><tr><th>Patterns</th><th>File</th><th>Will be mutated</th></tr></thead><tbody><tr><td>null</td><td>MyFolder/MyFactory.cs</td><td>Yes</td></tr><tr><td>'**/*.*'</td><td>MyFolder/MyFactory.cs</td><td>Yes</td></tr><tr><td>'!**/MyFactory.cs'</td><td>MyFolder/MyFactory.cs</td><td>No</td></tr></tbody></table><para>To allow more fine-grained filtering you can also specify the span of text that should be in- or excluded. A span is defined by the indices of the first character and the last character. <code>dotnet stryker -m "MyFolder/MyService.cs{10..100}"</code></para></summary>
    [Argument(Format = "--mutate {value}")] public IReadOnlyList<string> Mutate => Get<List<string>>(() => Mutate);
    /// <summary>Allows you to specify the build configuration to use when building the project. This can be useful when you want to test the release build of your project.</summary>
    [Argument(Format = "--configuration {value}")] public string Configuration => Get<string>(() => Configuration);
    /// <summary>Randomly selected. If the project targets multiple frameworks, this way you can specify the particular framework to build against. If you specify a non-existent target, Stryker will build the project against a random one (or the only one if so).</summary>
    [Argument(Format = "--target-framework {value}")] public string TargetFramework => Get<string>(() => TargetFramework);
    /// <summary>The version of the report. This should be filled with the branch name, git tag or git sha (although no validation is done). You can override a report of a specific version, like docker tags. Slashes in the version should not be encoded. For example, it's valid to use "feat/logging".</summary>
    [Argument(Format = "--version {value}")] public string ProjectInfoVersion => Get<string>(() => ProjectInfoVersion);
    /// <summary>Stryker supports multiple <see href="https://stryker-mutator.io/docs/stryker-net/configuration/#mutation-level-level">mutation level</see>s. Each level comes with a specific set of mutations. Each level contains the mutations of the levels below it. By setting the level to Complete you will get all possible mutations and thus the strictest mutation test. This comes at the price of longer runtime as more mutations will be generated and tested.</summary>
    [Argument(Format = "--mutation-level {value}")] public StrykerMutationLevel MutationLevel => Get<StrykerMutationLevel>(() => MutationLevel);
    /// <summary></summary>
    [Argument(Format = "--reporter {value}")] public IReadOnlyList<StrykerReporter> Reporters => Get<List<StrykerReporter>>(() => Reporters);
    /// <summary>When this option is passed, generated reports should open in the browser automatically once Stryker starts testing mutants, and will update the report till Stryker is done. Both html and dashboard reports can be opened automatically.</summary>
    [Argument(Format = "--open-report:{value}")] public StrykerOpenReport OpenReport => Get<StrykerOpenReport>(() => OpenReport);
    /// <summary>Change the amount of concurrent workers Stryker uses for the mutation test run. Defaults to using half your logical (virtual) processor count.<para><b>Example</b>: an intel i7 quad-core with hyperthreading has 8 logical cores and 4 physical cores. Stryker will use 4 concurrent workers when using the default.</para></summary>
    [Argument(Format = "--concurrency {value}")] public uint? Concurrency => Get<uint?>(() => Concurrency);
    /// <summary>Must be less than or equal to threshold low. When threshold break is set to anything other than 0 and the mutation score is lower than the threshold Stryker will exit with a non-zero code. This can be used in a CI pipeline to fail the pipeline when your mutation score is not sufficient.</summary>
    [Argument(Format = "--break-at {value}")] public uint? BreakAt => Get<uint?>(() => BreakAt);
    /// <summary>Minimum good mutation score. Must be higher than or equal to threshold low. Must be higher than 0.</summary>
    [Argument(Format = "--threshold-high {value}")] public short? ThresholdHigh => Get<short?>(() => ThresholdHigh);
    /// <summary>Minimum acceptable mutation score. Must be less than or equal to threshold high and more than or equal to threshold break.</summary>
    [Argument(Format = "--threshold-low {value}")] public short? ThresholdLow => Get<short?>(() => ThresholdLow);
    /// <summary>Changes the output path for Stryker logs and reports. This can be an absolute or relative path.</summary>
    [Argument(Format = "--output {value}")] public string Output => Get<string>(() => Output);
    /// <summary>Stryker aborts a unit testrun for a mutant as soon as one test fails because this is enough to confirm the mutant is killed. This can reduce the total runtime but also means you miss information about individual unit tests (e.g. if a unit test does not kill any mutants and is therefore useless). You can disable this behavior and run all unit tests for a mutant to completion. This can be especially useful when you want to find useless unit tests.</summary>
    [Argument(Format = "--disable-bail")] public bool? DisableBail => Get<bool?>(() => DisableBail);
    /// <summary><para>Use git information to test only code changes since the given target. Stryker will only report on mutants within the changed code. All other mutants will not have a result.</para><para>If you wish to test only changed sources and tests but would like to have a complete mutation report see with-baseline.</para><para>Set the diffing target on the command line by passing a committish with the since flag in the format <c>--since:&lt;committish&gt;</c>. Set the diffing target in the config file by setting the since target option.</para><para><i>* For changes on test project files all mutants covered by tests in that file will be seen as changed.</i></para></summary>
    [Argument(Format = "--since:{value}")] public string Since => Get<string>(() => Since);
    /// <summary>Change the console <c>verbosity</c> of Stryker when you want more or less details about the mutation testrun.</summary>
    [Argument(Format = "--verbosity {value}")] public StrykerVerbosity Verbosity => Get<StrykerVerbosity>(() => Verbosity);
    /// <summary>When creating an issue on github you can include a logfile so the issue can be diagnosed easier.<para><i>File logging always uses loglevel trace.</i></para></summary>
    [Argument(Format = "--log-to-file")] public bool? LogToFile => Get<bool?>(() => LogToFile);
    /// <summary>Stryker will not gracefully recover from compilation errors, but instead crash immediately. Used during development to quickly diagnose errors.<para>Also enables more debug logs not generally useful to normal users.</para></summary>
    [Argument(Format = "--dev-mode")] public bool? DevMode => Get<bool?>(() => DevMode);
    /// <summary>The API key for authentication with the Stryker dashboard.<br />Get your api key at the <see href="https://dashboard.stryker-mutator.io/">stryker dashboard</see>. To keep your api key safe, store it in an encrypted variable in your pipeline.</summary>
    [Argument(Format = "--dashboard-api-key {value}", Secret = true)] public string DashboardApiKey => Get<string>(() => DashboardApiKey);
    /// <summary>By default, Stryker tries to auto-discover msbuild on your system. If Stryker fails to discover msbuild you may supply the path to msbuild manually with this option.</summary>
    [Argument(Format = "--msbuild-path {value}")] public string MsBuildPath => Get<string>(() => MsBuildPath);
    /// <summary>Instruct Stryker to break execution when at least one test failed on initial test run.</summary>
    [Argument(Format = "--break-on-initial-test-failure")] public bool? BreakOnInitialTestFailure => Get<bool?>(() => BreakOnInitialTestFailure);
    /// <summary>Enabling with-baseline saves the mutation report to a storage location such as the filesystem. The mutation report is loaded at the start of the next mutation run. Any changed source code or unit test results in a reset of the mutants affected by the change. For unchanged mutants the previous result is reused. This feature expands on the since feature by providing you with a full report after a partial mutation testrun.</summary>
    [Argument(Format = "--with-baseline:{value}")] public string WithBaseline => Get<string>(() => WithBaseline);
}
#endregion
#region StrykerSettingsExtensions
/// <inheritdoc cref="StrykerTasks.Stryker(Candoumbe.Pipelines.Tools.StrykerSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class StrykerSettingsExtensions
{
    #region ConfigFile
    /// <inheritdoc cref="StrykerSettings.ConfigFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ConfigFile))]
    public static T SetConfigFile<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.ConfigFile, v));
    /// <inheritdoc cref="StrykerSettings.ConfigFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ConfigFile))]
    public static T ResetConfigFile<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.ConfigFile));
    #endregion
    #region Solution
    /// <inheritdoc cref="StrykerSettings.Solution"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Solution))]
    public static T SetSolution<T>(this T o, Nuke.Common.IO.AbsolutePath v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Solution, v));
    /// <inheritdoc cref="StrykerSettings.Solution"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Solution))]
    public static T ResetSolution<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Solution));
    #endregion
    #region Project
    /// <inheritdoc cref="StrykerSettings.Project"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Project))]
    public static T SetProject<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Project, v));
    /// <inheritdoc cref="StrykerSettings.Project"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Project))]
    public static T ResetProject<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Project));
    #endregion
    #region TestProjects
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T SetTestProjects<T>(this T o, params string[] v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.TestProjects, v));
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T SetTestProjects<T>(this T o, IEnumerable<string> v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.TestProjects, v));
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T AddTestProjects<T>(this T o, params string[] v) where T : StrykerSettings => o.Modify(b => b.AddCollection(() => o.TestProjects, v));
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T AddTestProjects<T>(this T o, IEnumerable<string> v) where T : StrykerSettings => o.Modify(b => b.AddCollection(() => o.TestProjects, v));
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T RemoveTestProjects<T>(this T o, params string[] v) where T : StrykerSettings => o.Modify(b => b.RemoveCollection(() => o.TestProjects, v));
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T RemoveTestProjects<T>(this T o, IEnumerable<string> v) where T : StrykerSettings => o.Modify(b => b.RemoveCollection(() => o.TestProjects, v));
    /// <inheritdoc cref="StrykerSettings.TestProjects"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TestProjects))]
    public static T ClearTestProjects<T>(this T o) where T : StrykerSettings => o.Modify(b => b.ClearCollection(() => o.TestProjects));
    #endregion
    #region Mutate
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T SetMutate<T>(this T o, params string[] v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Mutate, v));
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T SetMutate<T>(this T o, IEnumerable<string> v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Mutate, v));
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T AddMutate<T>(this T o, params string[] v) where T : StrykerSettings => o.Modify(b => b.AddCollection(() => o.Mutate, v));
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T AddMutate<T>(this T o, IEnumerable<string> v) where T : StrykerSettings => o.Modify(b => b.AddCollection(() => o.Mutate, v));
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T RemoveMutate<T>(this T o, params string[] v) where T : StrykerSettings => o.Modify(b => b.RemoveCollection(() => o.Mutate, v));
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T RemoveMutate<T>(this T o, IEnumerable<string> v) where T : StrykerSettings => o.Modify(b => b.RemoveCollection(() => o.Mutate, v));
    /// <inheritdoc cref="StrykerSettings.Mutate"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Mutate))]
    public static T ClearMutate<T>(this T o) where T : StrykerSettings => o.Modify(b => b.ClearCollection(() => o.Mutate));
    #endregion
    #region Configuration
    /// <inheritdoc cref="StrykerSettings.Configuration"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Configuration))]
    public static T SetConfiguration<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Configuration, v));
    /// <inheritdoc cref="StrykerSettings.Configuration"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Configuration))]
    public static T ResetConfiguration<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Configuration));
    #endregion
    #region TargetFramework
    /// <inheritdoc cref="StrykerSettings.TargetFramework"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TargetFramework))]
    public static T SetTargetFramework<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.TargetFramework, v));
    /// <inheritdoc cref="StrykerSettings.TargetFramework"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.TargetFramework))]
    public static T ResetTargetFramework<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.TargetFramework));
    #endregion
    #region ProjectInfoVersion
    /// <inheritdoc cref="StrykerSettings.ProjectInfoVersion"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ProjectInfoVersion))]
    public static T SetProjectInfoVersion<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.ProjectInfoVersion, v));
    /// <inheritdoc cref="StrykerSettings.ProjectInfoVersion"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ProjectInfoVersion))]
    public static T ResetProjectInfoVersion<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.ProjectInfoVersion));
    #endregion
    #region MutationLevel
    /// <inheritdoc cref="StrykerSettings.MutationLevel"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.MutationLevel))]
    public static T SetMutationLevel<T>(this T o, StrykerMutationLevel v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.MutationLevel, v));
    /// <inheritdoc cref="StrykerSettings.MutationLevel"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.MutationLevel))]
    public static T ResetMutationLevel<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.MutationLevel));
    #endregion
    #region Reporters
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T SetReporters<T>(this T o, params StrykerReporter[] v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Reporters, v));
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T SetReporters<T>(this T o, IEnumerable<StrykerReporter> v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Reporters, v));
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T AddReporters<T>(this T o, params StrykerReporter[] v) where T : StrykerSettings => o.Modify(b => b.AddCollection(() => o.Reporters, v));
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T AddReporters<T>(this T o, IEnumerable<StrykerReporter> v) where T : StrykerSettings => o.Modify(b => b.AddCollection(() => o.Reporters, v));
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T RemoveReporters<T>(this T o, params StrykerReporter[] v) where T : StrykerSettings => o.Modify(b => b.RemoveCollection(() => o.Reporters, v));
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T RemoveReporters<T>(this T o, IEnumerable<StrykerReporter> v) where T : StrykerSettings => o.Modify(b => b.RemoveCollection(() => o.Reporters, v));
    /// <inheritdoc cref="StrykerSettings.Reporters"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Reporters))]
    public static T ClearReporters<T>(this T o) where T : StrykerSettings => o.Modify(b => b.ClearCollection(() => o.Reporters));
    #endregion
    #region OpenReport
    /// <inheritdoc cref="StrykerSettings.OpenReport"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.OpenReport))]
    public static T SetOpenReport<T>(this T o, StrykerOpenReport v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.OpenReport, v));
    /// <inheritdoc cref="StrykerSettings.OpenReport"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.OpenReport))]
    public static T ResetOpenReport<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.OpenReport));
    #endregion
    #region Concurrency
    /// <inheritdoc cref="StrykerSettings.Concurrency"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Concurrency))]
    public static T SetConcurrency<T>(this T o, uint? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Concurrency, v));
    /// <inheritdoc cref="StrykerSettings.Concurrency"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Concurrency))]
    public static T ResetConcurrency<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Concurrency));
    #endregion
    #region BreakAt
    /// <inheritdoc cref="StrykerSettings.BreakAt"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakAt))]
    public static T SetBreakAt<T>(this T o, uint? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.BreakAt, v));
    /// <inheritdoc cref="StrykerSettings.BreakAt"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakAt))]
    public static T ResetBreakAt<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.BreakAt));
    #endregion
    #region ThresholdHigh
    /// <inheritdoc cref="StrykerSettings.ThresholdHigh"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ThresholdHigh))]
    public static T SetThresholdHigh<T>(this T o, short? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.ThresholdHigh, v));
    /// <inheritdoc cref="StrykerSettings.ThresholdHigh"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ThresholdHigh))]
    public static T ResetThresholdHigh<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.ThresholdHigh));
    #endregion
    #region ThresholdLow
    /// <inheritdoc cref="StrykerSettings.ThresholdLow"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ThresholdLow))]
    public static T SetThresholdLow<T>(this T o, short? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.ThresholdLow, v));
    /// <inheritdoc cref="StrykerSettings.ThresholdLow"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.ThresholdLow))]
    public static T ResetThresholdLow<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.ThresholdLow));
    #endregion
    #region Output
    /// <inheritdoc cref="StrykerSettings.Output"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Output))]
    public static T SetOutput<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Output, v));
    /// <inheritdoc cref="StrykerSettings.Output"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Output))]
    public static T ResetOutput<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Output));
    #endregion
    #region DisableBail
    /// <inheritdoc cref="StrykerSettings.DisableBail"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DisableBail))]
    public static T SetDisableBail<T>(this T o, bool? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DisableBail, v));
    /// <inheritdoc cref="StrykerSettings.DisableBail"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DisableBail))]
    public static T ResetDisableBail<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.DisableBail));
    /// <inheritdoc cref="StrykerSettings.DisableBail"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DisableBail))]
    public static T EnableDisableBail<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DisableBail, true));
    /// <inheritdoc cref="StrykerSettings.DisableBail"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DisableBail))]
    public static T DisableDisableBail<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DisableBail, false));
    /// <inheritdoc cref="StrykerSettings.DisableBail"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DisableBail))]
    public static T ToggleDisableBail<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DisableBail, !o.DisableBail));
    #endregion
    #region Since
    /// <inheritdoc cref="StrykerSettings.Since"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Since))]
    public static T SetSince<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Since, v));
    /// <inheritdoc cref="StrykerSettings.Since"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Since))]
    public static T ResetSince<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Since));
    #endregion
    #region Verbosity
    /// <inheritdoc cref="StrykerSettings.Verbosity"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Verbosity))]
    public static T SetVerbosity<T>(this T o, StrykerVerbosity v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.Verbosity, v));
    /// <inheritdoc cref="StrykerSettings.Verbosity"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.Verbosity))]
    public static T ResetVerbosity<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.Verbosity));
    #endregion
    #region LogToFile
    /// <inheritdoc cref="StrykerSettings.LogToFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.LogToFile))]
    public static T SetLogToFile<T>(this T o, bool? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.LogToFile, v));
    /// <inheritdoc cref="StrykerSettings.LogToFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.LogToFile))]
    public static T ResetLogToFile<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.LogToFile));
    /// <inheritdoc cref="StrykerSettings.LogToFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.LogToFile))]
    public static T EnableLogToFile<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.LogToFile, true));
    /// <inheritdoc cref="StrykerSettings.LogToFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.LogToFile))]
    public static T DisableLogToFile<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.LogToFile, false));
    /// <inheritdoc cref="StrykerSettings.LogToFile"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.LogToFile))]
    public static T ToggleLogToFile<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.LogToFile, !o.LogToFile));
    #endregion
    #region DevMode
    /// <inheritdoc cref="StrykerSettings.DevMode"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DevMode))]
    public static T SetDevMode<T>(this T o, bool? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DevMode, v));
    /// <inheritdoc cref="StrykerSettings.DevMode"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DevMode))]
    public static T ResetDevMode<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.DevMode));
    /// <inheritdoc cref="StrykerSettings.DevMode"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DevMode))]
    public static T EnableDevMode<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DevMode, true));
    /// <inheritdoc cref="StrykerSettings.DevMode"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DevMode))]
    public static T DisableDevMode<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DevMode, false));
    /// <inheritdoc cref="StrykerSettings.DevMode"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DevMode))]
    public static T ToggleDevMode<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DevMode, !o.DevMode));
    #endregion
    #region DashboardApiKey
    /// <inheritdoc cref="StrykerSettings.DashboardApiKey"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DashboardApiKey))]
    public static T SetDashboardApiKey<T>(this T o, [Secret] string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.DashboardApiKey, v));
    /// <inheritdoc cref="StrykerSettings.DashboardApiKey"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.DashboardApiKey))]
    public static T ResetDashboardApiKey<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.DashboardApiKey));
    #endregion
    #region MsBuildPath
    /// <inheritdoc cref="StrykerSettings.MsBuildPath"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.MsBuildPath))]
    public static T SetMsBuildPath<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.MsBuildPath, v));
    /// <inheritdoc cref="StrykerSettings.MsBuildPath"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.MsBuildPath))]
    public static T ResetMsBuildPath<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.MsBuildPath));
    #endregion
    #region BreakOnInitialTestFailure
    /// <inheritdoc cref="StrykerSettings.BreakOnInitialTestFailure"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakOnInitialTestFailure))]
    public static T SetBreakOnInitialTestFailure<T>(this T o, bool? v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.BreakOnInitialTestFailure, v));
    /// <inheritdoc cref="StrykerSettings.BreakOnInitialTestFailure"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakOnInitialTestFailure))]
    public static T ResetBreakOnInitialTestFailure<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.BreakOnInitialTestFailure));
    /// <inheritdoc cref="StrykerSettings.BreakOnInitialTestFailure"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakOnInitialTestFailure))]
    public static T EnableBreakOnInitialTestFailure<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.BreakOnInitialTestFailure, true));
    /// <inheritdoc cref="StrykerSettings.BreakOnInitialTestFailure"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakOnInitialTestFailure))]
    public static T DisableBreakOnInitialTestFailure<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.BreakOnInitialTestFailure, false));
    /// <inheritdoc cref="StrykerSettings.BreakOnInitialTestFailure"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.BreakOnInitialTestFailure))]
    public static T ToggleBreakOnInitialTestFailure<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Set(() => o.BreakOnInitialTestFailure, !o.BreakOnInitialTestFailure));
    #endregion
    #region WithBaseline
    /// <inheritdoc cref="StrykerSettings.WithBaseline"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.WithBaseline))]
    public static T SetWithBaseline<T>(this T o, string v) where T : StrykerSettings => o.Modify(b => b.Set(() => o.WithBaseline, v));
    /// <inheritdoc cref="StrykerSettings.WithBaseline"/>
    [Pure] [Builder(Type = typeof(StrykerSettings), Property = nameof(StrykerSettings.WithBaseline))]
    public static T ResetWithBaseline<T>(this T o) where T : StrykerSettings => o.Modify(b => b.Remove(() => o.WithBaseline));
    #endregion
}
#endregion
#region StrykerMutationLevel
/// <summary>Used within <see cref="StrykerTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<StrykerMutationLevel>))]
public partial class StrykerMutationLevel : Enumeration
{
    public static StrykerMutationLevel Basic = (StrykerMutationLevel) "Basic";
    public static StrykerMutationLevel Standard = (StrykerMutationLevel) "Standard";
    public static StrykerMutationLevel Advanced = (StrykerMutationLevel) "Advanced";
    public static StrykerMutationLevel Complete = (StrykerMutationLevel) "Complete";
    public static implicit operator StrykerMutationLevel(string value)
    {
        return new StrykerMutationLevel { Value = value };
    }
}
#endregion
#region StrykerReporter
/// <summary>Used within <see cref="StrykerTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<StrykerReporter>))]
public partial class StrykerReporter : Enumeration
{
    public static StrykerReporter All = (StrykerReporter) "All";
    public static StrykerReporter Html = (StrykerReporter) "Html";
    public static StrykerReporter Progress = (StrykerReporter) "Progress";
    public static StrykerReporter Dashboard = (StrykerReporter) "Dashboard";
    public static StrykerReporter Cleartext = (StrykerReporter) "Cleartext";
    public static StrykerReporter CleartextTree = (StrykerReporter) "CleartextTree";
    public static StrykerReporter Dots = (StrykerReporter) "Dots";
    public static StrykerReporter Json = (StrykerReporter) "Json";
    public static implicit operator StrykerReporter(string value)
    {
        return new StrykerReporter { Value = value };
    }
}
#endregion
#region StrykerOpenReport
/// <summary>Used within <see cref="StrykerTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<StrykerOpenReport>))]
public partial class StrykerOpenReport : Enumeration
{
    public static StrykerOpenReport Html = (StrykerOpenReport) "Html";
    public static StrykerOpenReport Dashboard = (StrykerOpenReport) "Dashboard";
    public static implicit operator StrykerOpenReport(string value)
    {
        return new StrykerOpenReport { Value = value };
    }
}
#endregion
#region StrykerVerbosity
/// <summary>Used within <see cref="StrykerTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<StrykerVerbosity>))]
public partial class StrykerVerbosity : Enumeration
{
    public static StrykerVerbosity Error = (StrykerVerbosity) "Error";
    public static StrykerVerbosity Warning = (StrykerVerbosity) "Warning";
    public static StrykerVerbosity Info = (StrykerVerbosity) "Info";
    public static StrykerVerbosity Debug = (StrykerVerbosity) "Debug";
    public static StrykerVerbosity Trace = (StrykerVerbosity) "Trace";
    public static implicit operator StrykerVerbosity(string value)
    {
        return new StrykerVerbosity { Value = value };
    }
}
#endregion
