using Fare;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

namespace HandlebarsDotNet.Helpers
{
    internal class XegerHelpers : BaseHelpers, IHelpers
    {
        public XegerHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.String)]
        public string Xeger(string pattern)
        {
            return new Xeger(pattern).Generate();
        }
    }
}