﻿using System;
using System.Globalization;
using CultureAwareTesting.xUnit;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class DateTimeHelpersTemplateTests
{
    private readonly DateTime _dateTimeNow = new(2020, 4, 15, 23, 59, 58);

    private readonly IHandlebars _handlebarsContext;

    public DateTimeHelpersTemplateTests()
    {
        var dateTimeServiceMock = new Mock<IDateTimeService>();
        dateTimeServiceMock.Setup(d => d.Now()).Returns(_dateTimeNow);
        dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(_dateTimeNow.ToUniversalTime);

        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext, o =>
        {
            o.UseCategoryPrefix = true;
            o.DateTimeService = dateTimeServiceMock.Object;
        });
    }

    [Theory]
    [InlineData("{{DateTime.Now}}", "2020-04-15T23:59:58.0000000")]
    [InlineData("{{DateTime.Now \"yyyy-MM-dd\"}}", "2020-04-15")]
    public void Now(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Format_AsDateTime_Template()
    {
        // Arrange
        var model = new
        {
            x = _dateTimeNow
        };

        var action = _handlebarsContext.Compile("{{DateTime.Format x \"yyyy-MMM-dd\"}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("2020-Apr-15");
    }

    [Fact]
    public void Format_AsString_Template()
    {
        // Arrange
        var model = new
        {
            x = "2020-04-15T11:12:13"
        };

        var action = _handlebarsContext.Compile("{{DateTime.Format x \"yyyy-MMM-dd\"}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("2020-Apr-15");
    }

    [Fact]
    public void Format_AsOther_Template()
    {
        // Arrange
        var model = new
        {
            x = 42
        };

        var action = _handlebarsContext.Compile("{{DateTime.Format x \"yyyy-MMM-dd\"}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData("{{DateTime.GreaterThan \"2000-01-02\" \"2000-01-01\"}}", "True")]
    [InlineData("{{DateTime.NotEqual \"2000-01-01\" \"2000-01-01\"}}", "False")]
    public void Compare(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{DateTime.GreaterThan \"02/01/2000\" \"01/01/2000\" \"dd/MM/yyyy\"}}", "True")]
    [InlineData("{{DateTime.NotEqual \"01/01/2000\" \"01/01/2000\" \"dd/MM/yyyy\"}}", "False")]
    public void Compare_Formatted(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [CulturedTheory("en-us")]
    [InlineData("{{DateTime.AddYears \"2000-01-01\" 1}}", "2001-01-01")]
    [InlineData("{{DateTime.AddMonths \"2000-01-01\" 1}}", "2000-02-01")]
    [InlineData("{{DateTime.AddDays \"2000-01-01\" 1}}", "2000-01-02")]
    public void Add(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Contain(expected);
    }

    [CulturedTheory("en-us")]
    [InlineData("{{DateTime.AddYears \"01/01/2000\" 1 \"dd/MM/yyyy\"}}", "2001-01-01")]
    [InlineData("{{DateTime.AddMonths \"01/01/2000\" 1 \"dd/MM/yyyy\"}}", "2000-02-01")]
    [InlineData("{{DateTime.AddDays \"01/01/2000\" 1 \"dd/MM/yyyy\"}}", "2000-01-02")]
    public void Add_Formatted(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Contain(expected);
    }

    [CulturedFact("en-us")]
    public void Parse()
    {
        // Arrange
        var model = new
        {
            x = "2000-01-01 4PM"
        };

        var action = _handlebarsContext.Compile("{{DateTime.Parse x}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().StartWith("2000-01-01T16:00:00");
    }

    [CulturedFact("en-us")]
    public void ParseExact()
    {
        // Arrange
        var model = new
        {
            x = "2000 01 01"
        };

        var action = _handlebarsContext.Compile("{{DateTime.ParseExact x \"yyyy MM dd\"}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().StartWith("2000-01-01T00:00:00");
    }
}