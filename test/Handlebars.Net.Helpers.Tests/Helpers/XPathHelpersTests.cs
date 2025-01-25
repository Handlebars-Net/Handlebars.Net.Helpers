using FluentAssertions;
using HandlebarsDotNet.Helpers.Options;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class XPathHelpersTests
{
    private readonly Mock<IHandlebars> _contextMock;

    private readonly XPathHelpers _sut;

    public XPathHelpersTests()
    {
        _contextMock = new Mock<IHandlebars>();
        _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

        _sut = new XPathHelpers(_contextMock.Object, HandlebarsHelpersOptions.Default);
    }

    [Fact]
    public void Evaluate_ToBool()
    {
        // Assign
        var xml = @"
        <todo-list>
            <todo-item id='a1'>abc</todo-item>
        </todo-list>";

        // Act
        var result = _sut.Evaluate(xml, "boolean(/todo-list[count(todo-item) = 1])") as bool?;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Evaluate_ToString()
    {
        // Act
        var result = _sut.Evaluate("<x><a id=\"1\"></a></x>", "//a/@id")?.ToString();

        // Assert
        result.Should().Be("1");
    }

    [Fact]
    public void EvaluateToString_ReturnString()
    {
        // Act
        var result = _sut.EvaluateToString("<x><a id=\"1\"></a></x>", "//a/@id");

        // Assert
        result.Should().Be("1");
    }

    [Fact]
    public void EvaluateToString_ReturnStringArray()
    {
        // Act
        var result = _sut.EvaluateToString("<x><a id=\"1\"></a><a id=\"2\"></a></x>", "//a/@id");

        // Assert
        result.Should().Be("1, 2");
    }
}