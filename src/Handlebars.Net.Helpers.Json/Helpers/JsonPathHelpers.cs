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

namespace HandlebarsDotNet.Helpers
{
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
                switch (result)
                {
                    case JToken jTokenResult:
                        return jTokenResult.ToString();

                    default:
                        return result.ToString();
                }
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
                var jTokens = valueAsJToken.SelectTokens(jsonPath) ?? new List<JToken>();
                return jTokens.ToDictionary(value => value.Path, value => value);
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

            switch (value)
            {
                case JToken tokenValue:
                    return tokenValue;

                case string stringValue:
                    return JsonUtils.Parse(stringValue);

                case IEnumerable enumerableValue:
                    return JArray.FromObject(enumerableValue);

                default:
                    throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars JsonPath {methodName}.");
            }
        }
    }
}