using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using HandlebarsDotNet.Helpers.Utils;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers
{
    internal class DynamicLinqHelpers : BaseHelpers, IHelpers
    {
        public DynamicLinqHelpers(IHandlebars context) : base(context)
        {
        }

        /// <summary>
        /// For backwards compatibility with WireMock.Net
        /// </summary>
        [HandlebarsWriter(WriterType.Value, "Linq")]
        public object Linq(object value, string linqStatement)
        {
            var valueToProcess = ParseAsJToken(value);

            try
            {
                return ExecuteDynamicLinq(valueToProcess, linqStatement);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static dynamic ExecuteDynamicLinq(JToken value, string linqStatement)
        {
            // Convert a single object to a Queryable JObject-list with 1 entry.
            var queryable1 = new[] { value }.AsQueryable();

            // Generate the DynamicLinq select statement.
            string dynamicSelect = JsonUtils.GenerateDynamicLinqStatement(value);

            // Execute DynamicLinq Select statement.
            var queryable2 = queryable1.Select(dynamicSelect);

            // Execute the Select(...) method and get first result with FirstOrDefault().
            return queryable2.Select(linqStatement).FirstOrDefault();
        }

        private static JToken ParseAsJToken(object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            switch (value)
            {
                case string valueAsString:
                    return new JValue(valueAsString);

                case JToken valueAsJToken:
                    return valueAsJToken;

                default:
                    throw new NotSupportedException($"The value '{value}' with type '{value?.GetType()}' cannot be used in Handlebars Linq.");
            }
        }
    }
}