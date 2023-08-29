using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Models;

public class OutputWithType
{
    public object? Value { get; set; }

    public string? Type { get; set; }

    public string? FullType { get; set; }

    public override string ToString()
    {
        return SimpleJsonUtils.SerializeObject(this) ?? string.Empty;
    }

    public string? Serialize()
    {
        return SimpleJsonUtils.SerializeObject(this);
    }

    public static OutputWithType? Deserialize(string? json)
    {
        return SimpleJsonUtils.DeserializeObject<OutputWithType>(json);
    }
}