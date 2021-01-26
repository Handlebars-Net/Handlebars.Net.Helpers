using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class GenericFormatHelpers : BaseHelpers, IHelpers
    {
        public GenericFormatHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.String)]
        public string Format(object value, string format)
        {
            switch (value)
            {
                case DateTime dateTime:
                    return dateTime.ToString(format, Context.Configuration.FormatProvider);

                default:
                    throw new NotSupportedException($"The method {nameof(Format)} cannot be used on value '{value}' of Type '{value?.GetType()}'.");
            }
        }
    }
}