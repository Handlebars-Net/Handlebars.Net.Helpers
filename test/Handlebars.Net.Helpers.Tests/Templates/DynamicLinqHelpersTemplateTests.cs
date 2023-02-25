using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class DynamicLinqHelpersTemplateTests
{
    private readonly DateTime _dateTimeNow = new(2020, 4, 15, 13, 14, 15);

    private readonly IHandlebars _handlebarsContext;

    public DynamicLinqHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();

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

    [Fact]
    public void Linq()
    {
        // Arrange
        var request = new
        {
            Path = "/test"
        };

        var action = _handlebarsContext.Compile("{{Linq Path 'it'}}");

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("/test");
    }

    [Fact]
    public void Linq1()
    {
        // Arrange
        var request = new
        {
            body = new JObject
            {
                { "Id", new JValue(9) },
                { "Name", new JValue("Test") }
            }
        };

        var action = _handlebarsContext.Compile("{{Linq body 'it.Name + \"_123\"' }}");

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("Test_123");
    }

    [Fact]
    public void Linq_DateTime_AddHour()
    {
        // Arrange
        var request = new
        {
        };

        var action = _handlebarsContext.Compile("{{Linq (Now) 'AddHours(1)' }}");

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("2020-04-15T14:14:15.0000000");
    }
}