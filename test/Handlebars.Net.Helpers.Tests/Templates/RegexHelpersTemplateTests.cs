using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class RegexHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public RegexHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.Regex);
        }

        [Theory]
        [InlineData("{{#Regex.IsMatch \"Hello\" \"Hello\"}}yes{{else}}no{{/Regex.IsMatch}}", "yes")]
        [InlineData("{{#Regex.IsMatch \"Hello\" \"x\"}}yes{{else}}no{{/Regex.IsMatch}}", "no")]
        public void BlockHelper_IsMatch(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{#Regex.IsMatch \"Hello\" \"Hello\"}}TRUE{{else}}FALSE{{/Regex.IsMatch}}", "TRUE")]
        [InlineData("{{#Regex.IsMatch \"Hello\" \"x\"}}TRUE{{else}}FALSE{{/Regex.IsMatch}}", "FALSE")]
        public void BlockHelper_IsMatch_With_True_And_False(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{#if (Regex.IsMatch \"Hello\" \"Hello\")}}this is true{{/if}}", "this is true")]
        [InlineData("{{#if (Regex.IsMatch \"Hello\" \"x\")}}this is true{{/if}}", "")]
        public void NormalHelper_IsMatch_With_If(string template, string expected)
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