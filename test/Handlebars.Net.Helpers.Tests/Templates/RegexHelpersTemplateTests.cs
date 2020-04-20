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
        public void IsMatch(string template, string expected)
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