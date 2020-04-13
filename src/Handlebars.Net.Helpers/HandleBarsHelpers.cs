using System;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Parsers;

namespace HandlebarsDotNet.Helpers
{
    public static class HandleBarsHelpers
    {
        private static readonly IHelper[] Helpers =
        {
            new StringHelper(),
            new MathHelper(),
            new ArrayHelper()
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
                        RegisterHelperToWriteAValue(handlebarsContext, helper, attribute.Type, methodInfo, attribute.Name);
                    }
                }
            }
        }

        private static void RegisterHelperToWriteAValue(IHandlebars handlebarsContext, object obj, WriterType type, MethodInfo methodInfo, string methodName)
        {
            string name = methodName ?? methodInfo.Name;
            handlebarsContext.RegisterHelper(name, (writer, context, arguments) =>
            {
                object value = methodInfo.Invoke(obj, ArgumentsParser.Parse(arguments));

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

            RegisterHelperToWriteATemplate(handlebarsContext, obj, methodInfo, methodName);
        }

        private static void RegisterHelperToWriteATemplate(IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string methodName)
        {
            string name = methodName ?? methodInfo.Name;
            handlebarsContext.RegisterHelper(name, (writer, options, context, arguments) =>
            {
                object value = methodInfo.Invoke(obj, ArgumentsParser.Parse(arguments));
                options.Template(writer, value);
            });
        }
    }
}