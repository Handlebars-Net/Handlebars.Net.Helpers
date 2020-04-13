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
        [InlineData("foo", "bar", "foobar")]
        public void Append(string value, string append, string expected)
        {
            // Act
            var result = _sut.Append(value, append);

            // Assert
            result.Should().Be(expected);
        }
    }
}