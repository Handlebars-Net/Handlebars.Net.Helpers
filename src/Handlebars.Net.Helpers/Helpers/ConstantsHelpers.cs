using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class ConstantsHelpers : BaseHelpers, IHelpers
    {
        [HandlebarsWriter(WriterType.Write, "Constants.Math.E")]
        public double E()
        {
            return Math.E;
        }

        [HandlebarsWriter(WriterType.Write, "Constants.Math.PI")]
        public double PI()
        {
            return Math.PI;
        }
        public ConstantsHelpers(HandlebarsHelpersOptions options) : base(options)
        {
        }
    }
}
