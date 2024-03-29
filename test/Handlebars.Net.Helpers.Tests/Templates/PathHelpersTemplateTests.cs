﻿using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class PathHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public PathHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext);
    }

    [Theory]
    [InlineData("{{[Lookup] data '_1' }}", "one")]
    [InlineData("{{[Lookup] data '_2' }}", "two")]
    [InlineData("{{[Lookup] data '_3' }}", "")]
    [InlineData("{{[Lookup] data '_3' 'not found'}}", "not found")]
    public void Lookup_Object(string template, string expected)
    {
        // Arrange
        var dictionary = new
        {
            _1 = "one",
            _2 = "two"
        };
        var model = new
        {
            data = dictionary
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[Lookup] data '1' }}", "one")]
    [InlineData("{{[Lookup] data '2' }}", "two")]
    [InlineData("{{[Lookup] data '3' }}", "")]
    [InlineData("{{[Lookup] data '3' 'not found'}}", "not found")]
    public void Lookup_Dictionary(string template, string expected)
    {
        // Arrange
        var dictionary = new Dictionary<string, object?>
        {
            { "1", "one" },
            { "2", "two" }
        };
        var model = new
        {
            data = dictionary
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[Lookup] data '1' }}", "one")]
    [InlineData("{{[Lookup] data '2' }}", "two")]
    [InlineData("{{[Lookup] data '3' }}", "")]
    [InlineData("{{[Lookup] data '3' 'not found'}}", "not found")]
    public void Lookup_JObject(string template, string expected)
    {
        // Arrange
        var jObject = new JObject
        {
            { "1", "one" },
            { "2", "two" }
        };
        var model = new
        {
            data = jObject
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData("{{[Lookup] data '1' }}", "one")]
    [InlineData("{{[Lookup] data '2' }}", "two")]
    [InlineData("{{[Lookup] data '3' }}", "")]
    [InlineData("{{[Lookup] data '3' 'not found'}}", "not found")]
    public void Lookup_JsonObject(string template, string expected)
    {
        // Arrange
        var jsonObject = new System.Text.Json.Nodes.JsonObject
        {
            { "1", "one" },
            { "2", "two" }
        };
        var model = new
        {
            data = jsonObject
        };

        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(expected);
    }
#endif
}