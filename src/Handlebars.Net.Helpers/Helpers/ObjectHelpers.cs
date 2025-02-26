using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using System;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class ObjectHelpers : BaseHelpers, IHelpers
{
    public ObjectHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? FormatAsObject(object? value)
    {
        return value;
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToString(object? value)
    {
        return value?.ToString() ?? string.Empty;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.IsNull")]
    public bool IsNull(object value)
    {
        return value is null;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.IsNotNull")]
    public bool IsNotNull(object value)
    {
        return value is not null;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.Equal")]
    public bool Equal(object value1, object value2)
    {
        if (value1 is null && value2 is null)
        {
            return true;
        } 
        
        if (value1 is null ^ value2 is null)
        {
            return false;
        }
        
        return value1!.Equals(value2);
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.NotEqual")]
    public bool NotEqual(object value1, object value2)
    {
        if (value1 is null && value2 is null)
        {
            return false;
        }
        
        if (value1 is null ^ value2 is null)
        {
            return true;
        }

        return !value1!.Equals(value2);        
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.GreaterThan")]
    public bool GreaterThan(object value1, object value2)
    {
       return CompareTo(value1, value2) > 0;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.GreaterThanEqual")]
    public bool GreaterThanEqual(object value1, object value2)
    {
        return CompareTo(value1, value2) >= 0;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.LowerThan")]
    public bool LowerThan(object value1, object value2)
    {
        return CompareTo(value1, value2) < 0;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.LowerThanEqual")]
    public bool LowerThanEqual(object value1, object value2)
    {
        return CompareTo(value1, value2) <= 0;
    }

    [HandlebarsWriter(WriterType.Value, Name = "Object.CompareTo")]
    public int? CompareTo(object value1, object value2)
    {
        if (value1 is null || value2 is null)
        {
            return null;
        }

        if (value1 is not IComparable comparable1 || value2 is not IComparable comparable2)
        {
            throw new ArgumentException("Both values should implement IComparable.");
        }

        return comparable1.CompareTo(comparable2);
    }

    public Category Category => Category.Object;
}