using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class DateTimeHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly DateTimeHelpers _sut;

        public DateTimeHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

            _sut = new DateTimeHelpers(_contextMock.Object);
        }

        [Fact]
        public void Now()
        {
            // Act
            var result = _sut.Now();

            // Assert
            result.Should().BeOnOrBefore(DateTime.Now);
        }

        [Fact]
        public void UtcNow()
        {
            // Act
            var result = _sut.UtcNow();

            // Assert
            result.Should().BeOnOrBefore(DateTime.UtcNow);
        }
    }
}