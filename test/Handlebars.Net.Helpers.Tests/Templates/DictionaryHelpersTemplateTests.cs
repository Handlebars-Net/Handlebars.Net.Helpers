using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class DictionaryHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public DictionaryHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext, Category.Dictionary);
    }

    [Theory]
    [InlineData("{{[Dictionary.Lookup] data '1' }}", "one")]
    [InlineData("{{[Dictionary.Lookup] data '2' }}", "two")]
    [InlineData("{{[Dictionary.Lookup] data '3' }}", "")]
    [InlineData("{{[Dictionary.Lookup] data '3' 'not found'}}", "not found")]
    public void Lookup_Dictionary(string template, string expected)
    {
        // Arrange
        var dictionary = new Dictionary<string, object?>
        {
            { "1", "one" },
            { "2", "two" }
        };
        var model = new
        {
            data = dictionary
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[Dictionary.Lookup] data '1' }}", "one")]
    [InlineData("{{[Dictionary.Lookup] data '2' }}", "two")]
    [InlineData("{{[Dictionary.Lookup] data '3' }}", "")]
    [InlineData("{{[Dictionary.Lookup] data '3' 'not found'}}", "not found")]
    public void Lookup_JObject(string template, string expected)
    {
        // Arrange
        var jObject = new JObject
        {
            { "1", "one" },
            { "2", "two" }
        };
        var model = new
        {
            data = jObject
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData("{{[Dictionary.Lookup] data '1' }}", "one")]
    [InlineData("{{[Dictionary.Lookup] data '2' }}", "two")]
    [InlineData("{{[Dictionary.Lookup] data '3' }}", "")]
    [InlineData("{{[Dictionary.Lookup] data '3' 'not found'}}", "not found")]
    public void Lookup_JsonObject(string template, string expected)
    {
        // Arrange
        var jsonObject = new System.Text.Json.Nodes.JsonObject
        {
            { "1", "one" },
            { "2", "two" }
        };
        var model = new
        {
            data = jsonObject
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }
#endif
}