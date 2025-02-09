using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class DateTimeHelpers : BaseHelpers, IHelpers
{
    private readonly IDateTimeService _dateTimeService;

    public DateTimeHelpers(IHandlebars context, IDateTimeService dateTimeService) : base(context)
    {
        _dateTimeService = Guard.NotNull(dateTimeService);
    }

    [HandlebarsWriter(WriterType.Value)]
    public object Now(string? format = null)
    {
        var now = _dateTimeService.Now();
        return format is null ? now : now.ToString(format, Context.Configuration.FormatProvider);
    }

    [HandlebarsWriter(WriterType.Value)]
    public object UtcNow(string? format = null)
    {
        var utc = _dateTimeService.UtcNow();
        return format is null ? utc : utc.ToString(format, Context.Configuration.FormatProvider);
    }

    [HandlebarsWriter(WriterType.String)]
    public string Format(object value, string format)
    {
        return value switch
        {
            DateTime valueAsDateTime => FormatToString(valueAsDateTime),
            string valueAsString when DateTime.TryParse(valueAsString, out var parsedAsDateTime) => FormatToString(parsedAsDateTime),
            _ => string.Empty
        };

        string FormatToString(DateTime dateTime)
        {
            return dateTime.ToString(format, Context.Configuration.FormatProvider);
        }
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Compare(object? value1, string operation, object? value2, string? format = null)
    {
        Guard.NotNullOrEmpty(operation);

        var dateTime1 = GetDatetime(value1, format);
        var dateTime2 = GetDatetime(value2, format);

        return operation switch
        {
            ">" => dateTime1 > dateTime2,
            "<" => dateTime1 < dateTime2,
            "==" => dateTime1 == dateTime2,
            "!=" => dateTime1 != dateTime2,
            ">=" => dateTime1 >= dateTime2,
            "<=" => dateTime1 <= dateTime2,

            _ => throw new ArgumentException("Invalid comparison operator."),
        };
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime Add(object value, int increment, string datePart, string? format = null)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(datePart);

        var dateTime = Guard.NotNull(GetDatetime(value, format));
        if (dateTime is null)
        {
            throw new NullReferenceException(nameof(datePart));
        }

        return datePart switch
        {
            "year" => dateTime.Value.AddYears(increment),
            "month" => dateTime.Value.AddMonths(increment),
            "day" => dateTime.Value.AddDays(increment),
            "hour" => dateTime.Value.AddHours(increment),
            "minute" => dateTime.Value.AddMinutes(increment),
            "second" => dateTime.Value.AddSeconds(increment),
            "millisecond" => dateTime.Value.AddMilliseconds(increment),

            _ => throw new ArgumentException("Invalid date part. It must be one of: [year, month, day, hour, minute, second, millisecond]."),
        };
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime Parse(string value)
    {
        return DateTime.Parse(value, Context.Configuration.FormatProvider);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime ParseExact(string value, string format)
    {
        Guard.NotNullOrEmpty(format);

        return DateTime.ParseExact(value, format, Context.Configuration.FormatProvider);
    }

    private DateTime? GetDatetime(object? value, string? format = null)
    {
        return value switch
        {
            null => null,
            DateTime dateTimeValue => dateTimeValue,
            string stringValue => string.IsNullOrEmpty(format) ? Parse(stringValue) : ParseExact(stringValue, format),
            _ => GetDatetime(value.ToString(), format),
        };
    }

    public Category Category => Category.DateTime;
}