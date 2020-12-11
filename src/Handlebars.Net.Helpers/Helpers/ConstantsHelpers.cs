using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class ConstantsHelpers : BaseHelpers, IHelpers
    {
        [HandlebarsWriter(WriterType.Value, "Constants.Math.E")]
        public double E()
        {
            return Math.E;
        }

        [HandlebarsWriter(WriterType.Value, "Constants.Math.PI")]
        public double PI()
        {
            return Math.PI;
        }
        public ConstantsHelpers(IHandlebars context) : base(context)
        {
        }
    }
}
