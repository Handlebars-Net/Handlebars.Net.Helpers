using System;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
#if !NETSTANDARD1_3
using Wmhelp.XPath2;
#endif

namespace HandlebarsDotNet.Helpers
{
    internal class XPathHelpers : BaseHelpers, IHelpers
    {
        public XPathHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.String)]
        public string? SelectSingleNode(string document, string xpath)
        {
            var nav = CreateNavigator(document);
            try
            {
#if NETSTANDARD1_3
                    var result = nav.SelectSingleNode(xpath);
#else
                var result = nav.XPath2SelectSingleNode(xpath);
#endif
                return result.OuterXml;
            }
            catch (Exception)
            {
                // Ignore Exception
                return null;
            }
        }

        [HandlebarsWriter(WriterType.String)]
        public string? SelectNodes(string document, string xpath)
        {
            var nav = CreateNavigator(document);
            try
            {
#if NETSTANDARD1_3
                var result = nav.Select(xpath);
#else
                var result = nav.XPath2SelectNodes(xpath);
#endif
                var resultXml = new StringBuilder();
                foreach (XPathNavigator node in result)
                {
                    resultXml.Append(node.OuterXml);
                }

                return resultXml.ToString();
            }
            catch (Exception)
            {
                // Ignore Exception
                return null;
            }
        }

        [HandlebarsWriter(WriterType.Value)]
        public object? Evaluate(string document, string xpath)
        {
            var nav = CreateNavigator(document);

            try
            {
#if NETSTANDARD1_3
                var result = nav.Evaluate(xpath);
#else
                var result = nav.XPath2Evaluate(xpath);
#endif
                return result;
            }
            catch (Exception)
            {
                // Ignore Exception
                return null;
            }
        }

        private XPathNavigator CreateNavigator(string document)
        {
            return new XmlDocument { InnerXml = document }.CreateNavigator();
        }
    }
}