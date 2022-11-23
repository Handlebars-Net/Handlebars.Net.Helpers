using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class BooleanHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly BooleanHelpers _sut;

        public BooleanHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.InvariantCulture });

            _sut = new BooleanHelpers(_contextMock.Object);
        }


        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, true, true)]
        [InlineData(false, false, true)]
        public void Equal(bool value, bool test, bool expected)
        {
            // Act
            var result = _sut.Equal(value, test);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, true, false)]
        [InlineData(false, false, false)]
        public void NotEqual(bool value, bool test, bool expected)
        {
            // Act
            var result = _sut.NotEqual(value, test);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Not(bool value, bool expected)
        {
            // Act
            var result = _sut.Not(value);

            // Assert
            result.Should().Be(expected);
        }
    }
}