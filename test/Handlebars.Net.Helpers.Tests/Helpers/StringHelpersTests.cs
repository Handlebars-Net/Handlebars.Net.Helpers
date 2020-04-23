using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class StringHelpersTests
    {
        private readonly StringHelpers _sut;

        public StringHelpersTests()
        {
            _sut = new StringHelpers();
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
    }
}