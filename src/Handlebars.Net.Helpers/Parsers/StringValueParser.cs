using System.Globalization;

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class StringValueParser
    {
        public static object Parse(IHandlebars context, string valueAsString)
        {
            if (int.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out int valueAsInt))
            {
                return valueAsInt;
            }
            
            if (long.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out long valueAsLong))
            {
                return valueAsLong;
            }
            
            if (double.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out double valueAsDouble))
            {
                return valueAsDouble;
            }

            return valueAsString;
        }
    }
}