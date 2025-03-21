using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using System;
using System.Globalization;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class DateTimeHelpersTests
{
    private static readonly DateTime DateTimeNow = new(2020, 4, 15, 23, 59, 58);
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

        var options = new HandlebarsHelpersOptions
        {
            DateTimeService = _dateTimeServiceMock.Object
        };

        _sut = new DateTimeHelpers(_contextMock.Object, options);
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

    [Theory]
    [InlineData("1:15:30 2000-02-01", 2000, 2, 1, 1, 15, 30, 0)]
    [InlineData("2000-02-01 1:15:30", 2000, 2, 1, 1, 15, 30, 0)]
    [InlineData("2000-02-01", 2000, 2, 1, 0, 0, 0, 0)]
    public void Parse_With_ValuesAsString(string value, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMilliseconds)
    {
        // Act
        var result = _sut.Parse(value);

        // Assert
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMilliseconds);
    }

    [Theory]
    [InlineData("2000 02 01", "yyyy MM dd", 2000, 2, 1, 0, 0, 0, 0)]
    [InlineData("01||02||2000 1-15-30", "dd||MM||yyyy h-mm-ss", 2000, 2, 1, 1, 15, 30, 0)]
    public void ParseExact_With_ValuesAsFormattedString(string value, string format, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMilliseconds)
    {
        // Act
        var result = _sut.ParseExact(value, format);

        // Assert
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMilliseconds);
    }

    [Theory]
    [InlineData(nameof(DateTimeHelpers.AddYears), -1, 1999, 1, 1, 0, 0, 0, 0, 630507456000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 1, 2001, 1, 1, 0, 0, 0, 0, 631139040000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), -1, 1999, 12, 1, 0, 0, 0, 0, 630796032000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 1, 2000, 2, 1, 0, 0, 0, 0, 630849600000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddDays), -1, 1999, 12, 31, 0, 0, 0, 0, 630821952000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 1, 2000, 1, 2, 0, 0, 0, 0, 630823680000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddHours), -1, 1999, 12, 31, 23, 0, 0, 0, 630822780000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 1, 2000, 1, 1, 1, 0, 0, 0, 630822852000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), -1, 1999, 12, 31, 23, 59, 0, 0, 630822815400000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 1, 2000, 1, 1, 0, 1, 0, 0, 630822816600000000L)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), -1, 1999, 12, 31, 23, 59, 59, 0, 630822815990000000L)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 1, 2000, 1, 1, 0, 0, 1, 0, 630822816010000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999990000L)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 1, 2000, 1, 1, 0, 0, 0, 1, 630822816000010000L)]
    [InlineData(nameof(DateTimeHelpers.AddTicks), -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999999999L)]
    [InlineData(nameof(DateTimeHelpers.AddTicks), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddTicks), 1, 2000, 1, 1, 0, 0, 0, 0, 630822816000000001L)]
    public void Add(string method, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds, long expectedTicks)
    {
        // Arrange
        object value = "2000-01-01 00:00:00";
        string? format = null;

        // Act
        DateTime result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);

        // Arrange
        value = DateTime.Parse("2000-01-01 00:00:00");

        // Act
        result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);

        // Arrange
        value = (DateTime?)DateTime.Parse("2000-01-01 00:00:00");

        // Act
        result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);
    }

    [Theory]
    [InlineData(nameof(DateTimeHelpers.AddYears), -1, 1999, 1, 1, 0, 0, 0, 0, 630507456000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 1, 2001, 1, 1, 0, 0, 0, 0, 631139040000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), -1, 1999, 12, 1, 0, 0, 0, 0, 630796032000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 1, 2000, 2, 1, 0, 0, 0, 0, 630849600000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddDays), -1, 1999, 12, 31, 0, 0, 0, 0, 630821952000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 1, 2000, 1, 2, 0, 0, 0, 0, 630823680000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddHours), -1, 1999, 12, 31, 23, 0, 0, 0, 630822780000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 1, 2000, 1, 1, 1, 0, 0, 0, 630822852000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), -1, 1999, 12, 31, 23, 59, 0, 0, 630822815400000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 1, 2000, 1, 1, 0, 1, 0, 0, 630822816600000000L)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), -1, 1999, 12, 31, 23, 59, 59, 0, 630822815990000000L)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 1, 2000, 1, 1, 0, 0, 1, 0, 630822816010000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999990000L)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 1, 2000, 1, 1, 0, 0, 0, 1, 630822816000010000L)]
    [InlineData(nameof(DateTimeHelpers.AddTicks), -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999999999L)]
    [InlineData(nameof(DateTimeHelpers.AddTicks), 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData(nameof(DateTimeHelpers.AddTicks), 1, 2000, 1, 1, 0, 0, 0, 0, 630822816000000001L)]
    public void Add_WithValueAsStringAndFormat(string method, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds, long expectedTicks)
    {
        // Arrange
        object value = "2000||01||01 00-00-00";

        string format = "yyyy||MM||dd hh-mm-ss";

        // Act
        DateTime result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);
    }

    [Theory]
    [InlineData("years", -1, 1999, 1, 1, 0, 0, 0, 0, 630507456000000000L)]
    [InlineData("years", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("years", 1, 2001, 1, 1, 0, 0, 0, 0, 631139040000000000L)]
    [InlineData("months", -1, 1999, 12, 1, 0, 0, 0, 0, 630796032000000000L)]
    [InlineData("months", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("months", 1, 2000, 2, 1, 0, 0, 0, 0, 630849600000000000L)]
    [InlineData("days", -1, 1999, 12, 31, 0, 0, 0, 0, 630821952000000000L)]
    [InlineData("days", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("days", 1, 2000, 1, 2, 0, 0, 0, 0, 630823680000000000L)]
    [InlineData("hours", -1, 1999, 12, 31, 23, 0, 0, 0, 630822780000000000L)]
    [InlineData("hours", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("hours", 1, 2000, 1, 1, 1, 0, 0, 0, 630822852000000000L)]
    [InlineData("minutes", -1, 1999, 12, 31, 23, 59, 0, 0, 630822815400000000L)]
    [InlineData("minutes", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("minutes", 1, 2000, 1, 1, 0, 1, 0, 0, 630822816600000000L)]
    [InlineData("seconds", -1, 1999, 12, 31, 23, 59, 59, 0, 630822815990000000L)]
    [InlineData("seconds", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("seconds", 1, 2000, 1, 1, 0, 0, 1, 0, 630822816010000000L)]
    [InlineData("milliseconds", -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999990000L)]
    [InlineData("milliseconds", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("milliseconds", 1, 2000, 1, 1, 0, 0, 0, 1, 630822816000010000L)]
    [InlineData("ticks", -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999999999L)]
    [InlineData("ticks", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("ticks", 1, 2000, 1, 1, 0, 0, 0, 0, 630822816000000001L)]
    public void Add_With_DatePart_With_ValuesAsString(string datePart, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds, long expectedTicks)
    {
        // Act
        var result = _sut.Add("2000-01-01 00:00:00", increment, datePart);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);
    }

    [Theory]
    [InlineData("years", -1, 1999, 1, 1, 0, 0, 0, 0, 630507456000000000L)]
    [InlineData("years", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("years", 1, 2001, 1, 1, 0, 0, 0, 0, 631139040000000000L)]
    [InlineData("months", -1, 1999, 12, 1, 0, 0, 0, 0, 630796032000000000L)]
    [InlineData("months", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("months", 1, 2000, 2, 1, 0, 0, 0, 0, 630849600000000000L)]
    [InlineData("days", -1, 1999, 12, 31, 0, 0, 0, 0, 630821952000000000L)]
    [InlineData("days", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("days", 1, 2000, 1, 2, 0, 0, 0, 0, 630823680000000000L)]
    [InlineData("hours", -1, 1999, 12, 31, 23, 0, 0, 0, 630822780000000000L)]
    [InlineData("hours", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("hours", 1, 2000, 1, 1, 1, 0, 0, 0, 630822852000000000L)]
    [InlineData("minutes", -1, 1999, 12, 31, 23, 59, 0, 0, 630822815400000000L)]
    [InlineData("minutes", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("minutes", 1, 2000, 1, 1, 0, 1, 0, 0, 630822816600000000L)]
    [InlineData("seconds", -1, 1999, 12, 31, 23, 59, 59, 0, 630822815990000000L)]
    [InlineData("seconds", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("seconds", 1, 2000, 1, 1, 0, 0, 1, 0, 630822816010000000L)]
    [InlineData("milliseconds", -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999990000L)]
    [InlineData("milliseconds", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("milliseconds", 1, 2000, 1, 1, 0, 0, 0, 1, 630822816000010000L)]
    [InlineData("ticks", -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999999999L)]
    [InlineData("ticks", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("ticks", 1, 2000, 1, 1, 0, 0, 0, 0, 630822816000000001L)]
    public void Add_With_DatePart_With_ValuesAsDateTime(string datePart, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds, long expectedTicks)
    {
        // Act
        var result = _sut.Add(DateTime.Parse("2000-01-01 00:00:00"), increment, datePart);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);
    }

    [Theory]
    [InlineData("years", -1, 1999, 1, 1, 0, 0, 0, 0, 630507456000000000L)]
    [InlineData("years", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("years", 1, 2001, 1, 1, 0, 0, 0, 0, 631139040000000000L)]
    [InlineData("months", -1, 1999, 12, 1, 0, 0, 0, 0, 630796032000000000L)]
    [InlineData("months", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("months", 1, 2000, 2, 1, 0, 0, 0, 0, 630849600000000000L)]
    [InlineData("days", -1, 1999, 12, 31, 0, 0, 0, 0, 630821952000000000L)]
    [InlineData("days", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("days", 1, 2000, 1, 2, 0, 0, 0, 0, 630823680000000000L)]
    [InlineData("hours", -1, 1999, 12, 31, 23, 0, 0, 0, 630822780000000000L)]
    [InlineData("hours", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("hours", 1, 2000, 1, 1, 1, 0, 0, 0, 630822852000000000L)]
    [InlineData("minutes", -1, 1999, 12, 31, 23, 59, 0, 0, 630822815400000000L)]
    [InlineData("minutes", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("minutes", 1, 2000, 1, 1, 0, 1, 0, 0, 630822816600000000L)]
    [InlineData("seconds", -1, 1999, 12, 31, 23, 59, 59, 0, 630822815990000000L)]
    [InlineData("seconds", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("seconds", 1, 2000, 1, 1, 0, 0, 1, 0, 630822816010000000L)]
    [InlineData("milliseconds", -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999990000L)]
    [InlineData("milliseconds", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("milliseconds", 1, 2000, 1, 1, 0, 0, 0, 1, 630822816000010000L)]
    [InlineData("ticks", -1, 1999, 12, 31, 23, 59, 59, 999, 630822815999999999L)]
    [InlineData("ticks", 0, 2000, 1, 1, 0, 0, 0, 0, 630822816000000000L)]
    [InlineData("ticks", 1, 2000, 1, 1, 0, 0, 0, 0, 630822816000000001L)]
    public void Add_With_DatePart_With_ValuesAsNullableDateTime(string datePart, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds, long expectedTicks)
    {
        // Act
        var result = _sut.Add((DateTime?)DateTime.Parse("2000-01-01 00:00:00"), increment, datePart);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds, expectedTicks);
    }

    [Fact]
    public void Add_With_InvalidDatepart()
    {
        // act & assert
        Assert.Throws<ArgumentException>(() => _sut.Add(DateTimeNow, 1, "century"));
    }

    [Fact]
    public void Add_With_NullOrEmptyDatePart()
    {
        // act & assert
        Assert.Throws<ArgumentException>(() => _sut.Add(DateTimeNow, 1, string.Empty));
        Assert.Throws<ArgumentException>(() => _sut.Add(DateTimeNow, 1, null));
    }

    [Fact]
    public void Add_With_NullValue()
    {
        // act & assert
        Assert.Throws<ArgumentNullException>(() => _sut.AddYears(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddMonths(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddDays(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddHours(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddMinutes(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddSeconds(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddMilliseconds(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.AddTicks(null, 1));
        Assert.Throws<ArgumentNullException>(() => _sut.Add(null, 1, "year"));
    }

    // Aux TestAdd methods
    private DateTime ActTestAdd(string method, int increment, object value, string? format)
    {
        return method switch
        {
            nameof(DateTimeHelpers.AddYears) => _sut.AddYears(value, increment, format),
            nameof(DateTimeHelpers.AddMonths) => _sut.AddMonths(value, increment, format),
            nameof(DateTimeHelpers.AddDays) => _sut.AddDays(value, increment, format),
            nameof(DateTimeHelpers.AddHours) => _sut.AddHours(value, increment, format),
            nameof(DateTimeHelpers.AddMinutes) => _sut.AddMinutes(value, increment, format),
            nameof(DateTimeHelpers.AddSeconds) => _sut.AddSeconds(value, increment, format),
            nameof(DateTimeHelpers.AddMilliseconds) => _sut.AddMilliseconds(value, increment, format),
            nameof(DateTimeHelpers.AddTicks) => _sut.AddTicks(value, increment, format),

            _ => throw new ArgumentException("Invalid method name.")
        };
    }

    private void AssertTestAdd(DateTime result, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds, long expectedTicks)
    {
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMiliseconds);
        result.Ticks.Should().Be(expectedTicks);
    }
}