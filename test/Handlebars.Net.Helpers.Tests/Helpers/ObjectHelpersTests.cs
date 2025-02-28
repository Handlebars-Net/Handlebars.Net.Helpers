using CultureAwareTesting.xUnit;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class ObjectHelpersTests
{
    private readonly ObjectHelpers _sut;

    public ObjectHelpersTests()
    {
        var contextMock = new Mock<IHandlebars>();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

        _sut = new ObjectHelpers(contextMock.Object, HandlebarsHelpersOptions.Default);
    }

    [CulturedTheory("en-US")]
    [InlineData(123456, "123456")]
    [InlineData(123.456, "123.456")]
    public void ToStringWithValues(object value, string expected)
    {
        // Act
        var result = _sut.ToString(value);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToStringWithNullValue()
    {
        // Act
        var result = _sut.ToString(null);

        // Assert
        result.Should().Be(string.Empty);
    }
}