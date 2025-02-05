using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using System;
using System.Globalization;
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

    [Theory]
    [InlineData(0, "==", 0, true)]
    [InlineData(0, "!=", 1, true)]
    [InlineData(0, "<", 1, true)]
    [InlineData(1, ">", 0, true)]
    [InlineData(1, ">=", 0, true)]
    [InlineData(0, ">=", 0, true)]
    [InlineData(0, "<=", 1, true)]
    [InlineData(0, "<=", 0, true)]
    [InlineData(1, "<", 0, false)]
    public void Compare_With_ValuesAsDateTime(int incrementDate1, string op, int incrementDate2, bool expected)
    {
        // Arrange
        var dateTime1 = DateTimeNow.AddMinutes(incrementDate1);

        var dateTime2 = DateTimeNow.AddMinutes(incrementDate2);

        // Act
        var result = _sut.Compare(dateTime1, op, dateTime2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, "==", 0, true)]
    [InlineData(1, "!=", 0, true)]
    [InlineData(0, "<", 1, true)]
    [InlineData(1, ">", 0, true)]
    [InlineData(1, ">=", 0, true)]
    [InlineData(0, ">=", 0, true)]
    [InlineData(0, "<=", 1, true)]
    [InlineData(0, "<=", 0, true)]
    [InlineData(1, "<", 0, false)]
    public void Compare_With_ValuesAsNullableDateTime(int incrementDate1, string op, int incrementDate2, bool expected)
    {
        // Arrange
        var dateTime1 = DateTimeNow.AddMinutes(incrementDate1);

        var dateTime2 = DateTimeNow.AddMinutes(incrementDate2);

        // Act
        var result = _sut.Compare((DateTime?)dateTime1, op, (DateTime?)dateTime2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("2000-01-01 1:00:00", "<", "2000-01-01 1:10:00", true)]
    [InlineData("2000-01-01 1:10:00", ">", "2000-01-01 1:00:00", true)]
    [InlineData("2000-01-01 1:00:00", "==", "2000-01-01 1:00:00", true)]
    [InlineData("2000-01-01 1:00:00", "!=", "2000-01-01 1:00:00", false)]
    public void Compare_With_ValuesAsString(string value1, string op, string value2, bool expected)
    {
        // Act
        var result = _sut.Compare(value1, op, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("1:00:00 2000-01-01", "<", "1:10:00 2000-01-01", true)]
    [InlineData("1:10:00 2000-01-01", ">", "1:00:00 2000-01-01", true)]
    [InlineData("1:00:00 2000-01-01", "==", "1:00:00 2000-01-01", true)]
    [InlineData("1:00:00 2000-01-01", "!=", "1:00:00 2000-01-01", false)]
    public void Compare_With_ValuesAsFormattedString(string value1, string op, string value2, bool expected)
    {
        // Arrange
        string format = "h:mm:ss yyyy-MM-dd";

        // Act
        var result = _sut.Compare(value1, op, value2, format);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Compare_With_NullValues()
    {
        // Act
        var result = _sut.Compare(null, "==", null);

        // Assert
        result.Should().Be(false);
    }

    [Theory]
    [InlineData("1:15:30 2000-02-01", 2000, 2, 1, 1, 15, 30, 0)]
    [InlineData("2000-02-01 1:15:30", 2000, 2, 1, 1, 15, 30, 0)]
    [InlineData("2000-02-01", 2000, 2, 1, 0, 0, 0, 0)]
    public void Parse_With_ValuesAsString(string value, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
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
        result.Millisecond.Should().Be(expectedMiliseconds);
    }

    [Theory]
    [InlineData("2000 02 01", "yyyy MM dd", 2000, 2, 1, 0, 0, 0, 0)]
    [InlineData("01||02||2000 1-15-30", "dd||MM||yyyy h-mm-ss", 2000, 2, 1, 1, 15, 30, 0)]
    public void ParseExact_With_ValuesAsFormattedString(string value, string format, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
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
        result.Millisecond.Should().Be(expectedMiliseconds);
    }

    [Theory]
    [InlineData("year", 1, 2001, 1, 1, 12, 0, 0, 0)]
    [InlineData("month", 1, 2000, 2, 1, 12, 0, 0, 0)]
    [InlineData("day", 1, 2000, 1, 2, 12, 0, 0, 0)]
    [InlineData("hour", 1, 2000, 1, 1, 13, 0, 0, 0)]
    [InlineData("minute", 1, 2000, 1, 1, 12, 1, 0, 0)]
    [InlineData("second", 1, 2000, 1, 1, 12, 0, 1, 0)]
    [InlineData("millisecond", 1, 2000, 1, 1, 12, 0, 0, 1)]
    public void Add_With_ValuesAsString(string datePart, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
    {
        // Act
        var result = _sut.Add("2000-01-01 12:00:00", increment, datePart);

        // Assert
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMiliseconds);
    }

    [Theory]
    [InlineData("year", 1, 2001, 1, 1, 12, 0, 0, 0)]
    [InlineData("month", 1, 2000, 2, 1, 12, 0, 0, 0)]
    [InlineData("day", 1, 2000, 1, 2, 12, 0, 0, 0)]
    [InlineData("hour", 1, 2000, 1, 1, 13, 0, 0, 0)]
    [InlineData("minute", 1, 2000, 1, 1, 12, 1, 0, 0)]
    [InlineData("second", 1, 2000, 1, 1, 12, 0, 1, 0)]
    [InlineData("millisecond", 1, 2000, 1, 1, 12, 0, 0, 1)]
    [InlineData("day", -1, 1999, 12, 31, 12, 0, 0, 0)]
    [InlineData("minute", -1, 2000, 1, 1, 11, 59, 0, 0)]
    [InlineData("second", -1, 2000, 1, 1, 11, 59, 59, 0)]
    public void Add_With_ValuesAsDateTime(string datePart, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
    {
        // Act
        var result = _sut.Add(DateTime.Parse("2000-01-01 12:00:00"), increment, datePart);

        // Assert
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMiliseconds);
    }

    [Theory]
    [InlineData("year", 1, 2001, 1, 1, 12, 0, 0, 0)]
    [InlineData("month", 1, 2000, 2, 1, 12, 0, 0, 0)]
    [InlineData("day", 1, 2000, 1, 2, 12, 0, 0, 0)]
    [InlineData("hour", 1, 2000, 1, 1, 13, 0, 0, 0)]
    [InlineData("minute", 1, 2000, 1, 1, 12, 1, 0, 0)]
    [InlineData("second", 1, 2000, 1, 1, 12, 0, 1, 0)]
    [InlineData("millisecond", 1, 2000, 1, 1, 12, 0, 0, 1)]
    public void Add_With_ValuesAsNullableDateTime(string datePart, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
    {
        // Act
        var result = _sut.Add((DateTime?)DateTime.Parse("2000-01-01 12:00:00"), increment, datePart);

        // Assert
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMiliseconds);
    }
}