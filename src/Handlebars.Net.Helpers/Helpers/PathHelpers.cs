using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.PathStructure;
using HandlebarsDotNet.PathStructure;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class PathHelpers : BaseHelpers, IHelpers
{
    private readonly IPathResolverProxy _pathResolver;

    [HandlebarsWriter(WriterType.Value)]
    public object? LookupWithDefault(IHelperOptions helperOptions, object instance, object path, object? valueIfNotFound = null)
    {
        return Lookup(helperOptions, instance, path, valueIfNotFound);
    }

    /// <summary>
    /// Note: lookup is a existing helper, so this method can only be used when prefixed with Path.
    /// </summary>
    [HandlebarsWriter(WriterType.Value, "Path.Lookup")]
    public object? Lookup(IHelperOptions helperOptions, object instance, object path, object? valueIfNotFound = null)
    {
        Guard.NotNull(instance);
        Guard.NotNull(path);

        var chainSegment = ChainSegment.Create(path);

        return _pathResolver.TryAccessMember(helperOptions.Frame, instance, chainSegment, out var value) ? value : valueIfNotFound;
    }

    public PathHelpers(IHandlebars context, IPathResolverProxy pathResolver) : base(context)
    {
        _pathResolver = Guard.NotNull(pathResolver);
    }
}