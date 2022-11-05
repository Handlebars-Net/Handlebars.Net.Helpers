using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;

namespace JsonConverter.SimpleJson;

[GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
internal
#else
public
#endif
interface IJsonSerializerStrategy
{
    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    bool TrySerializeNonPrimitiveObject(object? input, [NotNullWhen(true)] out object? output);
    
    object? DeserializeObject(object? value, Type type);
}