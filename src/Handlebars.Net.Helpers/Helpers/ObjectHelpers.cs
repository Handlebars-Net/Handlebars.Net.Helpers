using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class ObjectHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{

    [HandlebarsWriter(WriterType.Value)]
    public object? FormatAsObject(object? value)
    {
        return value;
    }

    [HandlebarsWriter(WriterType.String)]
    public string ToString(object? value)
    {
        return value?.ToString() ?? string.Empty;
    }

    public Category Category => Category.Object;
}