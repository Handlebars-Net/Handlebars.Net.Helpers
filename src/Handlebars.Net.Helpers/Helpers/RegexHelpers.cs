using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class RegexHelpers : IHelpers
    {
        [HandlebarsWriter(WriterType.WriteSafeString)]
        public bool IsMatch(string value, string regexPattern)
        {
            return Regex.Match(value, regexPattern).Success;
        }
    }
}