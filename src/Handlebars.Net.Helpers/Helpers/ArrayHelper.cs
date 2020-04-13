using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    /// <summary>
    /// Some ideas based on https://github.com/helpers/handlebars-helpers#array
    /// </summary>
    internal class ArrayHelper : IHelper
    {
        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object> Skip(IEnumerable<object> value, int after)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Skip(after);
        }
    }
}