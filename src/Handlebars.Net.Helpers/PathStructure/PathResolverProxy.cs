using HandlebarsDotNet.PathStructure;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.PathStructure;

internal class PathResolverProxy : IPathResolverProxy
{
    public bool TryAccessMember(BindingContext context, object instance, ChainSegment chainSegment, out object value)
    {
        Guard.NotNull(context);
        Guard.NotNull(instance);
        Guard.NotNull(chainSegment);

        return PathResolver.TryAccessMember(context, instance, chainSegment, out value);
    }
}