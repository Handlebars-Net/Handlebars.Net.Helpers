using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Models;
using HandlebarsDotNet.Helpers.Utils;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Models;

[ExcludeFromCodeCoverage]
public class StringEncoderTests
{
    [Theory]
    [InlineData("Hello, World!", "ϟЊҜߍHello, World!ߍҜЊϟ")]
    [InlineData("1234567890", "ϟЊҜߍ1234567890ߍҜЊϟ")]
    [InlineData("", "ϟЊҜߍߍҜЊϟ")]
    public void Encode(string input, string expected)
    {
        // Act
        var result = StringEncoder.Encode(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("ϟЊҜߍHello, World!ߍҜЊϟ", true, "Hello, World!")]
    [InlineData("ϟЊҜߍ1234567890ߍҜЊϟ", true, "1234567890")]
    [InlineData("Hello, World!", false, "Hello, World!")]
    [InlineData("", false, "")]
    public void TryDecode(string input, bool expectedResult, string expectedOutput)
    {
        // Act
        var result1 = StringEncoder.TryDecode(input, out var decoded1);

        // Assert
        result1.Should().Be(expectedResult);
        decoded1.Should().Be(expectedOutput);

        // Act
        var result2 = StringEncoder.TryDecode(HtmlUtils.HtmlEncode(input), out var decoded2);

        // Assert
        result2.Should().Be(expectedResult);
        decoded2.Should().Be(expectedOutput);
    }
}