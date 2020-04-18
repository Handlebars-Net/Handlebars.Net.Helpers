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
            int parameterCountRequired = methodInfo.GetParameters().Count(pi => !pi.IsOptional);
            int parameterCountOptional = methodInfo.GetParameters().Count(pi => pi.IsOptional);
            int[] parameterCountAllowed = Enumerable.Range(parameterCountRequired, parameterCountOptional + 1).ToArray();

            if (parameterCountRequired == 0 && parameterCountOptional == 0 && arguments.Length != 0)
            {
                throw new HandlebarsException($"The {name} helper should have no arguments.");
            }
            if (parameterCountAllowed.Length == 1 && arguments.Length != parameterCountAllowed[0])
            {
                throw new HandlebarsException($"The {name} helper must have exactly {parameterCountAllowed[0]} argument{(parameterCountAllowed[0] > 1 ? "s" : "")}.");
            }
            if (!parameterCountAllowed.Contains(arguments.Length))
            {
                throw new HandlebarsException($"The {name} helper must have {string.Join(" or ", parameterCountAllowed)} arguments.");
            }

            var parsedArguments = ArgumentsParser.Parse(arguments);

            // Add null for optional arguments
            for (int i = 0; i < parameterCountAllowed.Max() - arguments.Length; i++)
            {
                parsedArguments.Add(null);
            }

            try
            {
                return methodInfo.Invoke(obj, parsedArguments.ToArray());
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