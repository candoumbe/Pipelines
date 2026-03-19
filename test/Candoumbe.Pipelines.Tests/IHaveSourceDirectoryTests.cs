using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Nuke.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveSourceDirectoryTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingSourceDirectory_Then_ReturnsSrcUnderRootDirectory()
    {
        // Arrange
        IHaveSourceDirectory sut = new SourceDirectoryBuild();

        // Act
        AbsolutePath actual = sut.SourceDirectory;

        // Assert
        actual.Should().Be(sut.RootDirectory / "src");
    }
}
