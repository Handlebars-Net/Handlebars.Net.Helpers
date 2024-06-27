using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using Stef.Validation;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

internal class XsltHelpers : BaseHelpers, IHelpers
{
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

    // Remove the "<?xml version='1.0' standalone='no'?>" from a XML document.
    // (https://github.com/WireMock-Net/WireMock.Net/issues/618)
    private static readonly Regex RemoveXmlVersionRegex = new(@"(<\?xml.*?\?>)", RegexOptions.Compiled, RegexTimeout);

    public XsltHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.Value)]
    public XmlDocument Transform(string inputXml, string xsltString)
    {
        Guard.NotNullOrEmpty(inputXml);
        Guard.NotNullOrEmpty(xsltString);

        var inputDoc = CreateXmlDocument(inputXml);
        var xslt = CreateXslCompiledTransform(xsltString);

        var outputDoc = new XmlDocument();

        // Create an XmlWriter that writes directly into the XmlDocument
        using var xmlWriter = outputDoc.CreateNavigator()!.AppendChild();

        // Apply the transformation
        xslt.Transform(inputDoc, xmlWriter);

        return outputDoc;
    }

    [HandlebarsWriter(WriterType.String)]
    public string TransformToString(string document, string xslt, bool? indent = null, bool? removeXmlVersion = null)
    {
        var outputDoc = Transform(document, xslt);

        string result;
        if (indent == false)
        {
            result = outputDoc.OuterXml;
            return removeXmlVersion == false ? result : RemoveXmlVersion(result);
        }

        // Define the settings to use for indentation
        var settings = new XmlWriterSettings
        {
            Indent = true
        };

        // Now, serialize the XmlDocument with indentation
        var stringWriter = new StringWriter();
        using (var indentingWriter = XmlWriter.Create(stringWriter, settings))
        {
            outputDoc.WriteTo(indentingWriter);
        }

        result = stringWriter.ToString();
        return removeXmlVersion == false ? result : RemoveXmlVersion(result);
    }

    private static XmlDocument CreateXmlDocument(string document)
    {
        return new XmlDocument
        {
            InnerXml = RemoveXmlVersion(document)
        };
    }

    private static XslCompiledTransform CreateXslCompiledTransform(string xsltString)
    {
        var xslt = new XslCompiledTransform();
        using var xsltReader = XmlReader.Create(new StringReader(xsltString));
        xslt.Load(xsltReader);

        return xslt;
    }

    private static string RemoveXmlVersion(string xml)
    {
        return RemoveXmlVersionRegex.Replace(xml, string.Empty).Trim();
    }
}