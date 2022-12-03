using System.Linq;
using HandlebarsDotNet.Helpers.Helpers;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Extensions;

// ReSharper disable once InconsistentNaming
public static class IHandlebarsExtensions
{
    public static object? Evaluate(this IHandlebars handlebarsContext, string template, object? data)
    {
        Guard.NotNull(handlebarsContext);
        Guard.NotNullOrEmpty(template);

        var start = template.TakeWhile(c => c == '{').Count();
        var end = template.Reverse().TakeWhile(c => c == '}').Count();
        var updated = template.Substring(start, template.Length - start - end);

        handlebarsContext.Compile(new string('{', start) + EvaluateHelper.HelperName + " " + updated + new string('}', end))(data);

        return HandlebarsHelpers.AsyncLocalResultFromEvaluate.Value;
    }
}