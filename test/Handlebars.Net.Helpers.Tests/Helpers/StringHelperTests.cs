using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Xunit;

namespace Handlebars.Net.Helpers.Tests.Helpers
{
    public class StringHelperTests
    {
        private readonly StringHelper _sut;

        public StringHelperTests()
        {
            _sut = new StringHelper();
        }

        [Theory]
        [InlineData("", "bar", "bar")]
        [InlineData("foo", null, "foo")]
        [InlineData("foo", "bar", "foobar")]
        public void Append(string value, string append, string expected)
        {
            // Act
            var result = _sut.Append(value, append);

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
        [InlineData("foo", null, "foo")]
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
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("A", "A")]
        [InlineData("foo bar", "fooBar")]
        [InlineData("FOO Bar", "fooBar")]
        public void ToCamelCase(string value, string expected)
        {
            // Act
            var result = _sut.ToCamelCase(value);

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
        public void ToLower(string value, string expected)
        {
            // Act
            var result = _sut.ToLower(value);

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
        public void ToPascalCase(string value, string expected)
        {
            // Act
            var result = _sut.ToPascalCase(value);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "A")]
        [InlineData("foo bar", "FOO BAR")]
        [InlineData("FOO Bar", "FOO BAR")]
        public void ToUpper(string value, string expected)
        {
            // Act
            var result = _sut.ToUpper(value);

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
    }
}