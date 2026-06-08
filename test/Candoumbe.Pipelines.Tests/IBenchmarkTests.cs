using System.Collections.Generic;
using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Fallout.Common.ProjectModel;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class IBenchmarkTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingBenchmarkResultDirectory_Then_ReturnsBenchmarksUnderArtifactsDirectory()
    {
        // Arrange
        IBenchmark sut = new BenchmarkBuild();

        // Act
        AbsolutePath actual = sut.BenchmarkResultDirectory;

        // Assert
        actual.Should().Be(sut.ArtifactsDirectory / "benchmarks");
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingBenchmarkProjects_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IBenchmark sut = new BenchmarkBuild();

        // Act
        IEnumerable<Project> actual = sut.BenchmarkProjects;

        // Assert
        actual.Should().BeEmpty();
    }
}
