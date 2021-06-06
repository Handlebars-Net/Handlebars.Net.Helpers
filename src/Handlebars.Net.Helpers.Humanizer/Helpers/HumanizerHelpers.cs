using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Parsers;
using HandlebarsDotNet.Helpers.Utils;
using Humanizer;

namespace HandlebarsDotNet.Helpers
{
    internal class HumanizerHelpers : BaseHelpers, IHelpers
    {
        public HumanizerHelpers(IHandlebars context) : base(context)
        {
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
        public string Transform(string value, string transformer)
        {
            if (transformer == null)
            {
                return value.Transform();
            }

            return value.Transform(MapToStringTransformer(transformer));
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
        public string ToQuantity(string value, object quantity)
        {
            return ExecuteUtils.Execute(Context, quantity,
                (quantityAsInt) => value.ToQuantity(quantityAsInt),
                (quantityAsLong) => value.ToQuantity(quantityAsLong),
                (quantityAsInt) => value.ToQuantity(quantityAsInt)
            );
        }

        [HandlebarsWriter(WriterType.String)]
        public string Truncate(string value, int length, string? separator = null, string? truncator = null, string? truncateFrom = null)
        {
            if (string.IsNullOrWhiteSpace(separator))
            {
                separator = "…";
            }

            if (string.IsNullOrWhiteSpace(truncator))
            {
                truncator = "FixedLength";
            }

            if (string.IsNullOrWhiteSpace(truncateFrom))
            {
                truncateFrom = "Right";
            }

            if (!Enum.TryParse<TruncateFrom>(truncateFrom, out var truncateFromAsEnum))
            {
                throw new NotSupportedException($"The value '{truncateFrom}' cannot be converted to a '{typeof(TruncateFrom).FullName}'.");
            }

            return value.Truncate(length, separator, MapToTruncator(truncator), truncateFromAsEnum);
        }

        private static ITruncator MapToTruncator(string? value)
        {
            switch (value)
            {
                case nameof(Truncator.FixedLength):
                    return Truncator.FixedLength;

                case nameof(Truncator.FixedNumberOfCharacters):
                    return Truncator.FixedNumberOfCharacters;

                case nameof(Truncator.FixedNumberOfWords):
                    return Truncator.FixedNumberOfWords;

                default:
                    throw new NotSupportedException($"The value '{value}' cannot be converted to a '{typeof(ITruncator).FullName}'.");
            }
        }

        private static IStringTransformer MapToStringTransformer(string value)
        {
            switch (value)
            {
                case nameof(To.LowerCase):
                    return To.LowerCase;

                case nameof(To.SentenceCase):
                    return To.SentenceCase;

                case nameof(To.TitleCase):
                    return To.TitleCase;

                case nameof(To.UpperCase):
                    return To.UpperCase;

                default:
                    throw new NotSupportedException($"The value '{value}' cannot be converted to a '{typeof(IStringTransformer).FullName}'.");
            }
        }
    }
}