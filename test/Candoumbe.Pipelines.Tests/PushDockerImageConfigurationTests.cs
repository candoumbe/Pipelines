using System;
using Candoumbe.Pipelines.Components.Docker;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class PushDockerImageConfigurationTests
{
    [Fact]
    public void Given_RegistryProvided_When_CreatingInstance_Then_RegistryIsSet()
    {
        // Arrange
        Uri expectedRegistry = new Uri("https://registry.hub.docker.com");

        // Act
        var config = new PushDockerImageConfiguration(expectedRegistry);

        // Assert
        config.Registry.Should().Be(expectedRegistry);
    }

    [Fact]
    public void Given_NoLoginSettingsProvided_When_CreatingInstance_Then_LoginSettingsIsNull()
    {
        // Arrange & Act
        var config = new PushDockerImageConfiguration(new Uri("https://registry.hub.docker.com"));

        // Assert
        config.LoginSettings.Should().BeNull();
    }
}
