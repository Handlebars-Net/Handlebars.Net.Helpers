using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers.Parsers
{
    internal static class ArgumentsParser
    {
        // Bug: Handlebars.Net does provide only strings
        public static object[] Parse(object[] arguments)
        {
            var list = new List<object>();
            foreach (var argument in arguments)
            {
                object parsedValue = null;
                if (argument is string valueAsString)
                {
                    if (int.TryParse(valueAsString, out int valueAsInt))
                    {
                        list.Add(valueAsInt);
                    }
                    else if (long.TryParse(valueAsString, out long valueAsLong))
                    {
                        list.Add(valueAsLong);
                    }
                    else if (double.TryParse(valueAsString, out double valueAsDouble))
                    {
                        list.Add(valueAsDouble);
                    }
                    else
                    {
                        list.Add(valueAsString);
                    }
                }
                else if (argument.GetType().Name == "UndefinedBindingResult" && TryParseSpecialValue(argument, out parsedValue))
                {
                    list.Add(parsedValue);
                }
                else
                {
                    list.Add(argument);
                }
            }

            return list.ToArray();
        }


        /// <summary>
        /// In case it's an UndefinedBindingResult, just try to convert the value using Json
        /// This logic adds functionality like parsing an array
        /// </summary>
        /// <param name="undefinedBindingResult">The property value</param>
        /// <param name="parsedValue">The parsed value</param>
        /// <returns>true in case parsing is ok, else false</returns>
        private static bool TryParseSpecialValue(object undefinedBindingResult, out object parsedValue)
        {
            parsedValue = null;

            var fieldInfo = undefinedBindingResult.GetType().GetField("Value");
            if (fieldInfo == null)
            {
                return false;
            }

            try
            {
                JToken jToken = JToken.Parse(fieldInfo.GetValue(undefinedBindingResult).ToString());
                switch (jToken)
                {
                    case JArray jTokenArray:
                        parsedValue = jTokenArray.ToObject<string[]>().Cast<object>().ToList();
                        break;

                    default:
                        return jToken.ToObject<dynamic>();
                }

                return true;
            }
            catch (JsonException)
            {
                // Ignore and don't add this value
            }

            return false;
        }
    }
}
