using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
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

            // https://github.com/Handlebars-Net/Handlebars.Net#relaxedhelpernaming
            handlebarsContext.Configuration.Compatibility.RelaxedHelperNaming = options.PrefixSeparatorIsDot;

            var helpers = new Dictionary<Category, IHelpers>
            {
                { Category.Constants, new ConstantsHelpers(handlebarsContext) },
                { Category.Enumerable, new EnumerableHelpers(handlebarsContext) },
                { Category.Math, new MathHelpers(handlebarsContext) },
                { Category.Regex, new RegexHelpers(handlebarsContext) },
                { Category.String, new StringHelpers(handlebarsContext) },
                { Category.Url, new UrlHelpers(handlebarsContext) },
                { Category.DateTime, new DateTimeHelpers(handlebarsContext, options.DateTimeService ?? new DateTimeService()) }
            };

            foreach (var item in helpers.Where(h => options.Categories == null || options.Categories.Length == 0 || options.Categories.Contains(h.Key)))
            {
                var helper = item.Value;
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
                    var name = GetName(method.MethodInfo, method.HandlebarsWriterAttribute, options, item);

                    RegisterHelper(handlebarsContext, helper, method.HandlebarsWriterAttribute.Type, method.MethodInfo, name);
                    RegisterBlockHelper(handlebarsContext, helper, method.MethodInfo, name);
                }
            }
        }

        private static string GetName(MethodInfo methodInfo, HandlebarsWriterAttribute attribute, HandlebarsHelpersOptions options, KeyValuePair<Category, IHelpers> item)
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

            return string.Join(options.PrefixSeparator, names);
        }

        private static void RegisterHelper(IHandlebars handlebarsContext, object obj, WriterType writerType, MethodInfo methodInfo, string name)
        {
            foreach (string helperName in CreateHelperNames(name))
            {
                switch (writerType)
                {
                    case WriterType.String:
                        RegisterStringHelper(handlebarsContext, obj, methodInfo, helperName);
                        break;

                    case WriterType.Value:
                        RegisterValueHelper(handlebarsContext, obj, methodInfo, helperName);
                        break;

                    default:
                        break;
                }
            }
        }

        private static void RegisterStringHelper(IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string helperName)
        {
            handlebarsContext.RegisterHelper(helperName, (writer, context, arguments) =>
            {
                object? value = InvokeMethod(handlebarsContext, helperName, methodInfo, arguments, obj);

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

        private static void RegisterValueHelper(IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string helperName)
        {
            handlebarsContext.RegisterHelper(helperName, (context, arguments) =>
            {
                return InvokeMethod(handlebarsContext, helperName, methodInfo, arguments, obj);
            });
        }

        private static void RegisterBlockHelper(IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string name)
        {
            handlebarsContext.RegisterHelper(name, (writer, options, context, arguments) =>
            {
                object? value = InvokeMethod(handlebarsContext, name, methodInfo, arguments, obj);

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

        private static object? InvokeMethod(IHandlebars context, string helperName, MethodInfo methodInfo, Arguments arguments, object instance)
        {
            int parameterCountRequired = methodInfo.GetParameters().Count(pi => !pi.IsOptional);
            int parameterCountOptional = methodInfo.GetParameters().Count(pi => pi.IsOptional);
            int[] parameterCountAllowed = Enumerable.Range(parameterCountRequired, parameterCountOptional + 1).ToArray();

            if (parameterCountRequired == 0 && parameterCountOptional == 0 && arguments.Length != 0)
            {
                throw new HandlebarsException($"The {helperName} helper should have no arguments.");
            }
            if (parameterCountAllowed.Length == 1 && arguments.Length != parameterCountAllowed[0])
            {
                throw new HandlebarsException($"The {helperName} helper must have exactly {parameterCountAllowed[0]} argument{(parameterCountAllowed[0] > 1 ? "s" : "")}.");
            }
            if (!parameterCountAllowed.Contains(arguments.Length))
            {
                throw new HandlebarsException($"The {helperName} helper must have {string.Join(" or ", parameterCountAllowed)} arguments.");
            }

            var parsedArguments = ArgumentsParser.Parse(context, arguments);

            // Add null for optional arguments
            for (int i = 0; i < parameterCountAllowed.Max() - arguments.Length; i++)
            {
                parsedArguments.Add(null);
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