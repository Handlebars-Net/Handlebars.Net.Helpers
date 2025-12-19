using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class StringHelpersTests
{
    private readonly StringHelpers _sut;

    public StringHelpersTests()
    {
        var contextMock = new Mock<IHandlebars>();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.InvariantCulture });

        _sut = new StringHelpers(contextMock.Object, HandlebarsHelpersOptions.Default);
    }

    [Theory]
    [InlineData("", "bar", "bar")]
    [InlineData("foo", "bar", "foobar")]
    public void Append(string value, string append, string expected)
    {
        // Act
        var result = _sut.Append(value, append);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Hello, World!", "SGVsbG8sIFdvcmxkIQ==")]
    [InlineData("Test", "VGVzdA==")]
    [InlineData("", "")]
    public void Base64Encode(string input, string expected)
    {
        // Act
        var result = StringHelpers.Base64Encode(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("SGVsbG8sIFdvcmxkIQ==", "Hello, World!")]
    [InlineData("VGVzdA==", "Test")]
    [InlineData("", "")]
    public void Base64Decode(string input, string expected)
    {
        // Act
        var result = StringHelpers.Base64Decode(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("A", "A")]
    [InlineData("foo bar", "fooBar")]
    [InlineData("FOO Bar", "fooBar")]
    public void CamelCase(string value, string expected)
    {
        // Act
        var result = _sut.Camelcase(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("foo", "Foo")]
    public void Capitalize(string value, string expected)
    {
        // Act
        var result = _sut.Capitalize(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("a", "b", "ab")]
    [InlineData("foo", "bar", "foobar")]
    public void Concat(string value1, string value2, string expected)
    {
        // Act
        var result = _sut.Concat(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "bar", false)]
    [InlineData("foo", "bar", false)]
    [InlineData("foo", "", true)]
    [InlineData("foobar", "foo", true)]
    public void Contains(string value, string test, bool expected)
    {
        // Act
        var result = _sut.Contains(value, test);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("foobar", 0, "...")]
    [InlineData("foobar", 1, "f...")]
    [InlineData("foobar", 8, "foobar")]
    public void Ellipsis(string value, int length, string expected)
    {
        // Act
        var result = _sut.Ellipsis(value, length);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Hello &amp; World", "Hello & World")]
    [InlineData("1 &lt; 2 &gt; 0", "1 < 2 > 0")]
    [InlineData("Use &quot;quotes&quot;", "Use \"quotes\"")]
    [InlineData("&#39;single quotes&#39;", "'single quotes'")]
    [InlineData("&#65;", "A")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void HtmlDecode(string input, string expected)
    {
        // Act
        var result = _sut.HtmlDecode(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Hello & World", "Hello &amp; World")]
    [InlineData("1 < 2 > 0", "1 &lt; 2 &gt; 0")]
    [InlineData("Use \"quotes\"", "Use &quot;quotes&quot;")]
    [InlineData("'single quotes'", "&#39;single quotes&#39;")]
    [InlineData("A", "A")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void HtmlEncode(string input, string expected)
    {
        // Act
        var result = _sut.HtmlEncode(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("A", "a")]
    [InlineData("foo bar", "foo bar")]
    [InlineData("FOO Bar", "foo bar")]
    public void Lowercase(string value, string expected)
    {
        // Act
        var result = _sut.Lowercase(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("foo", 10, "0", "0000000foo")]
    [InlineData("foo", 10, "padding", "pppppppfoo")]
    [InlineData("foo", 10, "", "       foo")]
    [InlineData("", 10, "0", "0000000000")]
    [InlineData("", 10, "", "          ")]
    [InlineData("foobarbaz", 6, "0", "foobarbaz")]
    public void PadLeft(string value, int totalWidth, string padChar, string expected)
    {
        var result = _sut.PadLeft(value, totalWidth, padChar);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("foo", 10, "0", "foo0000000")]
    [InlineData("foo", 10, "padding", "fooppppppp")]
    [InlineData("foo", 10, "", "foo       ")]
    [InlineData("", 10, "0", "0000000000")]
    [InlineData("", 10, "", "          ")]
    [InlineData("foobarbaz", 6, "0", "foobarbaz")]
    public void PadRight(string value, int totalWidth, string padChar, string expected)
    {
        var result = _sut.PadRight(value, totalWidth, padChar);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("foo", "", "foo")]
    [InlineData("foo", "bar", "barfoo")]
    public void Prepend(string value, string prepend, string expected)
    {
        // Act
        var result = _sut.Prepend(value, prepend);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "a", "")]
    [InlineData("a", "b", "a")]
    [InlineData("foo", "o", "f")]
    public void Remove(string value, string oldValue, string expected)
    {
        // Act
        var result = _sut.Remove(value, oldValue);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", 2, "")]
    [InlineData("a", 0, "")]
    [InlineData("a", 3, "aaa")]
    [InlineData("foo", 2, "foofoo")]
    public void Repeat(string value, int count, string expected)
    {
        // Act
        var result = _sut.Repeat(value, count);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("Bar", "raB")]
    public void Reverse(string value, string expected)
    {
        // Act
        var result = _sut.Reverse(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "a", "b", "")]
    [InlineData("Bar", "a", "?", "B?r")]
    public void Replace(string value, string oldValue, string newValue, string expected)
    {
        // Act
        var result = _sut.Replace(value, oldValue, newValue);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("a", "A")]
    [InlineData("A", "A")]
    [InlineData("foo bar", "FooBar")]
    [InlineData("FOO Bar", "FOOBar")]
    public void Pascalcase(string value, string expected)
    {
        // Act
        var result = _sut.Pascalcase(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData(" foo bar ", "foo bar")]
    public void Trim(string value, string expected)
    {
        // Act
        var result = _sut.Trim(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData("foo bar ", "foo bar")]
    public void TrimEnd(string value, string expected)
    {
        // Act
        var result = _sut.TrimEnd(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData(" foo bar ", "foo bar ")]
    public void TrimStart(string value, string expected)
    {
        // Act
        var result = _sut.TrimStart(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", 0, "")]
    [InlineData("a", 1, "a")]
    [InlineData("a", 2, "a")]
    [InlineData("foo bar", 3, "foo")]
    public void Truncate(string value, int length, string expected)
    {
        // Act
        var result = _sut.Truncate(value, length);

        // Assert
        result.Should().Be(expected);
    }


    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("a", "A")]
    [InlineData("foo bar", "FOO BAR")]
    [InlineData("FOO Bar", "FOO BAR")]
    public void Uppercase(string value, string expected)
    {
        // Act
        var result = _sut.Uppercase(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("d", "04/15/2020")]
    [InlineData("o", "2020-04-15T23:59:58.0000000")]
    [InlineData("MM-dd-yyyy", "04-15-2020")]
    public void Format_DateTime(string format, string expected)
    {
        // Arrange
        var value = new DateTime(2020, 4, 15, 23, 59, 58);

        // Act
        var result1 = _sut.Format(value, format);
        var result2 = _sut.Format((DateTime?)value, format);

        // Assert
        result1.Should().Be(expected);
        result2.Should().Be(expected);
    }

    [Theory]
    [InlineData("N0", "1")]
    [InlineData("N1", "1.2")]
    [InlineData("N2", "1.20")]
    public void Format_Decimal(string format, string expected)
    {
        // Arrange
        var value = 1.2m;

        // Act
        var result1 = _sut.Format(value, format);
        var result2 = _sut.Format((decimal?)value, format);

        // Assert
        result1.Should().Be(expected);
        result2.Should().Be(expected);
    }

    [Fact]
    public void Format_NotIFormattableType()
    {
        // Arrange
        var value = new Version(1, 2, 3, 4);

        // Act
        var result = _sut.Format(value, "should-be-ignored");

        // Assert
        result.Should().Be(value.ToString());
    }

    [Fact]
    public void Format_Null()
    {
        // Act
        var result = _sut.Format(null, "should-be-ignored");

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "bar", false)]
    [InlineData("foo", "", false)]
    [InlineData("foo", "Foo", false)]
    [InlineData("Foo", "foo", false)]
    [InlineData("foo", "foo", true)]
    [InlineData(null, null, true)]
    [InlineData(null, "foo", false)]
    [InlineData("foo", null, false)]
    public void Equal(string value, string test, bool expected)
    {
        // Act
        var equal = _sut.Equal(value, test);
        var equals = _sut.Equals(value, test);

        // Assert
        equal.Should().Be(expected);
        equals.Should().Be(expected);
    }

    [Theory]
    [InlineData("Foo", "foo", "OrdinalIgnoreCase", true)]
    [InlineData("Foo", "foo", "5", true)]
    [InlineData("Foo", "foo", 5, true)]
    public void EqualWithStringStringComparison(string value, string test, object stringComparison, bool expected)
    {
        // Act
        var equal = _sut.Equal(value, test, stringComparison);
        var equals = _sut.Equals(value, test, stringComparison);

        // Assert
        equal.Should().Be(expected);
        equals.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "bar", true)]
    [InlineData("foo", "", true)]
    [InlineData("foo", "Foo", true)]
    [InlineData("Foo", "foo", true)]
    [InlineData("foo", "foo", false)]
    [InlineData(null, null, false)]
    [InlineData(null, "foo", true)]
    [InlineData("foo", null, true)]
    public void NotEqual(string value, string test, bool expected)
    {
        // Act
        var notEqual = _sut.NotEqual(value, test);
        var notEquals = _sut.NotEquals(value, test);

        // Assert
        notEqual.Should().Be(expected);
        notEquals.Should().Be(expected);
    }

    [Theory]
    [InlineData("Foo", "foo", "OrdinalIgnoreCase", false)]
    [InlineData("Foo", "foo", "5", false)]
    [InlineData("Foo", "foo", 5, false)]
    public void NotEqualWithStringStringComparison(string value, string test, object stringComparison, bool expected)
    {
        // Act
        var notEqual = _sut.NotEqual(value, test, stringComparison);
        var notEquals = _sut.NotEquals(value, test, stringComparison);

        // Assert
        notEqual.Should().Be(expected);
        notEquals.Should().Be(expected);
    }

    [Theory]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData("    ", true)]
    [InlineData(null, true)]
    [InlineData("foo", false)]
    public void IsNullOrWhitespace(string value, bool expected)
    {
        // Act
        var result = _sut.IsNullOrWhiteSpace(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("    ", false)]
    [InlineData(null, false)]
    [InlineData("foo", true)]
    public void IsNotNullOrWhitespace(string value, bool expected)
    {
        // Act
        var result = _sut.IsNotNullOrWhiteSpace(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", true)]
    [InlineData(" ", false)]
    [InlineData("    ", false)]
    [InlineData(null, true)]
    [InlineData("foo", false)]
    public void IsNullOrEmpty(string value, bool expected)
    {
        // Act
        var result = _sut.IsNullOrEmpty(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", true)]
    [InlineData("    ", true)]
    [InlineData(null, false)]
    [InlineData("foo", true)]
    public void IsNotNullOrEmpty(string value, bool expected)
    {
        // Act
        var result = _sut.IsNotNullOrEmpty(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("foobar", 3, "bar")]
    [InlineData(" ", 0, " ")]
    public void Substring_2params(string value, int start, string expected)
    {
        // Act
        var result = _sut.Substring(value, start);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 1)]
    [InlineData("", 1)]
    [InlineData("", 0)]
    [InlineData(" ", 2)]
    [InlineData("foo", -1)]
    public void Substring_2params_Exceptions(string value, int start)
    {
        // Act
        Action action = () => { _sut.Substring(value, start); };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("foobar", 0, 3, "foo")]
    [InlineData("foobar", 3, 3, "bar")]
    public void Substring_3params(string value, int start, int end, string expected)
    {
        // Act
        var result = _sut.Substring(value, start, end);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 1, 0)]
    [InlineData("", 1, 0)]
    [InlineData("", 0, 1)]
    [InlineData(" ", 2, 0)]
    [InlineData(" ", 1, 1)]
    [InlineData(" ", 1, -1)]
    [InlineData("foo", -1, 0)]
    public void Substring_3params_Exceptions(string value, int start, int end)
    {
        // Act
        Action action = () => { _sut.Substring(value, start, end); };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("a;b;c", ";", new[] { "a", "b", "c" })]
    [InlineData("a<br />b<br />c", "<br />", new[] { "a", "b", "c" })]
    [InlineData("Honors Algebra 2<br /><br />More text", "<br />", new[] { "Honors Algebra 2", "", "More text" })]
    [InlineData("no separator here", ";", new[] { "no separator here" })]
    public void Split(string value, string separator, string[] expected)
    {
        // Act
        var result = _sut.Split(value, separator);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void First_ReturnsFirstElement()
    {
        // Arrange
        var values = new object[] { "first", "second", "third" };

        // Act
        var result = _sut.First(values);

        // Assert
        result.Should().Be("first");
    }

    [Fact]
    public void First_ReturnsNullForEmptyCollection()
    {
        // Arrange
        var values = Array.Empty<object>();

        // Act
        var result = _sut.First(values);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void First_ThrowsForNullCollection()
    {
        // Act
        Action action = () => _sut.First(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Last_ReturnsLastElement()
    {
        // Arrange
        var values = new object[] { "first", "second", "third" };

        // Act
        var result = _sut.Last(values);

        // Assert
        result.Should().Be("third");
    }

    [Fact]
    public void Last_ReturnsNullForEmptyCollection()
    {
        // Arrange
        var values = Array.Empty<object>();

        // Act
        var result = _sut.Last(values);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Last_ThrowsForNullCollection()
    {
        // Act
        Action action = () => _sut.Last(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}