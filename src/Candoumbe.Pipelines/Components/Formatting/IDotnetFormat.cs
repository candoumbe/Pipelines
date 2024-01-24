using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

namespace Candoumbe.Pipelines.Components.Formatting;

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
    /// Sets to <see langword="true"/> will instruct to format code to match editorconfig settings for whitespaces.
    /// </summary>
    [Parameter("Sets of formatters that the tool must apply")]
    public DotNetFormatter[] Formatters => TryGetValue(() => Formatters) ?? [DotNetFormatter.Whitespace, DotNetFormatter.Style, DotNetFormatter.Analyzers];

    /// <summary>
    /// Sets of files / directories to exclude when formatting
    /// </summary>
    IReadOnlyCollection<AbsolutePath> Exclude => [];

    private Configure<DotNetFormatSettings> FormatSettingsBase => _ => _
        .SetProject(Workspace)
        .SetNoRestore(this is IRestore restore && SucceededTargets.Contains(restore.Restore))
        .SetVerifyNoChanges(VerifyNoChanges);

    /// <summary>
    /// Settings used to format the code
    /// </summary>
    Configure<DotNetFormatSettings> FormatSettings => _ => _;

    ///<inheritdoc/>
    Target IFormat.Format => _ => _
        .Inherit<IFormat>()
        .Description("Applies coding style using dotnet-format tool")
        .OnlyWhenDynamic(() => Workspace is not null && Formatters.Length > 0)
        .Executes(() =>
        {
            DotNetFormatSettings dotNetFormatSettings = new();
            dotNetFormatSettings = dotNetFormatSettings.Apply(FormatSettingsBase)
                                                       .Apply(FormatSettings);
            Arguments args = new();

            if (dotNetFormatSettings.Include.Count > 0)
            {
                Arguments filesIncludedArgs = HandleIncludedFiles(dotNetFormatSettings.Include);
                args.Add("--include")
                    .Concatenate(filesIncludedArgs);
            }

            if (dotNetFormatSettings.Exclude.Count > 0)
            {
                Arguments filesExcludedArgs = HandleExcludedFiles(dotNetFormatSettings.Exclude);
                args.Add("--exclude")
                    .Concatenate(filesExcludedArgs);
            }

            if (Formatters.Length > 0)
            {
                if (Formatters.Contains(DotNetFormatter.Analyzers))
                {
                    Arguments formatAnalyzersArgs = MapArgumentsToDotNetFormatsettings(dotNetFormatSettings);
                    DotNet($"format analyzers {formatAnalyzersArgs.Concatenate(args).Add("--severity {value}", dotNetFormatSettings.Severity)}");
                }

                if (Formatters.Contains(DotNetFormatter.Style))
                {
                    Arguments formatStyleArgs = MapArgumentsToDotNetFormatsettings(dotNetFormatSettings);
                    DotNet($"format style {formatStyleArgs.Concatenate(args).Add("--severity {value}", dotNetFormatSettings.Severity)}");
                }

                if (Formatters.Contains(DotNetFormatter.Whitespace))
                {
                    Arguments formatWhitespaceArgs = MapArgumentsToDotNetFormatsettings(dotNetFormatSettings);
                    DotNet($"format whitespace {formatWhitespaceArgs}");
                }
            }
            else
            {
                DotNetFormat(dotNetFormatSettings);
            }

            static Arguments HandleIncludedFiles(IEnumerable<string> files)
            {
                Arguments args = new();

                files.ForEach((file) =>
                {
                    Verbose("'{FileName}' will be formatted", file);
                    args.Add(file);
                });

                return args;
            }

            static Arguments HandleExcludedFiles(IEnumerable<string> files)
            {
                Arguments args = new();

                files.ForEach((file) =>
                {
                    Verbose("'{FileName}' will not be formatted", file);
                    args.Add(file);
                });

                return args;
            }

            static Arguments MapArgumentsToDotNetFormatsettings(DotNetFormatSettings dotNetFormatSettings)
            {
                Arguments args = new();
                args.Add("{value}", dotNetFormatSettings.Project)
                    .Add("--no-restore", condition: dotNetFormatSettings.NoRestore)
                    .Add("--verify-no-changes", dotNetFormatSettings.VerifyNoChanges)
                    .Add("--include-generated", dotNetFormatSettings.IncludeGenerated)
                    .Add("--verbosity {value}", dotNetFormatSettings.Verbosity)
                    .Add("--binarylog {value}", dotNetFormatSettings.BinaryLog)
                    .Add("--report {value}", dotNetFormatSettings.Report);

                return args;
            }
        });
}