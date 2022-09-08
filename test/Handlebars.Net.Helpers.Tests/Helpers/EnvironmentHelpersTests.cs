using System;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class EnvironmentHelpersTests
    {
        private readonly EnvironmentHelpers _sut;

        public EnvironmentHelpersTests()
        {
            var handlebarsContext = new Mock<IHandlebars>();
            _sut = new EnvironmentHelpers(handlebarsContext.Object);
        }

        [Fact]
        public void GetEnvironmentVariable_NonExisting()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();

            // Act
            var result = _sut.GetEnvironmentVariable(value);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetEnvironmentVariable_Existing()
        {
            // Arrange
            var variable = Guid.NewGuid().ToString();
            var value = Guid.NewGuid().ToString();
            try
            {
                Environment.SetEnvironmentVariable(variable, value);

                // Act
                var result = _sut.GetEnvironmentVariable(variable);

                // Assert
                result.Should().Be(value);

            }
            finally
            {
                Environment.SetEnvironmentVariable(variable, null);
            }
        }
    }
}