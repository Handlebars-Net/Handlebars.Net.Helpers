using System.Collections.Generic;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Options;

/// <summary>
/// The additional options
/// </summary>
public class HandlebarsHelpersOptions
{
    private const string Dot = ".";

    /// <summary>
    /// Default options.
    /// </summary>
    public static HandlebarsHelpersOptions Default => new();

    /// <summary>
    /// Set to false if you don't want to add the prefix from the category to the helper name. (Default is set to true).
    /// </summary>
    public bool UseCategoryPrefix { get; set; } = true;

    /// <summary>
    /// Define a custom separator when <see cref="UseCategoryPrefix"/> is set to <see langword="true"/>. (Default value is a dot '.').
    /// </summary>
    public string PrefixSeparator { get; set; } = Dot;

    /// <summary>
    /// Defines if a dot '.' is used as PrefixSeparator.
    /// </summary>
    public bool PrefixSeparatorIsDot => PrefixSeparator == Dot;

    /// <summary>
    /// Define a custom prefix which will be added before of the helper name.
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// The categories to register. By default all categories are registered. See the WIKI for details.
    /// </summary>
    public Category[]? Categories { get; set; }

    /// <summary>
    /// The helpers to register.
    /// </summary>
    public IHelpers[]? Helpers { get; set; }

    /// <summary>
    /// Used for unit-testing DateTime related functionality.
    /// </summary>
    public IDateTimeService? DateTimeService { get; set; }

    /// <summary>
    /// A Dictionary with additional Custom Helpers (Key = CategoryPrefix, Value = IHelpers)
    /// </summary>
    public IDictionary<string, IHelpers>? CustomHelpers { get; set; }

    /// <summary>
    /// The paths to search for additional helpers. If null, the CurrentDirectory and BaseDirectory are used.
    /// </summary>
    public IReadOnlyList<string>? CustomHelperPaths;

    /// <summary>
    /// Additional options for Handlebars.Net.Helpers.DynamicLinq.
    /// </summary>
    public HandlebarsDynamicLinqHelperOptions? DynamicLinqHelperOptions { get; set; }
}