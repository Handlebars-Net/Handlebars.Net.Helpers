using System.Linq;
using FluentAssertions;
using HandlebarsDotNet.Helpers.Helpers;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class EnumerableHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly A?[] _array;

        private readonly EnumerableHelpers _sut;

        public EnumerableHelpersTests()
        {
            _array = new[]
            {
                new A
                {
                    X = 100,
                    B = new B
                    {
                        Num = 1,
                        Field = "f"
                    }
                },
                new A
                {
                    X = 200,
                    B = null
                },
                null
            };

            _contextMock = new Mock<IHandlebars>();

            _sut = new EnumerableHelpers(_contextMock.Object);
        }

        private class A
        {
            public int? X { get; set; }

            public B? B { get; set; }
        }

        private class B
        {
            public int? Num { get; set; }

            public string? Field;
        }

        [Fact]
        public void IsEmpty()
        {
            // Act
            var result = _sut.IsEmpty(_array);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Select_NestedProperty_From_Array()
        {
            // Act
            var result = _sut.Select(_array, "B.Num").Cast<int?>();

            // Assert
            result.Should().BeEquivalentTo(1, null, null);
        }

        [Fact]
        public void Select_NestedProperty_From_Array_SkipNullValues()
        {
            // Act
            var result = _sut.Select(_array, "B.Num", true).Cast<int?>();

            // Assert
            result.Should().BeEquivalentTo(1);
        }

        [Fact]
        public void Select_NestedField_From_Array()
        {
            // Act
            var result = _sut.Select(_array, "B.Field").Cast<string?>();

            // Assert
            result.Should().BeEquivalentTo("f", null, null);
        }

        [Fact]
        public void Select_Property_From_DynamicArray()
        {
            // Arrange
            var array = new[]
            {
                new
                {
                    num = 1
                },
                new
                {
                    num = 2
                }
            };

            // Act
            var result = _sut.Select(array, "num").Cast<int>();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(1, 2);
        }

        [Fact]
        public void Select_NestedProperty_From_DynamicArray()
        {
            // Arrange
            var array = new[]
            {
                new
                {
                    nested = new
                    {
                        num = (int?) 9
                    }
                },
                new
                {
                    nested = new
                    {
                        num = (int?) null
                    }
                }
            };

            // Act
            var result = _sut.Select(array, "nested.num").Cast<int?>();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(9, null);
        }
    }
}
