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

        [Theory]
        [InlineData(-1, 1, 1)]
        [InlineData(-1.2, 1, 1)]
        [InlineData(1, -1.2, 1)]
        public void Max(object value1, object value2, object expected)
        {
            // Act
            var result = _sut.Max(value1, value2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, 1, -1)]
        [InlineData(-1.2, 1, -1.2)]
        [InlineData(1, -1.2, -1.2)]
        public void Min(object value1, object value2, object expected)
        {
            // Act
            var result = _sut.Min(value1, value2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0.0d, 0)]
        [InlineData(-2, -1)]
        [InlineData(-2.2, -1)]
        [InlineData(42, 1)]
        [InlineData(42.5, 1)]
        public void Sign(object value, object expected)
        {
            // Act
            var result = _sut.Sign(value);

            // Assert
            result.Should().Be(expected);
        }
    }
}