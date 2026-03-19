using System.Collections.Generic;
using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Nuke.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IMutationTestTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingMutationTestResultDirectory_Then_ReturnsMutationTestsUnderTestResultDirectory()
    {
        // Arrange
        IMutationTest sut = new MutationTestBuild();

        // Act
        AbsolutePath actual = sut.MutationTestResultDirectory;

        // Assert
        actual.Should().Be(sut.TestResultDirectory / "mutation-tests");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingMutationTestsProjects_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IMutationTest sut = new MutationTestBuild();

        // Act
        IEnumerable<MutationProjectConfiguration> actual = sut.MutationTestsProjects;

        // Assert
        actual.Should().BeEmpty();
    }
}
