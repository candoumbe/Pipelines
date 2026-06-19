using System;
using Candoumbe.Pipelines.Components.NuGet;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class GitHubPushNugetConfigurationTests
{
    [Fact]
    public void Given_ValidArguments_When_CreatingInstance_Then_NameIsGitHub()
    {
        // Arrange & Act
        GitHubPushNugetConfiguration config = new ("github-token", new Uri("https://nuget.pkg.github.com/owner/index.json"));

        // Assert
        config.Name.Should().Be("GitHub");
    }

    [Fact]
    public void Given_NoCanBeUsedProvided_When_CreatingInstance_Then_CanBeUsedReturnsTrue()
    {
        // Arrange & Act
        GitHubPushNugetConfiguration config = new ("github-token", new Uri("https://nuget.pkg.github.com/owner/index.json"));

        // Assert
        config.CanBeUsed().Should().BeTrue();
    }

    [Fact]
    public void Given_CanBeUsedReturnsFalse_When_CreatingInstance_Then_CanBeUsedReturnsFalse()
    {
        // Arrange & Act
        GitHubPushNugetConfiguration config = new ("github-token", new Uri("https://nuget.pkg.github.com/owner/index.json"), () => false);

        // Assert
        config.CanBeUsed().Should().BeFalse();
    }

    [Fact]
    public void Given_UriProvided_When_CreatingInstance_Then_SourceMatchesUri()
    {
        // Arrange
        Uri uri = new Uri("https://nuget.pkg.github.com/owner/index.json");

        // Act
        GitHubPushNugetConfiguration config = new ("github-token", uri);

        // Assert
        config.Source.Should().Be(uri.ToString());
    }

    [Fact]
    public void Given_GitHubTokenProvided_When_CreatingInstance_Then_KeyIsSet()
    {
        // Arrange
        string expectedToken = "my-github-token";

        // Act
        GitHubPushNugetConfiguration config = new (expectedToken, new Uri("https://nuget.pkg.github.com/owner/index.json"));

        // Assert
        config.Key.Should().Be(expectedToken);
    }
}
