using System;
using Candoumbe.Pipelines.Components.NuGet;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class NugetPushConfigurationTests
{
    [Fact]
    public void Given_StringSourceConstructor_When_CreatingInstance_Then_NameIsNuget()
    {
        // Arrange & Act
        NugetPushConfiguration config = new ("api-key", "https://api.nuget.org/v3/index.json");

        // Assert
        config.Name.Should().Be("Nuget");
    }

    [Fact]
    public void Given_NoCanBeUsedProvided_When_CreatingInstance_Then_CanBeUsedReturnsTrue()
    {
        // Arrange & Act
        NugetPushConfiguration config = new ("api-key", "https://api.nuget.org/v3/index.json");

        // Assert
        config.CanBeUsed().Should().BeTrue();
    }

    [Fact]
    public void Given_UriSourceConstructor_When_CreatingInstance_Then_SourceMatchesUri()
    {
        // Arrange
        Uri uri = new Uri("https://api.nuget.org/v3/index.json");

        // Act
        NugetPushConfiguration config = new ("api-key", uri);

        // Assert
        config.Source.Should().Be(uri.ToString());
    }

    [Fact]
    public void Given_StringSourceConstructor_When_CreatingInstance_Then_SourceMatchesString()
    {
        // Arrange
        string source = "https://api.nuget.org/v3/index.json";

        // Act
        NugetPushConfiguration config = new ("api-key", source);

        // Assert
        config.Source.Should().Be(source);
    }

    [Fact]
    public void Given_CanBeUsedReturnsFalse_When_CreatingInstance_Then_CanBeUsedReturnsFalse()
    {
        // Arrange & Act
        NugetPushConfiguration config = new ("api-key", "https://api.nuget.org/v3/index.json", () => false);

        // Assert
        config.CanBeUsed().Should().BeFalse();
    }

    [Fact]
    public void Given_ApiKeyProvided_When_CreatingInstance_Then_KeyIsSet()
    {
        // Arrange
        string expectedKey = "my-api-key";

        // Act
        NugetPushConfiguration config = new (expectedKey, "https://api.nuget.org/v3/index.json");

        // Assert
        config.Key.Should().Be(expectedKey);
    }
}
