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
    [InlineData("==", 1, 0, false)]
    [InlineData("==", 0, 0, true)]
    [InlineData("==", 0, 1, false)]
    [InlineData("!=", 1, 0, true)]
    [InlineData("!=", 0, 0, false)]
    [InlineData("!=", 0, 1, true)]
    [InlineData("<", 1, 0, false)]
    [InlineData("<", 0, 0, false)]
    [InlineData("<", 0, 1, true)]
    [InlineData("<=", 1, 0, false)]
    [InlineData("<=", 0, 0, true)]
    [InlineData("<=", 0, 1, true)]
    [InlineData(">", 1, 0, true)]
    [InlineData(">", 0, 0, false)]
    [InlineData(">", 0, 1, false)]
    [InlineData(">=", 1, 0, true)]
    [InlineData(">=", 0, 0, true)]
    [InlineData(">=", 0, 1, false)]
    public void Compare_WithValuesAsDateTime(string op, int incrementDate1, int incrementDate2, bool expected)
    {
        // Arrange
        var dateTime1 = DateTimeNow.AddMinutes(incrementDate1);
        var dateTime2 = DateTimeNow.AddMinutes(incrementDate2);
        string? format = null;

        // Act
        var result = _sut.Compare(dateTime1, op, dateTime2, format);

        // Assert
        result.Should().Be(expected);

        // Act
        result = _sut.Compare(dateTime1, op, dateTime2, format);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("==", "2001-01-01 00:00:00", "2000-01-01 00:00:00", false)]
    [InlineData("==", "2000-01-01 00:00:00", "2000-01-01 00:00:00", true)]
    [InlineData("==", "2000-01-01 00:00:00", "2001-01-01 00:00:00", false)]
    [InlineData("!=", "2001-01-01 00:00:00", "2000-01-01 00:00:00", true)]
    [InlineData("!=", "2000-01-01 00:00:00", "2000-01-01 00:00:00", false)]
    [InlineData("!=", "2000-01-01 00:00:00", "2001-01-01 00:00:00", true)]
    [InlineData("<", "2001-01-01 00:00:00", "2000-01-01 00:00:00", false)]
    [InlineData("<", "2000-01-01 00:00:00", "2000-01-01 00:00:00", false)]
    [InlineData("<", "2000-01-01 00:00:00", "2001-01-01 00:00:00", true)]
    [InlineData("<=", "2001-01-01 00:00:00", "2000-01-01 00:00:00", false)]
    [InlineData("<=", "2000-01-01 00:00:00", "2000-01-01 00:00:00", true)]
    [InlineData("<=", "2000-01-01 00:00:00", "2001-01-01 00:00:00", true)]
    [InlineData(">", "2001-01-01 00:00:00", "2000-01-01 00:00:00", true)]
    [InlineData(">", "2000-01-01 00:00:00", "2000-01-01 00:00:00", false)]
    [InlineData(">", "2000-01-01 00:00:00", "2001-01-01 00:00:00", false)]
    [InlineData(">=", "2001-01-01 00:00:00", "2000-01-01 00:00:00", true)]
    [InlineData(">=", "2000-01-01 00:00:00", "2000-01-01 00:00:00", true)]
    [InlineData(">=", "2000-01-01 00:00:00", "2001-01-01 00:00:00", false)]
    public void Compare_WithValuesAsString(string op, string dateTime1, string dateTime2, bool expected)
    {
        // Arrange 
        string? format = null;

        // Act
        var result = _sut.Compare(dateTime1, op, dateTime2, format);

        // Assert
        result.Should().Be(expected);       
    }

    [Theory]
    [InlineData("==", "2001||01||01 00-00-00", "2000||01||01 00-00-00", false)]
    [InlineData("==", "2000||01||01 00-00-00", "2000||01||01 00-00-00", true)]
    [InlineData("==", "2000||01||01 00-00-00", "2001||01||01 00-00-00", false)]
    [InlineData("!=", "2001||01||01 00-00-00", "2000||01||01 00-00-00", true)]
    [InlineData("!=", "2000||01||01 00-00-00", "2000||01||01 00-00-00", false)]
    [InlineData("!=", "2000||01||01 00-00-00", "2001||01||01 00-00-00", true)]
    [InlineData("<", "2001||01||01 00-00-00", "2000||01||01 00-00-00", false)]
    [InlineData("<", "2000||01||01 00-00-00", "2000||01||01 00-00-00", false)]
    [InlineData("<", "2000||01||01 00-00-00", "2001||01||01 00-00-00", true)]
    [InlineData("<=", "2001||01||01 00-00-00", "2000||01||01 00-00-00", false)]
    [InlineData("<=", "2000||01||01 00-00-00", "2000||01||01 00-00-00", true)]
    [InlineData("<=", "2000||01||01 00-00-00", "2001||01||01 00-00-00", true)]
    [InlineData(">", "2001||01||01 00-00-00", "2000||01||01 00-00-00", true)]
    [InlineData(">", "2000||01||01 00-00-00", "2000||01||01 00-00-00", false)]
    [InlineData(">", "2000||01||01 00-00-00", "2001||01||01 00-00-00", false)]
    [InlineData(">=", "2001||01||01 00-00-00", "2000||01||01 00-00-00", true)]
    [InlineData(">=", "2000||01||01 00-00-00", "2000||01||01 00-00-00", true)]
    [InlineData(">=", "2000||01||01 00-00-00", "2001||01||01 00-00-00", false)]
    public void Compare_WithValuesAsStringAndFormat(string op, string dateTime1, string dateTime2, bool expected)
    {
        // Arrange 
        string? format = "yyyy||MM||dd hh-mm-ss";

        // Act
        var result = _sut.Compare(dateTime1, op, dateTime2, format);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("==", null, "2000-01-01 00:00:00", false)]
    [InlineData("==", null, null, true)]
    [InlineData("==", "2000-01-01 00:00:00", null, false)]
    [InlineData("!=", null, "2000-01-01 00:00:00", true)]
    [InlineData("!=", null, null, false)]
    [InlineData("!=", "2000-01-01 00:00:00", null, true)]
    [InlineData("<", null, "2000-01-01 00:00:00", false)]
    [InlineData("<", null, null, false)]
    [InlineData("<", "2000-01-01 00:00:00", null, false)]
    [InlineData("<=", null, "2000-01-01 00:00:00", false)]
    [InlineData("<=", null, null, false)]
    [InlineData("<=", "2000-01-01 00:00:00", null, false)]
    [InlineData(">", null, "2000-01-01 00:00:00", false)]
    [InlineData(">", null, null, false)]
    [InlineData(">", "2000-01-01 00:00:00", null, false)]
    [InlineData(">=", null, "2000-01-01 00:00:00", false)]
    [InlineData(">=", null, null, false)]
    [InlineData(">=", "2000-01-01 00:00:00", null, false)]
    public void Compare_WithNullValues(string op, string dateTime1, string dateTime2, bool expected)
    {
        // Arrange 
        string? format = null;

        // Act
        var result = _sut.Compare(dateTime1, op, dateTime2, format);

        // Assert
        result.Should().Be(expected);
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
    [InlineData(nameof(DateTimeHelpers.AddYears), -1, 1999, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 1, 2001, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), -1, 1999, 12, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 1, 2000, 2, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddDays), -1, 1999, 12, 31, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 1, 2000, 1, 2, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddHours), -1, 1999, 12, 31, 23, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 1, 2000, 1, 1, 1, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), -1, 1999, 12, 31, 23, 59, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 1, 2000, 1, 1, 0, 1, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), -1, 1999, 12, 31, 23, 59, 59, 0)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 1, 2000, 1, 1, 0, 0, 1, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), -1, 1999, 12, 31, 23, 59, 59, 999)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 1, 2000, 1, 1, 0, 0, 0, 1)]
    public void Add(string method, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
    {
        // Arrange
        object value = "2000-01-01 00:00:00";
        string? format = null;

        // Act
        DateTime result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds);

        // Arrange
        value = DateTime.Parse("2000-01-01 00:00:00");        

        // Act
        result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds);

        // Arrange
        value = (DateTime?)DateTime.Parse("2000-01-01 00:00:00");

        // Act
        result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds);
    }

    [Theory]
    [InlineData(nameof(DateTimeHelpers.AddYears), -1, 1999, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddYears), 1, 2001, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), -1, 1999, 12, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMonths), 1, 2000, 2, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddDays), -1, 1999, 12, 31, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddDays), 1, 2000, 1, 2, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddHours), -1, 1999, 12, 31, 23, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddHours), 1, 2000, 1, 1, 1, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), -1, 1999, 12, 31, 23, 59, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMinutes), 1, 2000, 1, 1, 0, 1, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), -1, 1999, 12, 31, 23, 59, 59, 0)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddSeconds), 1, 2000, 1, 1, 0, 0, 1, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), -1, 1999, 12, 31, 23, 59, 59, 999)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 0, 2000, 1, 1, 0, 0, 0, 0)]
    [InlineData(nameof(DateTimeHelpers.AddMilliseconds), 1, 2000, 1, 1, 0, 0, 0, 1)]
    public void Add_WithValueAsStringAndFormat(string method, int increment, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
    {
        // Arrange
        object value = "2000||01||01 00-00-00";

        string format = "yyyy||MM||dd hh-mm-ss";

        // Act
        DateTime result = ActTestAdd(method, increment, value, format);

        // Assert
        AssertTestAdd(result, expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSeconds, expectedMiliseconds);        
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
    }

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

            _ => throw new ArgumentException("Invalid method name.")
        };
    }

    private void AssertTestAdd(DateTime result, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSeconds, int expectedMiliseconds)
    {
        result.Year.Should().Be(expectedYear);
        result.Month.Should().Be(expectedMonth);
        result.Day.Should().Be(expectedDay);
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
        result.Second.Should().Be(expectedSeconds);
        result.Millisecond.Should().Be(expectedMiliseconds);
    }
}