using FluentAssertions;
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

        _sut = new XPathHelpers(_contextMock.Object);
    }

    [Fact]
    public void Evaluate()
    {
        // Assign
        string xml = @"
                    <todo-list>
                        <todo-item id='a1'>abc</todo-item>
                    </todo-list>";

        // Act
        var result = _sut.Evaluate(xml, "boolean(/todo-list[count(todo-item) = 1])") as bool?;

        // Assert
        result.Should().BeTrue();
    }
}