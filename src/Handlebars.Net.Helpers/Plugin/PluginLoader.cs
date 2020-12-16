using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HandlebarsDotNet.Helpers.Plugin
{
    internal static class PluginLoader
    {
        private static ConcurrentDictionary<Type, object> Assemblies = new ConcurrentDictionary<Type, object>();

        public static T? LoadAndCreateInstance<T>(string name, params object[] args) where T : class
        {
            return Assemblies.GetOrAdd(typeof(T), (type) =>
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

                        pluginType = GetImplementationTypeByInterfaceAndName<T>(assembly, name);
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
                    return Activator.CreateInstance(pluginType, args);
                }

                throw new DllNotFoundException($"No dll found which implements type '{type}'");
            }) as T;
        }

        private static Type GetImplementationTypeByInterfaceAndName<T>(Assembly assembly, string name)
        {
            return assembly.GetTypes().FirstOrDefault(t => typeof(T).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface && t.Name == name);
        }
    }
}