using System.Globalization;
using System.Threading;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class MathHelpersTests
{
    private readonly Mock<IHandlebars> _contextMock;

    private readonly MathHelpers _sut;

    public MathHelpersTests()
    {
        _contextMock = new Mock<IHandlebars>();
        _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration { FormatProvider = CultureInfo.InvariantCulture });

        _sut = new MathHelpers(_contextMock.Object, new HandlebarsHelpersOptions());

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(1, 1)]
    [InlineData(-2147483649L, 2147483649L)]
    [InlineData(1.2, 1.2)]
    [InlineData(-1.2, 1.2)]
    [InlineData("-1", 1)]
    [InlineData("-1.2", 1.2)]
    public void Abs(object value, object expected)
    {
        // Act
        var result = _sut.Abs(value);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Abs_Complex()
    {
        // Act
        var result1 = _sut.Abs(new ComplexInt());
        var result2 = _sut.Abs(new ComplexDouble());

        // Assert
        result1.Should().Be(42);
        result2.Should().Be(42.1);
    }

    [Theory]
    [InlineData(-1, 1, 0)]
    [InlineData(1, 2, 3)]
    [InlineData(-1.2, 1.2, 0)]
    [InlineData(1.2, 1.3, 2.5)]
    [InlineData("1000", "1.2", 1001.2)]
    public void Add(object value1, object value2, object expected)
    {
        // Act
        var result = _sut.Add(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1, 43)]
    [InlineData("1", 43)]
    [InlineData(1.2, 43.2)]
    [InlineData("1.2", 43.2)]
    public void Add_Complex_And_Any(object value, object expected)
    {
        // Act
        var result1 = _sut.Add(new ComplexInt(), value);
        var result2 = _sut.Add(value, new ComplexInt());

        // Assert
        result1.Should().Be(expected);
        result2.Should().Be(expected);
    }

    [Fact]
    public void Add_Complex_And_Complex()
    {
        // Act
        var result1 = _sut.Add(new ComplexInt(), new ComplexInt());
        var result2 = _sut.Add(new ComplexInt(), new ComplexDouble());

        var result3 = _sut.Add(new ComplexDouble(), new ComplexDouble());
        var result4 = _sut.Add(new ComplexDouble(), new ComplexInt());

        // Assert
        result1.Should().Be(84);
        result2.Should().Be(84.1);
        result3.Should().Be(84.2);
        result4.Should().Be(84.1);
    }

    [Theory]
    [InlineData(-1, 1, -1)]
    [InlineData(-1.2, 1.2, -1)]
    [InlineData(1.2, 0.5, 2.4)]
    [InlineData("45", "22.5", 2)]
    public void Divide(object value1, object value2, double expected)
    {
        // Act
        var result = _sut.Divide(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 0, false)]
    [InlineData(-2147483649L, 0, false)]
    [InlineData(2147483649L, 0, true)]
    [InlineData(1.2, 0.2, true)]
    [InlineData(-1.2, 0.2, false)]
    [InlineData("-1", 0, false)]
    [InlineData("1.2", 0, true)]
    public void GreaterThan(object value1, object value2, bool expected)
    {
        // Act
        var result = _sut.GreaterThan(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1.1, 1.1, 0)]
    [InlineData("20", "30.0", 25)]
    public void Avg(object value1, object value2, double expected)
    {
        // Act
        var result = _sut.Avg(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 1, 1)]
    [InlineData(-1.2, 1, 1)]
    [InlineData(1, -1.2, 1)]
    public void Max(object value1, object value2, object expected)
    {
        // Act
        var result = _sut.Max(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 1, -1)]
    [InlineData(-1.2, 1, -1.2)]
    [InlineData(1, -1.2, -1.2)]
    public void Min(object value1, object value2, object expected)
    {
        // Act
        var result = _sut.Min(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 1, -1)]
    [InlineData(-1.2, 2, -2.4)]
    [InlineData(1.2, 0.5, 0.6)]
    [InlineData("2", "22.5", 45)]
    public void Multiply(object value1, object value2, double expected)
    {
        // Act
        var result = _sut.Multiply(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.0d, 0)]
    [InlineData(-2, -1)]
    [InlineData(-2.2, -1)]
    [InlineData(42, 1)]
    [InlineData(42.5, 1)]
    public void Sign(object value, object expected)
    {
        // Act
        var result = _sut.Sign(value);

        // Assert
        result.Should().Be(expected);
    }
}

class ComplexInt
{
    public override string ToString()
    {
        return "42";
    }
}

class ComplexDouble
{
    public override string ToString()
    {
        return "42.1";
    }
}