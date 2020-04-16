using System;

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
                    throw new NotSupportedException();
            }
        }

        public static object Execute(object value1, object value2, Func<int, int, int> intFunc, Func<long, long, long> longFunc, Func<double, double, double> doubleFunc)
        {
            switch (value1, value2)
            {
                case (int int1, int int2):
                    return intFunc(int1, int2);

                case (double double1, double double2):
                    return doubleFunc(double1, double2);

                case (int int1, double double2):
                    return doubleFunc(int1, double2);

                case (double double1, int int2):
                    return doubleFunc(double1, int2);

                case (int int1, string string2):
                    return Execute(int1.ToString(), string2, longFunc, doubleFunc);

                case (string string1, int int2):
                    return Execute(string1, int2.ToString(), longFunc, doubleFunc);

                case (string string1, string string2):
                    return Execute(string1, string2, longFunc, doubleFunc);
            }

            throw new NotSupportedException();
        }

        //public static T Execute<T>(object value1, object value2, Func<char, char, T> charFunc, Func<string, string, T> stringFunc)
        //{
        //    switch (value1, value2)
        //    {
        //        case (char char1, char char2):
        //            return charFunc(char1, char2);

        //        case (string string1, string string2):
        //            return stringFunc(string1, string2);

        //        case (char char1, string string2):
        //            return stringFunc(char1.ToString(), string2);

        //        case (string string1, char char2):
        //            return stringFunc(string1, char2.ToString());
        //    }

        //    throw new NotSupportedException();
        //}

        private static object Execute(string string1, string string2, Func<long, long, long> longFunc, Func<double, double, double> doubleFunc)
        {
            if (long.TryParse(string1, out long long1) && long.TryParse(string2, out long long2))
            {
                return longFunc(long1, long2);
            }

            if (double.TryParse(string1, out double double1) && double.TryParse(string2, out double double2))
            {
                return doubleFunc(double1, double2);
            }

            throw new NotSupportedException();
        }
    }
}