using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveReportTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingReportDirectory_Then_ReturnsReportsUnderArtifactsDirectory()
    {
        // Arrange
        IHaveReport sut = new ReportBuild();

        // Act
        AbsolutePath actual = sut.ReportDirectory;

        // Assert
        actual.Should().Be(sut.ArtifactsDirectory / "reports");
    }
}
