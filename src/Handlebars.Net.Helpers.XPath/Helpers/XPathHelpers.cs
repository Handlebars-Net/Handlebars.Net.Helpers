using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
#if !NETSTANDARD1_3
using Wmhelp.XPath2;
#endif

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

public class XPathHelpers : BaseHelpers, IHelpers
{
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

    // Remove the "<?xml version='1.0' standalone='no'?>" from a XML document.
    // (https://github.com/WireMock-Net/WireMock.Net/issues/618)
    private static readonly Regex RemoveXmlVersionRegex = new(@"(<\?xml.*?\?>)", RegexOptions.Compiled, RegexTimeout);

    public XPathHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.Value)]
    public XPathNavigator SelectNode(string document, string xpath)
    {
        return SelectSingleNode(document, xpath);
    }

    [HandlebarsWriter(WriterType.Value)]
    public XPathNavigator SelectSingleNode(string document, string xpath)
    {
        var nav = CreateNavigator(document);
        try
        {
#if NETSTANDARD1_3
            return nav.SelectSingleNode(xpath);
#else
            return nav.XPath2SelectSingleNode(xpath);
#endif
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(SelectSingleNode), ex);
        }
    }

    [HandlebarsWriter(WriterType.String)]
    public string SelectNodeAsXml(string document, string xpath)
    {
        return SelectSingleNode(document, xpath).OuterXml;
    }

    /// <summary>
    /// Added for backwards compatibility.
    /// This method just returns a concatenated string from all the string values.
    /// </summary>
    [HandlebarsWriter(WriterType.String)]
    public string SelectNodesAsString(string document, string xpath)
    {
        var listXPathNavigator = SelectNodes(document, xpath);

        var resultXml = new StringBuilder();
        foreach (var node in listXPathNavigator)
        {
            resultXml.Append(node.Value);
        }

        return resultXml.ToString();
    }

    [HandlebarsWriter(WriterType.String)]
    public string SelectNodesAsXml(string document, string xpath)
    {
        var listXPathNavigator = SelectNodes(document, xpath);

        var resultXml = new StringBuilder();
        foreach (var node in listXPathNavigator)
        {
            resultXml.Append(node.OuterXml);
        }

        return resultXml.ToString();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IReadOnlyList<XPathNavigator> SelectNodes(string document, string xpath)
    {
        var nav = CreateNavigator(document);
        try
        {
#if NETSTANDARD1_3
            var result = nav.Select(xpath);
#else
            var result = nav.XPath2SelectNodes(xpath);
#endif
            var list = new List<XPathNavigator>();
            foreach (XPathNavigator node in result)
            {
                list.Add(node);
            }

            return list.AsReadOnly();
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(SelectNodes), ex);
        }
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Evaluate(string document, string xpath)
    {
        var nav = CreateNavigator(document);

        try
        {
#if NETSTANDARD1_3
            return nav.Evaluate(xpath);
#else
            return nav.XPath2Evaluate(xpath);
#endif
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(Evaluate), ex);
        }
    }

    [HandlebarsWriter(WriterType.String)]
    public string? EvaluateToString(string document, string xpath)
    {
        return Evaluate(document, xpath)?.ToString();
    }

    private static XPathNavigator CreateNavigator(string document)
    {
        return new XmlDocument
        {
            InnerXml = RemoveXmlVersionRegex.Replace(document, string.Empty)
        }.CreateNavigator()!;
    }

    public Category Category => Category.XPath;
}