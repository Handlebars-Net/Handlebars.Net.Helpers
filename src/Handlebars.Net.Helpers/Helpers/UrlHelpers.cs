using System.Net;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class UrlHelpers : IHelpers
    {
        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string DecodeUri(string value)
        {
            return WebUtility.UrlDecode(value);
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string EncodeUri(string value)
        {
            return WebUtility.UrlEncode(value);
        }
    }
}
