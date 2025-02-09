using FluentAssertions;
using HandlebarsDotNet.Helpers.Options;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class XegerHelpersTests
{
    private readonly Mock<IHandlebars> _contextMock;

    private readonly XegerHelpers _sut;

    public XegerHelpersTests()
    {
        _contextMock = new Mock<IHandlebars>();
        _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

        _sut = new XegerHelpers(_contextMock.Object, HandlebarsHelpersOptions.Default);
    }

    [Fact]
    public void Xeger_Generate_Test1()
    {
        // Act
        var result = _sut.Generate("[1-9]{1}\\d{3}");

        // Assert
        int.Parse(result).Should().BeInRange(1000, 9999);
    }

    [Fact]
    public void Xeger_Generate_Test2()
    {
        // Act
        var result = _sut.Generate("{[\"]A[\"]:[\"]A[0-9]{3}[1-9][\"]}");

        // Assert
        result.Should().StartWith("{").And.EndWith("}").And.Contain(":");
    }
}