using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using System;
using System.Collections.Generic;

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
    [Parameter("Instructs the formatter to match editorconfig settings for whitespaces. (default : 'false')")]
    public bool ApplyOnlyWhitespace { get; }

    /// <summary>
    /// Sets to <see langword="true"/> will instruct to format code to match editorconfig settings for analyzers.
    /// </summary>
    [Parameter("Instructs the formatter to match editorconfig settings for analyzers. (default : 'false')")]
    public bool ApplyOnlyAnalyzers { get; }

    /// <summary>
    /// Sets to <see langword="true"/> will instruct to format code to match editorconfig settings for code style.
    /// </summary>
    [Parameter("Instructs the formatter to match editorconfig settings for code style. (default : 'false')")]
    public bool ApplyOnlyStyle { get; }

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
        .OnlyWhenDynamic(() => Workspace is not null)
        .Executes(() =>
        {
            DotNetFormatSettings dotNetFormatSettings = new();
            dotNetFormatSettings = dotNetFormatSettings.Apply(FormatSettingsBase)
                                                       .Apply(FormatSettings);
            Arguments args = new();
            args.Add("{value}", dotNetFormatSettings.Project)
                .Add("--no-restore", condition: dotNetFormatSettings.NoRestore)
                .Add("--severity {value}", dotNetFormatSettings.Severity)
                .Add("--verify-no-changes", dotNetFormatSettings.VerifyNoChanges)
                .Add("--include-generated", dotNetFormatSettings.IncludeGenerated)
                .Add("--verbosity {value}", dotNetFormatSettings.Verbosity)
                .Add("--binarylog {value}", dotNetFormatSettings.BinaryLog)
                .Add("--report {value}", dotNetFormatSettings.Report);

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

            if (ApplyOnlyAnalyzers || ApplyOnlyStyle || ApplyOnlyWhitespace)
            {
                if (ApplyOnlyAnalyzers)
                {
                    DotNet($"format analyzers {args}");
                }

                if (ApplyOnlyStyle)
                {
                    DotNet($"format style {args}");
                }

                if (ApplyOnlyWhitespace)
                {
                    DotNet($"format whitespace {args}");
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
        });
}