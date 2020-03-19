using System;

namespace HandlebarsDotNet.Helpers.Models
{
    internal static class ExecuteUtils
    {
        public static object Execute(object value, Func<int, int> intFunc, Func<long, long> longFunc, Func<double, double> doubleFunc)
        {
            if (value == null)
            {
                return null;
            }

            switch (value)
            {
                case int valueAsInt:
                    return intFunc(valueAsInt);

                case string valueAsString:
                    if (long.TryParse(valueAsString, out long valueAsLong))
                    {
                        return longFunc(valueAsLong);
                    }

                    return doubleFunc(double.Parse(valueAsString));

                default:
                    throw new NotSupportedException();
            }
        }
    }
}