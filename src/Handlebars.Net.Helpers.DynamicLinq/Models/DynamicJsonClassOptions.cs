// Copied from https://github.com/StefH/JsonConverter

namespace HandlebarsDotNet.Helpers.Models;

internal class DynamicJsonClassOptions
{
    public IntegerBehavior IntegerConvertBehavior { get; set; } = IntegerBehavior.UseLong;

    public FloatBehavior FloatConvertBehavior { get; set; } = FloatBehavior.UseDouble;
}