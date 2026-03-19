using Candoumbe.Pipelines.Components.Docker;
using FluentAssertions;
using Nuke.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class DockerFileTests
{
    [Fact]
    public void Given_NoTagProvided_When_CreatingDockerFile_Then_TagDefaultsToLatest()
    {
        // Arrange & Act
        var dockerFile = new DockerFile((AbsolutePath)"/app/Dockerfile", "my-image");

        // Assert
        dockerFile.Tag.Should().Be("latest");
    }

    [Fact]
    public void Given_TagProvided_When_CreatingDockerFile_Then_TagUsesProvidedValue()
    {
        // Arrange
        string expectedTag = "v1.0";

        // Act
        var dockerFile = new DockerFile((AbsolutePath)"/app/Dockerfile", "my-image", expectedTag);

        // Assert
        dockerFile.Tag.Should().Be(expectedTag);
    }

    [Fact]
    public void Given_NameProvided_When_CreatingDockerFile_Then_NameIsSet()
    {
        // Arrange
        string expectedName = "my-image";

        // Act
        var dockerFile = new DockerFile((AbsolutePath)"/app/Dockerfile", expectedName);

        // Assert
        dockerFile.Name.Should().Be(expectedName);
    }

    [Fact]
    public void Given_PathProvided_When_CreatingDockerFile_Then_PathIsSet()
    {
        // Arrange
        AbsolutePath expectedPath = (AbsolutePath)"/app/Dockerfile";

        // Act
        var dockerFile = new DockerFile(expectedPath, "my-image");

        // Assert
        dockerFile.Path.Should().Be(expectedPath);
    }
}
