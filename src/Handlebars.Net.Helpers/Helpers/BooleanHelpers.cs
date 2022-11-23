using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class BooleanHelpers : BaseHelpers, IHelpers
{
    public BooleanHelpers(IHandlebars context) : base(context)
    {
    }

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
}