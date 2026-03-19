using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Nuke.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

/// <summary>
/// Tests that validate the full path hierarchy produced by composing multiple infrastructure interfaces.
/// </summary>
public class PathHierarchyTests
{
    [Fact]
    public void Given_DefaultComposition_When_AccessingTestResultDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IHaveTests sut = new TestResultsBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.TestResultDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "tests-results");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingReportDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IHaveReport sut = new ReportBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.ReportDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "reports");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingCoverageReportDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IHaveCoverage sut = new CoverageBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.CoverageReportDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "reports" / "coverage-report");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingCoverageHistoryDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IHaveCoverage sut = new CoverageBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.CoverageReportHistoryDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "reports" / "coverage-history");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingPackagesDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IPack sut = new PackBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.PackagesDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "packages");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingUnitTestResultsDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IUnitTest sut = new UnitTestBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.UnitTestResultsDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "tests-results" / "unit-tests");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingIntegrationTestResultsDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IIntegrationTest sut = new IntegrationTestBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.IntegrationTestResultsDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "tests-results" / "integration-tests");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingMutationTestResultDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IMutationTest sut = new MutationTestBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.MutationTestResultDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "tests-results" / "mutation-tests");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingBenchmarkResultDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IBenchmark sut = new BenchmarkBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.BenchmarkResultDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "benchmarks");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingUnitTestCoverageReportDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IReportUnitTestCoverage sut = new ReportUnitTestCoverageBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.UnitTestCoverageReportDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "reports" / "coverage-report" / "unit-tests-coverage");
    }

    [Fact]
    public void Given_DefaultComposition_When_AccessingIntegrationTestCoverageReportDirectory_Then_FollowsFullPathConvention()
    {
        // Arrange
        IReportIntegrationTestCoverage sut = new ReportIntegrationTestCoverageBuild();
        AbsolutePath root = sut.RootDirectory;

        // Act
        AbsolutePath actual = sut.IntegrationTestCoverageReportDirectory;

        // Assert
        actual.Should().Be(root / "output" / "artifacts" / "reports" / "coverage-report" / "integration-tests-coverage");
    }
}

