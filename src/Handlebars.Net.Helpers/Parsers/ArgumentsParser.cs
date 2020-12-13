using System.Collections.Generic;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class ArgumentsParser
    {
        public static List<object?> Parse(IHandlebars context, Arguments arguments)
        {
            var list = new List<object?>();
            foreach (var argument in arguments)
            {
                switch (argument)
                {
                    case string valueAsString:
                        list.Add(StringValueParser.Parse(context, valueAsString));
                        break;

                    case UndefinedBindingResult valueAsUndefinedBindingResult:
                        list.Add(TryParseSpecialValue(valueAsUndefinedBindingResult, out var parsedValue) ? parsedValue : argument);
                        break;

                    default:
                        list.Add(argument);
                        break;
                }
            }

            return list;
        }

        /// <summary>
        /// In case it's an UndefinedBindingResult, just try to convert the value using Json.
        /// This logic adds functionality like parsing an array.
        /// </summary>
        /// <param name="undefinedBindingResult">The property value</param>
        /// <param name="parsedValue">The parsed value</param>
        /// <returns>true in case parsing is ok, else false</returns>
        private static bool TryParseSpecialValue(UndefinedBindingResult undefinedBindingResult, out object? parsedValue)
        {
            parsedValue = null;

            if (ArrayUtils.TryParse(undefinedBindingResult.Value, out var parsedArray))
            {
                parsedValue = parsedArray;
                return true;
            }

            return false;
        }
    }
}