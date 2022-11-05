using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Stef.Validation;

namespace JsonConverter.SimpleJson;

[GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
internal
#else
public
#endif
class PocoJsonSerializerStrategy : IJsonSerializerStrategy
{
    internal IDictionary<Type, ReflectionUtils.ConstructorDelegate> ConstructorCache;
    internal IDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>> GetCache;
    internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> SetCache;

    internal static readonly Type[] EmptyTypes = new Type[0];
    internal static readonly Type[] ArrayConstructorParameterTypes = { typeof(int) };

    private static readonly string[] Iso8601Format =
    {
        @"yyyy-MM-dd\THH:mm:ss.FFFFFFF\Z",
        @"yyyy-MM-dd\THH:mm:ss\Z",
        @"yyyy-MM-dd\THH:mm:ssK"
    };

    public PocoJsonSerializerStrategy()
    {
        ConstructorCache = new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(ContructorDelegateFactory);
        GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(GetterValueFactory);
        SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(SetterValueFactory);
    }

    protected virtual string MapClrMemberNameToJsonFieldName(string clrPropertyName)
    {
        return clrPropertyName;
    }

    internal virtual ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
    {
        return ReflectionUtils.GetConstructor(key, key.IsArray ? ArrayConstructorParameterTypes : EmptyTypes);
    }

    internal virtual IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
    {
        IDictionary<string, ReflectionUtils.GetDelegate> result = new Dictionary<string, ReflectionUtils.GetDelegate>();
        foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
        {
            if (propertyInfo.CanRead)
            {
                MethodInfo getMethod = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                if (getMethod.IsStatic || !getMethod.IsPublic)
                    continue;
                result[MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = ReflectionUtils.GetGetMethod(propertyInfo);
            }
        }
        foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
        {
            if (fieldInfo.IsStatic || !fieldInfo.IsPublic)
                continue;
            result[MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = ReflectionUtils.GetGetMethod(fieldInfo);
        }
        return result;
    }

    internal virtual IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
    {
        IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> result = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
        foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
        {
            if (propertyInfo.CanWrite)
            {
                MethodInfo setMethod = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                if (setMethod.IsStatic || !setMethod.IsPublic)
                    continue;
                result[MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
            }
        }
        foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
        {
            if (fieldInfo.IsInitOnly || fieldInfo.IsStatic || !fieldInfo.IsPublic)
                continue;
            result[MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
        }
        return result;
    }

    public virtual bool TrySerializeNonPrimitiveObject(object? input, out object? output)
    {
        return TrySerializeKnownTypes(input, out output) || TrySerializeUnknownTypes(input!, out output);
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public virtual object? DeserializeObject(object? value, Type type)
    {
        Guard.NotNull(type);

        string? str = value as string;

        if (type == typeof(Guid) && string.IsNullOrEmpty(str))
            return default(Guid);

        if (value == null)
            return null;

        object? obj = null;

        if (str != null)
        {
            if (str.Length != 0) // We know it can't be null now.
            {
                if (type == typeof(DateTime) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime)))
                    return DateTime.ParseExact(str, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                if (type == typeof(DateTimeOffset) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset)))
                    return DateTimeOffset.ParseExact(str, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                if (type == typeof(Guid) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid)))
                    return new Guid(str);

                if (type == typeof(Uri))
                {
                    bool isValid = Uri.IsWellFormedUriString(str, UriKind.RelativeOrAbsolute);

                    if (isValid && Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out var result))
                    {
                        return result;
                    }

                    return null;
                }

                if (type == typeof(string))
                {
                    return str;
                }

                return Convert.ChangeType(str, type, CultureInfo.InvariantCulture);
            }

            if (type == typeof(Guid))
                obj = default(Guid);
            else if (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
                obj = null;
            else
                obj = str;

            // Empty string case
            if (!ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
            {
                return str;
            }
        }
        else if (value is bool)
        {
            return value;
        }

        bool valueIsLong = value is long;
        bool valueIsDouble = value is double;
        if ((valueIsLong && type == typeof(long)) || (valueIsDouble && type == typeof(double)))
        {
            return value;
        }

        if ((valueIsDouble && type != typeof(double)) || (valueIsLong && type != typeof(long)))
        {
            obj = type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(float) || type == typeof(bool) || type == typeof(decimal) || type == typeof(byte) || type == typeof(short)
                        ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture)
                        : value;
        }
        else
        {
            if (value is IDictionary<string, object> objects)
            {
                IDictionary<string, object> jsonObject = objects;

                if (ReflectionUtils.IsTypeDictionary(type))
                {
                    // if dictionary then
                    Type[] types = ReflectionUtils.GetGenericTypeArguments(type);
                    Type keyType = types[0];
                    Type valueType = types[1];

                    Type genericType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

                    IDictionary dict = (IDictionary)ConstructorCache[genericType]();

                    foreach (var kvp in jsonObject)
                    {
                        dict.Add(kvp.Key, DeserializeObject(kvp.Value, valueType));
                    }

                    obj = dict;
                }
                else
                {
                    if (type == typeof(object))
                    {
                        obj = value;
                    }
                    else
                    {
                        obj = ConstructorCache[type]();
                        foreach (KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> setter in SetCache[type])
                        {
                            if (jsonObject.TryGetValue(setter.Key, out var jsonValue))
                            {
                                jsonValue = DeserializeObject(jsonValue, setter.Value.Key);
                                setter.Value.Value(obj, jsonValue!);
                            }
                        }
                    }
                }
            }
            else
            {
                if (value is IList<object> valueAsList)
                {
                    IList<object> jsonObject = valueAsList;
                    IList? list = null;

                    if (type.IsArray)
                    {
                        list = (IList)ConstructorCache[type](jsonObject.Count);
                        int i = 0;
                        foreach (object o in jsonObject)
                        {
                            list[i++] = DeserializeObject(o, type.GetElementType()!);
                        }
                    }
                    else if (ReflectionUtils.IsTypeGenericCollectionInterface(type) || ReflectionUtils.IsAssignableFrom(typeof(IList), type))
                    {
                        Type innerType = ReflectionUtils.GetGenericListElementType(type);
                        list = (IList)(ConstructorCache[type] ?? ConstructorCache[typeof(List<>).MakeGenericType(innerType)])(jsonObject.Count);
                        foreach (object o in jsonObject)
                        {
                            list.Add(DeserializeObject(o, innerType));
                        }
                    }
                    obj = list;
                }
            }
            return obj;
        }

        if (ReflectionUtils.IsNullableType(type))
        {
            return ReflectionUtils.ToNullableType(obj, type);
        }

        return obj;
    }

    protected virtual object SerializeEnum(Enum p)
    {
        return Convert.ToDouble(p, CultureInfo.InvariantCulture);
    }

    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    protected virtual bool TrySerializeKnownTypes(object? input, [NotNullWhen(true)] out object? output)
    {
        switch (input)
        {
            case DateTime dateTime:
                output = dateTime.ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
                return true;

            case DateTimeOffset dateTimeOffset:
                output = dateTimeOffset.ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
                return true;

            case Guid guid:
                output = guid.ToString("D");
                return true;

            case Uri uri:
                output = uri.ToString();
                return true;

            case Enum inputEnum:
                output = SerializeEnum(inputEnum);
                return true;

            default:
                output = default;
                return false;
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    protected virtual bool TrySerializeUnknownTypes(object input, [NotNullWhen(true)] out object? output)
    {
        Guard.NotNull(input);

        output = default;

        var type = input.GetType();
        if (type.FullName == null)
        {
            return false;
        }

        IDictionary<string, object> obj = new JsonObject();
        IDictionary<string, ReflectionUtils.GetDelegate> getters = GetCache[type];
        foreach (var getter in getters)
        {
            if (getter.Value != null)
            {
                obj.Add(MapClrMemberNameToJsonFieldName(getter.Key), getter.Value(input));
            }
        }

        output = obj;
        return true;
    }
}