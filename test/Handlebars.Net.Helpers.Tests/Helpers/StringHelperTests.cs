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
    }
}