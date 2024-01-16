using System.Collections.Generic;
using Candoumbe.Pipelines.Components.Formatting;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using static Serilog.Log;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using System.Text;
using System;
using Candoumbe.Pipelines.Components;
using Nuke.Common.Tools.DotNet;

/// <summary>
/// Represents an interface for formatting code using the dotnet-format tool.
/// </summary>
[NuGetPackageRequirement("dotnet-format")]
public interface IDotnetFormat : IFormat
{

    /// <summary>
    /// The workspace onto which the formatter will operate
    /// </summary>
    AbsolutePath Workspace => this.As<IHaveSolution>()?.Solution ?? TryGetValue(() => Workspace);

    /// <summary>
    /// Sets to <see langword="true"/> will halt the pipeline execution if the component will change any format.
    /// </summary>
    [Parameter("Sets this parameter to 'true' and the pipeline will fail if any file does not follow coding styles. (default : 'false')")]
    public bool VerifyNoChanges { get; }

    /// <summary>
    /// Sets of file to include / exclude
    /// </summary>
    (IReadOnlyCollection<AbsolutePath> IncludedFiles, IReadOnlyCollection<AbsolutePath> ExcludedFiles) Files { get; }

    ///<inheritdoc/>
    Target IFormat.Format => _ => _
        .Inherit<IFormat>()
        .Description("Applies format code style using dotnet-format tool")
        .OnlyWhenDynamic(() => Workspace is not null || Files.IncludedFiles.AtLeastOnce() || Files.ExcludedFiles.AtLeastOnce())
        .Executes(() =>
        {

            Arguments args = new();
            args.Add("{value}", Workspace);
            args.Add("--no-restore", condition: this is IRestore restore && SucceededTargets.Contains(restore.Restore));

            if (Files.IncludedFiles.Count > 0)
            {
                Arguments filesIncludedArgs = HandleIncludedFiles(Files.IncludedFiles);
                args.Add("--include")
                    .Concatenate(filesIncludedArgs);
            }

            if (Files.ExcludedFiles.Count > 0)
            {
                Arguments filesExcludedArgs = HandleExcludedFiles(Files.ExcludedFiles);
                args.Add("--exclude")
                    .Concatenate(filesExcludedArgs);
            }

            DotNet($"format {args}");

            static Arguments HandleIncludedFiles(IEnumerable<AbsolutePath> files)
            {
                Arguments args = new();

                files.ForEach((file, index) =>
                {
                    Information("'{FileName}' will be formatted", file);
                    args.Add(file);
                });

                return args;
            }

            static Arguments HandleExcludedFiles(IEnumerable<AbsolutePath> files)
            {
                Arguments args = new();

                files.ForEach((file, index) =>
                {
                    Information("'{FileName}' will not be formatted", file);
                    args.Add(file);
                });

                return args;
            }
        });
}