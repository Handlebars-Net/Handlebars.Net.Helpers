using System.Linq;
using System.Reflection;

namespace HandlebarsDotNet.Helpers.Utils
{
    internal static class ReflectionUtils
    {
        // https://stackoverflow.com/questions/2535287/getting-nested-object-property-value-using-reflection
        public static object? GetPropertyOrFieldValue(object? obj, string propertyOrFieldName)
        {
            return propertyOrFieldName.Contains(".") ?
                GetPropertyOrFieldValue(GetPropertyOrFieldValueInternal(obj, propertyOrFieldName.Split('.').First()), string.Join(".", propertyOrFieldName.Split('.').Skip(1))) :
                GetPropertyOrFieldValueInternal(obj, propertyOrFieldName);
        }

        private static object? GetPropertyOrFieldValueInternal(object? obj, string propertyOrFieldName)
        {
            var propertyInfo = obj?.GetType().GetTypeInfo().GetDeclaredProperty(propertyOrFieldName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj);
            }

            var fieldInfo = obj?.GetType().GetTypeInfo().GetDeclaredField(propertyOrFieldName);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(obj);
            }

            return null;
        }
    }
}