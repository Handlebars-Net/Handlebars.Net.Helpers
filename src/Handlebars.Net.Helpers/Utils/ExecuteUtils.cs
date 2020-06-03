using System;
using System.Collections.Generic;
using System.Linq;

namespace HandlebarsDotNet.Helpers.Utils
{
    internal static class ExecuteUtils
    {
        public static object Execute(object value, Func<int, int> intFunc, Func<long, long> longFunc, Func<double, double> doubleFunc)
        {
            switch (value)
            {
                case int valueAsInt:
                    return intFunc(valueAsInt);

                case long valueAsLong:
                    return longFunc(valueAsLong);

                case double valueAsDouble:
                    return doubleFunc(valueAsDouble);

                case string valueAsString:
                    if (long.TryParse(valueAsString, out long valueParsedAsLong))
                    {
                        return longFunc(valueParsedAsLong);
                    }

                    return doubleFunc(double.Parse(valueAsString));

                default:
                    // Just call ToString()
                    return Execute(value.ToString(), intFunc, longFunc, doubleFunc);
            }
        }

        public static object Execute(object value1, object value2, Func<int, int, int> intFunc, Func<long, long, long> longFunc, Func<double, double, double> doubleFunc)
        {
            var supported = new[] { typeof(int), typeof(long), typeof(double) };

            switch (value1, value2)
            {
                case (int int1, int int2):
                    return intFunc(int1, int2);

                case (int int1, long long2):
                    return longFunc(int1, long2);

                case (int int1, double double2):
                    return doubleFunc(int1, double2);

                case (long long1, long long2):
                    return longFunc(long1, long2);

                case (long long1, int int2):
                    return longFunc(long1, int2);

                case (long long1, double double2):
                    return doubleFunc(long1, double2);

                case (double double1, double double2):
                    return doubleFunc(double1, double2);

                case (double double1, int int2):
                    return doubleFunc(double1, int2);

                case (double double1, long long2):
                    return doubleFunc(double1, long2);

                default:
                    object object1 = value1;
                    object object2 = value2;
                    if (!supported.Contains(value1.GetType()))
                    {
                        object1 = Parse(value1 is string string1 ? string1 : value1.ToString());
                    }
                    if (!supported.Contains(value2.GetType()))
                    {
                        object2 = Parse(value2 is string string2 ? string2 : value2.ToString());
                    }
                    return Execute(object1, object2, intFunc, longFunc, doubleFunc);
            }
        }

        public static double Execute(object value, Func<double, double> doubleFunc)
        {
            try
            {
                double @double = (double)value;
                return doubleFunc(@double);
            }
            catch
            {
                throw new NotSupportedException();
            }
        }

        public static double Execute(IEnumerable<object> values, Func<IEnumerable<double>, double> doubleFunc)
        {
            try
            {
                return doubleFunc(values.Cast<double>());
            }
            catch
            {
                throw new NotSupportedException();
            }
        }

        public static double Execute(object value1, object value2, Func<double, double, double> doubleFunc)
        {
            try
            {
                double double1 = (double)value1;
                double double2 = (double)value2;
                return doubleFunc(double1, double2);
            }
            catch
            {
                throw new NotSupportedException();
            }
        }

        private static object Parse(string stringValue)
        {
            if (int.TryParse(stringValue, out int intValue))
            {
                return intValue;
            }

            if (long.TryParse(stringValue, out long longValue))
            {
                return longValue;
            }

            if (double.TryParse(stringValue, out double doubleValue))
            {
                return doubleValue;
            }

            throw new NotSupportedException();
        }
    }
}