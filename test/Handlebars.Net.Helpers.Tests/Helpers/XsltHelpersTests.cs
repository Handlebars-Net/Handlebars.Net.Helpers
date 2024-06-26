#if !(NET451 || NET452)
using FluentAssertions;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class XsltHelpersTests
{
    private readonly XsltHelpers _sut;

    public XsltHelpersTests()
    {
        Mock<IHandlebars> contextMock = new();
        contextMock.SetupGet(c => c.Configuration).Returns(new HandlebarsConfiguration());

        _sut = new XsltHelpers(contextMock.Object);
    }

    [Fact]
    public void TransformToString()
    {
        // Assign
        var xml = @"<People>
    <Person FirstName=""John"" LastName=""Doe"" />
    <Person FirstName=""Jane"" LastName=""Doe"" />
  </People>";

        var xslt = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
            <xsl:output method=""xml"" indent=""yes"" />
            
            <xsl:template match=""/"">
                <All>
                    <xsl:apply-templates select=""People/Person"" />
                </All>
            </xsl:template>
            
            <xsl:template match=""Person"">
                <Employee>
                    <xsl:attribute name=""FullName"">
                        <xsl:value-of select=""@FirstName"" /> <xsl:value-of select=""@LastName"" />
                    </xsl:attribute>
                </Employee>
            </xsl:template>
         </xsl:stylesheet>";

        // Act
        var result = _sut.TransformToString(xml, xslt);

        // Assert
        var expected = """
                        <All>
                          <Employee FullName="John Doe" />
                          <Employee FullName="Jane Doe" />
                        </All>
                      """;
        result.Should().BeEquivalentTo(expected);
    }
}
#endif