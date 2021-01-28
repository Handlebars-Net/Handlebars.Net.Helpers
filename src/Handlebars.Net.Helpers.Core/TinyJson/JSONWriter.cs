using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HandlebarsDotNet.Helpers.TinyJson
{
    //Really simple JSON writer
    //- Outputs JSON structures from an object
    //- Really simple API (new List<int> { 1, 2, 3 }).ToJson() == "[1,2,3]"
    //- Will only output public fields and property getters on objects
    public static class JSONWriter
    {
        public static string ToJson(this object? item)
        {
            var stringBuilder = new StringBuilder();
            AppendValue(stringBuilder, item);
            return stringBuilder.ToString();
        }

        static void AppendValue(StringBuilder stringBuilder, object? item)
        {
            if (item == null)
            {
                stringBuilder.Append("null");
                return;
            }

            Type type = item.GetType();
            if (type == typeof(string) || type == typeof(char))
            {
                stringBuilder.Append('"');
                string str = item.ToString();
                for (int i = 0; i < str.Length; ++i)
                {
                    if (str[i] < ' ' || str[i] == '"' || str[i] == '\\')
                    {
                        stringBuilder.Append('\\');
                        int j = "\"\\\n\r\t\b\f".IndexOf(str[i]);
                        if (j >= 0)
                        {
                            stringBuilder.Append("\"\\nrtbf"[j]);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("u{0:X4}", (UInt32)str[i]);
                        }
                    }
                    else
                        stringBuilder.Append(str[i]);
                }
                stringBuilder.Append('"');
            }
            else if (type == typeof(byte) || type == typeof(sbyte))
            {
                stringBuilder.Append(item.ToString());
            }
            else if (type == typeof(short) || type == typeof(ushort))
            {
                stringBuilder.Append(item.ToString());
            }
            else if (type == typeof(int) || type == typeof(uint))
            {
                stringBuilder.Append(item.ToString());
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                stringBuilder.Append(item.ToString());
            }
            else if (type == typeof(float))
            {
                stringBuilder.Append(((float)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(double))
            {
                stringBuilder.Append(((double)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(decimal))
            {
                stringBuilder.Append(((decimal)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(bool))
            {
                stringBuilder.Append(((bool)item) ? "true" : "false");
            }
            else if (type.GetTypeInfo().IsEnum)
            {
                stringBuilder.Append('"');
                stringBuilder.Append(item.ToString());
                stringBuilder.Append('"');
            }
            else if (item is IEnumerable list)
            {
                stringBuilder.Append('[');

                bool isFirst = true;
                foreach (var @object in list)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        stringBuilder.Append(',');
                    }

                    AppendValue(stringBuilder, @object);
                }

                stringBuilder.Append(']');
            }
            else if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type keyType = type.GetGenericArguments()[0];

                //Refuse to output dictionary keys that aren't of type string
                if (keyType != typeof(string))
                {
                    stringBuilder.Append("{}");
                    return;
                }

                stringBuilder.Append('{');
                var dict = (IDictionary)item;
                bool isFirst = true;
                foreach (object key in dict.Keys)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        stringBuilder.Append(',');
                    }

                    stringBuilder.Append('\"');
                    stringBuilder.Append((string)key);
                    stringBuilder.Append("\":");
                    AppendValue(stringBuilder, dict[key]);
                }
                stringBuilder.Append('}');
            }
            else
            {
                stringBuilder.Append('{');

                bool isFirst = true;
                var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    if (fieldInfos[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    {
                        continue;
                    }

                    object value = fieldInfos[i].GetValue(item);
                    if (value != null)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            stringBuilder.Append(',');
                        }

                        stringBuilder.Append('\"');
                        stringBuilder.Append(GetMemberName(fieldInfos[i]));
                        stringBuilder.Append("\":");
                        AppendValue(stringBuilder, value);
                    }
                }

                var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    if (!propertyInfos[i].CanRead || propertyInfos[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    {
                        continue;
                    }

                    object value = propertyInfos[i].GetValue(item, null);
                    if (value != null)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            stringBuilder.Append(',');
                        }

                        stringBuilder.Append('\"');
                        stringBuilder.Append(GetMemberName(propertyInfos[i]));
                        stringBuilder.Append("\":");
                        AppendValue(stringBuilder, value);
                    }
                }

                stringBuilder.Append('}');
            }
        }

        static string GetMemberName(MemberInfo member)
        {
            if (member.IsDefined(typeof(DataMemberAttribute), true))
            {
                var dataMemberAttribute = member.GetCustomAttribute<DataMemberAttribute>();
                if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                {
                    return dataMemberAttribute.Name;
                }
            }

            return member.Name;
        }
    }
}