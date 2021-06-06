﻿using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using Humanizer;

namespace HandlebarsDotNet.Helpers
{
    internal class HumanizerHelpers : BaseHelpers, IHelpers
    {
        public HumanizerHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.String)]
        public string Humanize(string value)
        {
            return value.Humanize();
        }

        [HandlebarsWriter(WriterType.String)]
        public string Dehumanize(string value)
        {
            return value.Dehumanize();
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
        public string Truncate(string value, int length, string? separator = null, string? truncator = null, string? truncateFrom = null)
        {
            if (string.IsNullOrWhiteSpace(separator))
            {
                return value.Truncate(length);
            }

            if (string.IsNullOrWhiteSpace(truncator))
            {
                return value.Truncate(length, separator);
            }

            if (string.IsNullOrWhiteSpace(truncateFrom))
            {
                return value.Truncate(length, separator, MapToTruncator(truncator));
            }

            if (!Enum.TryParse<TruncateFrom>(truncateFrom, out var truncateFromAsEnum))
            {
                throw new ArgumentOutOfRangeException($"The value '{truncateFrom}' cannot be converted to a '{typeof(TruncateFrom).FullName}'.");
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
                    throw new ArgumentOutOfRangeException($"The value '{value}' cannot be converted to a '{typeof(ITruncator).FullName}'.");
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
                    throw new ArgumentOutOfRangeException($"The value '{value}' cannot be converted to a '{typeof(IStringTransformer).FullName}'.");
            }
        }
    }
}