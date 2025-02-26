using System;
using System.Globalization;
using CultureAwareTesting.xUnit;
using FluentAssertions;
using Xunit;

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

        var template = "{{[Object].ToString x }}";

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("123.456");
    }

    [CulturedTheory("en-us")]
    [InlineData("{{[Object].Equal value1 value2 }}", false)]
    [InlineData("{{[Object].NotEqual value1 value2 }}", true)]
    [InlineData("{{[Object].GreaterThan value1 value2 }}", true)]
    [InlineData("{{[Object].GreaterThanEqual value1 value2 }}", true)]
    [InlineData("{{[Object].LowerThan value1 value2 }}", false)]
    [InlineData("{{[Object].LowerThanEqual value1 value2 }}", false)]
    [InlineData("{{[Object].CompareTo value1 value2}}", 1)]
    public void Compare(string template, object expected)
    {
        // Arrange
        var model = new
        {
            value1 = DateTime.Parse("2000-01-01"),
            value2 = DateTime.Parse("1999-01-01"),
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected.ToString());
    }

    [Fact]
    public void IsNull()
    {
        // Arrange
        var model = new
        {
            value = default(object)
        };

        var template = "{{[Object].IsNull value}}";

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("True");
    }

    [Fact]
    public void IsNotNull()
    {
        // Arrange
        var model = new
        {
            value = default(object)
        };

        var template = "{{[Object].IsNotNull value}}";

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("False");
    }
}