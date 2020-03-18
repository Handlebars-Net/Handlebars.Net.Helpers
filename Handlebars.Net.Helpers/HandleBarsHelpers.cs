using System;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

namespace HandlebarsDotNet.Helpers
{
    public static class HandleBarsHelpers
    {
        public static void Register(IHandlebars handlebarsContext)
        {
            var helpers = new object[]
            {
                new StringHelper(),
                new MathHelper()
            };

            foreach (var helper in helpers)
            {
                Type classType = helper.GetType();

                foreach (var methodInfo in classType.GetMethods())
                {
                    var attribute = methodInfo.GetCustomAttribute<HandlebarsWriterAttribute>();
                    if (attribute != null)
                    {
                        Write(handlebarsContext, helper, attribute.Type, methodInfo, attribute.Name);
                    }
                }
            }
        }

        private static void Write(IHandlebars handlebarsContext, object obj, WriterType type, MethodInfo methodInfo, string methodName)
        {
            string name = methodName ?? methodInfo.Name;
            handlebarsContext.RegisterHelper(name, (writer, context, arguments) =>
            {
                object value = methodInfo.Invoke(obj, arguments);

                switch (type)
                {
                    case WriterType.WriteSafeString:
                        writer.WriteSafeString(value);
                        break;

                    default:
                        writer.Write(value);
                        break;
                }

            });

            WriteTemplate(handlebarsContext, obj, methodInfo, methodName);
        }

        private static void WriteTemplate(IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string methodName)
        {
            string name = methodName ?? methodInfo.Name;
            handlebarsContext.RegisterHelper(name, (writer, options, context, arguments) =>
            {
                object value = methodInfo.Invoke(obj, arguments);
                options.Template(writer, value);
            });
        }
    }
}