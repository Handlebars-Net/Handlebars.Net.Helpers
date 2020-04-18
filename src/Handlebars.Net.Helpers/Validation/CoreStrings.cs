// copied from https://github.com/aspnet/EntityFramework/blob/dev/src/Microsoft.EntityFrameworkCore/Properties/CoreStrings.resx
namespace HandlebarsDotNet.Helpers.Validation
{
    internal static class CoreStrings
    {
        /// <summary>
        /// The property '{property}' of the argument '{argument}' cannot be null.
        /// </summary>
        public static string ArgumentPropertyNull(string property, string argument)
        {
            return $"The property '{property}' of the argument '{argument}' cannot be null.";
        }

        /// <summary>
        /// The string argument '{argumentName}' cannot be empty.
        /// </summary>
        public static string ArgumentIsEmpty(string argumentName)
        {
            return $"The string argument '{argumentName}' cannot be empty.";
        }

        /// <summary>
        /// The collection argument '{argumentName}' must contain at least one element.
        /// </summary>
        public static string CollectionArgumentIsEmpty(string argumentName)
        {
            return $"The collection argument '{argumentName}' must contain at least one element.";
        }
    }
}