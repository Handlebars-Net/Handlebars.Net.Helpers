using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class DictionaryHelpersTests
{
    private readonly DictionaryHelpers _sut;

    public DictionaryHelpersTests()
    {
        var contextMock = new Mock<IHandlebars>();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.InvariantCulture });

        _sut = new DictionaryHelpers(contextMock.Object);
    }


    [Theory]
    [InlineData("1", "", "one")]
    [InlineData("2", "", "two")]
    [InlineData("3", null, null)]
    [InlineData("3", "", "")]
    [InlineData("3", "not found", "not found")]
    public void Lookup_Dictionary(string key, object? defaultValue, string? expected)
    {
        // Arrange
        var data = new Dictionary<string, object>
        {
            { "1", "one" },
            { "2", "two" }
        };

        // Act
        var result = _sut.Lookup(data, key, defaultValue);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("1", "", "one")]
    [InlineData("2", "", "two")]
    [InlineData("3", null, null)]
    [InlineData("3", "", "")]
    [InlineData("3", "not found", "not found")]
    public void Lookup_JObject(string key, string? defaultValue, string? expected)
    {
        // Arrange
        var data = new JObject
        {
            { "1", "one" },
            { "2", "two" }
        };

        // Act
        var result = _sut.Lookup(data, key, defaultValue);

        // Assert
        result.Should().Be(expected);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData("1", "", "one")]
    [InlineData("2", "", "two")]
    [InlineData("3", null, null)]
    [InlineData("3", "", "")]
    [InlineData("3", "not found", "not found")]
    public void Lookup_JsonObject(string key, string? defaultValue, string? expected)
    {
        // Arrange
        var data = new System.Text.Json.Nodes.JsonObject
        {
            { "1", "one" },
            { "2", "two" }
        };

        // Act
        var result = _sut.Lookup(data, key, defaultValue);

        // Assert
        result.Should().Be(expected);
    }
#endif
}