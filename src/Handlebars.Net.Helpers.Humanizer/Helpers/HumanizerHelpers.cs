using System;
using System.Linq;
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
        public string Transform(string value, string transformer)
        {
            if (transformer == null)
            {
                return value.Transform();
            }

            return value.Transform(MapToStringTransformer(transformer));
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