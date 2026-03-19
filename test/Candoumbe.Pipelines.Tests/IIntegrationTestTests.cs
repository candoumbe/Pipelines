using System.Collections.Generic;
using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IIntegrationTestTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingIntegrationTestResultsDirectory_Then_ReturnsIntegrationTestsUnderTestResultDirectory()
    {
        // Arrange
        IIntegrationTest sut = new IntegrationTestBuild();

        // Act
        AbsolutePath actual = sut.IntegrationTestResultsDirectory;

        // Assert
        actual.Should().Be(sut.TestResultDirectory / "integration-tests");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingIntegrationTestsProjects_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IIntegrationTest sut = new IntegrationTestBuild();

        // Act
        IEnumerable<Project> actual = sut.IntegrationTestsProjects;

        // Assert
        actual.Should().BeEmpty();
    }
}
