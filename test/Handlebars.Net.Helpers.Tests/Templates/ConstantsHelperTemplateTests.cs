using FluentAssertions;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace Handlebars.Net.Helpers.Tests.Templates
{
    public class ConstantsHelperTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public ConstantsHelperTemplateTests()
        {
            _handlebarsContext = HandlebarsDotNet.Handlebars.Create();

            HandleBarsHelpers.Register(_handlebarsContext, HelperType.Constants);
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