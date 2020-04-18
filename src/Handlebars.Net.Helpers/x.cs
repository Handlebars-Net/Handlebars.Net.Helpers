using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HandlebarsDotNet.Helpers
{
    class x
    {
        public interface IHandlebarsContextConverter
        {
            IDictionary<string, object> Convert(object @object);
        }

        public class HandlebarsContextConverterFields : IHandlebarsContextConverter
        {
            public IDictionary<string, object> Convert(object @object)
            {
                return @object.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .ToDictionary(x => x.Name, x => x.GetValue(@object));
            }
        }

        public class HandlebarsContextConverterProperties : IHandlebarsContextConverter
        {
            public IDictionary<string, object> Convert(object @object)
            {
                return @object.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .ToDictionary(x => x.Name, x => x.GetValue(@object, null));
            }
        }

        //var converter = new HandlebarsContextConverterMerge(
        //    new HandlebarsContextConverterFields(),
        //    new HandlebarsContextConverterProperties());
    }
}
