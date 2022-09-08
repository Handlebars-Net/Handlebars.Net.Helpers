//-----------------------------------------------------------------------
// <copyright file="SimpleJson.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation.
//
//    Licensed under the MIT License (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.opensource.org/licenses/mit-license.php
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/simple-json</website>
//-----------------------------------------------------------------------

// VERSION:

// NOTE: uncomment the following line to make SimpleJson class internal.
//#define SIMPLE_JSON_INTERNAL

// NOTE: uncomment the following line to make JsonArray and JsonObject class internal.
//#define SIMPLE_JSON_OBJARRAYINTERNAL

// NOTE: uncomment the following line to enable dynamic support.
//#define SIMPLE_JSON_DYNAMIC

// NOTE: uncomment the following line to enable DataContract support.
//#define SIMPLE_JSON_DATACONTRACT

// NOTE: uncomment the following line to enable IReadOnlyCollection<T> and IReadOnlyList<T> support.
//#define SIMPLE_JSON_READONLY_COLLECTIONS

// NOTE: uncomment the following line to disable linq expressions/compiled lambda (better performance) instead of method.invoke().
// define if you are using .net framework <= 3.0 or < WP7.5
//#define SIMPLE_JSON_NO_LINQ_EXPRESSION

// NOTE: uncomment the following line if you are compiling under Window Metro style application/library.
// usually already defined in properties
//#define NETFX_CORE;

// If you are targetting WinStore, WP8 and NET4.5+ PCL make sure to #define SIMPLE_JSON_TYPEINFO;

// original json parsing code from http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html

#if NETFX_CORE
#define SIMPLE_JSON_TYPEINFO
#endif

using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
#if !SIMPLE_JSON_NO_LINQ_EXPRESSION
using System.Linq.Expressions;
#endif
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
#if SIMPLE_JSON_DYNAMIC
using System.Dynamic;
#endif
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace JsonConverter.SimpleJson;

