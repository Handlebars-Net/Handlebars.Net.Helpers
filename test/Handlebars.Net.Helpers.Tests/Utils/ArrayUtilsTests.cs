using System.Collections.Generic;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Utils;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Utils
{
    public class ArrayUtilsTests
    {
        [Theory]
        [InlineData(new object?[] { 1, "c", "str", null }, @"[1,""c"",""str"",null]")]
        [InlineData(new object?[] { 1, 'c', "str", null }, @"[1,""c"",""str"",null]")]
        public void ToArray(IEnumerable<object?> values, string expected)
        {
            // Act
            var result = ArrayUtils.ToArray(values);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("", false, null)]
        [InlineData("[", false, null)]
        [InlineData("[]", true, new object?[] { })]
        [InlineData(@"[1,""c"",""str"",null]", true, new object?[] { 1, "c", "str", null })]
        public void TryParse(string value, bool valid, object?[] expected)
        {
            // Act
            bool result = ArrayUtils.TryParse(value, out object?[]? parsed);

            // Assert
            result.Should().Be(valid);
            parsed.Should().BeEquivalentTo(expected);
        }
    }
}