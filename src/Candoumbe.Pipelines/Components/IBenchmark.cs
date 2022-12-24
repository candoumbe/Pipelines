using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using System.Collections.Generic;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Defines a directory to store benchmarks result
/// </summary>
public interface IBenchmark : ICompile, IHaveArtifacts
{
    /// <summary>
    /// Directory where to publish benchmark results.
    /// </summary>
    public AbsolutePath BenchmarkResultDirectory => ArtifactsDirectory / "benchmarks";

    /// <summary>
    /// Projects that contains benchmarks
    /// </summary>
    public IEnumerable<Project> BenchmarkProjects { get; }

    /// <summary>
    /// Runs performance tests using <see href="https://benchmarkdotnet.org">BenchmarkDotNet</see>
    /// </summary>
    public Target Benchmarks => _ => _
        .Description("Run all performance tests.")
        .DependsOn(Compile)
        //.OnlyWhenDynamic(() => IsServerBuild)
        .Produces(BenchmarkResultDirectory / "*")
        .Executes(() =>
        {
            BenchmarkProjects.ForEach(csproj =>
            {
                DotNetRun(s => s.SetConfiguration(nameof(Configuration.Release))
                                .SetProjectFile(csproj)
                                .CombineWith(csproj.GetTargetFrameworks(),
                                             (setting, framework) => setting.SetFramework(framework))
                                                                            .Apply(BenchmarksSettingsBase)
                                                                            .Apply(BenchmarksSettings));
            });
        });

    /// <summary>
    /// Configures the way performance tests are run.
    /// </summary>
    public sealed Configure<DotNetRunSettings> BenchmarksSettingsBase => _ => _
            .SetConfiguration(nameof(Configuration.Release))
            .SetProcessArgumentConfigurator(args => args.Add("-- --filter {0}", "*", customValue: true)
                                                        .Add("--artifacts {0}", BenchmarkResultDirectory)
                                                        .Add("--join"));

    /// <summary>
    /// Configures the way performance tests are run.
    /// </summary>
    public Configure<DotNetRunSettings> BenchmarksSettings => _ => _;
}