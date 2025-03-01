using System;
using CultureAwareTesting.xUnit;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class ObjectHelpersTests
{
    private readonly ObjectHelpers _sut;

    public ObjectHelpersTests()
    {
        var contextMock = new Mock<IHandlebars>();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

        _sut = new ObjectHelpers(contextMock.Object, HandlebarsHelpersOptions.Default);
    }

    [CulturedTheory("en-US")]
    [InlineData(123456, "123456")]
    [InlineData(123.456, "123.456")]
    public void ToStringWithValues(object value, string expected)
    {
        // Act
        var result = _sut.ToString(value);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToStringWithNullValue()
    {
        // Act
        var result = _sut.ToString(null);

        // Assert
        result.Should().Be(string.Empty);
    }

    [CulturedTheory("en-us")]
    [InlineData(nameof(ObjectHelpers.Equal), "2000-01-01 0:00:00", "2000-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.Equal), "1999-01-01 0:00:00", "2000-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.Equal), "2000-01-01 0:00:00", "1999-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "2000-01-01 0:00:00", "2000-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "1999-01-01 0:00:00", "2000-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "2000-01-01 0:00:00", "1999-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "2000-01-01 0:00:00", "2000-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "1999-01-01 0:00:00", "2000-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "2000-01-01 0:00:00", "1999-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "2000-01-01 0:00:00", "2000-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "1999-01-01 0:00:00", "2000-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "2000-01-01 0:00:00", "1999-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "2000-01-01 0:00:00", "2000-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "1999-01-01 0:00:00", "2000-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "2000-01-01 0:00:00", "1999-01-01 0:00:00", false)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "2000-01-01 0:00:00", "2000-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "1999-01-01 0:00:00", "2000-01-01 0:00:00", true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "2000-01-01 0:00:00", "1999-01-01 0:00:00", false)]
    public void All_Compare_Using_DateTime(string method, string value1, string value2, bool expected)
    {
        // Act
        var result = ActTestCompare(method, DateTime.Parse(value1), DateTime.Parse(value2));

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(nameof(ObjectHelpers.Equal), 0, 0, true)]
    [InlineData(nameof(ObjectHelpers.Equal), 1, 0, false)]
    [InlineData(nameof(ObjectHelpers.Equal), 0, 1, false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), 0, 0, false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), 1, 0, true)]
    [InlineData(nameof(ObjectHelpers.NotEqual), 0, 1, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), 0, 0, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), 1, 0, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), 0, 1, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), 0, 0, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), 1, 0, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), 0, 1, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), 0, 0, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), 1, 0, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), 0, 1, true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), 0, 0, true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), 1, 0, false)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), 0, 1, true)]
    public void All_Compare_Using_Int(string method, object value1, object value2, bool expected)
    {
        // Act
        var result = ActTestCompare(method, value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(nameof(ObjectHelpers.Equal), 0, 0, true)]
    [InlineData(nameof(ObjectHelpers.Equal), 1, 0, false)]
    [InlineData(nameof(ObjectHelpers.Equal), 0, 1, false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), 0, 0, false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), 1, 0, true)]
    [InlineData(nameof(ObjectHelpers.NotEqual), 0, 1, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), 0, 0, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), 1, 0, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), 0, 1, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), 0, 0, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), 1, 0, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), 0, 1, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), 0, 0, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), 1, 0, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), 0, 1, true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), 0, 0, true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), 1, 0, false)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), 0, 1, true)]
    public void All_Compare_Using_Custom_Object(string method, int value1, int value2, bool expected)
    {
        // Act
        var result = ActTestCompare(method, new CustomObject(value1), new CustomObject(value2));

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(nameof(ObjectHelpers.Equal), "a", "a", true)]
    [InlineData(nameof(ObjectHelpers.Equal), "a", "b", false)]
    [InlineData(nameof(ObjectHelpers.Equal), "b", "a", false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "a", "a", false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "a", "b", true)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "b", "a", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "a", "a", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "a", "b", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "b", "a", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "a", "a", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "a", "b", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "b", "a", true)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "a", "a", false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "a", "b", true)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "b", "a", false)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "a", "a", true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "a", "b", true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "b", "a", false)]
    public void All_Compare_Using_String(string method, object value1, object value2, bool expected)
    {
        // Act
        var result = ActTestCompare(method, value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(nameof(ObjectHelpers.Equal), null, null, true)]
    [InlineData(nameof(ObjectHelpers.Equal), "a", null, false)]
    [InlineData(nameof(ObjectHelpers.Equal), null, "a", false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), null, null, false)]
    [InlineData(nameof(ObjectHelpers.NotEqual), "a", null, true)]
    [InlineData(nameof(ObjectHelpers.NotEqual), null, "a", true)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), null, null, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), "a", null, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThan), null, "a", false)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), null, null, true)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), "a", null, false)]
    [InlineData(nameof(ObjectHelpers.GreaterThanEqual), null, "a", false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), null, null, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), "a", null, false)]
    [InlineData(nameof(ObjectHelpers.LowerThan), null, "a", false)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), null, null, true)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), "a", null, false)]
    [InlineData(nameof(ObjectHelpers.LowerThanEqual), null, "a", false)]
    public void All_Compare_Using_Null(string method, object value1, object value2, bool expected)
    {
        // Act
        var result = ActTestCompare(method, value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(0, 1, -1)]
    [InlineData(null, null, 0)]
    [InlineData(1, null, 0)]
    [InlineData(null, 1, 0)]
    public void CompareTo(object value1, object value2, int? expected)
    {
        // Act
        var result = _sut.CompareTo(value1, value2);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void CompareTo_Using_Uncomparable_Obj()
    {
        Assert.Throws<ArgumentException>(() => _sut.CompareTo(new { }, new { }));
    }

    // Aux Methods / Classes
    private bool ActTestCompare(string method, object value1, object value2)
    {
        return method switch
        {
            nameof(ObjectHelpers.Equal) => _sut.Equal(value1, value2),
            nameof(ObjectHelpers.NotEqual) => _sut.NotEqual(value1, value2),
            nameof(ObjectHelpers.GreaterThan) => _sut.GreaterThan(value1, value2),
            nameof(ObjectHelpers.GreaterThanEqual) => _sut.GreaterThanEqual(value1, value2),
            nameof(ObjectHelpers.LowerThan) => _sut.LowerThan(value1, value2),
            nameof(ObjectHelpers.LowerThanEqual) => _sut.LowerThanEqual(value1, value2),

            _ => throw new ArgumentException("Invalid method name.")
        };
    }

    private class CustomObject : IComparable
    {
        public CustomObject(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public int CompareTo(object? other)
        {
            if (other is not CustomObject comparable)
            {
                return 0;
            }

            return Value == comparable?.Value ? 0 : Value > comparable?.Value ? 1 : -1;
        }

        public override bool Equals(object? obj)
        {
            return obj is CustomObject @object && Value == @object.Value;
        }
    }
}