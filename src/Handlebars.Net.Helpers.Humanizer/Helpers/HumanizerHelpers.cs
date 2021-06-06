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
        public string Transform(string value, params string[] transformers)
        {
            if (transformers == null || transformers.Length == 0)
            {
                return value.Transform();
            }

            // todo
            // "HUMANIZER".Transform(To.LowerCase, To.TitleCase) => "Humanizer"
            return value.Transform();
        }
    }
}