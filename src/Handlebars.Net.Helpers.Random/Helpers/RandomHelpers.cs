using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Models;
using HandlebarsDotNet.Helpers.Parsers;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

internal class RandomHelpers : BaseHelpers, IHelpers
{
    public RandomHelpers(IHandlebars context) : base(context)
    {
    }

    /// <summary>
    /// For backwards compatibility with WireMock.Net
    /// </summary>
    [HandlebarsWriter(WriterType.Value, "Random")]
    public object? Random(IDictionary<string, object?> hash)
    {
        return Generate(hash);
    }

    /// <summary>
    /// For backwards compatibility with WireMock.Net
    /// </summary>
    [HandlebarsWriter(WriterType.Value, "RandomAsOutputWithType")]
    public string? RandomAsOutputWithType(IDictionary<string, object?> hash)
    {
        return GenerateAsOutputWithType(hash);
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Generate(IDictionary<string, object?> hash)
    {
        return GenerateInternal(hash, false);
    }

    [HandlebarsWriter(WriterType.String)]
    public string? GenerateAsOutputWithType(IDictionary<string, object?> hash)
    {
        return GenerateInternal(hash, true) is not OutputWithType outputWithType ? null : outputWithType.Serialize();
    }

    private object? GenerateInternal(IDictionary<string, object?> hash, bool outputWithType)
    {
        var fieldOptions = GetFieldOptionsFromHash(hash);
        dynamic randomizer = RandomizerFactory.GetRandomizerAsDynamic(fieldOptions);

        // Format DateTime as ISO 8601
        if (fieldOptions is IFieldOptionsDateTime)
        {
            DateTime? date = randomizer.Generate();
            return GetRandomValue(outputWithType, () => date?.ToString("s", Context.Configuration.FormatProvider));
        }

        // If the IFieldOptionsGuid defines Uppercase, use the 'GenerateAsString' method.
        if (fieldOptions is IFieldOptionsGuid fieldOptionsGuid)
        {
            return GetRandomValue(outputWithType, () => fieldOptionsGuid.Uppercase ? randomizer.GenerateAsString() : randomizer.Generate());
        }

        return GetRandomValue(outputWithType, () => randomizer.Generate());
    }

    private FieldOptionsAbstract GetFieldOptionsFromHash(IDictionary<string, object?> hash)
    {
        if (!hash.TryGetValue("Type", out var value) || value is not string randomType)
        {
            throw new HandlebarsException("The Type argument is missing.");
        }

        var newProperties = new Dictionary<string, object?>();
        foreach (var item in hash.Where(p => p.Key != "Type"))
        {
            var convertObjectArrayToStringList = randomType == "StringList";
            var parsedArgumentValue = ArgumentsParser.Parse(Context, item.Value, convertObjectArrayToStringList);

            newProperties.Add(item.Key, parsedArgumentValue);
        }

        return FieldOptionsFactory.GetFieldOptions(randomType, newProperties!);
    }

    private static object? GetRandomValue(bool outputWithType, Func<object?> func)
    {
        var value = func();

        return outputWithType ? new OutputWithType
        {
            Value = value,
            FullType = value?.GetType().FullName,
            Type = value?.GetType().Name
        } : value;
    }
}