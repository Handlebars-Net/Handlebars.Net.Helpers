using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers
{
    internal class JsonPathHelpers : BaseHelpers, IHelpers
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None
        };

        public JsonPathHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.Value)]
        public object SelectToken(object value, string jsonPath)
        {
            var valueAsJToken = ParseAsJToken(value, nameof(SelectToken));

            try
            {
                return valueAsJToken.SelectToken(jsonPath);
            }
            catch (JsonException ex)
            {
                return ex;
            }
        }

        [HandlebarsWriter(WriterType.Value)]
        public object SelectTokens(object value, string jsonPath)
        {
            var valueAsJToken = ParseAsJToken(value, nameof(SelectTokens));

            try
            {
                var jTokens = valueAsJToken.SelectTokens(jsonPath) ?? new List<JToken>();
                return jTokens.ToDictionary(value => value.Path, value => value);
            }
            catch (JsonException ex)
            {
                return ex;
            }
        }

        private static JToken ParseAsJToken(object value, string methodName)
        {
            switch (value)
            {
                case JToken tokenValue:
                    return tokenValue;

                case string stringValue:
                    return Parse(stringValue);

                case IEnumerable enumerableValue:
                    return JArray.FromObject(enumerableValue);

                default:
                    throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars JsonPath {methodName}.");
            }
        }

        private static JToken Parse(string json)
        {
            return JsonConvert.DeserializeObject<JToken>(json, JsonSerializerSettings);
        }
    }
}