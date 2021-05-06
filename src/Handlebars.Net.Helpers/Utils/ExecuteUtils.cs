using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HandlebarsDotNet.Helpers.Parsers;

namespace HandlebarsDotNet.Helpers.Utils
{
    internal static class ExecuteUtils
    {
        private static Type[] SupportedTypes = new Type[] { typeof(int), typeof(long), typeof(double) };

        public static object Execute(IHandlebars context, object value, Func<int, int> intFunc, Func<long, long> longFunc, Func<double, double> doubleFunc)
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
                    if (int.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out int valueParsedAsInt))
                    {
                        return intFunc(valueParsedAsInt);
                    }

                    if (long.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out long valueParsedAsLong))
                    {
                        return longFunc(valueParsedAsLong);
                    }

                    if (double.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out double valueParsedAsDouble))
                    {
                        return doubleFunc(valueParsedAsDouble);
                    }

                    return valueAsString;

                default:
                    // Just call ToString()
                    return Execute(context, value.ToString(), intFunc, longFunc, doubleFunc);
            }
        }

        public static TResult Execute<TResult>(IHandlebars context, object value1, object value2, Func<int, int, TResult> intFunc, Func<long, long, TResult> longFunc, Func<double, double, TResult> doubleFunc)
        {
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
                    object object1 = ParseObjectValue<object>(context, value1);
                    object object2 = ParseObjectValue<object>(context, value2);
                    //if (!SupportedTypes.Contains(value1.GetType()))
                    //{
                    //    object1 = StringValueParser.Parse(context, value1 is string string1 ? string1 : value1.ToString());
                    //}

                    //if (!SupportedTypes.Contains(value2.GetType()))
                    //{
                    //    object2 = StringValueParser.Parse(context, value2 is string string2 ? string2 : value2.ToString());
                    //}

                    return Execute(context, object1, object2, intFunc, longFunc, doubleFunc);
            }
        }

        public static double Execute(object value, Func<double, double> doubleFunc)
        {
            try
            {
                double @double = Convert.ToDouble(value);
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

        public static double Execute(IHandlebars context, object value1, object value2, Func<double, double, double> doubleFunc)
        {
            try
            {
                var double1 = ParseObjectValue<double>(context, value1);
                var double2 = ParseObjectValue<double>(context, value2);
                return doubleFunc(double1, double2);
            }
            catch
            {
                throw new NotSupportedException();
            }
        }

        private static T ParseObjectValue<T>(IHandlebars context, object value)
        {
            if (!SupportedTypes.Contains(value.GetType()))
            {
                return (T)Convert.ChangeType(StringValueParser.Parse(context, value is string stringValue ? stringValue : value.ToString()), typeof(T));
            }

            return (T)(typeof(T) == typeof(object) ? value : Convert.ChangeType(value, typeof(T)));
        }
    }
}