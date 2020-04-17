using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class MathHelper : IHelper
    {
        [HandlebarsWriter(WriterType.Write)]
        public object Add(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => x1 + x2, (x1, x2) => x1 + x2, (x1, x2) => x1 + x2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Abs(object value)
        {
            return ExecuteUtils.Execute(value, Math.Abs, Math.Abs, Math.Abs);
        }

        [HandlebarsWriter(WriterType.Write)]
        public double Avg(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => (x1 + x2) / 2.0);
        }

        [HandlebarsWriter(WriterType.Write)]
        public double Ceil(object value)
        {
            return ExecuteUtils.Execute(value, x => Math.Ceiling(1.0 * x));
        }

        [HandlebarsWriter(WriterType.Write)]
        public double Divide(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => 1.0 * x1 / 1.0 * x2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public double Floor(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => Math.Floor(1.0 * x1 / 1.0 * x2));
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Max(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, Math.Max, Math.Max, Math.Max);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Min(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, Math.Min, Math.Min, Math.Min);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Minus(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => x1 - x2, (x1, x2) => x1 - x2, (x1, x2) => x1 - x2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Modulo(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => x1 % x2, (x1, x2) => x1 % x2, (x1, x2) => x1 % x2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Multiply(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => x1 * x2, (x1, x2) => x1 * x2, (x1, x2) => x1 * x2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Plus(object value1, object value2)
        {
            return Add(value1, value2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Pow(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, Math.Pow);
        }

        [HandlebarsWriter(WriterType.Write)]
        public double Round(object value)
        {
            return ExecuteUtils.Execute(value, Math.Round);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Sign(object value)
        {
            return ExecuteUtils.Execute(value, Math.Sign, l => Math.Sign(l), d => Math.Sign(d));
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Subtract(object value1, object value2)
        {
            return Minus(value1, value2);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Sqrt(object value)
        {
            return ExecuteUtils.Execute(value, Math.Sqrt);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Sum(IEnumerable<object> values)
        {
            return ExecuteUtils.Execute(values, x => x.Sum());
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Times(object value1, object value2)
        {
            return Multiply(value1, value2);
        }
    }
}