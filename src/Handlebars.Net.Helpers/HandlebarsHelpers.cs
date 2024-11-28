using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Extensions;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Parsers;
using HandlebarsDotNet.Helpers.Plugin;
using HandlebarsDotNet.Helpers.Utils;
using Stef.Validation;
using HandlebarsDotNet.Helpers.Models;
using System.Diagnostics;
using HandlebarsDotNet.Helpers.Compatibility;


#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NET6_0_OR_GREATER
using System.Threading;
#else
using HandlebarsDotNet.Polyfills;
#endif

namespace HandlebarsDotNet.Helpers;

/// <summary>
/// Extension class to Register additional Helpers and BlockHelpers.
/// </summary>
public static class HandlebarsHelpers
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1
    /// </summary>
    internal static AsyncLocal<EvaluateResult> AsyncLocalResultFromEvaluate = new();

    /// <summary>
    /// Register all (default) categories. See the WIKI for details.
    /// </summary>
    /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
    public static void Register(IHandlebars handlebarsContext)
    {
        Register(handlebarsContext, o => { });
    }

    /// <summary>
    /// Register all (default) or specific categories.
    /// </summary>
    /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
    /// <param name="categories">The categories to register. By default, all categories are registered. See the WIKI for details.</param>
    public static void Register(IHandlebars handlebarsContext, params Category[] categories)
    {
        Register(handlebarsContext, o => { o.Categories = categories; });
    }

    /// <summary>
    /// Register specific helpers.
    /// </summary>
    /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
    /// <param name="helpers">The helpers to register.</param>
    public static void Register(IHandlebars handlebarsContext, params IHelpers[] helpers)
    {
        Register(handlebarsContext, o => { o.Helpers = helpers; });
    }

    /// <summary>
    /// Register all (default) or specific categories and use the prefix from the categories.
    /// </summary>
    /// <param name="handlebarsContext">The <see cref="IHandlebars"/>-context.</param>
    /// <param name="optionsCallback">The options callback.</param>
    public static void Register(IHandlebars handlebarsContext, Action<HandlebarsHelpersOptions> optionsCallback)
    {
        Guard.NotNull(optionsCallback);

        var options = new HandlebarsHelpersOptions();
        optionsCallback(options);

        var helpers = new Dictionary<Category, IHelpers>
        {
            { Category.Regex, new RegexHelpers(handlebarsContext) },
            { Category.Constants, new ConstantsHelpers(handlebarsContext) },
            { Category.Enumerable, new EnumerableHelpers(handlebarsContext) },
            { Category.Environment, new EnvironmentHelpers(handlebarsContext) },
            { Category.Math, new MathHelpers(handlebarsContext) },
            { Category.String, new StringHelpers(handlebarsContext) },
            { Category.Url, new UrlHelpers(handlebarsContext) },
            { Category.DateTime, new DateTimeHelpers(handlebarsContext, options.DateTimeService ?? new DateTimeService()) },
            { Category.Boolean, new BooleanHelpers(handlebarsContext) },
            { Category.Object, new ObjectHelpers(handlebarsContext) }
        };

        var dynamicLoadedHelpers = new Dictionary<Category, string>
        {
            { Category.XPath, "XPathHelpers" },
            { Category.Xeger, "XegerHelpers" },
            { Category.Random, "RandomHelpers" },
            { Category.JsonPath, "JsonPathHelpers" },
            { Category.DynamicLinq, "DynamicLinqHelpers" },
            { Category.Humanizer, "HumanizerHelpers" },
            { Category.Xslt, "XsltHelpers" }
        };

        List<string> paths;
        if (options.CustomHelperPaths != null)
        {
            paths = options.CustomHelperPaths.ToList();
        }
        else
        {
            paths = new List<string>
            {
                Directory.GetCurrentDirectory(),
                AppContextHelper.GetBaseDirectory(),
            };

#if !NETSTANDARD1_3_OR_GREATER
            void Add(string? path, ICollection<string> customHelperPaths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    customHelperPaths.Add(path!);
                }
            }
            Add(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location), paths);
            Add(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), paths);
            Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), paths);

            if (!RuntimeInformationUtils.IsBlazorWASM)
            {
                Add(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName), paths);
            }
