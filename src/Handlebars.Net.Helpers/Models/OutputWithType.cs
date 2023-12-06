﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HandlebarsDotNet.Helpers.Json;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Models;

public class OutputWithType
{
    public object? Value { get; set; }

    public string? TypeName { get; set; }

    public string? FullTypeName { get; set; }

    public static OutputWithType Parse(object? value)
    {
        var typeName = value?.GetType().Name;
        var fullTypeName = value?.GetType().FullName;

        if (value is IEnumerable<object> array)
        {
            value = ArrayUtils.ToArray(array);
        }

        return new OutputWithType
        {
            Value = value,
            FullTypeName = fullTypeName,
            TypeName = typeName
        };
    }

    public override string ToString()
    {
        return Serialize() ?? string.Empty;
    }

    public string? Serialize()
    {
        return SimpleJsonUtils.SerializeObject(this);
    }

    public static OutputWithType Deserialize(string? json)
    {
        var jsonObject = SimpleJsonUtils.DeserializeObject<JsonObject>(json);

        if (jsonObject == null)
        {
            throw new InvalidOperationException();
        }

        if (!TryGetValue(jsonObject, out var value))
        {
            throw new MissingMemberException($"Property '{nameof(Value)}' is not found on type '{nameof(OutputWithType)}'.");
        }

        if (!TryGetTypeName(jsonObject, out var typeName))
        {
            throw new MissingMemberException($"Property '{nameof(TypeName)}' is not found on type '{nameof(OutputWithType)}'.");
        }

        if (!TryGetFullTypeName(jsonObject, out var fullTypeName))
        {
            throw new MissingMemberException($"Property '{nameof(FullTypeName)}' is not found on type '{nameof(OutputWithType)}'.");
        }

        if (!TryGetType(fullTypeName, out var fullType))
        {
            throw new TypeLoadException($"Unable to load Type with FullTypeName '{fullTypeName}'.");
        }

        return new OutputWithType
        {
            Value = TryConvert(value, fullType, out var convertedValue) ? convertedValue : value,
            TypeName = typeName,
            FullTypeName = fullTypeName
        };
    }

    public static bool TryDeserialize(string? json, [NotNullWhen(true)] out OutputWithType? outputWithType)
    {
        JsonObject? jsonObject;
        try
        {
            jsonObject = SimpleJsonUtils.DeserializeObject<JsonObject>(json);
        }
        catch
        {
            outputWithType = null;
            return false;
        }

        if (jsonObject != null &&
            TryGetValue(jsonObject, out var value) &&
            TryGetTypeName(jsonObject, out var typeName) &&
            TryGetFullTypeName(jsonObject, out var fullTypeName) &&
            TryGetType(fullTypeName, out var fullType)
        )
        {
            outputWithType = new OutputWithType
            {
                Value = TryConvert(value, fullType, out var convertedValue) ? convertedValue : value,
                TypeName = typeName,
                FullTypeName = fullTypeName
            };
            return true;
        }

        outputWithType = default;
        return false;
    }

    private static bool TryGetValue(JsonObject jsonObject, out object value)
    {
        return jsonObject.TryGetValue(nameof(Value), out value);
    }

    private static bool TryGetTypeName(JsonObject jsonObject, [NotNullWhen(true)] out string? value)
    {
        if (jsonObject.TryGetValue(nameof(TypeName), out var typeName) && typeName is string typeNameAsString)
        {
            value = typeNameAsString;
            return true;
        }

        value = default;
        return false;
    }

    private static bool TryGetFullTypeName(JsonObject jsonObject, [NotNullWhen(true)] out string? value)
    {
        if (jsonObject.TryGetValue(nameof(FullTypeName), out var fullTypeName) && fullTypeName is string fullTypeNameAsString)
        {
            value = fullTypeNameAsString;
            return true;
        }

        value = default;
        return false;
    }

    private static bool TryGetType(string fullTypeName, [NotNullWhen(true)] out Type? type)
    {
        type = Type.GetType(fullTypeName) ?? Type.GetType($"{fullTypeName}, System");
        return type != null;
    }

    private static bool TryConvert(object? value, Type fullType, [NotNullWhen(true)] out object? result)
    {
        try
        {
            if (fullType == typeof(Guid) && value is string guidAsString)
            {
                result = new Guid(guidAsString);
                return true;
            }

            if (fullType == typeof(Uri) && value is string uriAsString)
            {
                result = new Uri(uriAsString);
                return true;
            }

            if (fullType == typeof(TimeSpan) && value is JsonObject timeSpanAsJsonObject)
            {
                result = TimeSpan.FromTicks((long)timeSpanAsJsonObject["Ticks"]);
                return true;
            }

            if (fullType.IsArray && value is JsonArray jsonArray)
            {
                var elementType = fullType.GetElementType()!;
                var newArray = Array.CreateInstance(elementType, jsonArray.Count);
                for (var i = 0; i < jsonArray.Count; i++)
                {
                    newArray.SetValue(Convert.ChangeType(jsonArray[i], elementType), i);
                }

                result = newArray;
            }
            else
            {
                result = Convert.ChangeType(value, fullType);
            }

            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}