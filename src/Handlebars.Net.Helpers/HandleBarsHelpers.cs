using System;
using System.Collections.Generic;
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
        private static readonly IHelper[] Helpers =
        {
            new ArrayHelper(),
            new MathHelper(),
            new RegexHelper(), 
            new StringHelper()
        };

        public static void Register(IHandlebars handlebarsContext)
        {
            foreach (var helper in Helpers)
            {
                Type classType = helper.GetType();

                foreach (var methodInfo in classType.GetMethods())
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

        private static void RegisterHelper(IHandlebars handlebarsContext, object obj, WriterType type, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, context, arguments) =>
            {
                object value = InvokeMethod(name, methodInfo, arguments, obj);

                switch (type)
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