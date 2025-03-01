namespace HandlebarsDotNet.Helpers.Options;

/// <summary>
/// Additional options for Handlebars.Net.Helpers.DynamicLinq.
/// </summary>
public class HandlebarsDynamicLinqHelperOptions
{
    /// <summary>
    /// When set to <c>true</c> the Handlebars.Net.Helpers.DynamicLinq will be loaded (if present).
    ///
    /// Default value is <c>false</c>.
    /// </summary>
    public bool Allow { get; set; }

    /// <summary>
    /// When set to <c>true</c>, the DynamicLinq helper will allow the use of the Equals(object obj), Equals(object objA, object objB), ReferenceEquals(object objA, object objB) and ToString() methods on the <see cref="object"/> type.
    ///
    /// Default value is <c>false</c>.
    /// </summary>
    public bool AllowEqualsAndToStringMethodsOnObject { get; set; }
}