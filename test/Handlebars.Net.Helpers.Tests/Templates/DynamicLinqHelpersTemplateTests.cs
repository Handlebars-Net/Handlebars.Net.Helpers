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
    public void LinqIt()
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
    public void LinqItContains()
    {
        // Arrange
        var request = new
        {
            Path = "/test"
        };

        var action = _handlebarsContext.Compile("{{Linq Path 'it.Contains(\"e\")'}}");

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("True");
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

    [Theory]
    [InlineData("{{Where a 'it.Contains(\"s\")'}}", "stef,test")]
    [InlineData("{{Where d 'Year <= 2022'}}", "2022-01-01T00:00:00.0000000")]
    public void Linq_Where(string linqStatement, string expected)
    {
        // Arrange
        var request = new
        {
            x = DateTime.Now,
            a = new[] { "stef", "test", "other" },
            d = new[] { new DateTime(2022, 1, 1), DateTime.Now }
        };

        var action = _handlebarsContext.Compile(linqStatement);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{Expression '1 + 2'}}", "3")]
    [InlineData("{{Expression '(1 > 2).ToString().ToLower()'}}", "false")]
    public void Expression(string expression, string expected)
    {
        // Arrange
        var request = true;

        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be(expected);
    }
}