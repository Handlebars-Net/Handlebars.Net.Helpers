﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Parsers;
using HandlebarsDotNet.Helpers.Plugin;
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
                { Category.Regex, new RegexHelpers(handlebarsContext) },
                { Category.Constants, new ConstantsHelpers(handlebarsContext) },
                { Category.Enumerable, new EnumerableHelpers(handlebarsContext) },
                { Category.Math, new MathHelpers(handlebarsContext) },
                { Category.String, new StringHelpers(handlebarsContext) },
                { Category.Url, new UrlHelpers(handlebarsContext) },
                { Category.DateTime, new DateTimeHelpers(handlebarsContext, options.DateTimeService ?? new DateTimeService()) }
            };

            var extra = new Dictionary<Category, string>
            {
                { Category.XPath, "XPathHelpers" },
                { Category.Xeger, "XegerHelpers" },
                { Category.Random, "RandomHelpers" },
                { Category.JsonPath, "JsonPathHelpers" },
                { Category.DynamicLinq, "DynamicLinqHelpers" }
            };

            var paths = options.CustomHelperPaths ?? new List<string> { Directory.GetCurrentDirectory() };
            var extraHelpers = PluginLoader.Load(paths, extra, handlebarsContext);

            foreach (var item in extraHelpers)
            {
                helpers.Add(item.Key, item.Value);
            }

            // https://github.com/Handlebars-Net/Handlebars.Net#relaxedhelpernaming
            handlebarsContext.Configuration.Compatibility.RelaxedHelperNaming = options.PrefixSeparatorIsDot;

            foreach (var item in helpers.Where(h => options.Categories == null || options.Categories.Length == 0 || options.Categories.Contains(h.Key)))
            {
                RegisterCustomHelper(handlebarsContext, options, item.Key.ToString(), item.Value);
            }

            if (options.CustomHelpers is { })
            {
                foreach (var item in options.CustomHelpers)
                {
                    RegisterCustomHelper(handlebarsContext, options, item.Key, item.Value);
                }
            }
        }

        private static void RegisterCustomHelper(IHandlebars handlebarsContext, HandlebarsHelpersOptions options, string categoryPrefix, IHelpers helper)
        {
            Type helperClassType = helper.GetType();

            var methods = helperClassType.GetMethods()
                .Select(methodInfo => new
                {
                    MethodInfo = methodInfo,
                    HandlebarsWriterAttribute = methodInfo.GetCustomAttribute<HandlebarsWriterAttribute>()
                })
                .Where(x => x.HandlebarsWriterAttribute is { });

            foreach (var method in methods)
            {
                var name = GetName(method.MethodInfo, method.HandlebarsWriterAttribute, options, categoryPrefix);

                switch (method.HandlebarsWriterAttribute.Usage)
                {
                    case HelperUsage.Normal:
                        RegisterHelper(handlebarsContext, helper, method.HandlebarsWriterAttribute, method.MethodInfo, name);
                        break;

                    case HelperUsage.Block:
                        RegisterBlockHelper(true, handlebarsContext, helper, method.MethodInfo, name);
                        break;

                    default:
                        RegisterHelper(handlebarsContext, helper, method.HandlebarsWriterAttribute, method.MethodInfo, name);
                        RegisterBlockHelper(false, handlebarsContext, helper, method.MethodInfo, name);
                        break;
                }
            }
        }

        private static string GetName(MethodInfo methodInfo, HandlebarsWriterAttribute attribute, HandlebarsHelpersOptions options, string categoryPrefix)
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
                    names.Add(categoryPrefix);
                }

                names.Add(methodInfo.Name);
            }

            return string.Join(options.PrefixSeparator, names);
        }

        private static void RegisterHelper(IHandlebars handlebarsContext, object instance, HandlebarsWriterAttribute attribute, MethodInfo methodInfo, string name)
        {
            foreach (string helperName in CreateHelperNames(name))
            {
                switch (attribute.Type)
                {
                    case WriterType.String:
                        RegisterStringHelper(handlebarsContext, instance, methodInfo, helperName, attribute.PassContext);
                        break;

                    case WriterType.Value:
                        RegisterValueHelper(handlebarsContext, instance, methodInfo, helperName, attribute.PassContext);
                        break;

                    default:
                        break;
                }
            }
        }

        private static void RegisterStringHelper(IHandlebars handlebarsContext, object instance, MethodInfo methodInfo, string helperName, bool passContext)
        {
            handlebarsContext.RegisterHelper(helperName, (writer, context, arguments) =>
            {
                object? value = InvokeMethod(passContext ? context : null, false, handlebarsContext, helperName, methodInfo, arguments, instance);

                if (value is IEnumerable<object> array)
                {
                    writer.WriteSafeString(ArrayUtils.ToArray(array));
                }
                else
                {
                    writer.WriteSafeString(value);
                }
            });
        }

        private static void RegisterValueHelper(IHandlebars handlebarsContext, object instance, MethodInfo methodInfo, string helperName, bool passContext)
        {
            handlebarsContext.RegisterHelper(helperName, (context, arguments) =>
            {
                return InvokeMethod(passContext ? context : null, false, handlebarsContext, helperName, methodInfo, arguments, instance);
            });
        }

        private static void RegisterBlockHelper(bool methodIsOnlyUsedInContextOfABlockHelper, IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, options, context, arguments) =>
            {
                object? value = InvokeMethod(null, methodIsOnlyUsedInContextOfABlockHelper, handlebarsContext, name, methodInfo, arguments, obj);

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

        private static object? InvokeMethod(
            Context? model,
            bool methodIsOnlyUsedInContextOfABlockHelper,
            IHandlebars context,
            string helperName,
            MethodInfo methodInfo,
            Arguments arguments,
            object instance)
        {
            int numberOfArguments = arguments.Length;
            int parameterCountRequired = methodInfo.GetParameters().Count(pi => !pi.IsOptional);

            if (model is { })
            {
                numberOfArguments += 1;
            }

            if (methodIsOnlyUsedInContextOfABlockHelper)
            {
                numberOfArguments += 1;
            }

            int parameterCountOptional = methodInfo.GetParameters().Count(pi => pi.IsOptional);
            int[] parameterCountAllowed = Enumerable.Range(parameterCountRequired, parameterCountOptional + 1).ToArray();

            if (parameterCountRequired == 0 && parameterCountOptional == 0 && numberOfArguments != 0)
            {
                throw new HandlebarsException($"The {helperName} helper should have no arguments.");
            }
            if (parameterCountAllowed.Length == 1 && numberOfArguments != parameterCountAllowed[0])
            {
                throw new HandlebarsException($"The {helperName} helper must have exactly {parameterCountAllowed[0]} argument{(parameterCountAllowed[0] > 1 ? "s" : "")}.");
            }
            if (!parameterCountAllowed.Contains(numberOfArguments))
            {
                throw new HandlebarsException($"The {helperName} helper must have {string.Join(" or ", parameterCountAllowed)} arguments.");
            }

            var parsedArguments = ArgumentsParser.Parse(context, arguments);

            // Add null for optional arguments
            for (int i = 0; i < parameterCountAllowed.Max() - numberOfArguments; i++)
            {
                parsedArguments.Add(null);
            }

            if (methodIsOnlyUsedInContextOfABlockHelper)
            {
                parsedArguments.Insert(0, true);
            }

            if (model is { })
            {
                parsedArguments.Insert(0, model);
            }

            try
            {
                return methodInfo.Invoke(instance, parsedArguments.ToArray());
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

        /// <summary>
        /// Return also a helperName surrounded by [ ] to support the official handlebarsjs rules
        ///
        /// See https://github.com/StefH/Handlebars.Net.Helpers/issues/7
        /// </summary>
        private static string[] CreateHelperNames(string helperName)
        {
            return new[] { helperName, $"[{helperName}]" };
        }
    }
}