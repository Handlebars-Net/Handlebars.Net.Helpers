using HandlebarsDotNet.PathStructure;

namespace HandlebarsDotNet.Helpers.PathStructure;

internal interface IPathResolverProxy
{
    bool TryAccessMember(BindingContext context, object instance, ChainSegment chainSegment, out object value);
}