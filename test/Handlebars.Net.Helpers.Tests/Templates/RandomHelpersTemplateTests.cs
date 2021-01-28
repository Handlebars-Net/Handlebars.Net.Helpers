using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class RandomHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public RandomHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.Random);
        }

        [Fact]
        public void Random()
        {
            // Arrange
            var action = _handlebarsContext.Compile("{{Random.Random Type=\"Integer\" Min=1000 Max=9999}}");

            // Act
            var result = action("");

            // Assert
            int.Parse(result).Should().BeInRange(1000, 9999);
        }
    }
}