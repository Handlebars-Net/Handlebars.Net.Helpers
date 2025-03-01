using Fare;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

public class XegerHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
    [HandlebarsWriter(WriterType.String, "Xeger.Generate")]
    public string Generate(string pattern)
    {
        return new Xeger(pattern).Generate();
    }

    public Category Category => Category.Xeger;
}