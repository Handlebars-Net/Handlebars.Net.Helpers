using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers.Extensions;

/// <summary>
/// Provides an easy extension method to convert JObjects to nested Dictionary&lt;string, object&gt; instances
/// </summary>
public static class JObjectExtensions
{
    private static readonly JTokenResolvers Resolvers = new()
    {
        { JTokenType.None, _ => null },
        { JTokenType.Array, ConvertJTokenArray },
        { JTokenType.Property, ConvertJTokenProperty },
        { JTokenType.Integer, o => o.Value<int>() },
        { JTokenType.String, o => o.Value<string>() },
        { JTokenType.Boolean, o => o.Value<bool>() },
        { JTokenType.Null, _ => null },
        { JTokenType.Undefined, _ => null },
        { JTokenType.Date, o => o.Value<DateTime>() },
        { JTokenType.Bytes, o => o.Value<byte[]>() },
        { JTokenType.Guid, o => o.Value<Guid>() },
        { JTokenType.Uri, o => o.Value<Uri>() },
        { JTokenType.TimeSpan, o => o.Value<TimeSpan>() },
        { JTokenType.Object, TryConvertObject }
    };

    /// <summary>
    /// Generates a nested dictionary of Dictionary&lt;string, object&gt; from a JObject so you can use JObjects easily with DuckTyping.
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    public static Dictionary<string, object?> ToDictionary(this JObject? src)
    {
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        if (src == null)
        {
            return result;
        }

        foreach (var prop in src.Properties())
        {
            result[prop.Name] = Resolvers[prop.Type](prop.Value);
        }

        return result;
    }

    public static IEnumerable ToX(this JArray? src)
    {
        if (src == null)
        {
            return new object?[0];
        }

        return ConvertJTokenArray(src);
    }

    private static object? TryConvertObject(JToken arg)
    {
        if (arg is JObject asJObject)
        {
            return asJObject.ToDictionary();
        }

        return GetResolverFor(arg)(arg);
    }

    private static object PassThrough(JToken arg)
    {
        return arg;
    }

    private static Func<JToken, object?> GetResolverFor(JToken arg)
    {
        return Resolvers.TryGetValue(arg.Type, out var result) ? result : PassThrough;
    }

    private static object? ConvertJTokenProperty(JToken arg)
    {
        var resolver = GetResolverFor(arg);
        if (resolver is null)
        {
            throw new InvalidOperationException($"Unable to handle JToken of type: {arg.Type}");
        }

        return resolver(arg);
    }

    private static IEnumerable ConvertJTokenArray(JToken arg)
    {
        if (arg is not JArray array)
        {
            throw new NotImplementedException();
        }

        var result = new List<object?>();
        foreach (var item in array)
        {
            result.Add(TryConvertObject(item));
        }

        var distinctType = FindSameTypeOf(result);
        return distinctType == null ? result.ToArray() : ConvertToTypedArray(result, distinctType);
    }

    private static Type? FindSameTypeOf(IEnumerable<object?> src)
    {
        var types = src.Select(o => o?.GetType()).Distinct().OfType<Type>().ToArray();
        return types.Length == 1 ? types[0] : null;
    }

    private static IEnumerable ConvertToTypedArray(IEnumerable<object?> src, Type newType)
    {
        var method = ConvertToTypedArrayGenericMethod.MakeGenericMethod(newType);
        return (IEnumerable)method.Invoke(null, new object[] { src });
    }

    private static readonly MethodInfo ConvertToTypedArrayGenericMethod = typeof(JObjectExtensions).GetMethod(nameof(ConvertToTypedArrayGeneric), BindingFlags.NonPublic | BindingFlags.Static)!;

    private static T[] ConvertToTypedArrayGeneric<T>(IEnumerable<object> src)
    {
        return src.Cast<T>().ToArray();
    }

    private class JTokenResolvers : Dictionary<JTokenType, Func<JToken, object?>>
    {
    }
}