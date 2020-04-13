using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

namespace HandlebarsDotNet.Helpers
{
    public static class HandleBarsHelpers
    {
        private static readonly IHelper[] Helpers = 
        {
            new StringHelper(),
            new MathHelper()
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
                object value = methodInfo.Invoke(obj, FixArguments(arguments));

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
                object value = methodInfo.Invoke(obj, FixArguments(arguments));
                options.Template(writer, value);
            });
        }

        // Bug: Handlebars.Net does provide only strings
        private static object[] FixArguments(object[] arguments)
        {
            var list = new List<object>();
            foreach (var argument in arguments)
            {
                if (argument is string valueAsString)
                {
                    if (int.TryParse(valueAsString, out int valueAsInt))
                    {
                        list.Add(valueAsInt);
                    }
                    else if (long.TryParse(valueAsString, out long valueAsLong))
                    {
                        list.Add(valueAsLong);
                    }
                    else if (double.TryParse(valueAsString, out double valueAsDouble))
                    {
                        list.Add(valueAsDouble);
                    }
                    else
                    {
                        list.Add(valueAsString);
                    }
                }
                else
                {
                    list.Add(argument);
                }
            }

            return list.ToArray();
        }
    }
}