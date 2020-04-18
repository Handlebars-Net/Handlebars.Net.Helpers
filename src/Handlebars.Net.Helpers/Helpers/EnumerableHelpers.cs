using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Utils;

namespace HandlebarsDotNet.Helpers.Helpers
{
    /// <summary>
    /// Some ideas based on https://github.com/helpers/handlebars-helpers#array
    /// </summary>
    internal class EnumerableHelpers : IHelpers
    {
        [HandlebarsWriter(WriterType.Write)]
        public bool IsEmpty(IEnumerable<object> value)
        {
            return value == null || !value.Any();
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object> Skip(IEnumerable<object> value, int after)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Skip(after);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Sum(IEnumerable<object> values)
        {
            return ExecuteUtils.Execute(values, x => x.Sum());
        }
    }
}