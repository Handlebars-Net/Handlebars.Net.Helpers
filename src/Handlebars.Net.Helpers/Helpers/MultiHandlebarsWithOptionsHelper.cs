using System;
using System.Collections.Generic;
using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class MultiHandlebarsWithOptionsHelper(string name, HandlebarsHelperWithOptions first) : IHelperDescriptor<HelperOptions>
{
    public PathInfo Name { get; } = name;

    public List<HandlebarsHelperWithOptions> Helpers { get; } = [first];

    public object Invoke(in HelperOptions options, in Context context, in Arguments arguments)
    {
        return this.ReturnInvoke(options, context, arguments);
    }

    public void Invoke(in EncodedTextWriter output, in HelperOptions options, in Context context, in Arguments arguments)
    {
        Exception? lastException = null;
        foreach (var helper in Helpers)
        {
            try
            {
                helper(output, options, context, arguments);
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