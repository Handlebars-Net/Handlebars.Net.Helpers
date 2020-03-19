using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

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

        [HandlebarsWriter(WriterType.Write)]
        public object Sign(object value)
        {
            return ExecuteUtils.Execute(value, i => Math.Sign(i), l => Math.Sign(l), d => Math.Sign(d));
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Min(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, Math.Min, Math.Min, Math.Min);
        }
    }
}