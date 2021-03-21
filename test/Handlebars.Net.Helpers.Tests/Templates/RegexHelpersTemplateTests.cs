using System.Collections.Generic;
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
        [InlineData("{{#Regex.IsMatch \"Hello\" \"Hello\"}}TRUE{{else}}FALSE{{/Regex.IsMatch}}", "TRUE")]
        [InlineData("{{#Regex.IsMatch \"Hello\" \"x\"}}TRUE{{else}}FALSE{{/Regex.IsMatch}}", "FALSE")]
        [InlineData("{{#Regex.IsMatch \"Hello\" \"Hello\"}}{{.}}{{else}}no{{/Regex.IsMatch}}", "True")]
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

        [Fact]
        public void NormalHelper_IsMatch_With_If_And_ComplexObject()
        {
            // Arrange
            var template = "{{#if (#Regex.IsMatch Attributes 'visible')}}{{Value}}{{/if}}";
            var context = new Dictionary<string, object>()
            {
                ["Attributes"] = "visible",
                ["Value"] = "SomeString"
            };
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action(context);

            // Assert
            result.Should().Be("SomeString");
        }
    }
}