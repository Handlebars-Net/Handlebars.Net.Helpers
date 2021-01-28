using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class ArgumentsParser
    {
        public static List<object?> Parse(IHandlebars context, IEnumerable<object?> arguments)
        {
            return arguments.Select(argument => Parse(context, argument)).ToList();
        }

        public static object? Parse(IHandlebars context, object? argument)
        {
            switch (argument)
            {
                case string valueAsString:
                    return StringValueParser.Parse(context, valueAsString);

                case UndefinedBindingResult valueAsUndefinedBindingResult:
                    return TryParseSpecialValue(valueAsUndefinedBindingResult, out var parsedValue) ? parsedValue : argument;

                case IDictionary<string, object?> hash:
                    return hash;

                default:
                    return argument;
            }
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