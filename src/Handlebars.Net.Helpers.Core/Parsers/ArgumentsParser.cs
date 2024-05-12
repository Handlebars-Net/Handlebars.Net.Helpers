using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Extensions;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Parsers;

public static class ArgumentsParser
{
    private static readonly Type[] SupportedTypes = { typeof(int), typeof(long), typeof(double) };

    public static List<object?> Parse(IHandlebars context, ParameterInfo[] parameters, Arguments arguments)
    {
        var result = new List<(object? Argument, object? DefaultValue)>();

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].IsParam())
            {
                result.Add((arguments.Skip(i).ToArray(), DefaultValueCache.GetDefaultValue(parameters[i].ParameterType)));
            }
            else if (i < arguments.Length)
            {
                var defaultValue = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : DefaultValueCache.GetDefaultValue(parameters[i].ParameterType);
                result.Add((arguments[i], defaultValue));
            }
        }

        return result.Select(x => Parse(context, x.Argument, x.DefaultValue)).ToList();
    }

    public static object? Parse(IHandlebars context, object? argument, object? defaultValue, bool convertObjectArrayToStringList = false)
    {
        if (argument is UndefinedBindingResult argumentAsUndefinedBindingResult)
        {
            if (TryParseUndefinedBindingResult(argumentAsUndefinedBindingResult, out var parsedAsObjectList))
            {
                if (convertObjectArrayToStringList)
                {
                    return parsedAsObjectList.Cast<string?>().ToList();
                }

                return parsedAsObjectList;
            }

            // In case it's an UndefinedBindingResult and ThrowOnUnresolvedBindingExpression is set to false, return the default value.
            if (!context.Configuration.ThrowOnUnresolvedBindingExpression)
            {
                return defaultValue;
            }
        }

        return argument;
    }

    public static object ParseAsIntLongOrDouble(IHandlebars context, object? value)
    {
        // In case the value is null and ThrowOnUnresolvedBindingExpression is set to false, return the default value (0).
        if (value is null && !context.Configuration.ThrowOnUnresolvedBindingExpression)
        {
            return 0;
        }

        var parsedValue = StringValueParser.Parse(context, value as string ?? value?.ToString() ?? string.Empty);

        if (SupportedTypes.Contains(parsedValue.GetType()))
        {
            return parsedValue;
        }

        throw new NotSupportedException($"The value '{value}' cannot not be converted to an int, long or double.");
    }

    /// <summary>
    /// In case it's an UndefinedBindingResult, just try to convert the value using Json.
    /// This logic adds functionality like parsing a list.
    /// </summary>
    /// <param name="undefinedBindingResult">The property value</param>
    /// <param name="parsedValue">The parsed value</param>
    /// <returns>true in case parsing is ok, else false</returns>
    private static bool TryParseUndefinedBindingResult(UndefinedBindingResult undefinedBindingResult, [NotNullWhen(true)] out List<object?>? parsedValue)
    {
        return ArrayUtils.TryParseAsObjectList(undefinedBindingResult.Value, out parsedValue);
    }
}