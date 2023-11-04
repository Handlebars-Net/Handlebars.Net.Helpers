using System;
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
    public void Random_StringList()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.Generate Type=\"StringList\" Values=[\"a\", \"b\", \"c\"]}}");

        // Act
        var result = action("");

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Random_StringList_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"StringList\" Values=[\"a\", \"b\", \"c\"]}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<string>().Which.Should().NotBeNullOrEmpty();
        outputWithType.TypeName.Should().Be("String");
        outputWithType.FullTypeName.Should().Be("System.String");
    }

    [Fact]
    public void Random_Integer_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Integer\" Min=1000 Max=9999 }}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<int>().Which.Should().BeInRange(1000, 9999);
        outputWithType.TypeName.Should().Be("Int32");
        outputWithType.FullTypeName.Should().Be("System.Int32");
    }

    [Fact]
    public void Random_Guid_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Guid\" }}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<Guid>();
        outputWithType.TypeName.Should().Be("Guid");
        outputWithType.FullTypeName.Should().Be("System.Guid");
    }

    [Fact]
    public void Random_City_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"City\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<string>().Which.Should().NotBeNullOrEmpty();
        outputWithType.TypeName.Should().Be("String");
        outputWithType.FullTypeName.Should().Be("System.String");
    }

    [Fact]
    public void Random_Boolean_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Boolean\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<bool>();
        outputWithType.TypeName.Should().Be("Boolean");
        outputWithType.FullTypeName.Should().Be("System.Boolean");
    }

    [Fact]
    public void Random_Byte_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Byte\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<byte>().Which.Should().BeInRange(byte.MinValue, byte.MaxValue);
        outputWithType.TypeName.Should().Be("Byte");
        outputWithType.FullTypeName.Should().Be("System.Byte");
    }

    [Fact]
    public void Random_Bytes_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Bytes\" Min=1 Max=9 }}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<byte[]>().Which.Should().NotBeNullOrEmpty();
        outputWithType.TypeName.Should().Be("Byte[]");
        outputWithType.FullTypeName.Should().Be("System.Byte[]");
    }

    [Fact]
    public void Random_Double_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Double\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<double>();
        outputWithType.TypeName.Should().Be("Double");
        outputWithType.FullTypeName.Should().Be("System.Double");
    }

    [Fact]
    public void Random_Float_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"Float\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<float>();
        outputWithType.TypeName.Should().Be("Single");
        outputWithType.FullTypeName.Should().Be("System.Single");
    }

    [Fact]
    public void Random_DateTime_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"DateTime\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<DateTime>();
        outputWithType.TypeName.Should().Be("DateTime");
        outputWithType.FullTypeName.Should().Be("System.DateTime");
    }

    [Fact]
    public void Random_TimeSpan_OutputWithType()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{Random.GenerateAsOutputWithType Type=\"TimeSpan\"}}");

        // Act
        var result = action("");

        // Assert
        var outputWithType = OutputWithType.Deserialize(result);
        outputWithType.Value.Should().BeOfType<TimeSpan>();
        outputWithType.TypeName.Should().Be("TimeSpan");
        outputWithType.FullTypeName.Should().Be("System.TimeSpan");
    }
}