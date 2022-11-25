using System.Reflection;

namespace HandlebarsDotNet.Helpers.Extensions;

public static class ParameterInfoExtensions
{
    public static bool IsParam(this ParameterInfo? parameterInfo)
    {
        return parameterInfo?.IsDefined(typeof(ParamArrayAttribute)) == true;
    }
}