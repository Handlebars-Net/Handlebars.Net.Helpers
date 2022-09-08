using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

namespace JsonConverter.SimpleJson;

/// <summary>
/// Represents the json array.
/// </summary>
[GeneratedCode("simple-json", "1.0.0")]
[EditorBrowsable(EditorBrowsableState.Never)]
#if SIMPLE_JSON_OBJARRAYINTERNAL
internal
#else
public
#endif
    class JsonArray : List<object?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArray"/> class. 
    /// </summary>
    public JsonArray() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArray"/> class. 
    /// </summary>
    /// <param name="capacity">The capacity of the json array.</param>
    public JsonArray(int capacity) : base(capacity) { }

    /// <summary>
    /// The json representation of the array.
    /// </summary>
    /// <returns>The json representation of the array.</returns>
    public override string ToString()
    {
        return SimpleJson.SerializeObject(this) ?? string.Empty;
    }
}