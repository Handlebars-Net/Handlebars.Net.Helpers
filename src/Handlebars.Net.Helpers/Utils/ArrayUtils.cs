using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers.Utils
{
    internal static class ArrayUtils
    {
        public static string ToArray(IEnumerable<object> array)
        {
            return new JArray(array).ToString(Formatting.None);
        }

        public static bool TryParse(string value, out IEnumerable<object> parsedArray)
        {
            parsedArray = null;
            try
            {
                JToken jToken = JToken.Parse(value);
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
    }
}