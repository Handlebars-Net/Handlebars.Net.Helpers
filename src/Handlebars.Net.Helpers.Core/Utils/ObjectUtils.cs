using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HandlebarsDotNet.Helpers.Json;

namespace HandlebarsDotNet.Helpers.Utils;

public static class ObjectUtils
{
    public static bool TryParseAsDictionary(object? data, [NotNullWhen(true)] out IDictionary<string, object?>? dictionary)
    {
        try
        {
            dictionary = SimpleJson.DeserializeObject<IDictionary<string, object?>>(data?.ToString());
            return dictionary != null;
        }
        catch
        {
            dictionary = default;
            return false;
        }
    }
}