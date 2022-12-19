using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class HumanizerHelpersTests
    {
        private readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-us");

        private readonly Mock<IHandlebars> _contextMock;

        private readonly HumanizerHelpers _sut;

        public HumanizerHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = Culture });

            _sut = new HumanizerHelpers(_contextMock.Object);

            Thread.CurrentThread.CurrentCulture = Culture;
        }

        [Fact]
        public void Camelize()
        {
            // Arrange
            var value = "some_title for something";

            // Act
            var result = _sut.Camelize(value);

            // Assert
            result.Should().Be("someTitleForSomething");
        }

        [Fact]
        public void Dasherize()
        {
            // Arrange
            var value = "some_title";

            // Act
            var result = _sut.Dasherize(value);

            // Assert
            result.Should().Be("some-title");
        }

        [Theory]
        [InlineData("HTML", "HTML")]
        [InlineData("Pascal case input string is turned into sentence", "PascalCaseInputStringIsTurnedIntoSentence")]
        public void Dehumanize(string value, string expected)
        {
            // Act
            var result = _sut.Dehumanize(value);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void FormatWith()
        {
            // Arrange
            var value = "To be formatted -> {0}/{1}.";

            // Act
            var result = _sut.FormatWith(value, 1, "A");

            // Assert
            result.Should().Be("To be formatted -> 1/A.");
        }

        [Theory]
        [InlineData("I", 1)]
        [InlineData("XLII", 42)]
        public void FromRoman(string value, int expected)
        {
            // Act
            var result = _sut.FromRoman(value);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void HumanizeDateTime()
        {
            // Arrange
            var value = DateTime.UtcNow.AddHours(-30);

            // Act
            var result = _sut.Humanize(value);

            // Assert
            result.Should().Be("yesterday");
        }

        [Fact]
        public void HumanizeDateTimeAsString()
        {
            // Arrange
            var value = DateTime.UtcNow.AddHours(-30).ToString("O");

            // Act
            var result = _sut.Humanize(value);

            // Assert
            result.Should().Be("yesterday");
        }

        [Fact]
        public void HumanizeTimeSpan()
        {
            // Arrange
            var value = TimeSpan.FromDays(16);

            // Act
            var result = _sut.Humanize(value);

            // Assert
            result.Should().Be("2 weeks");
        }

        [Fact]
        public void HumanizeTimeSpanAsString()
        {
            // Arrange
            var value = TimeSpan.FromDays(16).ToString();

            // Act
            var result = _sut.Humanize(value);

            // Assert
            result.Should().Be("2 weeks");
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

        [Fact]
        public void Hyphenate()
        {
            // Arrange
            var value = "some_title";

            // Act
            var result = _sut.Hyphenate(value);

            // Assert
            result.Should().Be("some-title");
        }

        [Fact]
        public void Kebaberize()
        {
            // Arrange
            var value = "SomeText";

            // Act
            var result = _sut.Kebaberize(value);

            // Assert
            result.Should().Be("some-text");
        }

        [Theory]
        [InlineData("1", "1st")]
        [InlineData("2", "2nd")]
        [InlineData(3, "3rd")]
        public void Ordinalize(object value, string expected)
        {
            // Act
            var result = _sut.Ordinalize(value);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Pascalize()
        {
            // Arrange
            var value = "some_title for something";

            // Act
            var result = _sut.Pascalize(value);

            // Assert
            result.Should().Be("SomeTitleForSomething");
        }

        [Theory]
        [InlineData("Man", "Men")]
        [InlineData("string", "strings")]
        [InlineData("Sheep", "Sheep")]
        public void Pluralize(string value, string expected)
        {
            // Act
            var result = _sut.Pluralize(value);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("Men", "Man")]
        [InlineData("strings", "string")]
        [InlineData("Sheep", "Sheep")]
        public void Singularize(string value, string expected)
        {
            // Act
            var result = _sut.Singularize(value);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Titleize()
        {
            // Arrange
            var value = "some title";

            // Act
            var result = _sut.Titleize(value);

            // Assert
            result.Should().Be("Some Title");
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(1230d, "1.23k")]
        [InlineData(0.1d, "100m")]
        public void ToMetric(object value, string expected)
        {
            // Act
            var result = _sut.ToMetric(value);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "first")]
        [InlineData(2, "second")]
        [InlineData("2021-06-07T15:21:39Z", "June 7th, 2021")]
        public void ToOrdinalWords(object value, string expected)
        {
            // Act
            var result = _sut.ToOrdinalWords(value);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void ToOrdinalWordsWithDateTime()
        {
            // Arrange
            var value = new DateTime(2021, 6, 7);

            // Act
            var result = _sut.ToOrdinalWords(value);

            // Assert
            result.Should().Be("June 7th, 2021");
        }

        [Theory]
        [InlineData(1, "one")]
        [InlineData(long.MaxValue, "nine quintillion two hundred and twenty-three quadrillion three hundred and seventy-two trillion thirty-six billion eight hundred and fifty-four million seven hundred and seventy-five thousand eight hundred and seven")]
        public void ToWords(object value, string expected)
        {
            // Act
            var result = _sut.ToWords(value);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("Man", "1", "1 Man")]
        [InlineData("Man", "2", "2 Men")]
        [InlineData("Man", 3, "3 Men")]
        [InlineData("string", 3, "3 strings")]
        public void ToQuantity(string value, object quantity, string expected)
        {
            // Act
            var result = _sut.ToQuantity(value, quantity);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "I")]
        [InlineData(42, "XLII")]
        public void ToRoman(int value, string expected)
        {
            // Act
            var result = _sut.ToRoman(value);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("Sentence casing", "LowerCase", "sentence casing")]
        [InlineData("Sentence casing", "SentenceCase", "Sentence casing")]
        [InlineData("Sentence casing", "TitleCase", "Sentence Casing")]
        [InlineData("Sentence casing", "UpperCase", "SENTENCE CASING")]
        public void Transform(string value, string transformer, string expected)
        {
            // Act
            var result = _sut.Transform(value, transformer);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(10, null, null, null, "Long text?")]
        [InlineData(10, null, "FixedLength", null, "Long text?")]
        [InlineData(10, "---", "FixedLength", null, "Long te---")]
        [InlineData(6, null, "FixedNumberOfCharacters", null, "Long t?")]
        [InlineData(6, "---", "FixedNumberOfCharacters", null, "Lon---")]
        [InlineData(2, null, "FixedNumberOfWords", null, "Long text?")]
        [InlineData(2, "---", "FixedNumberOfWords", null, "Long text---")]
        [InlineData(10, null, "FixedLength", "Left", "?truncate")]
        public void Truncate(int length, string? separator, string? truncator, string? truncateFrom, string expected)
        {
            // Arrange
            string value = "Long text to truncate";

            // Act
            var result = _sut.Truncate(value, length, separator, truncator, truncateFrom);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Underscore()
        {
            // Arrange
            var value = "SomeTitle";

            // Act
            var result = _sut.Underscore(value);

            // Assert
            result.Should().Be("some_title");
        }
    }
}