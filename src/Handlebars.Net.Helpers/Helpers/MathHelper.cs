using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class MathHelper : IHelper
    {
        [HandlebarsWriter(WriterType.Write)]
        public object Abs(object value)
        {
            return ExecuteUtils.Execute(value, i => Math.Abs(i), l => Math.Abs(l), d => Math.Abs(d));
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