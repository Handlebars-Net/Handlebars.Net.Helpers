using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace JsonConverter.SimpleJson;

#if SIMPLE_JSON_DATACONTRACT
[GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
internal
#else
public
#endif
class DataContractJsonSerializerStrategy : PocoJsonSerializerStrategy
{
    public DataContractJsonSerializerStrategy()
    {
        GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(GetterValueFactory);
        SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(SetterValueFactory);
    }

    internal override IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
    {
        bool hasDataContract = ReflectionUtils.GetAttribute(type, typeof(DataContractAttribute)) != null;
        if (!hasDataContract)
        {
            return base.GetterValueFactory(type);
        }

        string jsonKey;
        IDictionary<string, ReflectionUtils.GetDelegate> result = new Dictionary<string, ReflectionUtils.GetDelegate>();
        foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
        {
            if (propertyInfo.CanRead)
            {
                MethodInfo getMethod = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                if (!getMethod.IsStatic && CanAdd(propertyInfo, out jsonKey))
                    result[jsonKey] = ReflectionUtils.GetGetMethod(propertyInfo);
            }
        }
        foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
        {
            if (!fieldInfo.IsStatic && CanAdd(fieldInfo, out jsonKey))
                result[jsonKey] = ReflectionUtils.GetGetMethod(fieldInfo);
        }
        return result;
    }

    internal override IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
    {
        bool hasDataContract = ReflectionUtils.GetAttribute(type, typeof(DataContractAttribute)) != null;
        if (!hasDataContract)
            return base.SetterValueFactory(type);
        string jsonKey;
        IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> result = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
        foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
        {
            if (propertyInfo.CanWrite)
            {
                MethodInfo setMethod = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                if (!setMethod.IsStatic && CanAdd(propertyInfo, out jsonKey))
                    result[jsonKey] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
            }
        }
        foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
        {
            if (!fieldInfo.IsInitOnly && !fieldInfo.IsStatic && CanAdd(fieldInfo, out jsonKey))
                result[jsonKey] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
        }
        // todo implement sorting for DATACONTRACT.
        return result;
    }

    private static bool CanAdd(MemberInfo info, out string jsonKey)
    {
        jsonKey = null;
        if (ReflectionUtils.GetAttribute(info, typeof(IgnoreDataMemberAttribute)) != null)
            return false;
        DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)ReflectionUtils.GetAttribute(info, typeof(DataMemberAttribute));
        if (dataMemberAttribute == null)
            return false;
        jsonKey = string.IsNullOrEmpty(dataMemberAttribute.Name) ? info.Name : dataMemberAttribute.Name;
        return true;
    }
}

#endif