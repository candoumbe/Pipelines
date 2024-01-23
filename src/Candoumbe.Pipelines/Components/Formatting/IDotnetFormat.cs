using System;
using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

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
    /// Sets of files / directories to include when formatting
    /// </summary>
    IReadOnlyCollection<AbsolutePath> Include => [];

    /// <summary>
    /// Sets of files / directories to exclude when formatting
    /// </summary>
    IReadOnlyCollection<AbsolutePath> Exclude => [];



    private Configure<DotNetFormatSettings> FormatSettingsBase => _ => _
        .SetProject(Workspace)
        .SetNoRestore(this is IRestore restore && SucceededTargets.Contains(restore.Restore))
        .SetVerifyNoChanges(VerifyNoChanges)
        .When(Include.Count > 0,
              settings => { Include.ForEach(f => settings = settings.AddInclude(f)); return settings; })
        .When(Exclude.Count > 0,
              settings => { Exclude.ForEach(f => settings = settings.AddExclude(f)); return settings; });

    /// <summary>
    /// Settings used to format the code
    /// </summary>
    Configure<DotNetFormatSettings> FormatSettings => _ => _;

    ///<inheritdoc/>
    Target IFormat.Format => _ => _
        .Inherit<IFormat>()
        .Description("Applies format code style using dotnet-format tool")
        .OnlyWhenDynamic(() => Workspace is not null || Include.AtLeastOnce() || Exclude.AtLeastOnce())
        .Executes(() =>
        {
            DotNetFormat(s => s.Apply(FormatSettingsBase)
                               .Apply(FormatSettings));
        });
}