using System;
using System.Collections.Generic;
using HandlebarsDotNet.IO;
using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class MultiHandlebarsBlockHelper(string name, HandlebarsBlockHelper first) : IHelperDescriptor<BlockHelperOptions>
{
    public PathInfo Name { get; } = name;

    public List<HandlebarsBlockHelper> Helpers { get; } = [first];

    public object Invoke(in BlockHelperOptions options, in Context context, in Arguments arguments)
    {
        return this.ReturnInvoke(options, context, arguments);
    }

    public void Invoke(in EncodedTextWriter output, in BlockHelperOptions options, in Context context, in Arguments arguments)
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