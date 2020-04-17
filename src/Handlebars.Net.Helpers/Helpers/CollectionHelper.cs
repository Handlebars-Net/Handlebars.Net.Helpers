using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    internal class CollectionHelper : IHelper
    {
        [HandlebarsWriter(WriterType.Write)]
        public bool IsEmpty(IEnumerable<object> value)
        {
            return value == null || !value.Any();
        }
    }
}