using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class MathHelpers : BaseHelpers, IHelpers
    {
        [HandlebarsWriter(WriterType.Value)]
        public object Add(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 + x2, (x1, x2) => x1 + x2, (x1, x2) => x1 + x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Abs(object value)
        {
            return ExecuteUtils.Execute(Context, value, Math.Abs, Math.Abs, Math.Abs);
        }

        [HandlebarsWriter(WriterType.Value)]
        public double Avg(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => (x1 + x2) / 2.0);
        }

        [HandlebarsWriter(WriterType.Value)]
        public double Ceiling(object value)
        {
            return ExecuteUtils.Execute(value, x => Math.Ceiling(1.0 * x));
        }

        [HandlebarsWriter(WriterType.Value)]
        public double Divide(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => 1.0 * x1 / 1.0 * x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public bool Equal(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 == x2, (x1, x2) => x1 == x2, (x1, x2) => x1 == x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public double Floor(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, (x1, x2) => Math.Floor(1.0 * x1 / 1.0 * x2));
        }

        [HandlebarsWriter(WriterType.Value)]
        public bool GreaterThan(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 > x2, (x1, x2) => x1 > x2, (x1, x2) => x1 > x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public bool GreaterThanEqual(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 >= x2, (x1, x2) => x1 >= x2, (x1, x2) => x1 >= x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public bool LessThan(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 < x2, (x1, x2) => x1 < x2, (x1, x2) => x1 < x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public bool LessThanEqual(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 <= x2, (x1, x2) => x1 <= x2, (x1, x2) => x1 <= x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Max(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => Math.Max(x1, x2), (x1, x2) => Math.Max(x1, x2), (x1, x2) => Math.Max(x1, x2));
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Min(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => Math.Min(x1, x2), (x1, x2) => Math.Min(x1, x2), (x1, x2) => Math.Min(x1, x2));
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Minus(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 - x2, (x1, x2) => x1 - x2, (x1, x2) => x1 - x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Modulo(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 % x2, (x1, x2) => x1 % x2, (x1, x2) => x1 % x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Multiply(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 * x2, (x1, x2) => x1 * x2, (x1, x2) => x1 * x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public bool NotEqual(object value1, object value2)
        {
            return ExecuteUtils.Execute(Context, value1, value2, (x1, x2) => x1 != x2, (x1, x2) => x1 != x2, (x1, x2) => x1 != x2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Plus(object value1, object value2)
        {
            return Add(value1, value2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Power(object value1, object value2)
        {
            return ExecuteUtils.Execute(value1, value2, Math.Pow);
        }

        [HandlebarsWriter(WriterType.Value)]
        public double Round(object value)
        {
            return ExecuteUtils.Execute(value, Math.Round);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Sign(object value)
        {
            return ExecuteUtils.Execute(Context, value, Math.Sign, l => Math.Sign(l), d => Math.Sign(d));
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Subtract(object value1, object value2)
        {
            return Minus(value1, value2);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Sqrt(object value)
        {
            return ExecuteUtils.Execute(value, Math.Sqrt);
        }

        [HandlebarsWriter(WriterType.Value)]
        public object Times(object value1, object value2)
        {
            return Multiply(value1, value2);
        }

        public MathHelpers(IHandlebars context) : base(context)
        {
        }
    }
}