using Nuke.Common;
using Nuke.Common.IO;

using System.Collections.Generic;
using System.Linq;

using static Nuke.Common.IO.FileSystemTasks;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that supports cleaning workflow.
/// </summary>
public interface IClean : INukeBuild
{

    /// <summary>
    /// Collection of directories to delete
    /// </summary>
    public IEnumerable<AbsolutePath> DirectoriesToDelete => Enumerable.Empty<AbsolutePath>();

    /// <summary>
    /// Collection of directories to clean
    /// </summary>
    IEnumerable<AbsolutePath> DirectoriesToClean => Enumerable.Empty<AbsolutePath>();

    /// <summary>
    /// Collection of directories for which to ensure they exist in the filesystem.
    /// </summary>
    IEnumerable<AbsolutePath> DirectoriesToEnsureExistance => Enumerable.Empty<AbsolutePath>();

    /// <summary>
    /// Performs the clean up
    /// </summary>
    public Target Clean => _ => _
       .TryBefore<IRestore>(x => x.Restore)
       .OnlyWhenDynamic(() => DirectoriesToClean.AtLeastOnce()
                              || DirectoriesToDelete.AtLeastOnce()
                              || DirectoriesToEnsureExistance.AtLeastOnce())
       .Executes(() =>
       {
           DirectoriesToDelete.ForEach(DeleteDirectory);
           DirectoriesToClean.ForEach(EnsureCleanDirectory);
           DirectoriesToEnsureExistance.ForEach(EnsureExistingDirectory);
       });
}
