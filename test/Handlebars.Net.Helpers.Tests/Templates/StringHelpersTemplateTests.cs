using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class StringHelpersTemplateTests
    {
        private readonly DateTime DateTimeNow = new DateTime(2020, 4, 15, 23, 59, 58);

        private readonly Mock<IDateTimeService> _dateTimeServiceMock;

        private readonly IHandlebars _handlebarsContext;

        public StringHelpersTemplateTests()
        {
            _dateTimeServiceMock = new Mock<IDateTimeService>();
            _dateTimeServiceMock.Setup(d => d.Now()).Returns(DateTimeNow);
            _dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(DateTimeNow.ToUniversalTime);

            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, o =>
            {
                o.DateTimeService = _dateTimeServiceMock.Object;
            });
        }

        [Theory]
        [InlineData("{{[String.Append] \"foo\" \"bar\"}}", "foobar")]
        [InlineData("{{[String.Append] \"foo\" \"b\"}}", "foob")]
        [InlineData("{{[String.Append] \"foo\" 'b'}}", "foob")]
        [InlineData("{{[String.Append] \"foo\" ([String.Append] \"a\" \"b\")}}", "fooab")]
        public void Append(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{#String.IsString \"Hello\"}}yes{{else}}no{{/String.IsString}}", "yes")]
        [InlineData("{{#String.IsString 1}}yes{{else}}no{{/String.IsString}}", "no")]
        public void IsString(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{String.Join [\"a\",\"b\",\"c\"] ':'}}", "a:b:c")]
        [InlineData("{{[String.Join] [\"a\",\"b\",\"c\"] \"?\"}}", "a?b?c")]
        public void Join(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action(null);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{[String.Split] \"a,b,c\" ','}}", "[\"a\",\"b\",\"c\"]")]
        [InlineData("{{[String.Split] \"a_;b_;c\" \"_;\"}}", "[\"a\",\"b\",\"c\"]")]
        public void Split(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{#String.StartsWith \"Hello\" \"He\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Hi")]
        [InlineData("{{#String.StartsWith \"Hello\" \"xx\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Goodbye")]
        [InlineData("{{#String.StartsWith \"Hello\" \"H\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Hi")]
        [InlineData("{{#String.StartsWith \"Hello\" \"x\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Goodbye")]
        [InlineData("{{#String.StartsWith \"Hello\" 'H'}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Hi")]
        [InlineData("{{#String.StartsWith \"Hello\" 'x'}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Goodbye")]
        public void StartsWith(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{[String.Append] \"foo\"}}")]
        [InlineData("{{[String.Append] \"foo\" \"bar\" \"bar2\"}}")]
        public void InvalidNumberOfArgumentsShouldThrowHandlebarsException(string template)
        {
            // Arrange
            var handleBarsAction = _handlebarsContext.Compile(template);

            // Act and Assert
            Assert.Throws<HandlebarsException>(() => handleBarsAction(""));
        }

        [Theory]
        [InlineData("{{[String.Lowercase] \"foo\"}}", "foo")]
        [InlineData("{{[String.Lowercase] \"FOO\"}}", "foo")]
        [InlineData("{{[String.Lowercase] \"42\"}}", "42")]
        public void ToLower(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void WithCustomPrefixSeparator()
        {
            // Arrange
            var handlebarsContext = Handlebars.Create();
            HandlebarsHelpers.Register(handlebarsContext, options =>
            {
                options.PrefixSeparator = "-";
            });
            var action = handlebarsContext.Compile("{{String-Append \"foo\" \"bar\"}}");

            // Act
            var result = action("");

            // Assert
            result.Should().Be("foobar");
        }

        [Fact]
        public void WithoutCategoryPrefix()
        {
            // Arrange
            var handlebarsContext = Handlebars.Create();
            HandlebarsHelpers.Register(handlebarsContext, options =>
            {
                options.UseCategoryPrefix = false;
            });
            var action = handlebarsContext.Compile("{{[Append] \"foo\" \"bar\"}}");

            // Act
            var result = action("");

            // Assert
            result.Should().Be("foobar");
        }

        [Fact]
        public void WithoutCategoryPrefixAndWithExtraPrefix()
        {
            // Arrange
            var handlebarsContext = Handlebars.Create();
            HandlebarsHelpers.Register(handlebarsContext, options =>
            {
                options.UseCategoryPrefix = false;
                options.Prefix = "test";
            });
            var action = handlebarsContext.Compile("{{[test.Append] \"foo\" \"bar\"}}");

            // Act
            var result = action("");

            // Assert
            result.Should().Be("foobar");
        }

        [Fact]
        public void WithCategoryPrefixAndExtraWithPrefix()
        {
            // Arrange
            var handlebarsContext = Handlebars.Create();
            HandlebarsHelpers.Register(handlebarsContext, options =>
            {
                options.UseCategoryPrefix = true;
                options.Prefix = "test";
            });
            var action = handlebarsContext.Compile("{{[test.String.Append] \"foo\" \"bar\"}}");

            // Act
            var result = action("");

            // Assert
            result.Should().Be("foobar");
        }

        [Fact]
        public void Format_Now()
        {
            // Arrange
            var action = _handlebarsContext.Compile("{{String.Format (DateTime.Now) \"yyyy-MM-dd\"}}");

            // Act
            var result = action("");

            // Assert
            result.Should().Be("2020-04-15");
        }

        [Fact]
        public void Format_Template_Now()
        {
            // Arrange
            var model = new
            {
                x = DateTimeNow
            };

            var action = _handlebarsContext.Compile("{{String.Format x \"yyyy-MMM-dd\"}}");

            // Act
            var result = action(model);

            // Assert
            result.Should().Be("2020-Apr-15");
        }
    }
}