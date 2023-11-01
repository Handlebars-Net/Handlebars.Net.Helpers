using System;
using System.Diagnostics.CodeAnalysis;
using HandlebarsDotNet.Helpers.Json;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Models;

public class OutputWithType
{
    public object? Value { get; set; }

    public string? TypeName { get; set; }

    public string? FullTypeName { get; set; }

    public override string ToString()
    {
        return SimpleJsonUtils.SerializeObject(this) ?? string.Empty;
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

        if (!jsonObject.TryGetValue(nameof(Value), out var value))
        {
            throw new MissingMemberException($"Property '{nameof(Value)}' is not found on type '{nameof(OutputWithType)}'.");
        }

        if (!jsonObject.TryGetValue(nameof(TypeName), out var typeName) || typeName is not string typeNameAsString)
        {
            throw new MissingMemberException($"Property '{nameof(TypeName)}' is not found on type '{nameof(OutputWithType)}'.");
        }

        if (!jsonObject.TryGetValue(nameof(FullTypeName), out var fullTypeName) || fullTypeName is not string fullTypeNameAsString)
        {
            throw new MissingMemberException($"Property '{nameof(FullTypeName)}' is not found on type '{nameof(OutputWithType)}'.");
        }

        var fullType = Type.GetType(fullTypeNameAsString);
        if (fullType == null)
        {
            throw new TypeLoadException($"Type '{fullTypeNameAsString}' is not found.");
        }

        if (TryConvert(value, fullType, out var convertedValue))
        {
            value = convertedValue;
        }

        return new OutputWithType
        {
            Value = value,
            TypeName = typeNameAsString,
            FullTypeName = fullTypeNameAsString
        };
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
            else if (fullType == typeof(TimeSpan) && value is JsonObject timeSpanAsJsonObject)
            {
                result = TimeSpan.FromTicks((long)timeSpanAsJsonObject["Ticks"]);
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