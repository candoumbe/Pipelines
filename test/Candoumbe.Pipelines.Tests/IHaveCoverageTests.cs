using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveCoverageTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingCoverageReportDirectory_Then_ReturnsCoverageReportUnderReportDirectory()
    {
        // Arrange
        IHaveCoverage sut = new CoverageBuild();

        // Act
        AbsolutePath actual = sut.CoverageReportDirectory;

        // Assert
        actual.Should().Be(sut.ReportDirectory / "coverage-report");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingCoverageReportHistoryDirectory_Then_ReturnsCoverageHistoryUnderReportDirectory()
    {
        // Arrange
        IHaveCoverage sut = new CoverageBuild();

        // Act
        AbsolutePath actual = sut.CoverageReportHistoryDirectory;

        // Assert
        actual.Should().Be(sut.ReportDirectory / "coverage-history");
    }
}
