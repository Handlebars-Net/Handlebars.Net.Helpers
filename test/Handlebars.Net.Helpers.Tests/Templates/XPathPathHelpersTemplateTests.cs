using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates;

public class XPathPathHelpersTemplateTests
{
    private const string MiniTestSoapMessage = @"
<?xml version='1.0' standalone='no'?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://www.Test.nl/XMLHeader/10"">
   <soapenv:Header>
      <ns:TestHeader>
         <ns:HeaderVersion>10</ns:HeaderVersion>
      </ns:TestHeader>
   </soapenv:Header>
   <soapenv:Body>
      <req>
         <TokenIdLijst>
            <TokenId>0000083256</TokenId>
            <TokenId>0000083259</TokenId>
         </TokenIdLijst>
      </req>
   </soapenv:Body>
</soapenv:Envelope>";

    private const string TestSoapMessage = @"
<?xml version='1.0' standalone='no'?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://www.Test.nl/XMLHeader/10"" xmlns:req=""http://www.Test.nl/Betalen/COU/Services/RdplDbTknLystByOvkLyst/8/Req"">
   <soapenv:Header>
      <ns:TestHeader>
         <ns:HeaderVersion>10</ns:HeaderVersion>
         <ns:MessageId>MsgId10</ns:MessageId>
         <ns:ServiceRequestorDomain>Betalen</ns:ServiceRequestorDomain>
         <ns:ServiceRequestorId>CRM</ns:ServiceRequestorId>
         <ns:ServiceProviderDomain>COU</ns:ServiceProviderDomain>
         <ns:ServiceId>RdplDbTknLystByOvkLyst</ns:ServiceId>
         <ns:ServiceVersion>8</ns:ServiceVersion>
         <ns:FaultIndication>N</ns:FaultIndication>
         <ns:MessageTimestamp>?</ns:MessageTimestamp>
      </ns:TestHeader>
   </soapenv:Header>
   <soapenv:Body>
      <req:RdplDbTknLystByOvkLyst_REQ>
         <req:AanleveraarCode>CRM</req:AanleveraarCode>
         <!--Optional:-->
         <req:AanleveraarDetail>CRMi</req:AanleveraarDetail>
         <req:BerichtId>BerId</req:BerichtId>
         <req:BerichtType>RdplDbTknLystByOvkLyst</req:BerichtType>
         <!--Optional:-->
         <req:OpgenomenBedragenGewenstIndicatie>N</req:OpgenomenBedragenGewenstIndicatie>
         <req:TokenIdLijst>
            <!--1 to 10 repetitions:-->
            <req:TokenId>0000083256</req:TokenId>
            <req:TokenId>0000083259</req:TokenId>
         </req:TokenIdLijst>
      </req:RdplDbTknLystByOvkLyst_REQ>
   </soapenv:Body>
</soapenv:Envelope>";

    private readonly IHandlebars _handlebarsContext;

    public XPathPathHelpersTemplateTests()
    {
        _handlebarsContext = Handlebars.Create();

        HandlebarsHelpers.Register(_handlebarsContext, Category.XPath);
    }

    [Fact]
    public void SelectNodes_AsCommaSeparatedString()
    {
        // Arrange
        var request = new
        {
            body = MiniTestSoapMessage
        };

        var expression = "{{XPath.SelectNodes body \"//*[local-name()='TokenId']/text()\"}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("0000083256,0000083259");
    }

    [Fact]
    public void SelectNodes_InEachLoop()
    {
        // Arrange
        var request = new
        {
            body = MiniTestSoapMessage
        };

        var expression = "{{#each (XPath.SelectNodes body \"//*[local-name()='TokenId']/text()\")}}\r\n{{this}}\r\n{{/each}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("0000083256\r\n0000083259\r\n");
    }

    [Fact]
    public void SelectNodesAsString()
    {
        // Arrange
        var request = new
        {
            body = MiniTestSoapMessage
        };

        var expression = "{{XPath.SelectNodesAsString body \"//*[local-name()='TokenId']/text()\"}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("00000832560000083259");
    }

    [Fact]
    public void SelectNodeAsXml_ReturnsOuterXml()
    {
        // Arrange
        var request = new
        {
            body = TestSoapMessage
        };

        var expression = "{{XPath.SelectNodeAsXml body \"//*[local-name()='TokenId']\"}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("<req:TokenId xmlns:req=\"http://www.Test.nl/Betalen/COU/Services/RdplDbTknLystByOvkLyst/8/Req\">0000083256</req:TokenId>");
    }

    [Fact]
    public void SelectNode_ReturnsXPathNavigator()
    {
        // Arrange
        var request = new
        {
            body = TestSoapMessage
        };

        var expression = "{{XPath.SelectNode body \"//*[local-name()='TokenId']\"}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("0000083256");
    }

    [Fact]
    public void SelectNode_ReturnsStringValue_Test1()
    {
        // Arrange
        var request = new
        {
            body = TestSoapMessage
        };

        var expression = "{{XPath.SelectNode body \"//*[local-name()='TokenId']/text()\"}}";
        var action = _handlebarsContext.Compile(expression);

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("0000083256");
    }

    [Fact]
    public void SelectNode_ReturnsStringValue_Test2()
    {
        // Arrange
        var request = new
        {
            body = TestSoapMessage
        };

        var action = _handlebarsContext.Compile("{{XPath.SelectNode body \"//*[local-name()='AanleveraarCode']/text()\"}}");

        // Act
        var result = action(request);

        // Assert
        result.Should().Be("CRM");
    }
}