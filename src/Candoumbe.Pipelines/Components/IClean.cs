using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.IO.FileSystemTasks;

namespace Candoumbe.Pipelines.Components;

public interface IClean : INukeBuild
{
    /// <summary>
    /// Cleans 
    /// </summary>
    public Target Clean => _ => _
       .TryBefore<ICompile>(x => x.Compile)
       .Executes(() =>
       {
           if (this is IHaveSourceDirectory sources)
           {
               sources.SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
           }

           if (this is IHaveTestDirectory tests)
           {
               tests.TestDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
           }

           if (this is IHaveArtifacts artifacts)
           {
               EnsureCleanDirectory(artifacts.ArtifactsDirectory);
           }

           if (this is IHaveCoverage coverage)
           {
               EnsureCleanDirectory(coverage.CoverageReportDirectory);
               EnsureExistingDirectory(coverage.CoverageReportHistoryDirectory);
           }
       });
}
