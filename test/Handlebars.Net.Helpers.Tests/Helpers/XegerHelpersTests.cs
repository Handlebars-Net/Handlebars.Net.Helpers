using FluentAssertions;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers
{
    public class XegerHelpersTests
    {
        private readonly Mock<IHandlebars> _contextMock;

        private readonly XegerHelpers _sut;

        public XegerHelpersTests()
        {
            _contextMock = new Mock<IHandlebars>();
            _contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

            _sut = new XegerHelpers(_contextMock.Object);
        }

        [Fact]
        public void Xeger()
        {
            // Act
            var result = _sut.Generate("[1-9]{1}\\d{3}");

            // Assert
            int.Parse(result).Should().BeInRange(1000, 9999);
        }
    }
}