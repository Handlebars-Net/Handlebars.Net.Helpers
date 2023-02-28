using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

internal class JsonPathHelpers : BaseHelpers, IHelpers
{
    public JsonPathHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.String)]
    public string SelectToken(object value, string jsonPath)
    {
        var valueAsJToken = ParseAsJToken(value, nameof(SelectToken));

        try
        {
            var result = valueAsJToken.SelectToken(jsonPath);
            return result switch
            {
                { } jTokenResult => jTokenResult.ToString(),
                _ => result!.ToString() // In case result is null, this will throw.
            };
        }
        catch (JsonException ex)
        {
            throw new HandlebarsException(nameof(SelectToken), ex);
        }
    }

    [HandlebarsWriter(WriterType.Value)]
    public object SelectTokens(object value, string jsonPath)
    {
        return SelectTokensInternal(value, jsonPath);
    }

    [HandlebarsWriter(WriterType.Value, usage: HelperUsage.Block)]
    public object SelectTokens(bool _, object value, string jsonPath)
    {
        return SelectTokensInternal(value, jsonPath);
    }

    private object SelectTokensInternal(object value, string jsonPath)
    {
        var valueAsJToken = ParseAsJToken(value, nameof(SelectTokens));

        try
        {
            var jTokens = valueAsJToken?.SelectTokens(jsonPath) ?? new List<JToken>();
            return jTokens.ToDictionary(jToken => jToken.Path, jToken => jToken);
        }
        catch (JsonException ex)
        {
            throw new HandlebarsException(nameof(SelectTokensInternal), ex);
        }
    }

    private static JToken ParseAsJToken(object? value, string methodName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value switch
        {
            JToken tokenValue => tokenValue,
            string stringValue => JsonUtils.Parse(stringValue),
            IEnumerable enumerableValue => JArray.FromObject(enumerableValue),
            _ => throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars JsonPath {methodName}.")
        };
    }
}