using System.Linq;
using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class RegexHelpers : BaseHelpers, IHelpers
    {
        [HandlebarsWriter(WriterType.WriteSafeString)]
        public bool IsMatch(string value, string regexPattern, string? options = null)
        {
            if (!string.IsNullOrWhiteSpace(options))
            {
                RegexOptions regexOptions = RegexOptions.None;
                foreach (char ch in options.Distinct())
                {
                    switch (ch)
                    {
                        case 'i':
                            regexOptions |= RegexOptions.IgnoreCase;
                            break;
                        case 'm':
                            regexOptions |= RegexOptions.Multiline;
                            break;
                        case 'n':
                            regexOptions |= RegexOptions.ExplicitCapture;
                            break;
                        case 'c':
                            regexOptions |= RegexOptions.Compiled;
                            break;
                        case 's':
                            regexOptions |= RegexOptions.Singleline;
                            break;
                        case 'x':
                            regexOptions |= RegexOptions.IgnorePatternWhitespace;
                            break;
                        case 'r':
                            regexOptions |= RegexOptions.RightToLeft;
                            break;
                        case 'e':
                            regexOptions |= RegexOptions.ECMAScript;
                            break;
                        case 'C':
                            regexOptions |= RegexOptions.CultureInvariant;
                            break;
                    }
                }

                return Regex.Match(value, regexPattern, regexOptions).Success;
            }

            return Regex.Match(value, regexPattern).Success;
        }

        public RegexHelpers(HandlebarsHelpersOptions options) : base(options)
        {
        }
    }
}