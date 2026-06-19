using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class ConfigurationTests
{
    [Fact]
    public void Given_DebugConfiguration_When_ConvertingToString_Then_ReturnsDebug()
    {
        // Arrange
        Configuration configuration = Configuration.Debug;

        // Act
        string actual = configuration;

        // Assert
        actual.Should().Be("Debug");
    }

    [Fact]
    public void Given_ReleaseConfiguration_When_ConvertingToString_Then_ReturnsRelease()
    {
        // Arrange
        Configuration configuration = Configuration.Release;

        // Act
        string actual = configuration;

        // Assert
        actual.Should().Be("Release");
    }

    [Fact]
    public void Given_DebugAndRelease_When_Comparing_Then_TheyAreDifferent()
    {
        // Arrange
        Configuration debug = Configuration.Debug;
        Configuration release = Configuration.Release;

        // Act & Assert
        debug.Should().NotBe(release);
    }
}
