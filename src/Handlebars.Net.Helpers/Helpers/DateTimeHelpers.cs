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

        [HandlebarsWriter(WriterType.Value)]
        public DateTime Now()
        {
            return DateTime.Now;
        }

        [HandlebarsWriter(WriterType.Value)]
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}