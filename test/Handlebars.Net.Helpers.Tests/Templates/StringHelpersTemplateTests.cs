using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class StringHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public StringHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.String);
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
        [InlineData("{{[String.StartsWith] \"foo\" 1}}")]
        public void InvalidArgumentTypeShouldThrowNotArgumentException(string template)
        {
            // Arrange
            var handleBarsAction = _handlebarsContext.Compile(template);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => handleBarsAction(""));
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
    }
}