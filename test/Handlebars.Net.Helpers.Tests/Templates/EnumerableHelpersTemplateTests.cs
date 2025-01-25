using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class EnumerableHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public EnumerableHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();
        _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

        HandlebarsHelpers.Register(_handlebarsContext, Category.Enumerable);
    }

    [Theory]
    [InlineData(null, "0")]
    [InlineData(new object[] { }, "0")]
    [InlineData(new object[] { "a", "b", "c" }, "3")]
    public void Count(IEnumerable<object?>? values, string expected)
    {
        // Arrange
        var template = "{{[Enumerable.Count] values}}";
        var compiled = _handlebarsContext.Compile(template);
        var data = new
        {
            values = values
        };

        // Act
        var result = compiled(data);

        // Assert
        result.Should().Be(expected);
    }
}
