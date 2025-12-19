using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Models;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

/// <summary>
/// Some code copied from https://www.30secondsofcode.org/c-sharp/t/string/p/1
/// and based on https://github.com/helpers/handlebars-helpers#string
/// </summary>
internal class StringHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    [HandlebarsWriter(WriterType.String)]
    public string Append(string value, string append)
    {
        if (string.IsNullOrEmpty(value))
        {
            return append;
        }

        if (string.IsNullOrEmpty(append))
        {
            return value;
        }

        return value + append;
    }

    [HandlebarsWriter(WriterType.String)]
    public static string Base64Decode(string value)
    {
        Guard.NotNull(value);

        var base64EncodedBytes = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    [HandlebarsWriter(WriterType.String)]
    public static string Base64Encode(string value)
    {
        Guard.NotNull(value);

        var plainTextBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(plainTextBytes);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Camelcase(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
        {
            return value;
        }

        string[] words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

        var builder = new StringBuilder(words[0].ToLower());
        for (int i = 1; i < words.Length; i++)
        {
            builder.Append(words[i].Substring(0, 1).ToUpper());
            builder.Append(words[i].Substring(1));
        }

        return builder.ToString();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Capitalize(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        char[] chars = value.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);
        return new string(chars);
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Coalesce(params object?[] arguments)
    {
        foreach (var arg in arguments.Where(a => a is not null and not UndefinedBindingResult))
        {
            if (arg is string stringValue)
            {
                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    return stringValue;
                }
            }
            else
            {
                return arg;
            }
        }

        return null;
    }

    [HandlebarsWriter(WriterType.String)]
    public string Concat(string value, string append)
    {
        return Append(value, append);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Contains(string value, string test)
    {
        Guard.NotNull(value);

        return value.Contains(test);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Ellipsis(string value, int length)
    {
        Guard.Condition(length, l => l >= 0);

        if (string.IsNullOrEmpty(value) || value.Length <= length)
        {
            return value;
        }

        return value.Substring(0, length) + "...";
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Equal(string value, string test, object? stringComparisonType = null)
    {
        return EnumUtils.TryParse<StringComparison>(stringComparisonType, out var comparisonType) ? string.Equals(value, test, comparisonType.Value) : value == test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Equals(string value, string test, object? stringComparisonType = null)
    {
        return Equal(value, test, stringComparisonType);
    }

    [HandlebarsWriter(WriterType.String)]
    public virtual string Format(object? value, string format)
    {
        var formatProvider = Context.Configuration.FormatProvider;

        // Attempt using a custom formatter from the format provider (if any)
        var customFormatter = formatProvider?.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
        var formattedValue = customFormatter?.Format(format, value, formatProvider);

        // Fallback to IFormattable
        formattedValue ??= (value as IFormattable)?.ToString(format, formatProvider);

        // Fallback to ToString
        formattedValue ??= value?.ToString();

        // Done
        return formattedValue ?? string.Empty;
    }

    [HandlebarsWriter(WriterType.Value)]
    public WrappedString FormatAsString(object? value, string? format = null)
    {
        return new WrappedString(Format(value, format ?? string.Empty));
    }

    [HandlebarsWriter(WriterType.String)]
    public string HtmlDecode(string? value)
    {
        return HtmlUtils.HtmlDecode(value);
    }

    [HandlebarsWriter(WriterType.String)]
    public string HtmlEncode(string? value)
    {
        return HtmlUtils.HtmlEncode(value);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool IsNotNullOrEmpty(string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool IsNotNullOrWhiteSpace(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool IsNullOrEmpty(string value)
    {
        return string.IsNullOrEmpty(value);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool IsNullOrWhiteSpace(string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool IsString(object value)
    {
        return value is string;
    }

    [HandlebarsWriter(WriterType.String)]
    public string Join(IEnumerable values, string? separator = null)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        return string.Join(separator ?? string.Empty, values.Cast<object>());
    }

    [HandlebarsWriter(WriterType.String)]
    public string Lowercase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.ToLower();
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool NotEqual(string value, string test, object? stringComparisonType = null)
    {
        return !Equal(value, test, stringComparisonType);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool NotEquals(string value, string test, object? stringComparisonType = null)
    {
        return NotEqual(value, test, stringComparisonType);
    }

    [HandlebarsWriter(WriterType.String)]
    public string PadLeft(string value, int totalWidth, string padChar)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return padChar?.Length > 0 ? value.PadLeft(totalWidth, padChar[0]) : value.PadLeft(totalWidth);
    }

    [HandlebarsWriter(WriterType.String)]
    public string PadRight(string value, int totalWidth, string padChar)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return padChar?.Length > 0 ? value.PadRight(totalWidth, padChar[0]) : value.PadRight(totalWidth);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Pascalcase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (value.Length < 2)
        {
            return value.ToUpper();
        }

        string[] words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

        var builder = new StringBuilder();
        foreach (string word in words)
        {
            builder.Append(word.Substring(0, 1).ToUpper());
            builder.Append(word.Substring(1));
        }

        return builder.ToString();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Prepend(string value, string pre)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return pre + value;
    }

    [HandlebarsWriter(WriterType.String)]
    public string Remove(string value, string oldValue)
    {
        return Replace(value, oldValue, string.Empty);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Repeat(string value, int count)
    {
        return string.Concat(Enumerable.Repeat(value, count));
    }

    [HandlebarsWriter(WriterType.String)]
    public string Replace(string value, string oldValue, string newValue)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (oldValue == null)
        {
            throw new ArgumentNullException(nameof(oldValue));
        }

        return value.Replace(oldValue, newValue);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Reverse(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return new string(value.ToCharArray().Reverse().ToArray());
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? First(IEnumerable<object?> values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        return values.FirstOrDefault();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Last(IEnumerable<object?> values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        return values.LastOrDefault();
    }

    [HandlebarsWriter(WriterType.Value)]
    public string[] Split(string value, string separator)
    {
        Guard.NotNull(value);
        Guard.NotNull(separator);

        return value.Split(new[] { separator }, StringSplitOptions.None);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool StartsWith(string value, string test)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.StartsWith(test);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Substring(string value, int start, int? end = null)
    {
        Guard.NotNullOrEmpty(value);
        return end is null ? value.Substring(start) : value.Substring(start, end.Value);
    }

    [HandlebarsWriter(WriterType.String)]
    public static string Titlecase(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var tokens = value.Split(new[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            tokens[i] = token == token.ToUpper()
                ? token
                : token.Substring(0, 1).ToUpper() + token.Substring(1).ToLower();
        }

        return string.Join(" ", tokens);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Trim(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.Trim();
    }

    [HandlebarsWriter(WriterType.String)]
    public string TrimEnd(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.TrimEnd();
    }

    [HandlebarsWriter(WriterType.String)]
    public string TrimStart(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.TrimStart();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Truncate(string value, int length)
    {
        Guard.Condition(length, l => l >= 0);

        if (string.IsNullOrEmpty(value) || value.Length <= length)
        {
            return value;
        }

        return value.Substring(0, length);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Uppercase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.ToUpper();
    }

    [HandlebarsWriter(WriterType.Value)]
    public WrappedString ToWrappedString(object? value)
    {
        return FormatAsString(value);
    }

    public Category Category => Category.String;
}