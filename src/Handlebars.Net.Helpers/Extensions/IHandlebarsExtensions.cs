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

        var updatedTemplate = template.TrimStart('{').TrimEnd('}');
        handlebarsContext.Compile("{{" + EvaluateHelper.HelperName + " " + updatedTemplate + "}}")(data);

        return HandlebarsHelpers.AsyncLocalResultFromEvaluate.Value;
    }
}