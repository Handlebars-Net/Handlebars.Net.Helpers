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
                case UndefinedBindingResult valueAsUndefinedBindingResult:
                    if (TryParseUndefinedBindingResult(valueAsUndefinedBindingResult, out List<object?>? parsedAsObjectList))
                    {
                        if (convertObjectArrayToStringList)
                        {
                            return parsedAsObjectList.Cast<string?>().ToList();
                        }

                        return parsedAsObjectList;
                    }

                    return argument;

                default:
                    return argument;
            }
        }

        /// <summary>
        /// In case it's an UndefinedBindingResult, just try to convert the value using Json.
        /// This logic adds functionality like parsing a list.
        /// </summary>
        /// <param name="undefinedBindingResult">The property value</param>
        /// <param name="parsedValue">The parsed value</param>
        /// <returns>true in case parsing is ok, else false</returns>
        private static bool TryParseUndefinedBindingResult(UndefinedBindingResult undefinedBindingResult, out List<object?>? parsedValue)
        {
            return ArrayUtils.TryParseAsObjectList(undefinedBindingResult.Value, out parsedValue);
        }
    }
}