using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class DateTimeHelpersTemplateTests
{
    private readonly DateTime DateTimeNow = new DateTime(2020, 4, 15, 23, 59, 58);

    private readonly Mock<IDateTimeService> _dateTimeServiceMock;

    private readonly IHandlebars _handlebarsContext;

    public DateTimeHelpersTemplateTests()
    {
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _dateTimeServiceMock.Setup(d => d.Now()).Returns(DateTimeNow);
        _dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(DateTimeNow.ToUniversalTime);

        _handlebarsContext = Handlebars.Create();

        HandlebarsHelpers.Register(_handlebarsContext, o =>
        {
            o.UseCategoryPrefix = false;
            o.DateTimeService = _dateTimeServiceMock.Object;
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