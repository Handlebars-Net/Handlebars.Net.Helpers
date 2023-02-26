using System;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

internal class DynamicLinqHelpers : BaseHelpers, IHelpers
{
    public DynamicLinqHelpers(IHandlebars context) : base(context)
    {
    }

    /// <summary>
    /// "Linq" = for backwards compatibility with WireMock.Net
    /// </summary>
    [HandlebarsWriter(WriterType.Value, "Linq")]
    public object Linq(object value, string linqStatement)
    {
        return FirstOrDefault(value, linqStatement);
    }

    [HandlebarsWriter(WriterType.Value)]
    public object FirstOrDefault(object value, string linqStatement)
    {
        var jToken = ParseAsJToken(value);

        try
        {
            // Convert a single object to a Queryable JObject-list with 1 entry.
            IQueryable queryable1 = new[] { jToken }.AsQueryable();

            // Generate the DynamicLinq select statement.
            string selector = JsonUtils.GenerateDynamicLinqStatement(jToken);

            // Execute DynamicLinq Select statement.
            var queryable2 = queryable1.Select(selector);

            // Execute the Select(...) method and call FirstOrDefault
            return queryable2.Select(linqStatement).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(FirstOrDefault), ex);
        }
    }

    [HandlebarsWriter(WriterType.Value)]
    public object Where(object value, string linqStatement)
    {
        if (value is not IEnumerable enumerable)
        {
            throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars DynamicLinq 'Where'.");
        }

        try
        {
            // Convert IEnumerable to IQueryable and execute Where(...) and call ToDynamicArray to return an array.
            return enumerable.AsQueryable().Where(linqStatement).ToDynamicArray();
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(Where), ex);
        }
    }

    private static JToken ParseAsJToken(object value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        switch (value)
        {
            case string valueAsString:
                return new JValue(valueAsString);

            case JToken valueAsJToken:
                return valueAsJToken;

            default:
                try
                {
                    return JToken.FromObject(value);
                }
                catch (Exception innerException)
                {
                    throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars Linq.", innerException);
                }
        }
    }
}