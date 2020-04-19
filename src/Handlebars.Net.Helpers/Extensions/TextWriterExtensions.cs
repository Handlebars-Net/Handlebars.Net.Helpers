using System;
using System.IO;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Extensions
{
    /// <summary>
    /// https://github.com/rexm/Handlebars.Net/issues/344
    /// </summary>
    internal static class TextWriterExtensions
    {
        public static void Write(this TextWriter writer, object? value, HandlebarsHelpersOptions options)
        {
            writer.Write(Convert.ToString(value, options.CultureInfo));
        }

        public static void WriteSafeString(this TextWriter writer, object? value, HandlebarsHelpersOptions options)
        {
            writer.WriteSafeString(Convert.ToString(value, options.CultureInfo));
        }
    }
}