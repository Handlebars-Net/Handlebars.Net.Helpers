using System;
using System.Globalization;
using CultureAwareTesting.xUnit;
using FluentAssertions;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class ObjectHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public ObjectHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext, o =>
        {   
        });
    }

    [CulturedFact("en-us")]
    public void ToString_WithInt()
    {
        // Arrange
        var model = new
        {
            x = 123.456
        };

        var template = $"{{{{[Object].ToString x }}}}";

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("123.456");
    }
}