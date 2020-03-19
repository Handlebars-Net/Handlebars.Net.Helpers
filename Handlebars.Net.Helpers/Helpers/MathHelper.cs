using System;
using System.Collections.Generic;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Models;

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
    }
}