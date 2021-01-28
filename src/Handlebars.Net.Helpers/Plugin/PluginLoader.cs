using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;

namespace HandlebarsDotNet.Helpers.Plugin
{
    internal static class PluginLoader
    {
        private static readonly ConcurrentDictionary<string, Type> Assemblies = new ConcurrentDictionary<string, Type>();

        public static IHelpers Load(Category category, string name, params object[] args)
        {
            string key = $"{typeof(IHelpers)}_{category}_{name}";
            var foundType = Assemblies.GetOrAdd(key, (type) =>
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");

                Type? pluginType = null;
                foreach (var file in files)
                {
                    try
                    {
                        var assembly = Assembly.Load(new AssemblyName
                        {
                            Name = Path.GetFileNameWithoutExtension(file)
                        });

                        pluginType = GetImplementationTypeByInterfaceAndName(assembly, name);
                        if (pluginType != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        // no-op: just try next .dll
                    }
                }

                if (pluginType != null)
                {
                    return pluginType;
                }

                throw new DllNotFoundException($"No dll found which implements type '{type}'");
            });

            return (IHelpers)Activator.CreateInstance(foundType, args);
        }

        private static Type GetImplementationTypeByInterfaceAndName(Assembly assembly, string name)
        {
            return assembly.GetTypes().FirstOrDefault(t => typeof(IHelpers).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface && t.Name == name);
        }
    }
}