using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.IO;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.IO;

public class PassthroughTextEncoderTests
{
    [Theory]
    [InlineData("Hello & World", "test Hello & World abc")]
    [InlineData("1 < 2 > 0", "test 1 < 2 > 0 abc")]
    [InlineData("Use \"quotes\"", "test Use \"quotes\" abc")]
    [InlineData("'single quotes'", "test 'single quotes' abc")]
    [InlineData("あ", "test あ abc")]
    [InlineData("A", "test A abc")]
    [InlineData(null, "test  abc")]
    [InlineData("", "test  abc")]
    public void UsePassthroughTextEncoder(string value, string expected)
    {
        var context = Handlebars.Create();
        context.Configuration.FormatProvider = CultureInfo.InvariantCulture;
        context.Configuration.TextEncoder = new PassthroughTextEncoder();

        var model = new
        {
            x = value
        };
        var action = context.Compile("test {{x}} abc");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }
}