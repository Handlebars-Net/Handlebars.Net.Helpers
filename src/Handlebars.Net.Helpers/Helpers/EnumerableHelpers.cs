using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;
using HandlebarsDotNet.Helpers.Utils;
using HandlebarsDotNet.Helpers.Validation;

namespace HandlebarsDotNet.Helpers.Helpers
{
    /// <summary>
    /// Some ideas based on https://github.com/helpers/handlebars-helpers#array
    /// </summary>
    internal class EnumerableHelpers : BaseHelpers, IHelpers
    {
        [HandlebarsWriter(WriterType.Write)]
        public object Average(IEnumerable<object> values)
        {
            return ExecuteUtils.Execute(values, x => x.Average());
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object?> Distinct(IEnumerable<object?> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Distinct();
        }

        [HandlebarsWriter(WriterType.Write)]
        public bool IsFirst(IEnumerable<object?> values, object? value)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.FirstOrDefault() == value;
        }

        [HandlebarsWriter(WriterType.Write)]
        public bool IsLast(IEnumerable<object?> values, object? value)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.LastOrDefault() == value;
        }

        [HandlebarsWriter(WriterType.Write)]
        public bool IsEmpty(IEnumerable<object?> value)
        {
            return value is null || !value.Any();
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Max(IEnumerable<object> values)
        {
            return ExecuteUtils.Execute(values, x => x.Max());
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Min(IEnumerable<object> values)
        {
            return ExecuteUtils.Execute(values, x => x.Min());
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object?> Page(IEnumerable<object?> values, int pageNumber, int numberOfObjectsPerPage = 0)
        {
            return values.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object?> Reverse(IEnumerable<object?> values)
        {
            return values.Reverse();
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object?> Select(IEnumerable<object?> values, string propertyName, bool skipNullValues = false)
        {
            Guard.NotNull(values, nameof(values));
            Guard.NotNullOrEmpty(propertyName, nameof(propertyName));

            foreach (var value in values)
            {
                var result = ReflectionUtils.GetPropertyOrFieldValue(value, propertyName);
                if (!skipNullValues || (skipNullValues && result != null))
                {
                    yield return result;
                }
            }
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object?> Skip(IEnumerable<object?> values, int count)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Skip(count);
        }

        [HandlebarsWriter(WriterType.Write)]
        public object Sum(IEnumerable<object> values)
        {
            return ExecuteUtils.Execute(values, x => x.Sum());
        }

        [HandlebarsWriter(WriterType.Write)]
        public IEnumerable<object?> Take(IEnumerable<object?> values, int count)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Take(count);
        }

        public EnumerableHelpers(HandlebarsHelpersOptions options) : base(options)
        {
        }
    }
}