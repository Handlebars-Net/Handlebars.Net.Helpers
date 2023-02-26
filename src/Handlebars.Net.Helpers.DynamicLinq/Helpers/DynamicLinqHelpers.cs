using System;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;
using Newtonsoft.Json.Linq;
using Stef.Validation;

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
        Guard.NotNull(value);

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
            throw new HandlebarsException(nameof(Linq), ex);
        }
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? FirstOrDefault(object value, string? linqStatement = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call FirstOrDefault to return first value.
        return CallWhere(value, linqStatement).FirstOrDefault();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object Where(object value, string linqStatement)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(linqStatement);

        // CallWhere(...) and call ToDynamicArray to return an array.
        return CallWhere(value, linqStatement).ToDynamicArray();
    }

    private static IQueryable CallWhere(object value, string? linqStatement = null, [CallerMemberName] string callerName = "")
    {
        if (value is not IEnumerable enumerable)
        {
            throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars DynamicLinq '{callerName}'.");
        }

        try
        {
            // Convert IEnumerable to IQueryable
            var queryable = enumerable.AsQueryable();

            // And call Where(...) if required.
            return !string.IsNullOrEmpty(linqStatement) ? queryable.Where(linqStatement) : queryable;
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(Where), ex);
        }
    }

    private static JToken ParseAsJToken(object value)
    {
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