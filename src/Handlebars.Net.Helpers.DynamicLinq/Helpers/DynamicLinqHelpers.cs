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
    //private readonly IDateTimeService _dateTimeService;
    //private readonly ParsingConfig _parsingConfig;

    //public DynamicLinqHelpers(IHandlebars context, IDateTimeService dateTimeService) : base(context)
    //{
    //    _dateTimeService = Guard.NotNull(dateTimeService);

    //    _parsingConfig = new ParsingConfig();
    //}

    public DynamicLinqHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Expression(object arg0, string? arg1 = null)
    {
        object value;
        string? selector;
        if (arg1 is null)
        {
            value = true;
            selector = arg0 as string;
        }
        else
        {
            value = arg0;
            selector = arg1;
        }

        Guard.NotNullOrEmpty(selector);

        try
        {
            JArray jArray;
            try
            {
                jArray = new JArray { value };
            }
            catch
            {
                jArray = new JArray { JToken.FromObject(value) };
            }

            return CallWhere(jArray).Select(selector!).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new HandlebarsException(nameof(Expression), ex);
        }
    }

    /// <summary>
    /// "Linq" = for backwards compatibility with WireMock.Net
    /// </summary>
    [HandlebarsWriter(WriterType.Value, "Linq")]
    public object? Linq(object value, string selector)
    {
        return Expression(value, selector);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool All(object value, string linqPredicate)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(linqPredicate);

        // CallWhere(...) and call All.
        return CallWhere(value).All(linqPredicate);
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Any(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Any.
        return CallWhere(value, linqPredicate).Any();
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool NotAny(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call !Any.
        return !Any(value, linqPredicate);
    }

    [HandlebarsWriter(WriterType.Value)]
    public double Average(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Average.
        return CallWhere(value, linqPredicate).Average();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Cast(object value, string typeName)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(typeName);

        // CallWhere(...) and call Cast.
        return CallWhere(value).Cast(typeName).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public int Count(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Count.
        return CallWhere(value, linqPredicate).Count();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable DefaultIfEmpty(object value, string? linqPredicate = null, object? defaultValue = null)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(linqPredicate);

        // CallWhere(...) and call DefaultIfEmpty.
        var queryable = CallWhere(value, linqPredicate);
        return defaultValue is null ?
            queryable.DefaultIfEmpty().ToDynamicArray() :
            queryable.DefaultIfEmpty(defaultValue).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Distinct(object value, string linqPredicate)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Distinct.
        return CallWhere(value, linqPredicate).Distinct().ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value, "DynamicLinq.Max")]
    public object? Max(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Max.
        return CallWhere(value, linqPredicate).Max();
    }

    [HandlebarsWriter(WriterType.Value, "DynamicLinq.Min")]
    public object? Min(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Min.
        return CallWhere(value, linqPredicate).Min();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? First(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call First.
        return CallWhere(value, linqPredicate).First();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? FirstOrDefault(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call FirstOrDefault.
        return CallWhere(value, linqPredicate).FirstOrDefault();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? Last(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Last.
        return CallWhere(value, linqPredicate).Last();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? LastOrDefault(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call LastOrDefault.
        return CallWhere(value, linqPredicate).LastOrDefault();
    }

    [HandlebarsWriter(WriterType.Value)]
    public long LongCount(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call LongCount.
        return CallWhere(value, linqPredicate).LongCount();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable OfType(object value, string typeName)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(typeName);

        // CallWhere(...) and call OfType.
        return CallWhere(value).OfType(typeName).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable OrderBy(object value, string ordering)
    {
        return OrderByThenBy(value, ordering);
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable OrderByThenBy(object value, string ordering, object? thenBy = null)
    {
        Guard.NotNull(value);
        Guard.NotNull(ordering);

        // CallWhere(...) and call OrderBy.
        var ordered = CallWhere(value).OrderBy(ordering);

        switch (thenBy)
        {
            case null:
                return ordered.ToDynamicArray();

            case string thenByAsString:
                return ordered.ThenBy(thenByAsString).ToDynamicArray();

            case IEnumerable thenByAsEnumerable:
                foreach (var item in thenByAsEnumerable.OfType<string>())
                {
                    ordered = ordered.ThenBy(item);
                }
                return ordered.ToDynamicArray();

            default:
                throw new NotSupportedException($"The value for {nameof(thenBy)} cannot be parsed as a string or an IEnumerable<string> in Handlebars DynamicLinq '{nameof(OrderByThenBy)}'.");
        }
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Page(object value, int page, int pageSize)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Page.
        return CallWhere(value).Page(page, pageSize).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Page(object value, string linqPredicate, int page, int pageSize)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Page.
        return CallWhere(value, linqPredicate).Page(page, pageSize).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public PagedResult PageResult(object value, int page, int pageSize, int? rowCount = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call PageResult.
        return CallWhere(value).PageResult(page, pageSize, rowCount);
    }

    [HandlebarsWriter(WriterType.Value)]
    public PagedResult PageResult(object value, string linqPredicate, int page, int pageSize, int? rowCount = null)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(linqPredicate);

        // CallWhere(...) and call PageResult.
        return CallWhere(value, linqPredicate).PageResult(page, pageSize, rowCount);
    }

    [HandlebarsWriter(WriterType.Value, "DynamicLinq.Reverse")]
    public IEnumerable Reverse(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Reverse.
        return CallWhere(value, linqPredicate).Reverse().ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object Single(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Single.
        return CallWhere(value, linqPredicate).Single();
    }

    [HandlebarsWriter(WriterType.Value)]
    public object? SingleOrDefault(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call SingleOrDefault.
        return CallWhere(value, linqPredicate).SingleOrDefault();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Skip(object value, int count)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Skip.
        return CallWhere(value).Skip(count).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable SkipWhile(object value, string linqPredicate)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Skip.
        return CallWhere(value).SkipWhile(linqPredicate).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable SkipAndTake(object value, int skip, int take)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Skip and Take.
        return CallWhere(value).Skip(skip).Take(take).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value, "DynamicLinq.Sum")]
    public object Sum(object value, string? linqPredicate = null)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Sum.
        return CallWhere(value, linqPredicate).Sum();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Take(object value, int count)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call Take.
        return CallWhere(value).Take(count).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable TakeWhile(object value, string linqPredicate)
    {
        Guard.NotNull(value);

        // CallWhere(...) and call TakeWhile.
        return CallWhere(value).TakeWhile(linqPredicate).ToDynamicArray();
    }

    [HandlebarsWriter(WriterType.Value)]
    public IEnumerable Where(object value, string linqPredicate)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(linqPredicate);

        return CallWhere(value, linqPredicate).ToDynamicArray();
    }

    private static IQueryable CallWhere(object value, string? linqPredicate = null, [CallerMemberName] string callerName = "")
    {
        if (value is not IEnumerable enumerable)
        {
            throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars DynamicLinq '{callerName}'.");
        }

        if (enumerable is JArray jArray)
        {
            enumerable = jArray.ToDynamicClassArray();
        }

        try
        {
            // Convert IEnumerable to IQueryable
            var queryable = enumerable.AsQueryable();

            // And call Where(...) if required.
            return !string.IsNullOrEmpty(linqPredicate) ? queryable.Where(linqPredicate!) : queryable;
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