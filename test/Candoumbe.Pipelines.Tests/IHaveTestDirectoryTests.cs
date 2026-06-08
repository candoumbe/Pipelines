using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveTestDirectoryTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingTestDirectory_Then_ReturnsTestUnderRootDirectory()
    {
        // Arrange
        IHaveTestDirectory sut = new TestDirectoryBuild();

        // Act
        AbsolutePath actual = sut.TestDirectory;

        // Assert
        actual.Should().Be(sut.RootDirectory / "test");
    }
}
