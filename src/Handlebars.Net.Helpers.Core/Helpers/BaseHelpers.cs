using HandlebarsDotNet.Helpers.Options;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Helpers;

public abstract class BaseHelpers(IHandlebars context, HandlebarsHelpersOptions options)
{
    protected readonly IHandlebars Context = Guard.NotNull(context);
    protected readonly HandlebarsHelpersOptions Options = Guard.NotNull(options);
}