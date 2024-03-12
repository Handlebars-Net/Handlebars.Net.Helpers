using HandlebarsDotNet.Helpers.Json;

namespace HandlebarsDotNet.Helpers.Utils;

public static class SimpleJsonUtils
{
    public static string? SerializeObject(object? value)
    {
        return SimpleJson.SerializeObject(value);
    }

    public static T? DeserializeObject<T>(string? json)
    {
        return SimpleJson.DeserializeObject<T>(json);
    }
}