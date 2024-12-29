using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class UrlHelpersTests
{
    private readonly UrlHelpers _sut;

    public UrlHelpersTests()
    {
        var contextMock = new Mock<IHandlebars>();

        _sut = new UrlHelpers(contextMock.Object);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("arinet%2FHandlebarDocs%2Fblob%2Fmaster%2FcustomHelpers%2Furl.md%23decodeuri", "arinet/HandlebarDocs/blob/master/customHelpers/url.md#decodeuri")]
    [InlineData("%2Fsearch%2Finventory%2Fbrand%2FPolaris+Industries%2Fsort%2Fbest-match", "/search/inventory/brand/Polaris Industries/sort/best-match")]
    public void DecodeUri(string value, string expected)
    {
        // Act
        var result = _sut.DecodeUri(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("arinet/HandlebarDocs/blob/master/customHelpers/url.md#decodeuri", "arinet%2FHandlebarDocs%2Fblob%2Fmaster%2FcustomHelpers%2Furl.md%23decodeuri")]
    [InlineData("/search/inventory/brand/Polaris Industries/sort/best-match", "%2Fsearch%2Finventory%2Fbrand%2FPolaris+Industries%2Fsort%2Fbest-match")]
    public void EncodeUri(string value, string expected)
    {
        // Act
        var result = _sut.EncodeUri(value);

        // Assert
        result.Should().Be(expected);
    }
}