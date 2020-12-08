using System;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Extensions
{
    /// <summary>
    /// https://github.com/rexm/Handlebars.Net/issues/344
    /// </summary>
    internal static class EncodedTextWriterExtensions
    {
        public static void Write(this in EncodedTextWriter writer, object? value, HandlebarsHelpersOptions options)
        {
            writer.Write(Convert.ToString(value, options.CultureInfo));
        }

        public static void WriteSafeString(this in EncodedTextWriter writer, object? value, HandlebarsHelpersOptions options)
        {
            if (value is string strValue)
            {
                writer.WriteSafeString(strValue);
                return;
            }

            writer.WriteSafeString(Convert.ToString(value, options.CultureInfo));
        }
    }
}