using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.TinyJson;

namespace HandlebarsDotNet.Helpers.Utils
{
    internal static class ArrayUtils
    {
        public static string ToArray(IEnumerable<object?> array)
        {
            return Fix(array).ToJson();
        }

        public static bool TryParse(string value, out object?[]? parsedArray)
        {
            parsedArray = null;
            try
            {
                parsedArray = value.FromJson<object?[]>();
                return true;
            }
            catch (Exception)
            {
                // Ignore and return false
            }

            return false;
        }

        private static IEnumerable<object?> Fix(IEnumerable<object?> array)
        {
            return array.Select(a => a is char ? a.ToString() : a);
        }
    }
}