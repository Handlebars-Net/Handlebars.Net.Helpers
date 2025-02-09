using System;
using System.Collections.Generic;
using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class MultiHandlebarsReturnWithOptionsHelper(string name, HandlebarsReturnWithOptionsHelper first) : IHelperDescriptor<HelperOptions>
{
    public PathInfo Name { get; } = name;

    public List<HandlebarsReturnWithOptionsHelper> Helpers { get; } = [first];

    public object Invoke(in HelperOptions options, in Context context, in Arguments arguments)
    {
        Exception? lastException = null;
        foreach (var helper in Helpers)
        {
            try
            {
                return helper.Invoke(options, context, arguments);
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }

        throw lastException ?? throw new InvalidOperationException();
    }

    public void Invoke(in EncodedTextWriter output, in HelperOptions options, in Context context, in Arguments arguments)
    {
        Exception? lastException = null;
        foreach (var helper in Helpers)
        {
            try
            {
                output.Write(helper(options, context, arguments));
                return;
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }

        throw lastException ?? new InvalidOperationException();
    }
}