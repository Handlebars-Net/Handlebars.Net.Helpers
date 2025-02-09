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

        switch (operation)
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
    public DateTime AddYears(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddYears(increment);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime AddMonths(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddMonths(increment);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime AddDays(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddDays(increment);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime AddHours(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddHours(increment);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime AddMinutes(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddMinutes(increment);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime AddSeconds(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddSeconds(increment);
    }

    [HandlebarsWriter(WriterType.Value)]
    public DateTime AddMilliseconds(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddMilliseconds(increment);
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

    private DateTime GetDateTimeNonNullabe(object value, string? format)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        var dateTime = Guard.NotNull(GetDatetime(value, format));

        return dateTime.GetValueOrDefault();
    }

    public Category Category => Category.DateTime;
}