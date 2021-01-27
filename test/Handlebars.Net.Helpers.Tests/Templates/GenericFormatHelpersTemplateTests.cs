using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class GenericFormatHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public GenericFormatHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.String);
        }

        /*
         * "{{Format x \"o\"}}",
                "{{Now}}",
                "{{UtcNow}}",
                "{{Now \"yyyy-MM-dd\"}}",
                "{{Format (Now) \"yyyy-MM-dd\"}}",*/

        //[Theory]
        //[InlineData("{{Now}}", "yes")]
        //[InlineData("{{#String.IsString 1}}yes{{else}}no{{/String.IsString}}", "no")]
        //public void IsString(string template, string expected)
        //{
        //    // Arrange
        //    var action = _handlebarsContext.Compile(template);

        //    // Act
        //    var result = action("");

        //    // Assert
        //    result.Should().Be(expected);
        //}

    }
}