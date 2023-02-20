using System.Collections.Generic;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class DictionaryHelpers : BaseHelpers, IHelpers
{
    [HandlebarsWriter(WriterType.Value)]
    public object? Lookup(object data, object key, object? valueIfNotFound = null)
    {
        Guard.NotNull(data);
        Guard.NotNull(key);

        var keyAsString = key as string ?? key.ToString();

        if (data is IDictionary<string, object> realDictionary && realDictionary.TryGetValue(keyAsString, out var valueFromRealDictionary))
        {
            return valueFromRealDictionary;
        }

        if (ObjectUtils.TryParseAsDictionary(data, out var parsedDictionary) && parsedDictionary.TryGetValue(keyAsString, out var valueFromParsedDictionary))
        {
            return valueFromParsedDictionary;
        }

        return valueIfNotFound;
    }

    public DictionaryHelpers(IHandlebars context) : base(context)
    {
    }
}