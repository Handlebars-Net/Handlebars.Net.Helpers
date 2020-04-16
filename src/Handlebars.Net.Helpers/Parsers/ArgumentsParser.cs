using System.Collections.Generic;
using System.Reflection;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class ArgumentsParser
    {
        // Bug: Handlebars.Net does provide only strings
        public static object[] Parse(object[] arguments)
        {
            var list = new List<object>();
            foreach (var argument in arguments)
            {
                if (argument is string valueAsString)
                {
                    if (int.TryParse(valueAsString, out int valueAsInt))
                    {
                        list.Add(valueAsInt);
                    }
                    else if (long.TryParse(valueAsString, out long valueAsLong))
                    {
                        list.Add(valueAsLong);
                    }
                    else if (double.TryParse(valueAsString, out double valueAsDouble))
                    {
                        list.Add(valueAsDouble);
                    }
                    //else if (valueAsString.Length == 1)
                    //{
                    //    list.Add(valueAsString[0]);
                    //}
                    else
                    {
                        list.Add(valueAsString);
                    }
                }
                else if (argument.GetType().Name == "UndefinedBindingResult")
                {
                    list.Add(TryParseSpecialValue(argument, out var parsedValue) ? parsedValue : argument);
                }
                else
                {
                    list.Add(argument);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// In case it's an UndefinedBindingResult, just try to convert the value using Json
        /// This logic adds functionality like parsing an array
        /// </summary>
        /// <param name="undefinedBindingResult">The property value</param>
        /// <param name="parsedValue">The parsed value</param>
        /// <returns>true in case parsing is ok, else false</returns>
        private static bool TryParseSpecialValue(object undefinedBindingResult, out object parsedValue)
        {
            parsedValue = null;

            var fieldInfo = undefinedBindingResult.GetType().GetField("Value");
            if (fieldInfo == null)
            {
                return false;
            }

            string value = fieldInfo.GetValue(undefinedBindingResult).ToString();
            if (ArrayUtils.TryParse(value, out var parsedArray))
            {
                parsedValue = parsedArray;
                return true;
            }

            return false;
        }
    }
}
