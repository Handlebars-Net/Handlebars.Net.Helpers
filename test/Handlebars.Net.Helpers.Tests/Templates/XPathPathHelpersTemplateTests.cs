using FluentAssertions;
using HandlebarsDotNet.Helpers.Enums;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.Templates
{
    public class XPathPathHelpersTemplateTests
    {
        private readonly IHandlebars _handlebarsContext;

        public XPathPathHelpersTemplateTests()
        {
            _handlebarsContext = Handlebars.Create();

            HandlebarsHelpers.Register(_handlebarsContext, Category.XPath);
        }

        [Fact]
        public void SelectToken_With_SoapXMLMessage()
        {
            // Arrange
            string soap = @"
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

            var request = new
            {
                body = soap
            };

            var action = _handlebarsContext.Compile("{{XPath.SelectNode body \"//*[local-name()='TokenIdLijst']\"}}");

            // Act
            var result = action(request);

            // Assert
            result.Should().NotBeNull().And.Contain("TokenIdLijst").And.Contain("0000083256").And.Contain("0000083259");
        }
    }
}