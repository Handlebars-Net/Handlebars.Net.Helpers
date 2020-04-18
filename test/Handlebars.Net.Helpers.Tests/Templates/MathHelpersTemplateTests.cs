using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class MathHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public MathHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.Math);
        }

        [Theory]
        [InlineData("{{Math.Add 1 2}}", "3")]
        [InlineData("{{Math.Add 2.2 3.1}}", "5.3")]
        public void Add(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().StartWith(expected);
        }
    }
}