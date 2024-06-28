using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class BooleanHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public BooleanHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext, Category.Boolean);
    }

    [Fact]
    public void Boolean_Not()
    {
        // Arrange
        var template = """[ {{#each data}}{{#if (Boolean.Not @first)}}, "{{this}}"{{else}}"{{this}}"{{/if}}{{/each}} ]""";
        var compiled = _handlebarsContext.Compile(template);
        var data = new
        {
            data = new[] { "text1", "text2", "text3" }
        };

        // Act
        var result = compiled(data);

        // Assert
        result.Should().Be("""[ "text1", "text2", "text3" ]""");
    }

    [Theory]
    [InlineData(true, "False")]
    [InlineData(false, "True")]
    public void Not(bool value, string expected)
    {
        // Arrange
        var template = "{{[Boolean.Not] value}}";
        var compiled = _handlebarsContext.Compile(template);
        var data = new
        {
            value = value
        };

        // Act
        var result = compiled(data);

        // Assert
        result.Should().Be(expected);
    }
}