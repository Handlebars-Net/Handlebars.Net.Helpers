using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.PathStructure;
using HandlebarsDotNet.PathStructure;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class PathHelpersTests
{
    private readonly Mock<IPathResolverProxy> _pathResolverProxyMock;

    private readonly PathHelpers _sut;

    public PathHelpersTests()
    {
        _pathResolverProxyMock = new Mock<IPathResolverProxy>();

        var contextMock = new Mock<IHandlebars>();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.InvariantCulture });

        _sut = new PathHelpers(contextMock.Object, _pathResolverProxyMock.Object);
    }


    [Theory]
    [InlineData(true, "x")]
    [InlineData(false, "not found")]
    public void Lookup(bool tryAccessMemberResult, string expected)
    {
        // Arrange
        object value = "x";
        _pathResolverProxyMock.Setup(proxy => proxy.TryAccessMember(It.IsAny<BindingContext>(), It.IsAny<object>(), It.IsAny<ChainSegment>(), out value)).Returns(tryAccessMemberResult);

        // Act
        var result = _sut.Lookup(new HelperOptions(), "data", "path", "not found");

        // Assert
        result.Should().Be(expected);
    }
}