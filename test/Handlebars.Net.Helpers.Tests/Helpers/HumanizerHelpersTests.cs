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
        [InlineData("Pascal case input string is turned into sentence", "PascalCaseInputStringIsTurnedIntoSentence")]
        public void DehumanizeString(string value, string expected)
        {
            // Act
            var result = _sut.Dehumanize(value);

            // Assert
            result.Should().Be(expected);
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

        [Theory]
        [InlineData("Sentence casing", "LowerCase", "sentence casing")]
        [InlineData("Sentence casing", "SentenceCase", "Sentence casing")]
        [InlineData("Sentence casing", "TitleCase", "Sentence Casing")]
        [InlineData("Sentence casing", "UpperCase", "SENTENCE CASING")]
        public void TransformString(string value, string transformer, string expected)
        {
            // Act
            var result = _sut.Transform(value, transformer);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(10, null, null, null, "Long text…")]
        [InlineData(10, null, "FixedLength", null, "Long text…")]
        [InlineData(10, "---", "FixedLength", null, "Long te---")]
        [InlineData(6, null, "FixedNumberOfCharacters", null, "Long t…")]
        [InlineData(6, "---", "FixedNumberOfCharacters", null, "Lon---")]
        [InlineData(2, null, "FixedNumberOfWords", null, "Long text…")]
        [InlineData(2, "---", "FixedNumberOfWords", null, "Long text---")]
        [InlineData(10, null, "FixedLength", "Left", "… truncate")]
        public void TruncateString(int length, string? separator, string? truncator, string? truncateFrom, string expected)
        {
            // Arrange
            string value = "Long text to truncate";

            // Act
            var result = _sut.Truncate(value, length, separator, truncator, truncateFrom);

            // Assert
            result.Should().Be(expected);
        }
    }
}