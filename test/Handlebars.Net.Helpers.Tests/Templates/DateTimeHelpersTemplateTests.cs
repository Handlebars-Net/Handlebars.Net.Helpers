using System;
using System.Globalization;
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
            o.UseCategoryPrefix = false;
            o.DateTimeService = dateTimeServiceMock.Object;
        });
    }

    [Theory]
    [InlineData("{{Now}}", "2020-04-15T23:59:58.0000000")]
    [InlineData("{{Now \"yyyy-MM-dd\"}}", "2020-04-15")]
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

        var action = _handlebarsContext.Compile("{{Format x \"yyyy-MMM-dd\"}}");

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

        var action = _handlebarsContext.Compile("{{Format x \"yyyy-MMM-dd\"}}");

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

        var action = _handlebarsContext.Compile("{{Format x \"yyyy-MMM-dd\"}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be(string.Empty);
    }
}