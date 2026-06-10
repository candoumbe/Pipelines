using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveChangeLogTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingChangeLogFile_Then_ReturnsChangelogMdUnderRootDirectory()
    {
        // Arrange
        IHaveChangeLog sut = new ChangeLogBuild();

        // Act
        AbsolutePath actual = sut.ChangeLogFile;

        // Assert
        actual.Should().Be(sut.RootDirectory / "CHANGELOG.md");
    }
}
