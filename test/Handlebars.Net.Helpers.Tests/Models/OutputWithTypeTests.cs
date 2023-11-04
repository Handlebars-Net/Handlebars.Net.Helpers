using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Models;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Models;

[ExcludeFromCodeCoverage]
public class OutputWithTypeTests
{
    [Fact]
    public void Deserialize_ForString_ShouldReturnOutputWithType_WhenJsonIsValid()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"TypeName\":\"String\",\"FullTypeName\":\"System.String\"}";

        // Act
        var result = OutputWithType.Deserialize(json);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeOfType<string>().And.Be("test");
        result.TypeName.Should().Be("String");
        result.FullTypeName.Should().Be("System.String");
    }

    [Fact]
    public void Deserialize_ForInteger_ShouldReturnOutputWithType_WhenJsonIsValid()
    {
        // Arrange
        var json = "{\"Value\":\"12345\",\"TypeName\":\"Int32\",\"FullTypeName\":\"System.Int32\"}";

        // Act
        var result = OutputWithType.Deserialize(json);

        result.Should().NotBeNull();
        result.Value.Should().BeOfType<int>().And.Be(12345);
        result.TypeName.Should().Be("Int32");
        result.FullTypeName.Should().Be("System.Int32");
    }

    [Fact]
    public void Deserialize_WhenValueCannotBeConvertedToFullType_ShouldKeepOriginalValue()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"TypeName\":\"Int32\",\"FullTypeName\":\"System.Int32\"}";

        // Act
        var result = OutputWithType.Deserialize(json);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeOfType<string>().And.Be("test");
        result.TypeName.Should().Be("Int32");
        result.FullTypeName.Should().Be("System.Int32");
    }

    [Fact]
    public void Deserialize_ShouldThrowMissingMemberException_WhenValueIsMissing()
    {
        // Arrange
        var json = "{\"TypeName\":\"String\",\"FullTypeName\":\"System.String\"}";

        // Act
        Action act = () => OutputWithType.Deserialize(json);

        // Assert
        act.Should().Throw<MissingMemberException>()
           .WithMessage("*'Value'*");
    }

    [Fact]
    public void Deserialize_ShouldThrowMissingMemberException_WhenTypeNameIsMissing()
    {
        // Arrange
        var json = "{\"Value\":\"12345\",\"FullTypeName\":\"System.Int32\"}";

        // Act
        Action act = () => OutputWithType.Deserialize(json);

        // Assert
        act.Should().Throw<MissingMemberException>()
            .WithMessage("*'TypeName'*");
    }

    [Fact]
    public void Deserialize_ShouldThrowMissingMemberException_WhenFullTypeNameIsMissing()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"TypeName\":\"String\"}";

        // Act
        Action act = () => OutputWithType.Deserialize(json);

        // Assert
        act.Should().Throw<MissingMemberException>()
            .WithMessage("*'FullTypeName'*");
    }

    [Fact]
    public void Deserialize_ShouldThrowInvalidOperationException_WhenJsonIsNull()
    {
        // Act
        Action act = () => OutputWithType.Deserialize(null);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Deserialize_ShouldThrowJsonException_WhenJsonIsInvalid()
    {
        // Act
        Action act = () => OutputWithType.Deserialize("invalid json");

        // Assert
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void TryDeserialize_ForString_ShouldReturnTrue_WhenJsonIsValid()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"TypeName\":\"String\",\"FullTypeName\":\"System.String\"}";

        // Act
        var result = OutputWithType.TryDeserialize(json, out var output);

        // Assert
        result.Should().BeTrue();
        output!.Value.Should().NotBeNull().And.BeOfType<string>().And.Be("test");
        output.TypeName.Should().Be("String");
        output.FullTypeName.Should().Be("System.String");
    }

    [Fact]
    public void TryDeserialize_ForInteger_ShouldReturnTrue()
    {
        // Arrange
        var json = "{\"Value\":\"12345\",\"TypeName\":\"Int32\",\"FullTypeName\":\"System.Int32\"}";

        // Act
        var result = OutputWithType.TryDeserialize(json, out var output);

        result.Should().BeTrue();
        output!.Value.Should().BeOfType<int>().And.Be(12345);
        output.TypeName.Should().Be("Int32");
        output.FullTypeName.Should().Be("System.Int32");
    }

    [Fact]
    public void TryDeserialize_WhenValueCannotBeConvertedToFullType_ShouldReturnTrue_And_KeepOriginalValue()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"TypeName\":\"Int32\",\"FullTypeName\":\"System.Int32\"}";

        // Act
        var result = OutputWithType.TryDeserialize(json, out var output);

        // Assert
        result.Should().BeTrue();
        output!.Value.Should().BeOfType<string>().And.Be("test");
        output.TypeName.Should().Be("Int32");
        output.FullTypeName.Should().Be("System.Int32");
    }

    [Fact]
    public void TryDeserialize_ShouldReturnFalse_WhenValueIsMissing()
    {
        // Arrange
        var json = "{\"TypeName\":\"String\",\"FullTypeName\":\"System.String\"}";

        // Act
        var result = OutputWithType.TryDeserialize(json, out var output);

        // Assert
        result.Should().BeFalse();
        output.Should().BeNull();
    }

    [Fact]
    public void TryDeserialize_ShouldReturnFalse_WhenTypeNameIsMissing()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"FullTypeName\":\"System.String\"}";

        // Act
        var result = OutputWithType.TryDeserialize(json, out var output);

        // Assert
        result.Should().BeFalse();
        output.Should().BeNull();
    }

    [Fact]
    public void TryDeserialize_ShouldReturnFalse_WhenFullTypeNameIsMissing()
    {
        // Arrange
        var json = "{\"Value\":\"test\",\"TypeName\":\"String\"}";

        // Act
        var result = OutputWithType.TryDeserialize(json, out var output);

        // Assert
        result.Should().BeFalse();
        output.Should().BeNull();
    }

    [Fact]
    public void TryDeserialize_ShouldReturnFalse_WhenJsonIsNull()
    {
        // Act
        var result = OutputWithType.TryDeserialize(null, out var output);

        // Assert
        result.Should().BeFalse();
        output.Should().BeNull();
    }

    [Fact]
    public void TryDeserialize_ShouldReturnFalse_WhenJsonIsInvalid()
    {
        // Act
        var result = OutputWithType.TryDeserialize("invalid json", out var output);

        // Assert
        result.Should().BeFalse();
        output.Should().BeNull();
    }
}