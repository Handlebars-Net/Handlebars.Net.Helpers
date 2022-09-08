using System.Collections.Generic;
using System.Linq;
using JsonConverter.SimpleJson;

namespace HandlebarsDotNet.Helpers.Utils
{
    public static class ArrayUtils
    {
        public static string ToArray(IEnumerable<object?> array)
        {
            return SimpleJson.SerializeObject(Fix(array))!;
        }

        public static bool TryParseAsObjectList(string value, out List<object?>? list)
        {
            if (SimpleJson.TryDeserializeObject(value, out object? obj))
            {
                list = (List<object?>?)obj;
                return true;
            }

            list = null;
            return false;
        }

        private static IEnumerable<object?> Fix(IEnumerable<object?> array)
        {
            return array.Select(a => a is char ? a.ToString() : a);
        }
    }
}