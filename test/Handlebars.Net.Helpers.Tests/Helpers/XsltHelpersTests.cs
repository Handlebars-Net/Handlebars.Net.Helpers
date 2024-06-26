#if !(NET451 || NET452)
using FluentAssertions;
using Moq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Helpers;

public class XsltHelpersTests
{
    private static string xml = @"<People>
  <Person FirstName='John' LastName='Doe' />
  <Person FirstName='Jane' LastName='Doe' />
</People>";

    private static string xslt = @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
  <xsl:output method='xml' />
  <xsl:template match='/People'>
    <All>
      <xsl:apply-templates select='Person'/>
    </All>
  </xsl:template>
  <xsl:template match='Person'>
    <Employee>
      <xsl:attribute name='FullName'>
        <xsl:value-of select='concat(@FirstName, "" "", @LastName)'/>
      </xsl:attribute>
    </Employee>
  </xsl:template>
</xsl:stylesheet>";

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
        // Act
        var result = _sut.TransformToString(xml, xslt);

        // Assert
        const string expected = @"<All>
  <Employee FullName=""John Doe"" />
  <Employee FullName=""Jane Doe"" />
</All>";
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void TransformToString_IndentIsFalse()
    {
        // Act
        var result = _sut.TransformToString(xml, xslt, false);

        // Assert
        const string expected = @"<All><Employee FullName=""John Doe"" /><Employee FullName=""Jane Doe"" /></All>";
        result.Should().BeEquivalentTo(expected);
    }
}
#endif