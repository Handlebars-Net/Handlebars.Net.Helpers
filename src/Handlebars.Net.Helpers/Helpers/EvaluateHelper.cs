using HandlebarsDotNet.PathStructure;
using Stef.Validation;
using HandlebarsDotNet.Helpers.Models;
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

    private readonly AsyncLocal<EvaluateResult> _asyncLocal;

    public EvaluateHelper(AsyncLocal<EvaluateResult> asyncLocal)
    {
        _asyncLocal = Guard.NotNull(asyncLocal);
    }

    public object Invoke(in HelperOptions options, in Context context, in Arguments arguments)
    {
        return SetValue(arguments);
    }

    public void Invoke(in EncodedTextWriter output, in HelperOptions options, in Context context, in Arguments arguments)
    {
        SetValue(arguments);
    }

    private EvaluateResult SetValue(Arguments arguments)
    {
        return _asyncLocal.Value = new EvaluateResult { IsEvaluated = true, Result = arguments[0] };
    }
}