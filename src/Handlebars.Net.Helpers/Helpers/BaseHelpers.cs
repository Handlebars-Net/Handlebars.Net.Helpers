namespace HandlebarsDotNet.Helpers.Helpers
{
    internal abstract class BaseHelpers
    {
        private protected IHandlebars Context;

        protected BaseHelpers(IHandlebars context)
        {
            Context = context;
        }
    }
}