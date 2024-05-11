using System.Collections.Concurrent;
using System.Reflection;

namespace HandlebarsDotNet.Helpers.Utils;

internal static class DefaultValueCache
{
    private static readonly ConcurrentDictionary<Type, object?> Cache = new();

    public static object? GetDefaultValue(Type type)
    {
        // Try to get the value from the cache
        if (!Cache.TryGetValue(type, out var value))
        {
            // Value not found in cache, compute and add to cache
            value = CreateDefaultValue(type);
            Cache.TryAdd(type, value);
        }

        return value;
    }

    private static object? CreateDefaultValue(Type type)
    {
        // Uses reflection to create a generic method call
        return typeof(DefaultValueCreator<>)
            .MakeGenericType(type)
            .GetMethod("GetDefault")!
            .Invoke(null, null);
    }

    private static class DefaultValueCreator<T>
    {
        public static T? GetDefault() => default(T);
    }
}