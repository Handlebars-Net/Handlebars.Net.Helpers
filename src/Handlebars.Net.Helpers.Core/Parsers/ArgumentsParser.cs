using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Parsers
{
    public static class ArgumentsParser
    {
        public static List<object?> Parse(IHandlebars context, IEnumerable<object?> arguments)
        {
            return arguments.Select(argument => Parse(context, argument)).ToList();
        }

        public static object? Parse(IHandlebars context, object? argument, bool convertObjectArrayToStringList = false)
        {
            switch (argument)
            {
                case string valueAsString:
                    return StringValueParser.Parse(context, valueAsString);

                case UndefinedBindingResult valueAsUndefinedBindingResult:
                    if (convertObjectArrayToStringList)
                    {
                        if (TryParseAsArray(valueAsUndefinedBindingResult, out List<string>? parsedAsStringList))
                        {
                            return parsedAsStringList;
                        }

                        return argument;
                    }

                    if (TryParseAsArray(valueAsUndefinedBindingResult, out object?[]? parsedAsObjectArray))
                    {
                        return parsedAsObjectArray;
                    }

                    return argument;

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
        private static bool TryParseAsArray<T>(UndefinedBindingResult undefinedBindingResult, out T? parsedValue) where T : class
        {
            return ArrayUtils.TryParse(undefinedBindingResult.Value, out parsedValue);
        }
    }
}