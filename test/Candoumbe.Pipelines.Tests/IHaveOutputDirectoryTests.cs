using Candoumbe.Pipelines.Components;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveOutputDirectoryTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingOutputDirectoryName_Then_ReturnsOutput()
    {
        // Arrange
        IHaveOutputDirectory sut = new OutputDirectoryBuild();

        // Act
        string actual = sut.OutputDirectoryName;

        // Assert
        actual.Should().Be("output");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingOutputDirectory_Then_ReturnsOutputDirectoryNameUnderRootDirectory()
    {
        // Arrange
        IHaveOutputDirectory sut = new OutputDirectoryBuild();

        // Act
        AbsolutePath actual = sut.OutputDirectory;

        // Assert
        actual.Should().Be(sut.RootDirectory / sut.OutputDirectoryName);
    }
}
