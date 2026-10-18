using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.RegularExpressions;

namespace HandlebarsDotNet.Helpers.Models;

/// <summary>
/// Wraps string values with known delimiters so they can be preserved through template rendering.
/// </summary>
public class WrappedString
{
    private const string Start = "ϟЊҜߍ";
    private const string End = "ߍҜЊϟ";
    private const string Pattern = Start + "(.*)" + End;
    private const string Replacement = "$1";

    /// <summary>
    /// Gets the original unwrapped value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the wrapped representation of <see cref="Value"/>.
    /// </summary>
    public string WrappedValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WrappedString"/> class.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public WrappedString(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        WrappedValue = Start + Value + End;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return WrappedValue;
    }

    /// <summary>
    /// Attempts to decode a wrapped string from plain or HTML-encoded input.
    /// </summary>
    /// <param name="text">The text to decode.</param>
    /// <param name="decoded">The decoded text when successful; otherwise the original input.</param>
    /// <returns><c>true</c> when a wrapped value could be decoded; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
    public static bool TryDecode(string text, [NotNullWhen(true)] out string? decoded)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (TryRegexReplace(text, out decoded))
        {
            return true;
        }

        try
        {
            var htmlDecoded = WebUtility.HtmlDecode(text);

            if (TryRegexReplace(htmlDecoded, out decoded))
            {
                return true;
            }

            decoded = text;
            return false;
        }
        catch
        {
        }

        decoded = text;
        return false;
    }

    private static bool TryRegexReplace(string input, [NotNullWhen(true)] out string? output)
    {
        if (input.Contains(Start) && input.Contains(End))
        {
            output = Regex.Replace(input, Pattern, Replacement);
            return true;
        }

        output = null;
        return false;
    }
}