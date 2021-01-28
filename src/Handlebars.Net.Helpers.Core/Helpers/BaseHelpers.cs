namespace HandlebarsDotNet.Helpers.Helpers
{
    public abstract class BaseHelpers
    {
        protected IHandlebars Context;

        protected BaseHelpers(IHandlebars context)
        {
            Context = context;
        }
    }
}