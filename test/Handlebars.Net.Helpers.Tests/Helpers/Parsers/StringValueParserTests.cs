using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Parsers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers.Parsers;

public class StringValueParserTests
{
    private readonly Mock<IHandlebars> _handlebarsContext;

    public StringValueParserTests()
    {
        var configuration = new HandlebarsConfiguration
        {
            FormatProvider = CultureInfo.InvariantCulture
        };

        _handlebarsContext = new Mock<IHandlebars>();
        _handlebarsContext.SetupGet(c => c.Configuration).Returns(configuration);
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("-1", -1)]
    [InlineData("2147483647", int.MaxValue)]
    [InlineData("-2147483648", int.MinValue)]
    public void Parse_ShouldReturnInt_WhenInputIsInt(string input, int expected)
    {
        var result = StringValueParser.Parse(_handlebarsContext.Object, input);
        result.Should().BeOfType<int>().And.Be(expected);
    }

    [Theory]
    [InlineData("9223372036854775807", long.MaxValue)]
    [InlineData("-9223372036854775808", long.MinValue)]
    public void Parse_ShouldReturnLong_WhenInputIsLong(string input, long expected)
    {
        var result = StringValueParser.Parse(_handlebarsContext.Object, input);
        result.Should().BeOfType<long>().And.Be(expected);
    }

    [Theory]
    [InlineData("123.456", 123.456)]
    [InlineData("0.0001", 0.0001)]
    [InlineData("-1.42", -1.42)]
    public void Parse_ShouldReturnDouble_WhenInputIsDouble(string input, double expected)
    {
        var result = StringValueParser.Parse(_handlebarsContext.Object, input);
        result.Should().BeOfType<double>().And.Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("123test")]
    public void Parse_ShouldReturnString_WhenInputIsNonNumericString(string input)
    {
        var result = StringValueParser.Parse(_handlebarsContext.Object, input);
        result.Should().BeOfType<string>().And.Be(input);
    }
}