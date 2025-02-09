using HandlebarsDotNet.Helpers.Options;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

public abstract class BaseHelpers
{
    protected readonly IHandlebars Context;
    protected readonly HandlebarsHelpersOptions Options;

    protected BaseHelpers(IHandlebars context, HandlebarsHelpersOptions options)
    {
        Context = Guard.NotNull(context);
        Options = Guard.NotNull(options);
    }
}