using Candoumbe.Pipelines.Components;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class ExtensionsTests
{
    [Fact]
    public void Given_NonNullObject_When_CallingWhenNotNull_Then_ConfiguratorIsApplied()
    {
        // Arrange
        string settings = "initial";
        string obj = "value";

        // Act
        string actual = settings.WhenNotNull(obj, (s, o) => s + "-" + o);

        // Assert
        actual.Should().Be("initial-value");
    }

    [Fact]
    public void Given_NullObject_When_CallingWhenNotNull_Then_SettingsAreReturnedUnchanged()
    {
        // Arrange
        string settings = "initial";
        string obj = null;

        // Act
        string actual = settings.WhenNotNull(obj, (s, o) => s + "-" + o);

        // Assert
        actual.Should().Be("initial");
    }

    [Fact]
    public void Given_NonNullNullableInt_When_CallingWhenNotNull_Then_BothArgumentsArePassedToConfigurator()
    {
        // Arrange
        int baseValue = 10;
        int? multiplier = 3;

        // Act
        int actual = baseValue.WhenNotNull(multiplier, (b, m) => b * m.Value);

        // Assert
        actual.Should().Be(30);
    }

    [Fact]
    public void Given_NullNullableInt_When_CallingWhenNotNull_Then_ConfiguratorIsNotInvoked()
    {
        // Arrange
        int baseValue = 10;
        int? multiplier = null;
        bool configuratorCalled = false;

        // Act
        int actual = baseValue.WhenNotNull(multiplier, (b, m) =>
        {
            configuratorCalled = true;
            return b * m.Value;
        });

        // Assert
        actual.Should().Be(10);
        configuratorCalled.Should().BeFalse();
    }
}
