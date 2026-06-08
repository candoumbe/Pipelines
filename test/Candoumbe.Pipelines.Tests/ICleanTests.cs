using System.Collections.Generic;
using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Fallout.Common.IO;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class ICleanTests
{
    [Fact]
    public void Given_DefaultImplementation_When_AccessingDirectoriesToDelete_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IClean sut = new CleanBuild();

        // Act
        IEnumerable<AbsolutePath> actual = sut.DirectoriesToDelete;

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingDirectoriesToClean_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IClean sut = new CleanBuild();

        // Act
        IEnumerable<AbsolutePath> actual = sut.DirectoriesToClean;

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Given_DefaultImplementation_When_AccessingDirectoriesToEnsureExistence_Then_ReturnsEmptyCollection()
    {
        // Arrange
        IClean sut = new CleanBuild();

        // Act
        IEnumerable<AbsolutePath> actual = sut.DirectoriesToEnsureExistence;

        // Assert
        actual.Should().BeEmpty();
    }
}
