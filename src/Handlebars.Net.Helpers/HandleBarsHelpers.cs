using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Parsers;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers
{
    public static class HandleBarsHelpers
    {
        private static readonly IDictionary<HelperType, IHelpers> Helpers = new Dictionary<HelperType, IHelpers>
        {
            { HelperType.Constants, new ConstantsHelpers() },
            { HelperType.Enumerable, new EnumerableHelpers() },
            { HelperType.Math, new MathHelpers() },
            { HelperType.Regex, new RegexHelpers() },
            { HelperType.String, new StringHelpers() }
        };

        public static void Register(IHandlebars handlebarsContext, params HelperType[] helpers)
        {
            foreach (var item in Helpers.Where(h => helpers == null || helpers.Length == 0 || helpers.Contains(h.Key)))
            {
                var helper = item.Value;
                Type helperClassType = helper.GetType();

                foreach (var methodInfo in helperClassType.GetMethods())
                {
                    var attribute = methodInfo.GetCustomAttribute<HandlebarsWriterAttribute>();
                    if (attribute != null)
                    {
                        string name = attribute.Name ?? methodInfo.Name;

                        RegisterHelper(handlebarsContext, helper, attribute.Type, methodInfo, name);
                        RegisterBlockHelper(handlebarsContext, helper, methodInfo, name);
                    }
                }
            }
        }

        private static void RegisterHelper(IHandlebars handlebarsContext, object obj, WriterType writerType, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, context, arguments) =>
            {
                object value = InvokeMethod(name, methodInfo, arguments, obj);

                switch (writerType)
                {
                    case WriterType.WriteSafeString:
                        writer.WriteSafeString(value);
                        break;

                    default:
                        if (value is IEnumerable<object> array)
                        {
                            writer.WriteSafeString(ArrayUtils.ToArray(array));
                        }
                        else
                        {
                            writer.Write(value);
                        }
                        break;
                }

            });
        }

        private static void RegisterBlockHelper(IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, options, context, arguments) =>
            {
                object value = InvokeMethod(name, methodInfo, arguments, obj);

                if (value is bool valueAsBool)
                {
                    if (valueAsBool)
                    {
                        options.Template(writer, value);
                    }
                    else
                    {
                        options.Inverse(writer, value);
                    }
                }
                else
                {
                    options.Template(writer, value);
                }
            });
        }

        private static object InvokeMethod(string name, MethodInfo methodInfo, object[] arguments, object obj)
        {
            int methodArgumentCount = methodInfo.GetParameters().Length;
            if (arguments.Length != methodArgumentCount)
            {
                throw new HandlebarsException($"The {name} helper must have exactly {methodArgumentCount} argument{(methodArgumentCount > 1 ? "s" : "")}.");
            }

            var parsedArguments = ArgumentsParser.Parse(arguments);

            try
            {
                return methodInfo.Invoke(obj, parsedArguments);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    throw e.InnerException;
                }

                throw;
            }
        }
    }
}