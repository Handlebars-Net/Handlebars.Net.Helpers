using System;
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
        [InlineData(10, null, null, null, "Long text…")]
        [InlineData(10, null, "FixedLength", null, "Long text…")]
        [InlineData(10, "---", "FixedLength", null, "Long te---")]
        [InlineData(6, null, "FixedNumberOfCharacters", null, "Long t…")]
        [InlineData(6, "---", "FixedNumberOfCharacters", null, "Lon---")]
        [InlineData(2, null, "FixedNumberOfWords", null, "Long text…")]
        [InlineData(2, "---", "FixedNumberOfWords", null, "Long text---")]
        [InlineData(10, null, "FixedLength", "Left", "… truncate")]
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