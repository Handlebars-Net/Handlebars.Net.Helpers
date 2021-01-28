using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class JsonPathHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public JsonPathHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.JsonPath);
        }

        [Fact]
        public void SelectToken_With_String()
        {
            // Arrange
            var request = new
            {
                body = "{ \"Price\": 99 }"
            };

            var action = _handlebarsContext.Compile("{{JsonPath.SelectToken body \"..Price\"}}");

            // Act
            var result = action(request);

            // Assert
            int.Parse(result).Should().Be(99);
        }

        [Fact]
        public void SelectTokens_With_JObject()
        {
            // Arrange
            var request = new
            {
                body = JObject.Parse(@"{
                  'Stores': [
                    'Lambton Quay',
                    'Willis Street'
                  ],
                  'Manufacturers': [
                    {
                      'Name': 'Acme Co',
                      'Products': [
                        {
                          'Name': 'Anvil',
                          'Price': 50
                        }
                      ]
                    },
                    {
                      'Name': 'Contoso',
                      'Products': [
                        {
                          'Name': 'Elbow Grease',
                          'Price': 99.95
                        },
                        {
                          'Name': 'Headlight Fluid',
                          'Price': 4
                        }
                      ]
                    }
                  ]
                }")
            };

            var action = _handlebarsContext.Compile("{{#JsonPath.SelectTokens body \"$..Products[?(@.Price >= 50)].Name\"}}{{#each this}}%{{@index}}:{{this}}%{{/each}}{{/JsonPath.SelectTokens}}");

            // Act
            var result = action(request);

            // Assert
            result.Should().Be("%0:Anvil%%1:Elbow Grease%");
        }
    }
}