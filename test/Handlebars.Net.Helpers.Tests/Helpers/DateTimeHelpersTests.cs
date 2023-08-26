using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class DateTimeHelpersTests
{
    private static readonly DateTime DateTimeNow = new DateTime(2020, 4, 15, 23, 59, 58);
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;

    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IHandlebars> _contextMock;

    private readonly DateTimeHelpers _sut;

    public DateTimeHelpersTests()
    {
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _dateTimeServiceMock.Setup(d => d.Now()).Returns(DateTimeNow);
        _dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(DateTimeNow.ToUniversalTime);

        _contextMock = new Mock<IHandlebars>();
        _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = FormatProvider });

        _sut = new DateTimeHelpers(_contextMock.Object, _dateTimeServiceMock.Object);
    }

    [Fact]
    public void Now()
    {
        // Act
        var result = _sut.Now() as DateTime?;

        // Assert
        result.Should().Be(DateTimeNow);
    }

    [Theory]
    [InlineData("d", "04/15/2020")]
    [InlineData("o", "2020-04-15T23:59:58.0000000")]
    [InlineData("MM-dd-yyyy", "04-15-2020")]
    public void Now_Format(string format, string expected)
    {
        // Act
        var result = _sut.Now(format) as string;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("d", "04/15/2020")]
    [InlineData("o", "2020-04-15T23:59:58.0000000")]
    [InlineData("MM-dd-yyyy", "04-15-2020")]
    public void Format_With_ValueAsDateTime(string format, string expected)
    {
        // Arrange
        var dateTime = DateTimeNow;

        // Act
        var result = _sut.Format(dateTime, format);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("d", "04/15/2020")]
    [InlineData("t", "23:59")]
    [InlineData("o", "2020-04-15T23:59:58.0000000")]
    [InlineData("MM-dd-yyyy", "04-15-2020")]
    public void Format_With_ValueAsString(string format, string expected)
    {
        // Arrange
        var dateTime = "2020-04-15T23:59:58.0000000";

        // Act
        var result = _sut.Format(dateTime, format);

        // Assert
        result.Should().Be(expected);
    }
}