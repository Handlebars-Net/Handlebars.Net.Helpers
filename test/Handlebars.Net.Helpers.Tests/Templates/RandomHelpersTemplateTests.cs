using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Models;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class RandomHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public RandomHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();

        HandlebarsHelpers.Register(_handlebarsContext, Category.Random);
    }

    [Fact]
    public void Random_Integer()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.Generate Type=\"Integer\" Min=1000 Max=9999}}");

        // Act
        var result = action("");

        // Assert
        int.Parse(result).Should().BeInRange(1000, 9999);
    }

    [Fact]
    public void Random_Integer_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Integer\" Min=1000 Max=9999}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result)!;
        outputWithType.Value.Should().BeOfType<long>().Which.Should().BeInRange(1000, 9999);
        outputWithType.Type.Should().Be("Int32");
        outputWithType.FullType.Should().Be("System.Int32");
    }

    [Fact]
    public void Random_StringList()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.Generate Type=\"StringList\" Values=[\"a\", \"b\", \"c\"]}}");

        // Act
        var result = action("");

        // Assert
        result.Should().NotBeEmpty("a");
    }
}