using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IReportUnitTestCoverageTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingUnitTestCoverageReportDirectory_Then_ReturnsExpectedPath()
    {
        // Arrange
        IReportUnitTestCoverage sut = new ReportUnitTestCoverageBuild();

        // Act
        AbsolutePath actual = sut.UnitTestCoverageReportDirectory;

        // Assert
        actual.Should().Be(sut.CoverageReportDirectory / "unit-tests-coverage");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingUnitTestCoverageReportHistoryDirectory_Then_ReturnsExpectedPath()
    {
        // Arrange
        IReportUnitTestCoverage sut = new ReportUnitTestCoverageBuild();

        // Act
        AbsolutePath actual = sut.UnitTestCoverageReportHistoryDirectory;

        // Assert
        actual.Should().Be(sut.CoverageReportHistoryDirectory / "unit-tests-coverage-history");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingCodeCoverageReportArtifactName_Then_ReturnsUnitTestsCoverage()
    {
        // Arrange
        IReportUnitTestCoverage sut = new ReportUnitTestCoverageBuild();

        // Act
        string actual = sut.CodeCoverageReportArtifactName;

        // Assert
        actual.Should().Be("unit-tests-coverage");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingCodeCoverageHistoryReportArtifactName_Then_ReturnsUnitTestsCoverageHistory()
    {
        // Arrange
        IReportUnitTestCoverage sut = new ReportUnitTestCoverageBuild();

        // Act
        string actual = sut.CodeCoverageHistoryReportArtifactName;

        // Assert
        actual.Should().Be("unit-tests-coverage-history");
    }
}
