using System.Globalization;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class StringValueParser
    {
        public static object Parse(HandlebarsHelpersOptions options, string valueAsString)
        {
            if (int.TryParse(valueAsString, NumberStyles.Any, options.CultureInfo, out int valueAsInt))
            {
                return valueAsInt;
            }
            
            if (long.TryParse(valueAsString, NumberStyles.Any, options.CultureInfo, out long valueAsLong))
            {
                return valueAsLong;
            }
            
            if (double.TryParse(valueAsString, NumberStyles.Any, options.CultureInfo, out double valueAsDouble))
            {
                return valueAsDouble;
            }

            return valueAsString;
        }
    }
}