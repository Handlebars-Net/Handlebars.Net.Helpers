using System;
using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class MathHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public MathHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();
            _handlebarsContext.Configuration.FormatProvider = CultureInfo.InvariantCulture;

            HandlebarsHelpers.Register(_handlebarsContext, Category.Math);
        }

        [Theory]
        [InlineData("{{[Math.Add] 1 2}}", "3")]
        [InlineData("{{[Math.Add] 2.2 3.1}}", "5.3")]
        public void Add(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().StartWith(expected);
        }

        [Theory]
        [InlineData("{{[Math.LessThan] 2 1}}", "False")]
        [InlineData("{{[Math.LessThan] 1 2}}", "True")]
        [InlineData("{{[Math.LessThan] 2.2 3.1}}", "True")]
        public void LessThan(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{{Math.Subtract 100 1}}", "99")]
        [InlineData("{{Math.Subtract 101.1 1}}", "100.1")]
        [InlineData("{{Math.Subtract 101 0.9}}", "100.1")]
        public void Subtract(string template, string expected)
        {
            // Arrange
            var action = _handlebarsContext.Compile(template);

            // Act
            var result = action("");

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Subtract_With_InvalidArguments_Should_Throw()
        {
            // Arrange
            var compiled = _handlebarsContext.Compile("{{Math.Subtract 100 ''}}");

            // Act
            Action action = () => compiled("");

            // Assert
            action.Should().Throw<NotSupportedException>();
        }
    }
}