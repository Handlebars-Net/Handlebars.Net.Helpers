using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class BooleanHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    [HandlebarsWriter(WriterType.Value)]
    public bool Equal(bool value, bool test)
    {
        return value == test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool NotEqual(bool value, bool test)
    {
        return value != test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Not(bool value)
    {
        return !value;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool And(bool value, bool test)
    {
        return value && test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool LogicalAnd(bool value, bool test)
    {
        return value & test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool Or(bool value, bool test)
    {
        return value || test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool LogicalOr(bool value, bool test)
    {
        return value | test;
    }

    [HandlebarsWriter(WriterType.Value)]
    public bool LogicalXor(bool value, bool test)
    {
        return value ^ test;
    }

    public Category Category => Category.Boolean;
}