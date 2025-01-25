using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class DateTimeHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    private readonly IDateTimeService _dateTimeService = options.DateTimeService ?? new DateTimeService();

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

    public Category Category => Category.DateTime;
}