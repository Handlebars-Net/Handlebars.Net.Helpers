using Fare;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

// ReSharper disable once CheckNamespace
namespace HandlebarsDotNet.Helpers;

public class XegerHelpers : BaseHelpers, IHelpers
{
    public XegerHelpers(IHandlebars context) : base(context)
    {
    }

    [HandlebarsWriter(WriterType.String, "Xeger.Generate")]
    public string Generate(string pattern)
    {
        return new Xeger(pattern).Generate();
    }

    public Category Category => Category.Xeger;
}