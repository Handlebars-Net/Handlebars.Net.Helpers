﻿using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class GenericFormatHelpersTemplateTests
    {
        private readonly DateTime DateTimeNow = new DateTime(2020, 4, 15, 23, 59, 58);

        private readonly Mock<IDateTimeService> _dateTimeServiceMock;

        private readonly IHandlebars _handlebarsContext;

        public GenericFormatHelpersTemplateTests()
        {
            _dateTimeServiceMock = new Mock<IDateTimeService>();
            _dateTimeServiceMock.Setup(d => d.Now()).Returns(DateTimeNow);
            _dateTimeServiceMock.Setup(d => d.UtcNow()).Returns(DateTimeNow.ToUniversalTime);

            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, o =>
            {
                o.UseCategoryPrefix = false;
                o.DateTimeService = _dateTimeServiceMock.Object;
            });
        }

        [Fact]
        public void Format_Now()
        {
            // Arrange
            var action = _handlebarsContext.Compile("{{Format (Now) \"yyyy-MM-dd\"}}");

            // Act
            var result = action("");

            // Assert
            result.Should().Be("2020-04-15");
        }

        [Fact]
        public void Format_Template_Now()
        {
            // Arrange
            var model = new
            {
                x = DateTimeNow
            };

            var action = _handlebarsContext.Compile("{{Format x \"yyyy-MMM-dd\"}}");

            // Act
            var result = action(model);

            // Assert
            result.Should().Be("2020-Apr-15");
        }
    }
}