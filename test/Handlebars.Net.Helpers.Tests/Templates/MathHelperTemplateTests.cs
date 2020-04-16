using FluentAssertions;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using Xunit;

namespace Handlebars.Net.Helpers.Tests.Templates
{
    public class MathHelperTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public MathHelperTemplateTests()
        {
            _handlebarsContext = HandlebarsDotNet.Handlebars.Create();

            HandleBarsHelpers.Register(_handlebarsContext);
        }

        [Theory]
        [InlineData("{{Add 1 2}}", "3")]
        [InlineData("{{Add 2.2 3.1}}", "5.3")]
        public void Add(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().StartWith(expected);
        }

        [Theory]
        [InlineData("{{Math.PI}}", "3.141592653589793")]
        [InlineData("{{Math.E}}", "2.718281828459045")]
        public void Constants(string template, string expected)
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