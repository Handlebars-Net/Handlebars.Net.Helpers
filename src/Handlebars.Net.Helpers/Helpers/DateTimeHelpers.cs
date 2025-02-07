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
        Guard.NotNullOrEmpty(operation);
        
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
        Guard.NotNullOrEmpty(datePart);

        if (value is null) throw new ArgumentNullException(nameof(value));

        var dateTime = GetDatetime(value, format);

        if (dateTime is null) throw new NullReferenceException(nameof(dateTime));

        switch (datePart)
        {
            case "year": return dateTime.Value.AddYears(increment);
            case "month": return dateTime.Value.AddMonths(increment);
            case "day": return dateTime.Value.AddDays(increment);
            case "hour": return dateTime.Value.AddHours(increment);
            case "minute": return dateTime.Value.AddMinutes(increment);
            case "second": return dateTime.Value.AddSeconds(increment);
            case "millisecond": return dateTime.Value.AddMilliseconds(increment);
            default: throw new ArgumentException("Invalid date part. It must be one of: [year, month, day, hour, minute, second, millisecond].");
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
        if (value == null) return null;
        
        if (value is DateTime dateTimeValue) return dateTimeValue;
        
        if (value is string stringValue)
            return string.IsNullOrEmpty(format) ? Parse(stringValue) : ParseExact(stringValue, format);
        
        return GetDatetime(value.ToString(), format);
    }

    public Category Category => Category.DateTime;
}