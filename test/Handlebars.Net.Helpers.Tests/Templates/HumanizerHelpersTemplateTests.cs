using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class HumanizerHelpersTemplateTests
{
    private readonly DateTime DateTimeNow = new DateTime(2020, 4, 15, 23, 59, 58);

    private readonly Mock<IDateTimeService> _dateTimeServiceMock;

    private readonly IHandlebars _handlebarsContext;

    public HumanizerHelpersTemplateTests()
    {
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _dateTimeServiceMock.Setup(d => d.Now()).Returns(DateTimeNow);
        _dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(DateTimeNow.ToUniversalTime);

        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext, o =>
        {
            o.DateTimeService = _dateTimeServiceMock.Object;
        });
    }

    [Fact]
    public void HumanizeDateTime()
    {
        var template = $"{{{{[Humanizer.Humanize] \"{DateTime.UtcNow.AddDays(-1):O}\" }}}}";

        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be("yesterday");
    }

    

    [Theory]
    [InlineData("{{[Humanizer.Humanize] \"HTML\"}}", "HTML")]
    [InlineData("{{[Humanizer.Humanize] \"PascalCaseInputStringIsTurnedIntoSentence\"}}", "Pascal case input string is turned into sentence")]
    public void HumanizeString(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[Humanizer.Truncate] \"This is a long sentence that needs truncating.\" 10}}", "This is a…")]
    [InlineData("{{[Humanizer.Truncate] \"Short sentence.\" 20}}", "Short sentence.")]
    [InlineData("{{[Humanizer.Truncate] \"Exact length.\" 13}}", "Exact length.")]
    [InlineData("{{[Humanizer.Truncate] \"This sentence will be truncated to zero.\" 0}}", "")]
    [InlineData("{{[Humanizer.Truncate] \"This is a test for truncating with custom ellipsis.\" 15 \"[...]\"}}", "This is a [...]")]
    public void HumanizeTruncate(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void HumanizeTruncateWithCategoryPrefix()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, o =>
        {
            o.PrefixSeparator = "_";
        });
        var template = "{{[Humanizer_Truncate] \"This is a long sentence that needs truncating.\" 10}}";
        var action = handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be("This is a…");
    }
}