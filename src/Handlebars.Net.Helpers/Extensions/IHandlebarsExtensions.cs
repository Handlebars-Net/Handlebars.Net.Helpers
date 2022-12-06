using System.Linq;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Models;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Extensions;

// ReSharper disable once InconsistentNaming
public static class IHandlebarsExtensions
{
    public static bool TryEvaluate(this IHandlebars handlebarsContext, string template, object? data, out object? result)
    {
        Guard.NotNull(handlebarsContext);
        Guard.NotNull(template);

        var start = template.TakeWhile(c => c == '{').Count();
        var end = template.Reverse().TakeWhile(c => c == '}').Count();
        var updated = template.Substring(start, template.Length - start - end);

        // Always set AsyncLocalResultFromEvaluate to NotEvaluated
        HandlebarsHelpers.AsyncLocalResultFromEvaluate.Value = EvaluateResult.NotEvaluated;

        result = null;

        try
        {
            var compiled = handlebarsContext.Compile(new string('{', start) + EvaluateHelper.HelperName + " " + updated + new string('}', end));

            compiled(data);

            if (HandlebarsHelpers.AsyncLocalResultFromEvaluate.Value.IsEvaluated)
            {
                result = HandlebarsHelpers.AsyncLocalResultFromEvaluate.Value.Result;
                return true;
            }

            return false;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}