using System.Globalization;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Options
{
    /// <summary>
    /// The additional options
    /// </summary>
    public class HandlebarsHelpersOptions
    {
        /// <summary>
        /// Set to false if you don't want to add the prefix from the category to the helper name. (Default is set to true).
        /// </summary>
        public bool UseCategoryPrefix { get; set; } = true;

        /// <summary>
        /// Define a custom prefix which will be added before of the helper name.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// The categories to register. By default all categories are registered. See the WIKI for details.
        /// </summary>
        public Category[]? Categories { get; set; } = null;

        /// <summary>
        /// The CultureInfo to use.  (Default is set to InvariantCulture).
        /// </summary>
        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
    }
}