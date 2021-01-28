using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class DynamicLinqHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public DynamicLinqHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.DynamicLinq);
        }

        [Fact]
        public void Linq()
        {
            // Arrange
            var request = new
            {
                Path = "/test"
            };

            var action = _handlebarsContext.Compile("{{DynamicLinq.Linq Path 'it'}}");

            // Act
            var result = action(request);

            // Assert
            result.Should().Be("/test");
        }

        [Fact]
        public void Linq1()
        {
            // Arrange
            var request = new
            {
                body = new JObject
                {
                    { "Id", new JValue(9) },
                    { "Name", new JValue("Test") }
                }
            };

            var action = _handlebarsContext.Compile("{{DynamicLinq.Linq body 'it.Name + \"_123\"' }}");

            // Act
            var result = action(request);

            // Assert
            result.Should().Be("Test_123");
        }
    }
}