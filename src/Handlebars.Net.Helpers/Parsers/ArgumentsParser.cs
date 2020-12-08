using System.Collections.Generic;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Utils;
#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class ArgumentsParser
    {
        public static List<object?> Parse(HandlebarsHelpersOptions options, Arguments arguments)
        {
            var list = new List<object?>();
            foreach (var argument in arguments)
            {
                switch (argument)
                {
                    case string valueAsString:
                        list.Add(StringValueParser.Parse(options, valueAsString));
                        break;

                    case { } when argument.GetType().Name == "UndefinedBindingResult":
                        list.Add(TryParseSpecialValue(argument, out var parsedValue) ? parsedValue : argument);
                        break;

                    default:
                        list.Add(argument);
                        break;
                }
            }

            return list;
        }

        /// <summary>
        /// In case it's an UndefinedBindingResult, just try to convert the value using Json
        /// This logic adds functionality like parsing an array
        /// </summary>
        /// <param name="undefinedBindingResult">The property value</param>
        /// <param name="parsedValue">The parsed value</param>
        /// <returns>true in case parsing is ok, else false</returns>
        private static bool TryParseSpecialValue(object undefinedBindingResult, out object? parsedValue)
        {
            parsedValue = null;

            var fieldInfo = undefinedBindingResult.GetType().GetField("Value");
            if (fieldInfo is null)
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
