using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class RegexHelper : IHelper
    {
        [HandlebarsWriter(WriterType.WriteSafeString)]
        public bool IsMatch(object value, object regexPattern)
        {
            var match = ExecuteUtils.Execute(value, regexPattern, (c1, c2) => Regex.Match(c1.ToString(), c2.ToString()), (s1, s2) => Regex.Match(s1, s2));

            return match.Success;
        }
    }
}