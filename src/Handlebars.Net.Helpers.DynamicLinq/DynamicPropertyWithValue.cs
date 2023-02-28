using System.Linq.Dynamic.Core;

namespace HandlebarsDotNet.Helpers;

public class DynamicPropertyWithValue : DynamicProperty
{
    public object? Value { get; }

    public DynamicPropertyWithValue(string name, object? value) : base(name, value?.GetType() ?? typeof(object))
    {
        Value = value;
    }
}