#endif
        }

        var additionalLoadedHelpers = PluginLoader.Load(paths, dynamicLoadedHelpers, handlebarsContext);

        foreach (var item in additionalLoadedHelpers)
        {
            helpers.Add(item.Key, item.Value);
        }

        if (options.Helpers != null)
        {
            foreach (var helper in options.Helpers)
            {
                if (!helpers.ContainsKey(helper.Category))
                {
                    helpers.Add(helper.Category, helper);
                }
            }
        }

        // https://github.com/Handlebars-Net/Handlebars.Net#relaxedhelpernaming
        handlebarsContext.Configuration.Compatibility.RelaxedHelperNaming = options.PrefixSeparatorIsDot;

        foreach (var item in helpers.Where(h => options.Categories == null || options.Categories.Length == 0 || options.Categories.Contains(h.Key)))
        {
            RegisterCustomHelper(handlebarsContext, options, item.Key.ToString(), item.Value);
        }

        if (options.CustomHelpers != null)
        {
            foreach (var item in options.CustomHelpers)
            {
                RegisterCustomHelper(handlebarsContext, options, item.Key, item.Value);
            }
        }

        RegisterEvaluateHelper(handlebarsContext);
    }

    private static void RegisterEvaluateHelper(IHandlebars handlebarsContext)
    {
        var helper = new EvaluateHelper(AsyncLocalResultFromEvaluate);
        handlebarsContext.RegisterHelper(helper);
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
            .Where(x => x.HandlebarsWriterAttribute != null)
            .ToArray();

        foreach (var method in methods)
        {
            var name = GetName(method.MethodInfo, method.HandlebarsWriterAttribute!, options, categoryPrefix);

            switch (method.HandlebarsWriterAttribute!.Usage)
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
        foreach (var helperName in CreateHelperNames(name))
        {
            switch (attribute.Type)
            {
                case WriterType.String:
                    RegisterStringHelper(handlebarsContext, instance, methodInfo, helperName, attribute.PassContext);
                    break;

                case WriterType.Value:
                    RegisterValueHelper(handlebarsContext, instance, methodInfo, helperName, attribute.PassContext);
                    break;

                // ReSharper disable once RedundantEmptySwitchSection
                default:
                    break;
            }
        }
    }

    private static void RegisterStringHelper(IHandlebars handlebarsContext, object instance, MethodInfo methodInfo, string helperName, bool passContext)
    {
        HandlebarsHelperWithOptions helper = (in EncodedTextWriter writer, in HelperOptions options, in Context context, in Arguments arguments) =>
        {
            object? value = InvokeMethod(passContext ? context : null, false, handlebarsContext, helperName, methodInfo, arguments, instance, options);

            if (value is IEnumerable<object> array)
            {
                writer.WriteSafeString(ArrayUtils.ToArray(array));
            }
            else
            {
                writer.WriteSafeString(value);
            }
        };

        handlebarsContext.RegisterHelper(helperName, helper);
    }

    private static void RegisterValueHelper(IHandlebars handlebarsContext, object instance, MethodInfo methodInfo, string helperName, bool passContext)
    {
        HandlebarsReturnWithOptionsHelper helper = (in HelperOptions options, in Context context, in Arguments arguments) =>
        {
            return InvokeMethod(passContext ? context : null, false, handlebarsContext, helperName, methodInfo, arguments, instance, options);
        };

        handlebarsContext.RegisterHelper(helperName, helper);
    }

    private static void RegisterBlockHelper(bool methodIsOnlyUsedInContextOfABlockHelper, IHandlebars handlebarsContext, object obj, MethodInfo methodInfo, string name)
    {
        HandlebarsBlockHelper helper = (writer, options, context, arguments) =>
        {
            var value = InvokeMethod(null, methodIsOnlyUsedInContextOfABlockHelper, handlebarsContext, name, methodInfo, arguments, obj, options);

            if (value is bool valueAsBool && !valueAsBool)
            {
                // If it's a boolean value, and if this is 'False', execute the Inverse.
                options.Inverse(writer, value);
            }
            else
            {
                options.Template(writer, value);
            }
        };

        handlebarsContext.RegisterHelper(name, helper);
    }

    private static object? InvokeMethod(
        Context? model,
        bool methodIsOnlyUsedInContextOfABlockHelper,
        IHandlebars context,
        string helperName,
        MethodInfo methodInfo,
        Arguments arguments,
        object instance,
        IHelperOptions options
    )
    {
        var numberOfArguments = arguments.Length;
        var lastIsParam = methodInfo.LastParameterIsParam();
        var firstIsHelperOptions = methodInfo.FirstParameterIsHelperOptions();
        var parameterCountRequired = methodInfo.GetParameters().Count(pi => !pi.IsOptional && !pi.IsParam());

        if (firstIsHelperOptions)
        {
            parameterCountRequired--;
        }

        if (model is { })
        {
            numberOfArguments += 1;
        }

        if (methodIsOnlyUsedInContextOfABlockHelper)
        {
            numberOfArguments += 1;
        }

        var parameterCountOptional = methodInfo.GetParameters().Count(pi => pi.IsOptional);
        var parameterCountAllowed = Enumerable.Range(parameterCountRequired, parameterCountOptional + 1).ToArray();

        if (lastIsParam)
        {
            if (parameterCountRequired > 0 && numberOfArguments < parameterCountRequired)
            {
                throw new HandlebarsException($"The {helperName} helper should have at least {parameterCountRequired} argument{(parameterCountRequired > 1 ? "s" : "")}.");
            }
        }
        else
        {
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
        }

        var parsedArguments = ArgumentsParser.Parse(context, methodInfo.GetParameters(), arguments);

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
            if (firstIsHelperOptions)
            {
                parsedArguments.Insert(0, options);
            }

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