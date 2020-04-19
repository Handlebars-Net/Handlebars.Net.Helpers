﻿using System;
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
        private static readonly IDictionary<Category, IHelpers> Helpers = new Dictionary<Category, IHelpers>
        {
            { Category.Constants, new ConstantsHelpers() },
            { Category.Enumerable, new EnumerableHelpers() },
            { Category.Math, new MathHelpers() },
            { Category.Regex, new RegexHelpers() },
            { Category.String, new StringHelpers() },
            { Category.Url, new UrlHelpers() }
        };

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

            foreach (var item in Helpers.Where(h => options.Categories == null || options.Categories.Length == 0 || options.Categories.Contains(h.Key)))
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
                            if (options.Prefix is { } && !string.IsNullOrWhiteSpace(options.Prefix))
                            {
                                names.Add(options.Prefix);
                            }

                            if (options.UseCategoryPrefix)
                            {
                                names.Add(item.Key.ToString());
                            }

                            names.Add(methodInfo.Name);
                        }

                        string name = string.Join(".", names);

                        RegisterHelper(options, handlebarsContext, helper, attribute.Type, methodInfo, name);
                        RegisterBlockHelper(options, handlebarsContext, helper, methodInfo, name);
                    }
                }
            }
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

        private static object InvokeMethod(HandlebarsHelpersOptions helperOptions, string name, MethodInfo methodInfo, object[] arguments, object obj)
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

            var parsedArguments = ArgumentsParser.Parse(arguments, helperOptions);

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