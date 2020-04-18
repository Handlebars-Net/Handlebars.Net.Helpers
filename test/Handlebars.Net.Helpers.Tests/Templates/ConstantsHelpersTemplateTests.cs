using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class ConstantsHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public ConstantsHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.Constants);
        }

        [Theory]
        [InlineData("{{Constants.Math.PI}}", "3.141592653589793")]
        [InlineData("{{Constants.Math.E}}", "2.718281828459045")]
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