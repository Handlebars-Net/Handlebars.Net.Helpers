using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Models;

public class WrappedString
{
    private const string Start = "ϟЊҜߍ";
    private const string End = "ߍҜЊϟ";
    private const string Pattern = $"{Start}(.*){End}"; // Regex pattern
    private const string Replacement = "$1"; // $1 refers to the first capture group

    public string Value { get; }
    public string WrappedValue { get; }

    public WrappedString(string value)
    {
        Value = Guard.NotNull(value);
        WrappedValue = $"{Start}{Value}{End}";
    }
    public override string ToString()
    {
        return WrappedValue;
    }

    public static bool TryDecode(string text, [NotNullWhen(true)] out string? decoded)
    {
        Guard.NotNull(text);

        if (TryRegexReplace(text, out decoded))
        {
            return true;
        }

        try
        {
            // Because Handlebars uses Html encoding on the Start and End tokens, try to HtmlDecode the provided text.
            var htmlDecoded = HtmlUtils.HtmlDecode(text);
            
            // Check if the html decoded value contains the Start and End token and try to use regex to extract the value between the Start and End tokens.
            if (TryRegexReplace(htmlDecoded, out decoded))
            {
                return true;
            }
            
            decoded = text;
            return false;
        }
        catch
        {
            // Ignore exceptions from HtmlUtils.HtmlDecode or TryRegexReplace
        }

        decoded = text;
        return false;
    }

    private static bool TryRegexReplace(string input, [NotNullWhen(true)] out string? output)
    {
        // Check if the string value contains the Start and End token.
        if (input.Contains(Start) && input.Contains(End))
        {
            // Use regex to extract the value between the Start and End tokens.
            output = Regex.Replace(input, Pattern, Replacement);
            return true;
        }

        output = null;
        return false;
    }
}