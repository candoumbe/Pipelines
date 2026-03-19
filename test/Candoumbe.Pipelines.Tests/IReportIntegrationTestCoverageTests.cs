using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Nuke.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IReportIntegrationTestCoverageTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingIntegrationTestCoverageReportDirectory_Then_ReturnsExpectedPath()
    {
        // Arrange
        IReportIntegrationTestCoverage sut = new ReportIntegrationTestCoverageBuild();

        // Act
        AbsolutePath actual = sut.IntegrationTestCoverageReportDirectory;

        // Assert
        actual.Should().Be(sut.CoverageReportDirectory / "integration-tests-coverage");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingIntegrationTestCoverageReportHistoryDirectory_Then_ReturnsExpectedPath()
    {
        // Arrange
        IReportIntegrationTestCoverage sut = new ReportIntegrationTestCoverageBuild();

        // Act
        AbsolutePath actual = sut.IntegrationTestCoverageReportHistoryDirectory;

        // Assert
        actual.Should().Be(sut.CoverageReportHistoryDirectory / "integration-tests-coverage-history");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingCodeCoverageReportArtifactName_Then_ReturnsIntegrationTestsCoverage()
    {
        // Arrange
        IReportIntegrationTestCoverage sut = new ReportIntegrationTestCoverageBuild();

        // Act
        string actual = sut.CodeCoverageReportArtifactName;

        // Assert
        actual.Should().Be("integration-tests-coverage");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingCodeCoverageHistoryReportArtifactName_Then_ReturnsIntegrationTestsCoverageHistory()
    {
        // Arrange
        IReportIntegrationTestCoverage sut = new ReportIntegrationTestCoverageBuild();

        // Act
        string actual = sut.CodeCoverageHistoryReportArtifactName;

        // Assert
        actual.Should().Be("integration-tests-coverage-history");
    }
}
