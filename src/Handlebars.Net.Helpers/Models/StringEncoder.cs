using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Models;

public static class StringEncoder
{
    private const string Start = "ϟЊҜߍ";
    private const string End = "ߍҜЊϟ";
    private const string Pattern = $"{Start}(.*){End}"; // Regex pattern
    private const string Replacement = "$1"; // $1 refers to the first capture group

    public static string Encode(string value)
    {
        return $"{Start}{Guard.NotNull(value)}{End}";
    }

    public static bool TryDecode(string value, [NotNullWhen(true)] out string? decoded)
    {
        Guard.NotNull(value);

        if (value.Contains(Start) && value.Contains(End))
        {
            decoded = RegexReplace(value);
            return true;
        }

        try
        {
            var htmlDecoded = HtmlUtils.HtmlDecode(value);
            if (htmlDecoded != value)
            {
                decoded = RegexReplace(htmlDecoded);
                return decoded != htmlDecoded;
            }

            decoded = value;
            return false;
        }
        catch
        {
            // Ignore
        }

        decoded = default;
        return false;
    }

    private static string RegexReplace(string input)
    {
        return Regex.Replace(input, Pattern, Replacement);
    }
}