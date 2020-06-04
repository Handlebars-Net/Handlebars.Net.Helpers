using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Extensions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Parsers;
using HandlebarsDotNet.Helpers.Utils;
using HandlebarsDotNet.Helpers.Validation;

namespace HandlebarsDotNet.Helpers
{
    /// <summary>
    /// Extension class to Register additional Helpers and BlockHelpers.
    /// </summary>
    public static class HandlebarsHelpers
    {
        /// <summary>
        /// Register all (default) or specific categories.
        /// </summary>
        /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
        /// <param name="categories">The categories to register. By default all categories are registered. See the WIKI for details.</param>
        public static void Register(IHandlebars handlebarsContext, params Category[] categories)
        {
            Register(handlebarsContext, o => { o.Categories = categories; });
        }

        /// <summary>
        /// Register all (default) or specific categories and use the prefix from the categories.
        /// </summary>
        /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
        /// <param name="optionsCallback">The options callback.</param>
        public static void Register(IHandlebars handlebarsContext, Action<HandlebarsHelpersOptions> optionsCallback)
        {
            Guard.NotNull(optionsCallback, nameof(optionsCallback));

            var options = new HandlebarsHelpersOptions();
            optionsCallback(options);

            var helpers = new Dictionary<Category, IHelpers>
            {
                { Category.Constants, new ConstantsHelpers(options) },
                { Category.Enumerable, new EnumerableHelpers(options) },
                { Category.Math, new MathHelpers(options) },
                { Category.Regex, new RegexHelpers(options) },
                { Category.String, new StringHelpers(options) },
                { Category.Url, new UrlHelpers(options) }
            };

            foreach (var item in helpers.Where(h => options.Categories == null || options.Categories.Length == 0 || options.Categories.Contains(h.Key)))
            {
                var helper = item.Value;
                Type helperClassType = helper.GetType();

                var methods = helperClassType.GetMethods()
                    .Select(methodInfo => (methodInfo, methodInfo.GetCustomAttribute<HandlebarsWriterAttribute>()))
                    .Where(x => x.Item2 is { });

                foreach (var x in methods)
                {
                    var name = GetName(x, options, item);

                    RegisterHelper(options, handlebarsContext, helper, x.Item2.Type, x.methodInfo, name);
                    RegisterBlockHelper(options, handlebarsContext, helper, x.methodInfo, name);
                }
            }
        }

        private static string GetName((MethodInfo methodInfo, HandlebarsWriterAttribute attribute) x, HandlebarsHelpersOptions options, KeyValuePair<Category, IHelpers> item)
        {
            var names = new List<string>();
            if (x.attribute.Name is { } && !string.IsNullOrWhiteSpace(x.attribute.Name))
            {
                names.Add(x.attribute.Name);
            }
            else
            {
                if (options.Prefix is { } && !string.IsNullOrWhiteSpace(options.Prefix))
                {
                    names.Add(options.Prefix);
                }

                if (options.UseCategoryPrefix)
                {
                    names.Add(item.Key.ToString());
                }

                names.Add(x.methodInfo.Name);
            }

            string name = string.Join(".", names);
            return name;
        }

        private static void RegisterHelper(HandlebarsHelpersOptions helperOptions, IHandlebars handlebarsContext, object obj, WriterType writerType, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, context, arguments) =>
            {
                object value = InvokeMethod(helperOptions, name, methodInfo, arguments, obj);

                switch (writerType)
                {
                    case WriterType.WriteSafeString:
                        writer.WriteSafeString(value, helperOptions);
                        break;

                    default:
                        if (value is IEnumerable<object> array)
                        {
                            writer.WriteSafeString(ArrayUtils.ToArray(array), helperOptions);
                        }
                        else
                        {
                            writer.Write(value, helperOptions);
                        }
                        break;
                }
            });
        }

        private static void RegisterBlockHelper(HandlebarsHelpersOptions helperOptions, IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, options, context, arguments) =>
            {
                object value = InvokeMethod(helperOptions, name, methodInfo, arguments, obj);

                if (value is bool valueAsBool && !valueAsBool)
                {
                    // If it's a boolean value, and if this is 'False', execute the Inverse.
                    options.Inverse(writer, value);
                }
                else
                {
                    options.Template(writer, value);
                }
            });
        }

        private static object InvokeMethod(HandlebarsHelpersOptions options, string name, MethodInfo methodInfo, object[] arguments, object obj)
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

            var parsedArguments = ArgumentsParser.Parse(options, arguments);

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