using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class ExecuteUtilsTests
    {
        [Fact]
        public void Execute_function_with_invalid_data_should_throw_exception()
        {
            // Arrange
            var value = "invalid data";

            // Act and Assert 
            value
                .Invoking(v => ExecuteUtils.Execute(v, Math.Sqrt))
                .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Execute_array_function_with_invalid_data_should_throw_exception()
        {
            // Arrange
            var array = new object[] { "invalid data" };
            var function = new Func<IEnumerable<double>, double>(x => x.FirstOrDefault());

            // Act and Assert 
            array
                .Invoking(v => ExecuteUtils.Execute(v, function))
                .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Execute_two_argument_function_with_invalid_data_should_throw_exception()
        {
            // Arrange
            var v1 = 10.0;
            var v2 = "invalid data";
            var function = new Func<double, double, double>((x, y) => x + y);

            // Act and Assert 
            v1
                .Invoking(v => ExecuteUtils.Execute(null!, v, v2, function))
                .Should().Throw<NotSupportedException>();
        }
    }
}
