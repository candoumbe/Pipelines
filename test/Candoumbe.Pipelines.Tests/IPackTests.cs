using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Nuke.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IPackTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingPackagesDirectory_Then_ReturnsPackagesUnderArtifactsDirectory()
    {
        // Arrange
        IPack sut = new PackBuild();

        // Act
        AbsolutePath actual = sut.PackagesDirectory;

        // Assert
        actual.Should().Be(sut.ArtifactsDirectory / "packages");
    }
}
