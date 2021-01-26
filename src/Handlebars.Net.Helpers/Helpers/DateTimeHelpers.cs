using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class DateTimeHelpers : BaseHelpers, IHelpers
    {
        public DateTimeHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.String)]
        public string Format(DateTime value, string format)
        {
            return value.ToString(format, Context.Configuration.FormatProvider);
        }
    }
}