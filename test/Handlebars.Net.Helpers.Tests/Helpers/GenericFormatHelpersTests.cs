using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class GenericFormatHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly GenericFormatHelpers _sut;

        public GenericFormatHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

            _sut = new GenericFormatHelpers(_contextMock.Object);
        }

        [Theory]
        [InlineData("d", "4/15/2020")]
        [InlineData("o", "2020-04-15T23:59:58.0000000")]
        [InlineData("MM-dd-yyyy", "04-15-2020")]
        public void Format_DateTime(string format, string expected)
        {
            // Arrange
            var value = new DateTime(2020, 4, 15, 23, 59, 58);

            // Act
            var result1 = _sut.Format(value, format);
            var result2 = _sut.Format((DateTime?) value, format);

            // Assert
            result1.Should().Be(expected);
            result2.Should().Be(expected);
        }
    }
}