using System.Globalization;

namespace HandlebarsDotNet.Helpers.Parsers;

public static class StringValueParser
{
    public static object Parse(IHandlebars context, string valueAsString)
    {
        if (int.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out var valueAsInt))
        {
            return valueAsInt;
        }
            
        if (long.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out var valueAsLong))
        {
            return valueAsLong;
        }
            
        if (double.TryParse(valueAsString, NumberStyles.Any, context.Configuration.FormatProvider, out var valueAsDouble))
        {
            return valueAsDouble;
        }

        return valueAsString;
    }
}