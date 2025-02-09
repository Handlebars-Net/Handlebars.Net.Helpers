using System;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class ConstantsHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    [HandlebarsWriter(WriterType.Value, "Constants.Math.E")]
    public double E()
    {
        return Math.E;
    }

    [HandlebarsWriter(WriterType.Value, "Constants.Math.PI")]
    public double PI()
    {
        return Math.PI;
    }

    public Category Category => Category.Constants;
}