using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Extensions;
using HandlebarsDotNet.Helpers.Models;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class EvaluateHelperTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public EvaluateHelperTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(_handlebarsContext);
    }

    [Theory]
    [InlineData("")]
    [InlineData("{")]
    [InlineData("{{")]
    [InlineData("}")]
    [InlineData("}}")]
    [InlineData("{x}")]
    [InlineData("{{x}")]
    [InlineData("{x}}")]
    public void TryEvaluateInvalid(string template)
    {
        // Arrange
        var data = new { x = 1 };

        // Act
        var isValid = _handlebarsContext.TryEvaluate(template, data, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(@"{{x}}", 'c')]
    [InlineData(@"{{x}}", "s")]
    [InlineData(@"{{x}}", 1)]
    [InlineData(@"{{x}}", int.MaxValue)]
    [InlineData(@"{{x}}", long.MaxValue)]
    [InlineData(@"{{x}}", float.MaxValue)]
    [InlineData(@"{{x}}", double.MaxValue)]
    public void TryEvaluateValid(string template, object x)
    {
        // Arrange
        var data = new { x };

        // Act
        var isValid = _handlebarsContext.TryEvaluate(template, data, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.Should().Be(x);
    }

    [Fact]
    public void TryEvaluateDecimal()
    {
        // Arrange
        var x = decimal.MaxValue;
        var data = new { x };

        // Act
        var isValid = _handlebarsContext.TryEvaluate(@"{{x}}", data, out var result);

        // Assert
        result.Should().Be(x);
    }

    [Fact]
    public void TryMultiple()
    {
        // Arrange
        var data = new { x = 42 };
        var templates = new[]
        {
            "{{x}}", // valid
            "test {{x}}", // invalid
            "{{", // invalid
            "", // invalid
            "{{x}}" // valid
        };

        // Act
        var results = new List<EvaluateResult>();
        foreach (var template in templates)
        {
            var isEvaluated = _handlebarsContext.TryEvaluate(template, data, out var result);
            results.Add(new EvaluateResult { IsEvaluated = isEvaluated, Result = result });
        }

        var joined = string.Join(", ", results.Select(r => $"{r.IsEvaluated}:{r.Result ?? "NULL"}"));

        // Assert
        joined.Should().Be("True:42, False:NULL, False:NULL, False:NULL, True:42");
    }

    [Fact]
    public void TryMultipleParallel()
    {
        // Arrange
        var data = new { x = 42 };
        var templates = new[]
        {
            "{{x}}", // valid
            "test {{x}}", // invalid
            "{{", // invalid
            "", // invalid
            "{{x}}" // valid
        };

        // Act
        var results = new List<EvaluateResult>();
        Parallel.ForEach(templates, template =>
        {
            var isEvaluated = _handlebarsContext.TryEvaluate(template, data, out var result);
            results.Add(new EvaluateResult { IsEvaluated = isEvaluated, Result = result });
        });

        var numIsEvaluated = results.Count(r => r.IsEvaluated);

        // Assert
        numIsEvaluated.Should().Be(2);
    }

#if NET6_0_OR_GREATER
    [Fact]
    public async Task TryMultipleParallelAsync()
    {
        // Arrange
        var data = new { x = 42 };
        var templates = new[]
        {
            "{{x}}", // valid
            "test {{x}}", // invalid
            "{{", // invalid
            "", // invalid
            "{{x}}" // valid
        };

        // Act
        var results = new List<EvaluateResult>();
        await Parallel.ForEachAsync(templates, (template, ct) =>
        {
            var isEvaluated = _handlebarsContext.TryEvaluate(template, data, out var result);
            results.Add(new EvaluateResult { IsEvaluated = isEvaluated, Result = result });

            return ValueTask.CompletedTask;
        });

        var numIsEvaluated = results.Count(r => r.IsEvaluated);

        // Assert
        numIsEvaluated.Should().Be(2);
    }
#endif
}