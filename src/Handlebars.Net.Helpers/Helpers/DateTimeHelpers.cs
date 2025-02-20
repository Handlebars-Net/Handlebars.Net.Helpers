using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class DateTimeHelpers(IHandlebars context, IDateTimeService dateTimeService) : BaseHelpers(context), IHelpers
{
    private readonly IDateTimeService _dateTimeService = Guard.NotNull(dateTimeService);
    private readonly StringHelpers _stringHelpers = new(context);

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

    [HandlebarsWriter(WriterType.String, Name = "DateTime.Format")]
    public string Format(object? value, string format)
    {
        return value switch
        {
            DateTime valueAsDateTime => _stringHelpers.Format(valueAsDateTime, format),
            string valueAsString when DateTime.TryParse(valueAsString, out var parsedAsDateTime) => _stringHelpers.Format(parsedAsDateTime, format),
            _ => string.Empty
        };
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.Parse")]
    public DateTime Parse(string value)
    {
        return DateTime.Parse(value, Context.Configuration.FormatProvider);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.ParseExact")]
    public DateTime ParseExact(string value, string format)
    {
        return DateTime.ParseExact(value, format, Context.Configuration.FormatProvider);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddYears")]
    public DateTime AddYears(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddYears(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddMonths")]
    public DateTime AddMonths(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddMonths(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddDays")]
    public DateTime AddDays(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddDays(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddHours")]
    public DateTime AddHours(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddHours(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddMinutes")]
    public DateTime AddMinutes(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddMinutes(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddSeconds")]
    public DateTime AddSeconds(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddSeconds(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddMilliseconds")]
    public DateTime AddMilliseconds(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddMilliseconds(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddTicks")]
    public DateTime AddTicks(object value, int increment, string? format = null)
    {
        return GetDateTimeNonNullable(value, format).AddTicks(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.Add")]
    public DateTime Add(object value, int increment, string datePart, string? format = null)
    {
        var dateTime = GetDateTimeNonNullable(value, format);

        return datePart switch
        {
            "years" => dateTime.AddYears(increment),
            "months" => dateTime.AddMonths(increment),
            "days" => dateTime.AddDays(increment),
            "hours" => dateTime.AddHours(increment),
            "minutes" => dateTime.AddMinutes(increment),
            "seconds" => dateTime.AddSeconds(increment),
            "milliseconds" => dateTime.AddMilliseconds(increment),
            "ticks" => dateTime.AddTicks(increment),
            _ => throw new ArgumentException("Invalid date part. It must be one of: [years, months, days, hours, minutes, seconds, milliseconds or ticks].")
        };
    }

    private DateTime? GetDatetime(object? value, string? format)
    {
        return value switch
        {
            null => null,
            DateTime dateTimeValue => dateTimeValue,
            string stringValue => string.IsNullOrEmpty(format) ? Parse(stringValue) : ParseExact(stringValue, format),
            _ => GetDatetime(value.ToString(), format)
        };
    }

    private DateTime GetDateTimeNonNullable(object value, string? format)
    {
        return Guard.NotNull(GetDatetime(Guard.NotNull(value), format)).GetValueOrDefault();
    }

    public Category Category => Category.DateTime;
}