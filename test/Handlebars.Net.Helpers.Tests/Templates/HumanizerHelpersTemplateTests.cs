using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class HumanizerHelpersTemplateTests
    {
        private readonly DateTime DateTimeNow = new DateTime(2020, 4, 15, 23, 59, 58);

        private readonly Mock<IDateTimeService> _dateTimeServiceMock;

        private readonly IHandlebars _handlebarsContext;

        public HumanizerHelpersTemplateTests()
        {
            _dateTimeServiceMock = new Mock<IDateTimeService>();
            _dateTimeServiceMock.Setup(d => d.Now()).Returns(DateTimeNow);
            _dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(DateTimeNow.ToUniversalTime);

            _handlebarsContext = Handlebars.Create();
            _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

            HandlebarsHelpers.Register(_handlebarsContext, o =>
            {
                o.DateTimeService = _dateTimeServiceMock.Object;
            });
        }

        [Fact]
        public void HumanizeDateTime()
        {
            var template = string.Format("{{{{[Humanizer.Humanize] \"{0}\" }}}}", DateTime.UtcNow.AddHours(-30).ToString("O"));

            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be("yesterday");
        }

        [Theory]
        [InlineData("{{[Humanizer.Humanize] \"HTML\"}}", "HTML")]
        [InlineData("{{[Humanizer.Humanize] \"PascalCaseInputStringIsTurnedIntoSentence\"}}", "Pascal case input string is turned into sentence")]
        public void HumanizeString(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }
    }
}