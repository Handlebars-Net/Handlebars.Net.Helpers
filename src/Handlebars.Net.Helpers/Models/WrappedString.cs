using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Models;

public class WrappedString
{
    private const string Start = "ϟЊҜߍ";
    private const string End = "ߍҜЊϟ";
    private const string Pattern = $"{Start}(.+){End}"; // Regex pattern
    private const string Replacement = "$1"; // $1 refers to the first capture group

    public string Value { get; }

    public string WrappedValue { get; }

    public WrappedString(string value)
    {
        Value = Guard.NotNull(value);
        WrappedValue = $"{Start}{value}{End}";
    }

    public override string ToString()
    {
        return WrappedValue;
    }

    public static bool TryDecode(string value, [NotNullWhen(true)] out string? decoded)
    {
        Guard.NotNull(value);

        static string RegexReplace(string input)
        {
            return Regex.Replace(input, Pattern, Replacement);
        }

        if (value.Contains(Start) && value.Contains(End))
        {
            decoded = RegexReplace(value);
            return true;
        }

        try
        {
            var htmlDecoded = HtmlUtils.HtmlDecode(value);
            decoded = RegexReplace(htmlDecoded);
            return true;
        }
        catch
        {
            // Ignore
        }

        decoded = default;
        return false;
    }
}