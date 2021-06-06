using System.Globalization;
using FluentAssertions;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class HumanizerHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly HumanizerHelpers _sut;

        public HumanizerHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.CreateSpecificCulture("en-us") });

            _sut = new HumanizerHelpers(_contextMock.Object);
        }

        [Theory]
        [InlineData("HTML", "HTML")]
        [InlineData("PascalCaseInputStringIsTurnedIntoSentence", "Pascal case input string is turned into sentence")]
        public void HumanizeString(string value, string expected)
        {
            // Act
            var result = _sut.Humanize(value);

            // Assert
            result.Should().Be(expected);
        }
    }
}