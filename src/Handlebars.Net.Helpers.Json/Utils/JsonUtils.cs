using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers.Utils;

public static class JsonUtils
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
    {
        DateParseHandling = DateParseHandling.None
    };

    /// <summary>
    /// Load a Newtonsoft.Json.Linq.JObject from a string that contains JSON.
    /// Using : DateParseHandling = DateParseHandling.None
    /// </summary>
    /// <param name="json">A System.String that contains JSON.</param>
    /// <returns>A Newtonsoft.Json.Linq.JToken populated from the string that contains JSON.</returns>
    public static JToken Parse(string json)
    {
        return JsonConvert.DeserializeObject<JToken>(json, JsonSerializerSettings)!;
    }
}