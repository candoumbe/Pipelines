using Candoumbe.Pipelines.Components;
using Nuke.Common;

namespace Candoumbe.Pipelines.Tests;

/// <summary>
/// Build stub that implements all the component interfaces for testing default implementations.
/// Inherits from <see cref="NukeBuild"/> to get all the internal <see cref="INukeBuild"/> members.
/// </summary>
internal class SourceDirectoryBuild : NukeBuild, IHaveSourceDirectory;

internal class OutputDirectoryBuild : NukeBuild, IHaveOutputDirectory;

internal class ArtifactsBuild : NukeBuild, IHaveArtifacts;

internal class TestDirectoryBuild : NukeBuild, IHaveTestDirectory;

internal class TestResultsBuild : NukeBuild, IHaveTests;

internal class ReportBuild : NukeBuild, IHaveReport;

internal class CoverageBuild : NukeBuild, IHaveCoverage;

internal class ChangeLogBuild : NukeBuild, IHaveChangeLog;

internal class CleanBuild : NukeBuild, IClean;

internal class PackBuild : NukeBuild, IPack
{
    public System.Collections.Generic.IEnumerable<Nuke.Common.IO.AbsolutePath> PackableProjects => [];
}

internal class UnitTestBuild : NukeBuild, IUnitTest
{
    public System.Collections.Generic.IEnumerable<Nuke.Common.ProjectModel.Project> UnitTestsProjects => [];
}

internal class IntegrationTestBuild : NukeBuild, IIntegrationTest
{
    public System.Collections.Generic.IEnumerable<Nuke.Common.ProjectModel.Project> IntegrationTestsProjects => [];
}

internal class MutationTestBuild : NukeBuild, IMutationTest
{
    public System.Collections.Generic.IEnumerable<MutationProjectConfiguration> MutationTestsProjects => [];
}

internal class BenchmarkBuild : NukeBuild, IBenchmark
{
    public System.Collections.Generic.IEnumerable<Nuke.Common.ProjectModel.Project> BenchmarkProjects => [];
}

internal class ReportUnitTestCoverageBuild : NukeBuild, IReportUnitTestCoverage
{
    public bool ReportToCodeCov => false;
    public System.Collections.Generic.IEnumerable<Nuke.Common.ProjectModel.Project> UnitTestsProjects => [];
}

internal class ReportIntegrationTestCoverageBuild : NukeBuild, IReportIntegrationTestCoverage
{
    public bool ReportToCodeCov => false;
    public System.Collections.Generic.IEnumerable<Nuke.Common.ProjectModel.Project> IntegrationTestsProjects => [];
}
