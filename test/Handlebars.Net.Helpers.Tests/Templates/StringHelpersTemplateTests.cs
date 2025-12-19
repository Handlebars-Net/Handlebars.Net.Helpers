using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Compiler;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Models;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class StringHelpersTemplateTests
{
    private static readonly DateTime DateTimeNow = new(2020, 4, 15, 23, 59, 58);

    private readonly Mock<IDateTimeService> _dateTimeServiceMock;

    private readonly IHandlebars _handlebarsContext;

    public StringHelpersTemplateTests()
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

    [Theory]
    [InlineData("{{[String.Append] \"foo\" \"bar\"}}", "foobar")]
    [InlineData("{{[String.Append] \"foo\" \"b\"}}", "foob")]
    [InlineData("{{[String.Append] \"foo\" 'b'}}", "foob")]
    [InlineData("{{[String.Append] \"foo\" ([String.Append] \"a\" \"b\")}}", "fooab")]
    public void Append(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{String.Coalesce null, \" \", \"\", \"a\", \"b\"}}", "a")]
    [InlineData("{{String.Coalesce \"\", \" \", \"\", \"a\", \"b\"}}", "a")]
    public void Coalesce(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Equal_WhenInputIsUnresolvedBinding_And_ThrowOnUnresolvedBindingExpressionIsFalse_ShouldReturnEmpty()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;
        handlebarsContext.Configuration.ThrowOnUnresolvedBindingExpression = false;

        HandlebarsHelpers.Register(handlebarsContext, o =>
        {
            o.DateTimeService = _dateTimeServiceMock.Object;
        });
        var action0 = handlebarsContext.Compile("x{{viewData.page}}y");
        var action1 = handlebarsContext.Compile("x{{viewData.page}}y");
        var action2 = handlebarsContext.Compile("{{#String.Equal viewData.page \"home\"}}yes{{else}}no{{/String.Equal}}");

        // Act 0
        var result0 = action0(null);

        // Assert 0
        result0.Should().Be("xy");

        var viewData = new
        {
            abc = "xyz"
        };

        // Act 1
        var result1 = action1(viewData);

        // Assert 1
        result1.Should().Be("xy");

        // Act 2
        var result2 = action2(viewData);

        // Assert 2
        result2.Should().Be("no");
    }

    [Fact]
    public void Equal_WhenInputIsUnresolvedBinding_And_ThrowOnUnresolvedBindingExpressionIsTrue_ShouldThrow()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;
        handlebarsContext.Configuration.ThrowOnUnresolvedBindingExpression = true;

        HandlebarsHelpers.Register(handlebarsContext, o =>
        {
            o.DateTimeService = _dateTimeServiceMock.Object;
        });
        var template = handlebarsContext.Compile("x{{viewData.page}}y");

        var viewData = new
        {
            abc = "xyz"
        };

        // Act
        Action action = () => template(viewData);

        // Assert
        action.Should().Throw<HandlebarsUndefinedBindingException>().WithMessage("viewData is undefined");
    }

    [Theory]
    [InlineData("{{[String.Equal] \"foo\" \"bar\"}}", "False")]
    [InlineData("{{[String.Equal] \"foo\" 'b'}}", "False")]
    [InlineData("{{[String.Equal] 'b' \"foo\"}}", "False")]
    [InlineData("{{[String.Equal] \"foo\" \"foo\"}}", "True")]
    [InlineData("{{[String.Equal] 'x' 'x'}}", "True")]
    [InlineData("{{[String.Equal] \"ab\" ([String.Append] \"a\" \"b\")}}", "True")]
    [InlineData("{{#String.Equal \"foo\" \"foo\"}}yes{{else}}no{{/String.Equal}}", "yes")]
    [InlineData("{{#String.Equal \"foo\" \"bar\"}}yes{{else}}no{{/String.Equal}}", "no")]
    public void Equal(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[String.Equal] \"foo\" \"Foo\" \"OrdinalIgnoreCase\"}}", "True")]
    [InlineData("{{[String.Equal] \"foo\" \"Foo\" \"5\"}}", "True")]
    [InlineData("{{[String.Equal] \"foo\" \"Foo\" 5}}", "True")]
    [InlineData("{{[String.Equals] \"foo\" \"Foo\" \"OrdinalIgnoreCase\"}}", "True")]
    [InlineData("{{[String.Equals] \"foo\" \"Foo\" \"5\"}}", "True")]
    [InlineData("{{[String.Equals] \"foo\" \"Foo\" 5}}", "True")]
    public void EqualWithStringComparison(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{#String.IsString \"Hello\"}}yes{{else}}no{{/String.IsString}}", "yes")]
    [InlineData("{{#String.IsString 1}}yes{{else}}no{{/String.IsString}}", "no")]
    public void IsString(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{String.Join [\"a\",\"b\",\"c\"] ':'}}", "a:b:c")]
    [InlineData("{{[String.Join] [\"a\",\"b\",\"c\"] \"?\"}}", "a?b?c")]
    public void Join(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(null);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{String.Join numbers}}", "1234")]
    [InlineData("{{String.Join numbers \", \"}}", "1, 2, 3, 4")]
    public void JoinArray(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action(new { numbers = new[] { 1, 2, 3, 4 } });

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[String.NotEqual] \"foo\" \"bar\"}}", "True")]
    [InlineData("{{[String.NotEqual] \"foo\" 'b'}}", "True")]
    [InlineData("{{[String.NotEqual] 'b' \"foo\"}}", "True")]
    [InlineData("{{[String.NotEqual] \"foo\" \"foo\"}}", "False")]
    [InlineData("{{[String.NotEqual] 'x' 'x'}}", "False")]
    [InlineData("{{[String.NotEqual] \"ab\" ([String.Append] \"a\" \"b\")}}", "False")]
    [InlineData("{{#String.NotEqual \"foo\" \"foo\"}}yes{{else}}no{{/String.NotEqual}}", "no")]
    [InlineData("{{#String.NotEqual \"foo\" \"bar\"}}yes{{else}}no{{/String.NotEqual}}", "yes")]
    public void NotEqual(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[String.NotEqual] \"foo\" \"Foo\" \"OrdinalIgnoreCase\"}}", "False")]
    [InlineData("{{[String.NotEqual] \"foo\" \"Foo\" \"5\"}}", "False")]
    [InlineData("{{[String.NotEqual] \"foo\" \"Foo\" 5}}", "False")]
    [InlineData("{{[String.NotEquals] \"foo\" \"Foo\" \"OrdinalIgnoreCase\"}}", "False")]
    [InlineData("{{[String.NotEquals] \"foo\" \"Foo\" \"5\"}}", "False")]
    [InlineData("{{[String.NotEquals] \"foo\" \"Foo\" 5}}", "False")]
    public void NotEqualWithStringComparison(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[String.Split] \"a,b,c\" ','}}", "a,b,c")]
    [InlineData("{{[String.Split] \"a_;b_;c\" \"_;\"}}", "a,b,c")]
    public void Split(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void SplitInEachLoop()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{#each (String.Split \"a;b;c\" ';')}}\r\n{{@Key}}:{{@Index}}:{{this}}\r\n{{/each}}");

        // Act
        var result = action("");

        // Assert
        result.Should().Be("0:0:a\r\n1:1:b\r\n2:2:c\r\n");
    }

    [Theory]
    [InlineData("{{#String.StartsWith \"Hello\" \"He\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Hi")]
    [InlineData("{{#String.StartsWith \"Hello\" \"xx\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Goodbye")]
    [InlineData("{{#String.StartsWith \"Hello\" \"H\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Hi")]
    [InlineData("{{#String.StartsWith \"Hello\" \"x\"}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Goodbye")]
    [InlineData("{{#String.StartsWith \"Hello\" 'H'}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Hi")]
    [InlineData("{{#String.StartsWith \"Hello\" 'x'}}Hi{{else}}Goodbye{{/String.StartsWith}}", "Goodbye")]
    public void StartsWith(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[String.SubString] \"foobar\" 3}}", "bar")]
    [InlineData("{{[String.SubString] \"foobar\" 0 3}}", "foo")]
    [InlineData("{{[String.SubString] \"foobar\" 3 3}}", "bar")]
    public void SubString(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{[String.Append] \"foo\"}}")]
    [InlineData("{{[String.Append] \"foo\" \"bar\" \"bar2\"}}")]
    public void InvalidNumberOfArgumentsShouldThrowHandlebarsException(string template)
    {
        // Arrange
        var handleBarsAction = _handlebarsContext.Compile(template);

        // Act and Assert
        Assert.Throws<HandlebarsException>(() => handleBarsAction(""));
    }

    [Fact]
    public void TitleCase_Dynamic()
    {
        // Arrange
        var data = new Dictionary<string, dynamic>
        {
            { "FirstName", "jill" }
        };
        var test = "{{[String.Titlecase] [FirstName]}}";
        var action = _handlebarsContext.Compile(test);

        // Act
        var result = action(data);

        // Assert
        result.Should().Be("Jill");
    }

    [Theory]
    [InlineData("{{[String.Lowercase] \"foo\"}}", "foo")]
    [InlineData("{{[String.Lowercase] \"FOO\"}}", "foo")]
    [InlineData("{{[String.Lowercase] \"42\"}}", "42")]
    public void ToLower(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void WithCustomPrefixSeparator()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, options =>
        {
            options.PrefixSeparator = "-";
        });
        var action = handlebarsContext.Compile("{{String-Append \"foo\" \"bar\"}}");

        // Act
        var result = action("");

        // Assert
        result.Should().Be("foobar");
    }

    [Fact]
    public void WithoutCategoryPrefix()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, options =>
        {
            options.UseCategoryPrefix = false;
        });
        var action = handlebarsContext.Compile("{{[Append] \"foo\" \"bar\"}}");

        // Act
        var result = action("");

        // Assert
        result.Should().Be("foobar");
    }

    [Fact]
    public void WithoutCategoryPrefixAndWithExtraPrefix()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, options =>
        {
            options.UseCategoryPrefix = false;
            options.Prefix = "test";
        });
        var action = handlebarsContext.Compile("{{[test.Append] \"foo\" \"bar\"}}");

        // Act
        var result = action("");

        // Assert
        result.Should().Be("foobar");
    }

    [Fact]
    public void WithCategoryPrefixAndExtraWithPrefix()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, options =>
        {
            options.UseCategoryPrefix = true;
            options.Prefix = "test";
        });
        var action = handlebarsContext.Compile("{{[test.String.Append] \"foo\" \"bar\"}}");

        // Act
        var result = action("");

        // Assert
        result.Should().Be("foobar");
    }

    [Fact]
    public void Format_Now()
    {
        // Arrange
        var action = _handlebarsContext.Compile("{{String.Format (DateTime.Now) \"yyyy-MM-dd\"}}");

        // Act
        var result = action("");

        // Assert
        result.Should().Be("2020-04-15");
    }

    [Fact]
    public void FormatAsString_WithFormat_Now()
    {
        // Arrange
        var action = _handlebarsContext.Compile("test {{String.FormatAsString (DateTime.Now) \"yyyy-MM-dd\"}} abc");

        // Act
        var result = action("");

        var decodeResult = WrappedString.TryDecode(result, out var decoded);

        // Assert
        decodeResult.Should().BeTrue();
        decoded.Should().Be("test 2020-04-15 abc");
    }

    [Fact]
    public void FormatAsString_WithoutFormat_Now()
    {
        // Arrange
        var action = _handlebarsContext.Compile("test {{String.FormatAsString (DateTime.Now)}} abc");

        // Act
        var result = action("");

        var decodeResult = WrappedString.TryDecode(result, out var decoded);

        // Assert
        decodeResult.Should().BeTrue();
        decoded.Should().Be("test 04/15/2020 23:59:58 abc");
    }

    [Theory]
    [InlineData("foo", "test foo abc")]
    [InlineData("あ", "test あ abc")]
    public void ToWrappedString(string value, string expected)
    {
        // Arrange
        var model = new
        {
            x = value
        };
        var action = _handlebarsContext.Compile("test {{String.ToWrappedString (x)}} abc");

        // Act
        var result = action(model);

        var decodeResult = WrappedString.TryDecode(result, out var decoded);

        // Assert
        decodeResult.Should().BeTrue();
        decoded.Should().Be(expected);
    }

    [Fact]
    public void ToWrappedString_Now()
    {
        // Arrange
        var action = _handlebarsContext.Compile("test {{String.ToWrappedString (DateTime.Now)}} abc");

        // Act
        var result = action("");

        var decodeResult = WrappedString.TryDecode(result, out var decoded);

        // Assert
        decodeResult.Should().BeTrue();
        decoded.Should().Be("test 04/15/2020 23:59:58 abc");
    }

    [Fact]
    public void ToWrappedString_Null()
    {
        // Arrange
        var action = _handlebarsContext.Compile("test {{String.ToWrappedString (null)}} abc");

        // Act
        var result = action("");

        var decodeResult = WrappedString.TryDecode(result, out var decoded);

        // Assert
        decodeResult.Should().BeTrue();
        decoded.Should().Be("test  abc");
    }

    [Fact]
    public void Truncate()
    {
        // Arrange
        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, o =>
        {
            o.UseCategoryPrefix = false;
            o.Categories = [Category.String, Category.Humanizer];
        });
        var action = handlebarsContext.Compile("{{Truncate \"This is a long sentence that needs truncating.\" 10 }}");

        // Act
        var result = action("");

        result.Should().Be("This is a ");
    }

    [Fact]
    public void Format_Template_Now()
    {
        // Arrange
        var model = new
        {
            x = DateTimeNow
        };

        var action = _handlebarsContext.Compile("{{String.Format x \"yyyy-MMM-dd\"}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("2020-Apr-15");
    }

    [Fact]
    public void FormatAsString_Template_Now()
    {
        // Arrange
        var model = new
        {
            x = DateTimeNow
        };

        var action = _handlebarsContext.Compile("test {{String.FormatAsString x \"yyyy-MMM-dd\"}} abc");

        // Act
        var result = action(model);

        // Assert
        var decodeResult = WrappedString.TryDecode(result, out var decoded);

        // Assert
        decodeResult.Should().BeTrue();
        decoded.Should().Be("test 2020-Apr-15 abc");
    }

    [Theory]
    [InlineData("{{String.First (String.Split \"a<br />b<br />c\" \"<br />\")}}", "a")]
    [InlineData("{{String.First (String.Split \"Honors Algebra 2<br /><br />More text\" \"<br />\")}}", "Honors Algebra 2")]
    [InlineData("{{String.First (String.Split \"single\" \";\")}}", "single")]
    public void FirstWithSplit(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("{{String.Last (String.Split \"a<br />b<br />c\" \"<br />\")}}", "c")]
    [InlineData("{{String.Last (String.Split \"single\" \";\")}}", "single")]
    public void LastWithSplit(string template, string expected)
    {
        // Arrange
        var action = _handlebarsContext.Compile(template);

        // Act
        var result = action("");

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FirstWithSplit_FromModel()
    {
        // Arrange
        var model = new
        {
            yourField = "Honors Algebra 2<br /><br /><span style=\"background-color: #fbeeb8;\"><strong>Textbook<br /></strong>Algebra and Trigonometry</span>"
        };
        var action = _handlebarsContext.Compile("{{String.First (String.Split yourField \"<br />\")}}");

        // Act
        var result = action(model);

        // Assert
        result.Should().Be("Honors Algebra 2");
    }
}