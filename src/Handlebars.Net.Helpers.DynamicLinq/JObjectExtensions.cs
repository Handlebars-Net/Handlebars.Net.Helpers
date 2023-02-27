using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers;

/// <summary>
/// Based on https://github.com/fluffynuts/PeanutButter/blob/master/source/Utils/PeanutButter.JObjectExtensions/JObjectExtensions.cs
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

    public static object? ToDynamicClass(this JValue? src)
    {
        if (src == null)
        {
            return null;
        }

        return src.Value;
    }

    public static DynamicClass? ToDynamicClass(this JObject? src)
    {
        if (src == null)
        {
            return null;
        }

        var dynamicPropertyWithValues = new List<DynamicPropertyWithValue>();

        foreach (var prop in src.Properties())
        {
            var value = Resolvers[prop.Type](prop.Value);
            if (value != null)
            {
                var dp = new DynamicPropertyWithValue(prop.Name, value);
                dynamicPropertyWithValues.Add(dp);
            }
        }

        return CreateInstance(dynamicPropertyWithValues);
    }

    public static IEnumerable ToDynamicClassArray(this JArray? src)
    {
        if (src == null)
        {
            return new object?[0];
        }

        return ConvertJTokenArray(src);
    }

    public static object? TryConvertObject(JToken arg)
    {
        if (arg is JObject asJObject)
        {
            return asJObject.ToDynamicClass();
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

    public static DynamicClass CreateInstance(IList<DynamicPropertyWithValue> dynamicPropertiesWithValue, bool createParameterCtor = true)
    {
        var type = DynamicClassFactory.CreateType(dynamicPropertiesWithValue.Cast<DynamicProperty>().ToArray(), createParameterCtor);
        var dynamicClass = (DynamicClass)Activator.CreateInstance(type);
        foreach (var dynamicPropertyWithValue in dynamicPropertiesWithValue.Where(p => p.Value != null))
        {
            dynamicClass.SetDynamicPropertyValue(dynamicPropertyWithValue.Name, dynamicPropertyWithValue.Value!);
        }

        return dynamicClass;
    }

    private class JTokenResolvers : Dictionary<JTokenType, Func<JToken, object?>>
    {
    }
}