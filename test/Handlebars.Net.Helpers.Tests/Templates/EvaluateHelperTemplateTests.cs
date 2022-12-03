using FluentAssertions;
using HandlebarsDotNet.Helpers.Extensions;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class EvaluateHelperTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public EvaluateHelperTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();
            HandlebarsHelpers.Register(_handlebarsContext);
        }

        [Theory]
        [InlineData(@"{{x}}", 'c')]
        [InlineData(@"{{x}}", "s")]
        [InlineData(@"{{x}}", 1)]
        [InlineData(@"{{x}}", int.MaxValue)]
        [InlineData(@"{{x}}", long.MaxValue)]
        [InlineData(@"{{x}}", float.MaxValue)]
        [InlineData(@"{{x}}", double.MaxValue)]
        public void Evaluate(string template, object x)
        {
            // Arrange
            var data = new { x };

            // Act
            var result = _handlebarsContext.Evaluate(template, data);

            // Assert
            result.Should().Be(x);
        }

        [Fact]
        public void EvaluateDecimal()
        {
            // Arrange
            var x = decimal.MaxValue;
            var data = new { x };

            // Act
            var result = _handlebarsContext.Evaluate(@"{{x}}", data);

            // Assert
            result.Should().Be(x);
        }
    }
}