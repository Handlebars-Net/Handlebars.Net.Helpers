using System.Net;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class UrlHelpers : BaseHelpers, IHelpers
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

        public UrlHelpers(HandlebarsHelpersOptions options) : base(options)
        {
        }
    }
}
