using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HandlebarsDotNet.Helpers.Json;

namespace HandlebarsDotNet.Helpers.Utils;

public static class ArrayUtils
{
    public static string ToArray(IEnumerable<object?> array)
    {
        return SimpleJson.SerializeObject(Fix(array))!;
    }

    public static bool TryParseAsObjectList(string value, [NotNullWhen(true)] out List<object?>? list)
    {
        if (SimpleJson.TryDeserializeObject(value, out var obj))
        {
            list = (List<object?>?)obj!;
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