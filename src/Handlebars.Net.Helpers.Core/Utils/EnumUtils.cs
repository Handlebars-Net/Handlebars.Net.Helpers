using System.Diagnostics.CodeAnalysis;

namespace HandlebarsDotNet.Helpers.Utils;

public static class EnumUtils
{
    public static bool TryParse<TEnum>(object? value, [NotNullWhen(true)] out TEnum? comparisonType) where TEnum : struct, Enum
    {
        switch (value)
        {
            case TEnum valueAsEnum:
                comparisonType = valueAsEnum;
                return true;

            case int valueAsInt when Enum.IsDefined(typeof(TEnum), valueAsInt):
                comparisonType = IntToEnum<TEnum>(valueAsInt);
                return true;

            case string stringValue when Enum.TryParse<TEnum>(stringValue, out var parsedAsEnum):
                comparisonType = parsedAsEnum;
                return true;

            case string stringValue when int.TryParse(stringValue, out var parsedAsInt) && Enum.IsDefined(typeof(TEnum), parsedAsInt):
                comparisonType = IntToEnum<TEnum>(parsedAsInt);
                return true;

            default:
                comparisonType = default;
                return false;
        }
    }

    public static TEnum IntToEnum<TEnum>(int value) where TEnum : struct, Enum
        => (TEnum)Enum.ToObject(typeof(TEnum), value);
}