using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class DateTimeHelpers(IHandlebars context, IDateTimeService dateTimeService) : StringHelpers(context)
{
    private readonly IDateTimeService _dateTimeService = Guard.NotNull(dateTimeService);

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
    public override string Format(object? value, string format)
    {
        return value switch
        {
            DateTime valueAsDateTime => base.Format(valueAsDateTime, format),
            string valueAsString when DateTime.TryParse(valueAsString, out var parsedAsDateTime) => base.Format(parsedAsDateTime, format),
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
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddYears(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddMonths")]
    public DateTime AddMonths(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddMonths(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddDays")]
    public DateTime AddDays(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddDays(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddHours")]
    public DateTime AddHours(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddHours(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddMinutes")]
    public DateTime AddMinutes(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddMinutes(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddSeconds")]
    public DateTime AddSeconds(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddSeconds(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddMilliseconds")]
    public DateTime AddMilliseconds(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddMilliseconds(increment);
    }

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.AddTicks")]
    public DateTime AddTicks(object value, int increment, string? format = null)
    {
        DateTime dateTime = GetDateTimeNonNullabe(value, format);

        return dateTime.AddTicks(increment);
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

    private DateTime GetDateTimeNonNullabe(object value, string? format)
    {
        var dateTime = Guard.NotNull(GetDatetime(Guard.NotNull(value), format));

        return dateTime.GetValueOrDefault();
    }


    public override Category Category => Category.DateTime;
}