using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveArtifactsTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingArtifactsDirectory_Then_ReturnsArtifactsUnderOutputDirectory()
    {
        // Arrange
        IHaveArtifacts sut = new ArtifactsBuild();

        // Act
        AbsolutePath actual = sut.ArtifactsDirectory;

        // Assert
        actual.Should().Be(sut.OutputDirectory / "artifacts");
    }
}
