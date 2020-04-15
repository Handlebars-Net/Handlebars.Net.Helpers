using FluentAssertions;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using Xunit;

namespace Handlebars.Net.Helpers.Tests.Templates
{
    public class RegexHelperTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public RegexHelperTemplateTests()
        {
            _handlebarsContext = HandlebarsDotNet.Handlebars.Create();

            HandleBarsHelpers.Register(_handlebarsContext);
        }

        [Theory]
        [InlineData("{{#IsMatch \"Hello\" \"*\"}}yes{{else}}no{{/Regex.IsMatch}}", "yes")]
        [InlineData("{{#IsMatch \"Hello\" \"x\"}}yes{{else}}no{{/Regex.IsMatch}}", "no")]
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