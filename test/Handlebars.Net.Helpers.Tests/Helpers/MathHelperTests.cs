using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Xunit;

namespace Handlebars.Net.Helpers.Tests.Helpers
{
    public class MathHelperTests
    {
        private readonly MathHelper _sut;

        public MathHelperTests()
        {
            _sut = new MathHelper();
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 1)]
        [InlineData(-2147483649L, 2147483649L)]
        [InlineData(1.2, 1.2)]
        [InlineData(-1.2, 1.2)]
        public void Abs(object value, object expected)
        {
            // Act
            var result = _sut.Abs(value);

            // Assert
            result.Should().Be(expected);
        }
    }
}