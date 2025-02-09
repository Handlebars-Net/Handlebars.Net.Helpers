using System.Net;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class UrlHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    [HandlebarsWriter(WriterType.String)]
    public string DecodeUri(string value)
    {
        return WebUtility.UrlDecode(value);
    }

    [HandlebarsWriter(WriterType.String)]
    public string? EncodeUri(string value)
    {
        return WebUtility.UrlEncode(value);
    }

    public Category Category => Category.Url;
}