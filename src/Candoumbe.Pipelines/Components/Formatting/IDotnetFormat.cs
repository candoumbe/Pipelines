namespace Candoumbe.Pipelines.Components.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Boots;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Serilog.Log;


/// <summary>
/// Represents an interface for formatting code using the dotnet-format tool.
/// </summary>
/// <remarks>
/// By default, the format tool will target specific files depending on :
/// <list type="bullet">
///     <item>if the pipeline <strong>does</strong> implement / extend <see cref="IHaveGitRepository"/> : only modified/added files will be included by default</item>
///     <item>if the pipeline <strong>does not</strong> implement / extend  <see cref="IHaveGitRepository"/> : all files will be included.</item>
/// </list>
/// </remarks>
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
    /// Sets of formatters that dotnet-format will apply.
    /// </summary>
    [Parameter("Sets of formatters that the tool must apply")]
    DotNetFormatter[] Formatters => TryGetValue(() => Formatters) ?? [];

    /// <summary>
    /// Sets of files / directories to exclude when formatting
    /// </summary>
    IReadOnlyCollection<AbsolutePath> Exclude => [];

    private Configure<DotNetFormatSettings> FormatSettingsBase => _ => _
        .SetProject(Workspace)
        .SetNoRestore(this is IRestore restore && SucceededTargets.Contains(restore.Restore))
        .SetVerifyNoChanges(VerifyNoChanges)
        .WhenNotNull(this.As<IHaveGitRepository>(),
                     (settings, gitRepository) => settings.SetInclude(Git(arguments: "status --porcelain",
                                                                          workingDirectory: gitRepository.RootDirectory,
                                                                          logOutput: IsLocalBuild || Verbosity is not Verbosity.Normal)
                      .Where(static output => output.Text.AsSpan().TrimStart()[..2] switch
                      {
                          ['M' or 'A', _] or [_, 'M' or 'A'] => true,
                          _ => false,
                      })
                      .Select(static output => output.Text.AsSpan()[2..].TrimStart().ToString())
                      .ToArray()));

    /// <summary>
    /// Settings used to format the code
    /// </summary>
    Configure<DotNetFormatSettings> FormatSettings => _ => _;

    ///<inheritdoc/>
    Target IFormat.Format => _ => _
        .Inherit<IFormat>()
        .TryDependsOn<IRestore>()
        .TryDependentFor<ICompile>()
        .Description("Applies coding style using dotnet-format tool")
        .OnlyWhenDynamic(() => Workspace is not null || Formatters.Length > 0)
        .Executes(() =>
        {
            if (Formatters.Length > 0)
            {
                if (Formatters.Contains(DotNetFormatter.Analyzers))
                {
                    DotNetFormatSettings analyzersSettings = new DotNetFormatSettings().SetProcessAdditionalArguments("analyzers");
                    analyzersSettings = FormatSettingsBase.Invoke(analyzersSettings);
                    analyzersSettings = FormatSettings.Invoke(analyzersSettings);

                    DotNetFormat(analyzersSettings);
                }

                if (Formatters.Contains(DotNetFormatter.Style))
                {
                    DotNetFormatSettings styleSettings = new DotNetFormatSettings().SetProcessAdditionalArguments("style");
                    styleSettings = FormatSettingsBase.Invoke(styleSettings);
                    styleSettings = FormatSettings.Invoke(styleSettings);

                    DotNetFormat(styleSettings);
                }

                if (Formatters.Contains(DotNetFormatter.Whitespace))
                {
                    DotNetFormatSettings whitespaceSettings = new DotNetFormatSettings().AddProcessAdditionalArguments("whitespace");
                    whitespaceSettings = whitespaceSettings.Apply(FormatSettingsBase).Apply(FormatSettings);

                    DotNetFormat(whitespaceSettings);
                }
            }
            else
            {
                DotNetFormatSettings dotNetFormatSettings = new();
                dotNetFormatSettings = dotNetFormatSettings.Apply(FormatSettingsBase)
                    .Apply(FormatSettings);

                DotNetFormat(dotNetFormatSettings);
            }
        });
}