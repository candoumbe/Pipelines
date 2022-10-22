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
public interface IBenchmarks : ICompile, IHaveOutputDirectory
{
    /// <summary>
    /// Directory where to publish benchmark results.
    /// </summary>
    public AbsolutePath BenchmarkDirectory => OutputDirectory / "benchmarks";

    /// <summary>
    /// Projects that contains benchmarks
    /// </summary>
    public IEnumerable<Project> BenchmarkProjects { get; }

    public Target Benchmarks => _ => _
        .Description("Run all performance tests.")
        .DependsOn(Compile)
        //.OnlyWhenDynamic(() => IsServerBuild)
        .Produces(BenchmarkDirectory / "*")
        .Executes(() =>
        {
            BenchmarkProjects.ForEach(csproj =>
            {
                DotNetRun(s =>
                {
                    IReadOnlyCollection<string> frameworks = csproj.GetTargetFrameworks();
                    return s.SetConfiguration(Configuration.Release)
                            .SetProjectFile(csproj)
                            .CombineWith(frameworks, (setting, framework) => setting.SetFramework(framework))
                            .Apply(BenchmarksSettingsBase);
                });
            });
        });

    public sealed Configure<DotNetRunSettings> BenchmarksSettingsBase => _ => _
            .SetConfiguration(Configuration.Release)
            .SetProcessArgumentConfigurator(args => args.Add("-- --filter {0}", "*", customValue: true)
                                                        .Add("--artifacts {0}", BenchmarkDirectory)
                                                        .Add("--join"));

    /// <summary>
    /// Configures the way performance tests are run.
    /// </summary>
    public Configure<DotNetRunSettings> BenchmarksSettings => _ => _;
}