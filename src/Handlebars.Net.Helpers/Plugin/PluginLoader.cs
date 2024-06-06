using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

namespace HandlebarsDotNet.Helpers.Plugin;

internal static class PluginLoader
{
    public static IDictionary<Category, IHelpers> Load(IEnumerable<string> paths, IDictionary<Category, string> items, params object[] args)
    {
        var helpers = new Dictionary<Category, IHelpers>();

        var pluginTypes = new List<Type>();
        try
        {
            foreach (var file in paths.Distinct().SelectMany(path => Directory.GetFiles(path, "*.dll")).Distinct())
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName
                    {
                        Name = Path.GetFileNameWithoutExtension(file)
                    });

                    pluginTypes.AddRange(GetImplementationTypeByInterface(assembly));
                }
                catch
                {
                    // Just try next .dll
                }
            }
        }
        catch
        {
            // File system access possibly denied, don't search for any more files
            return helpers;
        }


        foreach (var item in items)
        {
            var matchingType = pluginTypes.FirstOrDefault(pt => pt.Name == item.Value);
            if (matchingType is not null)
            {
                helpers.Add(item.Key, (IHelpers)Activator.CreateInstance(matchingType, args)!);
            }
        }

        return helpers;
    }

    private static IEnumerable<Type> GetImplementationTypeByInterface(Assembly assembly)
    {
        return assembly.GetTypes().Where(t => typeof(IHelpers).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface);
    }
}