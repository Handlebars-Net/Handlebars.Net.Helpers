using System;
using System.Collections.Generic;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Models;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class RandomHelpersTests
{
    private readonly RandomHelpers _sut;

    public RandomHelpersTests()
    {
        var contextMock = new Mock<IHandlebars>();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

        _sut = new RandomHelpers(contextMock.Object);
    }

    [Fact]
    public void Random_TypeIsMissing_Should_Throw()
    {
        // Arrange
        var hash = new Dictionary<string, object?>();

        // Act
        Action action = () => _sut.Random(hash);

        // Assert
        action.Should().Throw<HandlebarsException>().Which.Message.Should().Be("The Type argument is missing.");
    }

    [Fact]
    public void Random_Integer()
    {
        // Arrange
        var hash = new Dictionary<string, object?>
        {
            { "Type", "Integer" },
            { "Min", 1000 },
            { "Max", 9999 }
        };

        // Act
        var result = _sut.Random(hash);

        // Assert
        result.Should().BeOfType<int>().Which.Should().BeInRange(1000, 9999);
    }

    [Fact]
    public void RandomAsOutputWithType_Integer()
    {
        // Arrange
        var hash = new Dictionary<string, object?>
        {
            { "Type", "Integer" },
            { "Min", 1000 },
            { "Max", 9999 }
        };

        // Act
        var result = _sut.RandomAsOutputWithType(hash);

        // Assert
        var outputWithType = OutputWithType.Deserialize(result)!;
        outputWithType.Value.Should().BeOfType<long>().Which.Should().BeInRange(1000, 9999);
        outputWithType.Type.Should().Be("Int32");
        outputWithType.FullType.Should().Be("System.Int32");
    }
}