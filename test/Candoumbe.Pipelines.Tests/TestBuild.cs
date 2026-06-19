using Candoumbe.Pipelines.Components;
using Fallout.Common;

namespace Candoumbe.Pipelines.Tests;

/// <summary>
/// Build stub that implements all the component interfaces for testing default implementations.
/// Inherits from <see cref="FalloutBuild"/> to get all the internal <see cref="IFalloutBuild"/> members.
/// </summary>
internal class SourceDirectoryBuild : FalloutBuild, IHaveSourceDirectory;

internal class OutputDirectoryBuild : FalloutBuild, IHaveOutputDirectory;

internal class ArtifactsBuild : FalloutBuild, IHaveArtifacts;

internal class TestDirectoryBuild : FalloutBuild, IHaveTestDirectory;

internal class TestResultsBuild : FalloutBuild, IHaveTests;

internal class ReportBuild : FalloutBuild, IHaveReport;

internal class CoverageBuild : FalloutBuild, IHaveCoverage;

internal class ChangeLogBuild : FalloutBuild, IHaveChangeLog;

internal class CleanBuild : FalloutBuild, IClean;

internal class PackBuild : FalloutBuild, IPack
{
    public System.Collections.Generic.IEnumerable<Fallout.Common.IO.AbsolutePath> PackableProjects => [];
}

internal class UnitTestBuild : FalloutBuild, IUnitTest
{
    public System.Collections.Generic.IEnumerable<Fallout.Common.ProjectModel.Project> UnitTestsProjects => [];
}

internal class IntegrationTestBuild : FalloutBuild, IIntegrationTest
{
    public System.Collections.Generic.IEnumerable<Fallout.Common.ProjectModel.Project> IntegrationTestsProjects => [];
}

internal class MutationTestBuild : FalloutBuild, IMutationTest
{
    public System.Collections.Generic.IEnumerable<MutationProjectConfiguration> MutationTestsProjects => [];
}

internal class BenchmarkBuild : FalloutBuild, IBenchmark
{
    public System.Collections.Generic.IEnumerable<Fallout.Common.ProjectModel.Project> BenchmarkProjects => [];
}

internal class ReportUnitTestCoverageBuild : FalloutBuild, IReportUnitTestCoverage
{
    public bool ReportToCodeCov => false;
    public System.Collections.Generic.IEnumerable<Fallout.Common.ProjectModel.Project> UnitTestsProjects => [];
}

internal class ReportIntegrationTestCoverageBuild : FalloutBuild, IReportIntegrationTestCoverage
{
    public bool ReportToCodeCov => false;
    public System.Collections.Generic.IEnumerable<Fallout.Common.ProjectModel.Project> IntegrationTestsProjects => [];
}
