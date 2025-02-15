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

    [HandlebarsWriter(WriterType.Value, Name = "DateTime.Add")]
    public DateTime Add(object value, int increment, string datePart, string? format = null)
    {
        Guard.NotNull(value);
        Guard.NotNullOrEmpty(datePart);

        var dateTime = Guard.NotNull(GetDatetime(value, format))!;

        return datePart switch
        {
            "year" => dateTime.Value.AddYears(increment),
            "month" => dateTime.Value.AddMonths(increment),
            "day" => dateTime.Value.AddDays(increment),
            "hour" => dateTime.Value.AddHours(increment),
            "minute" => dateTime.Value.AddMinutes(increment),
            "second" => dateTime.Value.AddSeconds(increment),
            "millisecond" => dateTime.Value.AddMilliseconds(increment),
            "ticks" => dateTime.Value.AddTicks(increment),
            _ => throw new ArgumentException("Invalid date part. It must be one of: [year, month, day, hour, minute, second, millisecond].")
        };
    }

    private DateTime? GetDatetime(object? value, string? format = null)
    {
        return value switch
        {
            null => null,
            DateTime dateTimeValue => dateTimeValue,
            string stringValue => string.IsNullOrEmpty(format) ? Parse(stringValue) : ParseExact(stringValue, format!),
            _ => GetDatetime(value.ToString(), format)
        };
    }

    public override Category Category => Category.DateTime;
}