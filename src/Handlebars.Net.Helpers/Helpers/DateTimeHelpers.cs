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
        string FormatToString(DateTime dateTime)
        {
            return dateTime.ToString(format, Context.Configuration.FormatProvider);
        }

        return value switch
        {
            DateTime valueAsDateTime => FormatToString(valueAsDateTime),
            string valueAsString when DateTime.TryParse(valueAsString, out var parsedAsDateTime) => FormatToString(parsedAsDateTime),
            _ => string.Empty
        };
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Compare(object? value1, string operation, object? value2, string? format = null)
    {
        if (string.IsNullOrEmpty(operation)) throw new ArgumentNullException(nameof(operation));

        if (value1 is null || value2 is null) {
            if ("!=".Equals(operation)) return value1 is not null || value2 is not null;
            if ("==".Equals(operation)) return value1 is null && value2 is null;
            else return false;
        } 
        
        var dateTime1 = GetDatetime(value1, format);
        var dateTime2 = GetDatetime(value2, format);

        switch(operation)
        {
            case ">": return dateTime1 > dateTime2;
            case "<": return dateTime1 < dateTime2;
            case "==": return dateTime1 == dateTime2;
            case "!=": return dateTime1 != dateTime2;
            case ">=": return dateTime1 >= dateTime2;
            case "<=": return dateTime1 <= dateTime2;
            default: throw new ArgumentException("Invalid comparison operator.");
        };
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime Add(object? value, int increment, string datePart, string? format = null)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        if (string.IsNullOrEmpty(datePart)) throw new ArgumentNullException(nameof(datePart));

        var dateTime = GetDatetime(value, format);

        switch(datePart)
        {
            case "year": return dateTime.AddYears(increment);
            case "month": return dateTime.AddMonths(increment);
            case "day": return dateTime.AddDays(increment);
            case "hour": return dateTime.AddHours(increment);
            case "minute": return dateTime.AddMinutes(increment);
            case "second": return dateTime.AddSeconds(increment);
            case "millisecond": return dateTime.AddMilliseconds(increment);
            default: throw new ArgumentException("Invalid date part. It must be one of: [year, month, day, hour, minute, second, millisecond].");
        };
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime Parse(string value)
    {
        return DateTime.Parse(value, Context.Configuration.FormatProvider);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime ParseExact(string value, string? format)
    {
        return DateTime.ParseExact(value, format, Context.Configuration.FormatProvider);
    }

    private DateTime GetDatetime(object value, string? format)
    {
        if (value.GetType().Equals(typeof(DateTime))) return (DateTime)value;
        else if (value.GetType().Equals(typeof(DateTime?))) return ((DateTime?)value).Value;
        else return string.IsNullOrEmpty(format) ? Parse(value.ToString()) : ParseExact(value.ToString(), format);
    }

    public Category Category => Category.DateTime;
}