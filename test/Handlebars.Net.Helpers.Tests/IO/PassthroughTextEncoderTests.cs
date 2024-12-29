using System.Globalization;
using System.IO;
using System.Text;
using FluentAssertions;
using HandlebarsDotNet.Helpers.IO;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.IO;

public class PassthroughTextEncoderTests
{
    private readonly PassthroughTextEncoder _sut = new();

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
        context.Configuration.TextEncoder = _sut;

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

    [Fact]
    public void Encode_Enumerator_WritesToTarget()
    {
        // Arrange
        using var text = "Hello, World!".GetEnumerator();
        using var target = new StringWriter();

        // Act
        _sut.Encode(text, target);

        // Assert
        target.ToString().Should().Be("Hello, World!");
    }

    [Fact]
    public void Encode_String_WritesToTarget()
    {
        // Arrange
        const string text = "Hello, World!";
        using var target = new StringWriter();

        // Act
        _sut.Encode(text, target);

        // Assert
        target.ToString().Should().Be("Hello, World!");
    }

    [Fact]
    public void Encode_StringBuilder_WritesToTarget()
    {
        // Arrange
        var text = new StringBuilder("Hello, World!");
        using var target = new StringWriter();

        // Act
        _sut.Encode(text, target);

        // Assert
        target.ToString().Should().Be("Hello, World!");
    }
}