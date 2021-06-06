using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class StringHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly StringHelpers _sut;

        public StringHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.InvariantCulture });

            _sut = new StringHelpers(_contextMock.Object);
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
    }
}