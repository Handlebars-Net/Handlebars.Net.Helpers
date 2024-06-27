#if !(NET451 || NET452)
using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class XsltHelpersTemplateTests
{
    private readonly IHandlebars _handlebarsContext;

    public XsltHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();

        HandlebarsHelpers.Register(_handlebarsContext, Category.Xslt);
    }

    [Fact]
    public void TransformToString()
    {
        // Assign
        const string xml = @"<People>
  <Person FirstName='John' LastName='Doe' />
  <Person FirstName='Jane' LastName='Doe' />
</People>";

        const string xslt = @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
  <xsl:output method='xml' indent='yes'/>
  <xsl:template match='/People'>
    <All>
      <xsl:apply-templates select='Person'/>
    </All>
  </xsl:template>
  <xsl:template match='Person'>
    <Employee>
      <xsl:attribute name='FullName'>
        <xsl:value-of select='concat(@FirstName, &apos; &apos;, @LastName)'/>
      </xsl:attribute>
    </Employee>
  </xsl:template>
</xsl:stylesheet>";

        // Arrange
        var request = new
        {
            body = xml
        };

        const string expression = "{{Xslt.TransformToString body \"" + xslt + "\"}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        const string expected = @"<All>
  <Employee FullName=""John Doe"" />
  <Employee FullName=""Jane Doe"" />
</All>";
        result.Should().BeEquivalentTo(expected);
    }
}
#endif