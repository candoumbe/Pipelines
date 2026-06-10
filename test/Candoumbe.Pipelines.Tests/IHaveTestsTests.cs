using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IHaveTestsTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingTestResultDirectory_Then_ReturnsTestsResultsUnderArtifactsDirectory()
    {
        // Arrange
        IHaveTests sut = new TestResultsBuild();

        // Act
        AbsolutePath actual = sut.TestResultDirectory;

        // Assert
        actual.Should().Be(sut.ArtifactsDirectory / "tests-results");
    }

    [Fact]
    public void Given_TestResultDirectoryNameConstant_When_AccessingValue_Then_ReturnsTestsResults()
    {
        // Arrange
        string expected = "tests-results";

        // Act
        string actual = IHaveTests.TestResultDirectoryName;

        // Assert
        actual.Should().Be(expected);
    }
}
