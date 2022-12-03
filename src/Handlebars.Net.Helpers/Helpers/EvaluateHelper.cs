using HandlebarsDotNet.PathStructure;
using Stef.Validation;
#if NETSTANDARD1_3_OR_GREATER
using System.Threading;
#else
using HandlebarsDotNet.Polyfills;
#endif

namespace HandlebarsDotNet.Helpers.Helpers;

/// <summary>
/// https://github.com/Handlebars-Net/Handlebars.Net/issues/487
/// </summary>
internal class EvaluateHelper : IHelperDescriptor<HelperOptions>
{
    internal const string HelperName = "__evaluate";
    public PathInfo Name => HelperName;

    private readonly AsyncLocal<object?> _asyncLocal;

    public EvaluateHelper(AsyncLocal<object?> asyncLocal)
    {
        _asyncLocal = Guard.NotNull(asyncLocal);
    }
    
    public object? Invoke(in HelperOptions options, in Context context, in Arguments arguments)
    {
        return _asyncLocal.Value = arguments[0];
    }

    public void Invoke(in EncodedTextWriter output, in HelperOptions options, in Context context, in Arguments arguments)
    {
        _asyncLocal.Value = arguments[0];
    }
}