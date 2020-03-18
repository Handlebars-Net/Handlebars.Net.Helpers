using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    //[HandlebarsHelper]
    internal class MathHelper : IHelper
    {
        [HandlebarsWriter(WriterType.Write)]
        public object Abs(object value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value)
            {
                case int valueAsInt:
                    return Math.Abs(valueAsInt);

                case long valueAsLong:
                    return Math.Abs(valueAsLong);

                case string valueAsString:
                    return Math.Abs(double.Parse(valueAsString));

                default:
                    throw new NotSupportedException();
            }
        }
    }
}