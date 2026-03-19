using Candoumbe.Pipelines.Components.Formatting;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class DotNetFormatterTests
{
    [Fact]
    public void Given_AnalyzersFormatter_When_ConvertingToString_Then_ReturnsAnalyzers()
    {
        // Arrange
        DotNetFormatter formatter = DotNetFormatter.Analyzers;

        // Act
        string actual = formatter;

        // Assert
        actual.Should().Be("Analyzers");
    }

    [Fact]
    public void Given_StyleFormatter_When_ConvertingToString_Then_ReturnsStyle()
    {
        // Arrange
        DotNetFormatter formatter = DotNetFormatter.Style;

        // Act
        string actual = formatter;

        // Assert
        actual.Should().Be("Style");
    }

    [Fact]
    public void Given_WhitespaceFormatter_When_ConvertingToString_Then_ReturnsWhitespace()
    {
        // Arrange
        DotNetFormatter formatter = DotNetFormatter.Whitespace;

        // Act
        string actual = formatter;

        // Assert
        actual.Should().Be("Whitespace");
    }
}
