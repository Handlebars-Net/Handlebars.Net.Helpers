using FluentAssertions;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class RandomHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly RandomHelpers _sut;

        public RandomHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

            _sut = new RandomHelpers(_contextMock.Object);
        }

        [Fact]
        public void Random()
        {
            // Act
            //var result = _sut.Random("Type=\"Integer\" Min=1000 Max=9999");

            // Assert
            // int.Parse(result).Should().BeInRange(1000, 9999);
        }
    }
}