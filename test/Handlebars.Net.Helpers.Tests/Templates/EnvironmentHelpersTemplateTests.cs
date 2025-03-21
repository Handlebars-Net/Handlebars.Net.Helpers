using System;
using System.Linq;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class EnvironmentHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public EnvironmentHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();

        HandlebarsHelpers.Register(_handlebarsContext, o =>
        {
            o.UseCategoryPrefix = false;
            o.Categories = o.Categories.Concat([Category.Environment]).ToArray();
        });
    }

    [Fact]
    public void GetEnvironmentVariable_NonExisting()
    {
        // Arrange
        var variable = Guid.NewGuid().ToString();
        var template = $"{{{{GetEnvironmentVariable \"{variable}\"}}}}";
        var compiled = _handlebarsContext.Compile(template);
        var data = new { };

        // Act
        var result = compiled(data);

        // Assert
        result.Should().Be("");
    }

    [Fact]
    public void GetEnvironmentVariable_Existing()
    {
        // Arrange
        var variable = Guid.NewGuid().ToString();
        var value = Guid.NewGuid().ToString();
        try
        {
            Environment.SetEnvironmentVariable(variable, value);

            var template = $"{{{{GetEnvironmentVariable \"{variable}\"}}}}";
            var compiled = _handlebarsContext.Compile(template);
            var data = new { };

            // Act
            var result = compiled(data);

            // Assert
            result.Should().Be(value);

        }
        finally
        {
            Environment.SetEnvironmentVariable(variable, null);
        }
    }

    [Fact]
    public void GetEnvironmentVariable_CategoryNotDefined_ShouldThrowException()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, o =>
        {
            o.UseCategoryPrefix = false;
        });

        var variable = Guid.NewGuid().ToString();
        var template = $"{{{{GetEnvironmentVariable \"{variable}\"}}}}";
        var compiled = handlebarsContext.Compile(template);
        var data = new { };

        // Act
        Action act = () => compiled(data);

        // Assert
        act.Should().Throw<HandlebarsRuntimeException>().WithMessage("Template references a helper that cannot be resolved. Helper 'GetEnvironmentVariable'");
    }
}