using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers.Utils
{
    internal static class ArrayUtils
    {
        public static string ToArray(IEnumerable<object?> array)
        {
            return new JArray(Fix(array)).ToString(Formatting.None);
        }

        public static bool TryParse(string value, out IEnumerable<object>? parsedArray)
        {
            parsedArray = null;
            try
            {
                var jToken = JToken.Parse(value);
                if (jToken is JArray jTokenArray)
                {
                    parsedArray = jTokenArray.ToObject<IEnumerable<object>>();
                    return true;
                }
            }
            catch (JsonException)
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