/// <summary>
/// This class encodes and decodes JSON strings.
/// Spec. details, see http://www.json.org/
/// 
/// JSON uses Arrays and Objects. These correspond here to the datatypes JsonArray(IList&lt;object>) and JsonObject(IDictionary&lt;string,object>).
/// All numbers are parsed to doubles.
/// </summary>
[GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
internal
#else
public
#endif
static class SimpleJson
{
    private const int TOKEN_NONE = 0;
    private const int TOKEN_CURLY_OPEN = 1;
    private const int TOKEN_CURLY_CLOSE = 2;
    private const int TOKEN_SQUARED_OPEN = 3;
    private const int TOKEN_SQUARED_CLOSE = 4;
    private const int TOKEN_COLON = 5;
    private const int TOKEN_COMMA = 6;
    private const int TOKEN_STRING = 7;
    private const int TOKEN_NUMBER = 8;
    private const int TOKEN_TRUE = 9;
    private const int TOKEN_FALSE = 10;
    private const int TOKEN_NULL = 11;
    private const int BUILDER_CAPACITY = 2000;

    private static readonly char[] EscapeTable;
    private static readonly char[] EscapeCharacters = { '"', '\\', '\b', '\f', '\n', '\r', '\t' };
    private static readonly string EscapeCharactersString = new(EscapeCharacters);

    static SimpleJson()
    {
        EscapeTable = new char[93];
        EscapeTable['"'] = '"';
        EscapeTable['\\'] = '\\';
        EscapeTable['\b'] = 'b';
        EscapeTable['\f'] = 'f';
        EscapeTable['\n'] = 'n';
        EscapeTable['\r'] = 'r';
        EscapeTable['\t'] = 't';
    }

    /// <summary>
    /// Parses the string json into a value
    /// </summary>
    /// <param name="json">A JSON string.</param>
    /// <returns>An IList&lt;object>, a IDictionary&lt;string,object>, a double, a string, null, true, or false</returns>
    public static object DeserializeObject(string? json)
    {
        if (TryDeserializeObject(json, out var obj))
        {
            return obj;
        }

        throw new SerializationException("Invalid JSON string");
    }

    /// <summary>
    /// Try parsing the json string into a value.
    /// </summary>
    /// <param name="json">
    /// A JSON string.
    /// </param>
    /// <param name="obj">
    /// The object.
    /// </param>
    /// <returns>
    /// Returns true if successful otherwise false.
    /// </returns>
    public static bool TryDeserializeObject(string? json, [NotNullWhen(true)] out object? obj)
    {
        bool success = true;
        if (json != null)
        {
            var charArray = json.ToCharArray();
            int index = 0;
            obj = ParseValue(charArray, ref index, ref success);
        }
        else
        {
            obj = null;
        }

        return success;
    }

    public static object? DeserializeObject(string? json, Type? type, IJsonSerializerStrategy? jsonSerializerStrategy)
    {
        var jsonObject = DeserializeObject(json);
        return type == null || jsonObject != null && ReflectionUtils.IsAssignableFrom(jsonObject.GetType(), type)
            ? jsonObject
            : (jsonSerializerStrategy ?? CurrentJsonSerializerStrategy).DeserializeObject(jsonObject, type);
    }

    public static object? DeserializeObject(string? json, Type type)
    {
        return DeserializeObject(json, type, null);
    }

    public static T? DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy)
    {
        return (T?)DeserializeObject(json, typeof(T), jsonSerializerStrategy);
    }

    public static T? DeserializeObject<T>(string? json)
    {
        return (T?)DeserializeObject(json, typeof(T), null);
    }

    /// <summary>
    /// Converts a IDictionary&lt;string,object> / IList&lt;object> object into a JSON string
    /// </summary>
    /// <param name="json">A IDictionary&lt;string,object> / IList&lt;object></param>
    /// <param name="jsonSerializerStrategy">Serializer strategy to use</param>
    /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
    public static string? SerializeObject(object? json, IJsonSerializerStrategy jsonSerializerStrategy)
    {
        var builder = new StringBuilder(BUILDER_CAPACITY);
        bool success = SerializeValue(jsonSerializerStrategy, json, builder);
        return success ? builder.ToString() : null;
    }

    public static string? SerializeObject(object? json)
    {
        return SerializeObject(json, CurrentJsonSerializerStrategy);
    }

    public static string EscapeToJavascriptString(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
            return jsonString;

        StringBuilder sb = new StringBuilder();
        char c;

        for (int i = 0; i < jsonString.Length;)
        {
            c = jsonString[i++];

            if (c == '\\')
            {
                int remainingLength = jsonString.Length - i;
                if (remainingLength >= 2)
                {
                    char lookahead = jsonString[i];
                    if (lookahead == '\\')
                    {
                        sb.Append('\\');
                        ++i;
                    }
                    else if (lookahead == '"')
                    {
                        sb.Append("\"");
                        ++i;
                    }
                    else if (lookahead == 't')
                    {
                        sb.Append('\t');
                        ++i;
                    }
                    else if (lookahead == 'b')
                    {
                        sb.Append('\b');
                        ++i;
                    }
                    else if (lookahead == 'n')
                    {
                        sb.Append('\n');
                        ++i;
                    }
                    else if (lookahead == 'r')
                    {
                        sb.Append('\r');
                        ++i;
                    }
                }
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    private static IDictionary<string, object>? ParseObject(char[] json, ref int index, ref bool success)
    {
        IDictionary<string, object> dictionary = new JsonObject();

        // {
        NextToken(json, ref index);

        bool done = false;
        while (!done)
        {
            var token = LookAhead(json, index);
            if (token == TOKEN_NONE)
            {
                success = false;
                return null;
            }

            if (token == TOKEN_COMMA)
            {
                NextToken(json, ref index);
            }
            else if (token == TOKEN_CURLY_CLOSE)
            {
                NextToken(json, ref index);
                return dictionary;
            }
            else
            {
                // name
                var name = ParseString(json, ref index, ref success);
                if (!success)
                {
                    success = false;
                    return null;
                }

                // :
                token = NextToken(json, ref index);
                if (token != TOKEN_COLON)
                {
                    success = false;
                    return null;
                }

                // value
                var value = ParseValue(json, ref index, ref success);
                if (!success)
                {
                    success = false;
                    return null;
                }
                dictionary[name!] = value!;
            }
        }

        return dictionary;
    }

    private static JsonArray? ParseArray(char[] json, ref int index, ref bool success)
    {
        JsonArray array = new JsonArray();

        // [
        NextToken(json, ref index);

        bool done = false;
        while (!done)
        {
            int token = LookAhead(json, index);
            if (token == TOKEN_NONE)
            {
                success = false;
                return null;
            }

            if (token == TOKEN_COMMA)
            {
                NextToken(json, ref index);
            }
            else if (token == TOKEN_SQUARED_CLOSE)
            {
                NextToken(json, ref index);
                break;
            }
            else
            {
                var value = ParseValue(json, ref index, ref success);
                if (!success)
                {
                    return null;
                }

                array.Add(value!);
            }
        }

        return array;
    }

    private static object? ParseValue(char[] json, ref int index, ref bool success)
    {
        switch (LookAhead(json, index))
        {
            case TOKEN_STRING:
                return ParseString(json, ref index, ref success);
            case TOKEN_NUMBER:
                return ParseNumber(json, ref index, ref success);
            case TOKEN_CURLY_OPEN:
                return ParseObject(json, ref index, ref success);
            case TOKEN_SQUARED_OPEN:
                return ParseArray(json, ref index, ref success);
            case TOKEN_TRUE:
                NextToken(json, ref index);
                return true;
            case TOKEN_FALSE:
                NextToken(json, ref index);
                return false;
            case TOKEN_NULL:
                NextToken(json, ref index);
                return null;
            case TOKEN_NONE:
                break;
        }
        success = false;
        return null;
    }

    private static string? ParseString(char[] json, ref int index, ref bool success)
    {
        var stringBuilder = new StringBuilder(BUILDER_CAPACITY);

        EatWhitespace(json, ref index);

        // "
        var c = json[index++];
        bool complete = false;
        while (!complete)
        {
            if (index == json.Length)
            {
                break;
            }

            c = json[index++];
            if (c == '"')
            {
                complete = true;
                break;
            }

            if (c == '\\')
            {
                if (index == json.Length)
                    break;

                c = json[index++];
                if (c == '"')
                    stringBuilder.Append('"');
                else if (c == '\\')
                    stringBuilder.Append('\\');
                else if (c == '/')
                    stringBuilder.Append('/');
                else if (c == 'b')
                    stringBuilder.Append('\b');
                else if (c == 'f')
                    stringBuilder.Append('\f');
                else if (c == 'n')
                    stringBuilder.Append('\n');
                else if (c == 'r')
                    stringBuilder.Append('\r');
                else if (c == 't')
                    stringBuilder.Append('\t');
                else if (c == 'u')
                {
                    int remainingLength = json.Length - index;
                    if (remainingLength >= 4)
                    {
                        // parse the 32 bit hex into an integer codepoint
                        if (!(success = UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var codePoint)))
                            return "";

                        // convert the integer codepoint to a unicode char and add to string
                        if (0xD800 <= codePoint && codePoint <= 0xDBFF)  // if high surrogate
                        {
                            index += 4; // skip 4 chars
                            remainingLength = json.Length - index;
                            if (remainingLength >= 6)
                            {
                                if (new string(json, index, 2) == "\\u" && UInt32.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var lowCodePoint))
                                {
                                    if (0xDC00 <= lowCodePoint && lowCodePoint <= 0xDFFF) // if low surrogate
                                    {
                                        stringBuilder.Append((char)codePoint);
                                        stringBuilder.Append((char)lowCodePoint);
                                        index += 6; // skip 6 chars
                                        continue;
                                    }
                                }
                            }
                            success = false;    // invalid surrogate pair
                            return "";
                        }
                        stringBuilder.Append(ConvertFromUtf32((int)codePoint));
                        // skip 4 chars
                        index += 4;
                    }
                    else
                        break;
                }
            }
            else
                stringBuilder.Append(c);
        }

        if (!complete)
        {
            success = false;
            return null;
        }

        return stringBuilder.ToString();
    }

    private static string ConvertFromUtf32(int utf32)
    {
        // http://www.java2s.com/Open-Source/CSharp/2.6.4-mono-.net-core/System/System/Char.cs.htm
        if (utf32 < 0 || utf32 > 0x10FFFF)
            throw new ArgumentOutOfRangeException(nameof(utf32), "The argument must be from 0 to 0x10FFFF.");

        if (0xD800 <= utf32 && utf32 <= 0xDFFF)
            throw new ArgumentOutOfRangeException(nameof(utf32), "The argument must not be in surrogate pair range.");

        if (utf32 < 0x10000)
            return new string((char)utf32, 1);

        utf32 -= 0x10000;
        return new string(new[] { (char)((utf32 >> 10) + 0xD800), (char)(utf32 % 0x0400 + 0xDC00) });
    }

    private static object ParseNumber(char[] json, ref int index, ref bool success)
    {
        EatWhitespace(json, ref index);
        int lastIndex = GetLastIndexOfNumber(json, index);
        int charLength = lastIndex - index + 1;
        object returnNumber;

        string str = new string(json, index, charLength);
        if (str.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || str.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
        {
            success = double.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out var number);
            returnNumber = number;
        }
        else
        {
            success = long.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out var number);
            returnNumber = number;
        }

        index = lastIndex + 1;
        return returnNumber;
    }

    static int GetLastIndexOfNumber(char[] json, int index)
    {
        int lastIndex;
        for (lastIndex = index; lastIndex < json.Length; lastIndex++)
            if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1) break;
        return lastIndex - 1;
    }

    static void EatWhitespace(char[] json, ref int index)
    {
        for (; index < json.Length; index++)
            if (" \t\n\r\b\f".IndexOf(json[index]) == -1) break;
    }

    static int LookAhead(char[] json, int index)
    {
        int saveIndex = index;
        return NextToken(json, ref saveIndex);
    }

    static int NextToken(char[] json, ref int index)
    {
        EatWhitespace(json, ref index);

        if (index == json.Length)
        {
            return TOKEN_NONE;
        }

        char c = json[index];
        index++;
        switch (c)
        {
            case '{':
                return TOKEN_CURLY_OPEN;
            case '}':
                return TOKEN_CURLY_CLOSE;
            case '[':
                return TOKEN_SQUARED_OPEN;
            case ']':
                return TOKEN_SQUARED_CLOSE;
            case ',':
                return TOKEN_COMMA;
            case '"':
                return TOKEN_STRING;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            case '-':
                return TOKEN_NUMBER;
            case ':':
                return TOKEN_COLON;
        }

        index--;
        int remainingLength = json.Length - index;

        // false
        if (remainingLength >= 5)
        {
            if (json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
            {
                index += 5;
                return TOKEN_FALSE;
            }
        }

        // true
        if (remainingLength >= 4)
        {
            if (json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
            {
                index += 4;
                return TOKEN_TRUE;
            }
        }

        // null
        if (remainingLength >= 4)
        {
            if (json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
            {
                index += 4;
                return TOKEN_NULL;
            }
        }
        return TOKEN_NONE;
    }

    static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object? value, StringBuilder builder)
    {
        bool success = true;
        if (value is string stringValue)
        {
            success = SerializeString(stringValue, builder);
        }
        else
        {
            if (value is IDictionary<string, object> dict)
            {
                success = SerializeObject(jsonSerializerStrategy, dict.Keys, dict.Values, builder);
            }
            else
            {
                if (value is IDictionary<string, string> stringDictionary)
                {
                    success = SerializeObject(jsonSerializerStrategy, stringDictionary.Keys, stringDictionary.Values, builder);
                }
                else
                {
                    if (value is IEnumerable enumerableValue)
                    {
                        success = SerializeArray(jsonSerializerStrategy, enumerableValue, builder);
                    }
                    else if (IsNumeric(value))
                    {
                        success = SerializeNumber(value, builder);
                    }
                    else if (value is bool boolValue)
                    {
                        builder.Append(boolValue ? "true" : "false");
                    }
                    else if (value == null)
                    {
                        builder.Append("null");
                    }
                    else
                    {
                        success = jsonSerializerStrategy.TrySerializeNonPrimitiveObject(value, out var serializedObject);
                        if (success)
                        {
                            SerializeValue(jsonSerializerStrategy, serializedObject, builder);
                        }
                    }
                }
            }
        }
        return success;
    }

    static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
    {
        builder.Append("{");
        IEnumerator ke = keys.GetEnumerator();
        IEnumerator ve = values.GetEnumerator();
        bool first = true;
        while (ke.MoveNext() && ve.MoveNext())
        {
            var key = ke.Current;
            var value = ve.Current;
            if (!first)
                builder.Append(",");
            if (key is string stringKey)
            {
                SerializeString(stringKey, builder);
            }
            else
            if (!SerializeValue(jsonSerializerStrategy, value, builder)) return false;
            builder.Append(":");
            if (!SerializeValue(jsonSerializerStrategy, value, builder))
            {
                return false;
            }
            first = false;
        }
        builder.Append("}");
        return true;
    }

    static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
    {
        builder.Append("[");
        bool first = true;
        foreach (object value in anArray)
        {
            if (!first)
                builder.Append(",");
            if (!SerializeValue(jsonSerializerStrategy, value, builder))
                return false;
            first = false;
        }
        builder.Append("]");
        return true;
    }

    static bool SerializeString(string aString, StringBuilder builder)
    {
        // Happy path if there's nothing to be escaped. IndexOfAny is highly optimized (and unmanaged)
        if (aString.IndexOfAny(EscapeCharacters) == -1)
        {
            builder.Append('"');
            builder.Append(aString);
            builder.Append('"');

            return true;
        }

        builder.Append('"');
        int safeCharacterCount = 0;
        char[] charArray = aString.ToCharArray();

        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];

            // Non ascii characters are fine, buffer them up and send them to the builder
            // in larger chunks if possible. The escape table is a 1:1 translation table
            // with \0 [default(char)] denoting a safe character.
            if (c >= EscapeTable.Length || EscapeTable[c] == default(char))
            {
                safeCharacterCount++;
            }
            else
            {
                if (safeCharacterCount > 0)
                {
                    builder.Append(charArray, i - safeCharacterCount, safeCharacterCount);
                    safeCharacterCount = 0;
                }

                builder.Append('\\');
                builder.Append(EscapeTable[c]);
            }
        }

        if (safeCharacterCount > 0)
        {
            builder.Append(charArray, charArray.Length - safeCharacterCount, safeCharacterCount);
        }

        builder.Append('"');
        return true;
    }

    static bool SerializeNumber(object? number, StringBuilder builder)
    {
        switch (number)
        {
            case long l:
                builder.Append(l.ToString(CultureInfo.InvariantCulture));
                break;

            case ulong ul:
                builder.Append(ul.ToString(CultureInfo.InvariantCulture));
                break;

            case int i:
                builder.Append(i.ToString(CultureInfo.InvariantCulture));
                break;

            case uint ui:
                builder.Append(ui.ToString(CultureInfo.InvariantCulture));
                break;

            case decimal d:
                builder.Append(d.ToString(CultureInfo.InvariantCulture));
                break;

            case float f:
                builder.Append(f.ToString(CultureInfo.InvariantCulture));
                break;

            default:
                builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture).ToString("r", CultureInfo.InvariantCulture));
                break;
        }

        return true;
    }

    /// <summary>
    /// Determines if a given object is numeric in any way
    /// (can be integer, double, null, etc).
    /// </summary>
    static bool IsNumeric(object? value)
    {
        switch (value)
        {
            case sbyte:
            case byte:
            case short:
            case ushort:
            case int:
            case uint:
            case long:
            case ulong:
            case float:
            case double:
            case decimal:
                return true;

            default:
                return false;
        }
    }

    private static IJsonSerializerStrategy? _currentJsonSerializerStrategy;
    public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
    {
        get
        {
            return _currentJsonSerializerStrategy ??
                   (_currentJsonSerializerStrategy =
#if SIMPLE_JSON_DATACONTRACT
    DataContractJsonSerializerStrategy
#else
    PocoJsonSerializerStrategy
#endif
        );
        }

        set
        {
            _currentJsonSerializerStrategy = value;
        }
    }

    private static PocoJsonSerializerStrategy? _pocoJsonSerializerStrategy;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy => _pocoJsonSerializerStrategy ??= new PocoJsonSerializerStrategy();

#if SIMPLE_JSON_DATACONTRACT
    private static DataContractJsonSerializerStrategy? _dataContractJsonSerializerStrategy;
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static DataContractJsonSerializerStrategy DataContractJsonSerializerStrategy => _dataContractJsonSerializerStrategy ??= new DataContractJsonSerializerStrategy();
#endif
}