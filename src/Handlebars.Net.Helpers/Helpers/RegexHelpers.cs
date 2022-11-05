using System.Linq;
using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class RegexHelpers : BaseHelpers, IHelpers
{
    [HandlebarsWriter(WriterType.Value)]
    public bool IsMatch(string value, string regexPattern, string? options = null)
    {
        return Match(value, regexPattern, options) is not null;
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Match(string value, string regexPattern, string? options = null)
    {
        return MatchInternal(false, value, regexPattern, options);
    }

    [HandlebarsWriter(WriterType.Value, usage: HelperUsage.Block)]
    public object? Match(bool isBlockHelper, string value, string regexPattern, string? options = null)
    {
        return MatchInternal(isBlockHelper, value, regexPattern, options);
    }

    private static object? MatchInternal(bool isBlockHelper, string value, string regexPattern, string? options = null)
    {
        Regex regex;
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

            regex = new Regex(regexPattern, regexOptions);
        }
        else
        {
            regex = new Regex(regexPattern);
        }

        var namedGroups = RegexUtils.GetNamedGroups(regex, value);
        if (isBlockHelper && namedGroups.Any())
        {
            return namedGroups;
        }

        var match = regex.Match(value);
        if (match.Success)
        {
            return match.Value;
        }

        return null;
    }

    public RegexHelpers(IHandlebars context) : base(context)
    {
    }
}