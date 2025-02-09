using System.Collections.Generic;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Options;
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

        _sut = new RandomHelpers(contextMock.Object, HandlebarsHelpersOptions.Default);
    }

    [Fact]
    public void Random()
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
        (result as int?).Should().BeInRange(1000, 9999);
    }
}