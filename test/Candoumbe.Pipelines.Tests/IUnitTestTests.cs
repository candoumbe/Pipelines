using System.Collections.Generic;
using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Fallout.Common.ProjectModel;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IUnitTestTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingUnitTestResultsDirectory_Then_ReturnsUnitTestsUnderTestResultDirectory()
    {
        // Arrange
        IUnitTest sut = new UnitTestBuild();

        // Act
        AbsolutePath actual = sut.UnitTestResultsDirectory;

        // Assert
        actual.Should().Be(sut.TestResultDirectory / "unit-tests");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingUnitTestsProjects_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IUnitTest sut = new UnitTestBuild();

        // Act
        IEnumerable<Project> actual = sut.UnitTestsProjects;

        // Assert
        actual.Should().BeEmpty();
    }
}
