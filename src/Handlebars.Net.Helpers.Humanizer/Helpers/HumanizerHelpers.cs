using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Utils;
using Humanizer;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

public class HumanizerHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    private const string Separator = "…";

    [HandlebarsWriter(WriterType.String)]
    public string Camelize(string value)
    {
        return value.Camelize();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Dasherize(string value)
    {
        return value.Dasherize();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Dehumanize(string value)
    {
        return value.Dehumanize();
    }

    [HandlebarsWriter(WriterType.String)]
    public string FormatWith(string value, params object[] args)
    {
        return value.FormatWith(args);
    }

    [HandlebarsWriter(WriterType.String)]
    public double FromMetric(string value)
    {
        return value.FromMetric();
    }

    [HandlebarsWriter(WriterType.Value)]
    public int FromRoman(string value)
    {
        return value.FromRoman();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Humanize(object value)
    {
        switch (value)
        {
            case string stringValue:
                if (DateTime.TryParse(stringValue, out var parsedAsDateTime))
                {
                    return parsedAsDateTime.Humanize();
                }

                if (TimeSpan.TryParse(stringValue, out var parsedAsTimeSpan))
                {
                    return parsedAsTimeSpan.Humanize();
                }

                return stringValue.Humanize();

            case DateTime dateTimeValue:
                return dateTimeValue.Humanize();

            case TimeSpan timeSpanValue:
                return timeSpanValue.Humanize();

            default:
                throw new ArgumentOutOfRangeException($"The value '{value}' is not supported in the Humanize(...) method.");
        }
    }

    [HandlebarsWriter(WriterType.String)]
    public string Hyphenate(string value)
    {
        return value.Hyphenate();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Kebaberize(string value)
    {
        return value.Kebaberize();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Ordinalize(object value)
    {
        switch (value)
        {
            case int intValue:
                return intValue.Ordinalize();

            case string stringValue:
                return stringValue.Ordinalize();

            default:
                throw new NotSupportedException($"The value '{value}' must be an int or string.");
        }
    }

    [HandlebarsWriter(WriterType.String)]
    public string Pascalize(string value)
    {
        return value.Pascalize();
    }

    [HandlebarsWriter(WriterType.String)]
    public string Pluralize(string value, bool inputIsKnownToBeSingular = false)
    {
        return value.Pluralize(inputIsKnownToBeSingular);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Singularize(string value, bool inputIsKnownToBePlural = false, bool skipSimpleWords = false)
    {
        return value.Singularize(inputIsKnownToBePlural, skipSimpleWords);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Titleize(string value)
    {
        return value.Titleize();
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToMetric(object value)
    {
        return value switch
        {
            int intValue => intValue.ToMetric(),
            double doubleValue => doubleValue.ToMetric(),
            _ => throw new NotSupportedException($"The value '{value}' must be an int or double.")
        };
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToOrdinalWords(object value, string? grammaticalGender = nameof(GrammaticalGender.Masculine))
    {
        if (!Enum.TryParse<GrammaticalGender>(grammaticalGender, out var grammaticalGenderAsEnum))
        {
            throw new NotSupportedException($"The value '{grammaticalGender}' cannot be converted to a '{typeof(GrammaticalGender).FullName}'.");
        }

        switch (value)
        {
            case string stringValue:
                if (DateTime.TryParse(stringValue, out var parsedAsDateTime))
                {
                    return parsedAsDateTime.ToOrdinalWords();
                }

                throw new NotSupportedException($"The value '{value}' is not a valid DateTime.");

            case DateTime dateTimeValue:
                return dateTimeValue.ToOrdinalWords();

            case int intValue:
                return intValue.ToOrdinalWords(grammaticalGenderAsEnum);

            default:
                throw new NotSupportedException($"The value '{value}' must be an int, string or DateTime.");
        }
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToQuantity(string value, object quantity)
    {
        return ExecuteUtils.Execute(Context, quantity,
            quantityAsInt => value.ToQuantity(quantityAsInt),
            quantityAsLong => value.ToQuantity(quantityAsLong),
            value.ToQuantity
        );
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToRoman(int value)
    {
        return value.ToRoman();
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToWords(object number, string? grammaticalGender = nameof(GrammaticalGender.Masculine))
    {
        if (!Enum.TryParse<GrammaticalGender>(grammaticalGender, out var grammaticalGenderAsEnum))
        {
            throw new NotSupportedException($"The value '{grammaticalGender}' cannot be converted to a '{typeof(GrammaticalGender).FullName}'.");
        }

        return number switch
        {
            int intValue => intValue.ToWords(grammaticalGenderAsEnum),
            long longValue => longValue.ToWords(grammaticalGenderAsEnum),
            _ => throw new NotSupportedException($"The value '{number}' must be an int or long.")
        };
    }

    [HandlebarsWriter(WriterType.String)]
    public string Transform(string value, string? transformer)
    {
        if (transformer == null)
        {
            return value.Transform();
        }

        return value.Transform(MapToStringTransformer(transformer));
    }

    [HandlebarsWriter(WriterType.String, $"{nameof(Humanizer)}.{nameof(Truncate)}")]
    public string Truncate(string value, int length, string? separator = null, string? truncator = null, string? truncateFrom = null)
    {
        if (string.IsNullOrWhiteSpace(separator))
        {
            separator = Separator;
        }

        if (string.IsNullOrWhiteSpace(truncator))
        {
            truncator = nameof(Truncator.FixedLength);
        }

        if (string.IsNullOrWhiteSpace(truncateFrom))
        {
            truncateFrom = nameof(TruncateFrom.Right);
        }

        if (!Enum.TryParse<TruncateFrom>(truncateFrom, out var truncateFromAsEnum))
        {
            throw new NotSupportedException($"The value '{truncateFrom}' cannot be converted to a '{typeof(TruncateFrom).FullName}'.");
        }

        return value.Truncate(length, separator, MapToTruncator(truncator), truncateFromAsEnum);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Underscore(string value)
    {
        return value.Underscore();
    }

    private static ITruncator MapToTruncator(string? value)
    {
        return value switch
        {
            nameof(Truncator.FixedLength) => Truncator.FixedLength,
            nameof(Truncator.FixedNumberOfCharacters) => Truncator.FixedNumberOfCharacters,
            nameof(Truncator.FixedNumberOfWords) => Truncator.FixedNumberOfWords,
            _ => throw new NotSupportedException($"The value '{value}' cannot be converted to a '{typeof(ITruncator).FullName}'.")
        };
    }

    private static IStringTransformer MapToStringTransformer(string value)
    {
        return value switch
        {
            nameof(To.LowerCase) => To.LowerCase,
            nameof(To.SentenceCase) => To.SentenceCase,
            nameof(To.TitleCase) => To.TitleCase,
            nameof(To.UpperCase) => To.UpperCase,
            _ => throw new NotSupportedException($"The value '{value}' cannot be converted to a '{typeof(IStringTransformer).FullName}'.")
        };
    }

    public Category Category => Category.Humanizer;
}