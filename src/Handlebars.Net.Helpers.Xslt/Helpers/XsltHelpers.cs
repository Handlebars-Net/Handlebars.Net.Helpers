using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

internal class XsltHelpers : BaseHelpers, IHelpers
{
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

    // Remove the "<?xml version='1.0' standalone='no'?>" from a XML document.
    // (https://github.com/WireMock-Net/WireMock.Net/issues/618)
    private static readonly Regex RemoveXmlVersionRegex = new("(<\\?xml.*?\\?>)", RegexOptions.Compiled, RegexTimeout);

    public XsltHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.Value)]
    public XmlDocument Transform(string document, string xslt)
    {
        var inputXml = CreateXmlDocument(document);
        var xsltCompiledTransform = CreateXslCompiledTransform(xslt);

        var outputXml = new XmlDocument();
        using var writer = outputXml.CreateNavigator()!.AppendChild();
        xsltCompiledTransform.Transform(inputXml, null, writer);

        return outputXml;
    }

    [HandlebarsWriter(WriterType.String)]
    public string TransformToString(string document, string xslt)
    {
        return Transform(document, xslt).OuterXml;
    }

    private static XmlDocument CreateXmlDocument(string document)
    {
        return new XmlDocument
        {
            InnerXml = RemoveXmlVersionRegex.Replace(document, string.Empty)
        };
    }

    private static XslCompiledTransform CreateXslCompiledTransform(string document)
    {
        var xslt = new XslCompiledTransform();
        xslt.Load(document);
        return xslt;
    }
}