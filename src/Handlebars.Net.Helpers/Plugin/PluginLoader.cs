using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

namespace HandlebarsDotNet.Helpers.Plugin
{
    internal static class PluginLoader
    {
        public static IDictionary<Category, IHelpers> Load(IEnumerable<string> paths, IDictionary<Category, string> items, params object[] args)
        {
            var pluginTypes = new List<Type>();
            foreach (var file in paths.SelectMany(path => Directory.GetFiles(path, "*.dll")))
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
                    // no-op: just try next .dll
                }
            }

            var helpers = new Dictionary<Category, IHelpers>();
            foreach (var item in items)
            {
                var matchingType = pluginTypes.FirstOrDefault(pt => pt.Name == item.Value);
                if (matchingType is { })
                {
                    helpers.Add(item.Key, (IHelpers)Activator.CreateInstance(matchingType, args));
                }
            }

            return helpers;
        }

        private static IEnumerable<Type> GetImplementationTypeByInterface(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(IHelpers).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface);
        }
    }
}