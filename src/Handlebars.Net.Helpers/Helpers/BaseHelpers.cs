using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal abstract class BaseHelpers
    {
        private protected HandlebarsHelpersOptions Options;

        protected BaseHelpers(HandlebarsHelpersOptions options)
        {
            Options = options;
        }
    }
}