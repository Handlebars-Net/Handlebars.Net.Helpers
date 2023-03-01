using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers.Extensions;

// ReSharper disable once CheckNamespace
namespace System.Reflection;

/// <summary>
/// Based on https://stackoverflow.com/questions/13071805/dynamic-invoke-of-a-method-using-named-parameters
/// </summary>
internal static class MethodBaseExtensions
{
    public static bool LastParameterIsParam(this MethodBase methodInfo)
    {
        return methodInfo.GetParameters().LastOrDefault()?.IsParam() == true;
    }

    public static bool FirstParameterIsHelperOptions(this MethodBase methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        return parameters.Length > 0 && parameters[0].ParameterType == typeof(IHelperOptions);
    }

    public static object InvokeWithNamedParameters(this MethodBase self, object obj, IDictionary<string, object> namedParameters)
    {
        return self.Invoke(obj, MapParameters(self, namedParameters));
    }

    private static object[] MapParameters(MethodBase method, IDictionary<string, object> namedParameters)
    {
        var paramNames = method.GetParameters().Select(p => p.Name).ToArray();
        var parameters = new object[paramNames.Length];
        for (int i = 0; i < parameters.Length; ++i)
        {
            parameters[i] = Type.Missing;
        }

        foreach (var item in namedParameters)
        {
            int paramIndex = Array.FindIndex(paramNames, n => n.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
            if (paramIndex >= 0)
            {
                parameters[paramIndex] = item.Value;
            }
        }

        return parameters;
    }
}