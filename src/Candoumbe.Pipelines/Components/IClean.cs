using Nuke.Common;
using Nuke.Common.IO;

using System.Collections.Generic;
using System.Linq;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that supports cleaning workflow.
/// </summary>
public interface IClean : INukeBuild
{
    /// <summary>
    /// Collection of directories that <see cref="Clean"/> target will delete
    /// </summary>
    public IEnumerable<AbsolutePath> DirectoriesToDelete => Enumerable.Empty<AbsolutePath>();

    /// <summary>
    /// Collection of directories to clean
    /// </summary>
    IEnumerable<AbsolutePath> DirectoriesToClean => Enumerable.Empty<AbsolutePath>();

    /// <summary>
    /// Collection of directories that <see cref="Clean"/> target will make sure exist.
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
           DirectoriesToDelete.ForEach(directory => directory.DeleteDirectory());
           DirectoriesToClean.ForEach(directory => directory.CreateOrCleanDirectory());
           DirectoriesToEnsureExistance.ForEach(directory => directory.CreateDirectory());
       });
}
