using System;
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
}