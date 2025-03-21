// Copied from https://github.com/StefH/JsonConverter

namespace HandlebarsDotNet.Helpers.Models;

/// <summary>
/// Enum to define how to convert an Integer in the Json Object.
/// </summary>
internal enum IntegerBehavior
{
    /// <summary>
    /// Convert all Integer types in the Json Object to an int (unless overflow).
    /// (default)
    /// </summary>
    UseInt = 0,

    /// <summary>
    /// Convert all Integer types in the Json Object to a long.
    /// </summary>
    UseLong = 1
}