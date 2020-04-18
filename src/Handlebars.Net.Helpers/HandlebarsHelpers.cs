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
    /// <summary>
    /// Extension class to Register additional Helpers and BlockHelpers.
    /// </summary>
    public static class HandlebarsHelpers
    {
        private static readonly IDictionary<Category, IHelpers> Helpers = new Dictionary<Category, IHelpers>
        {
            { Category.Constants, new ConstantsHelpers() },
            { Category.Enumerable, new EnumerableHelpers() },
            { Category.Math, new MathHelpers() },
            { Category.Regex, new RegexHelpers() },
            { Category.String, new StringHelpers() }
        };

        /// <summary>
        /// Register all (default) or specific categories.
        /// </summary>
        /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
        /// <param name="categories">The categories to register. By default all categories are registered. See the WIKI for details.</param>
        public static void Register(IHandlebars handlebarsContext, params Category[] categories)
        {
            Register(handlebarsContext, true, categories);
        }

        /// <summary>
        /// Register all (default) or specific categories and use the prefix from the categories.
        /// </summary>
        /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
        /// <param name="useCategoryPrefix">Set to false if you don't want to add the prefix from the category to the helper name. (Default is set to true).</param>
        /// <param name="categories">The categories to register. By default all categories are registered. See the WIKI for details.</param>
        public static void Register(IHandlebars handlebarsContext, bool useCategoryPrefix, params Category[] categories)
        {
            Register(handlebarsContext, useCategoryPrefix, null, categories);
        }

        /// <summary>
        /// Register all (default) or specific categories and use the prefix from the categories.
        /// </summary>
        /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
        /// <param name="prefix">Define a custom prefix which will be added in front of the name.</param>
        /// <param name="categories">The categories to register. By default all categories are registered. See the WIKI for details.</param>
        public static void Register(IHandlebars handlebarsContext, string? prefix = null, params Category[] categories)
        {
            Register(handlebarsContext, true, prefix, categories);
        }

        /// <summary>
        /// Register all (default) or specific categories and use the prefix from the categories.
        /// </summary>
        /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
        /// <param name="useCategoryPrefix">Set to false if you don't want to add the prefix from the category to the helper name. (Default is set to true).</param>
        /// <param name="prefix">Define a custom prefix which will be added in front of the name.</param>
        /// <param name="categories">The categories to register. By default all categories are registered. See the WIKI for details.</param>
        public static void Register(IHandlebars handlebarsContext, bool useCategoryPrefix = true, string? prefix = null, params Category[] categories)
        {
            foreach (var item in Helpers.Where(h => categories == null || categories.Length == 0 || categories.Contains(h.Key)))
            {
                var helper = item.Value;
                Type helperClassType = helper.GetType();

                foreach (var methodInfo in helperClassType.GetMethods())
                {
                    var attribute = methodInfo.GetCustomAttribute<HandlebarsWriterAttribute>();
                    if (attribute != null)
                    {
                        var names = new List<string>();
                        if (attribute.Name is { } && !string.IsNullOrWhiteSpace(attribute.Name))
                        {
                            names.Add(attribute.Name);
                        }
                        else
                        {
                            if (prefix is { } && !string.IsNullOrWhiteSpace(prefix))
                            {
                                names.Add(prefix);
                            }

                            if (useCategoryPrefix)
                            {
                                names.Add(item.Key.ToString());
                            }

                            names.Add(methodInfo.Name);
                        }

                        string name = string.Join(".", names);

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