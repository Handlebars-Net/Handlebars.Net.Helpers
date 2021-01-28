using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Helpers;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace HandlebarsDotNet.Helpers
{
    internal class RandomHelpers : BaseHelpers, IHelpers
    {
        public RandomHelpers(IHandlebars context) : base(context)
        {
        }

        [HandlebarsWriter(WriterType.Value)]
        public object? Random(IDictionary<string, object?> hash)
        {
            var fieldOptions = GetFieldOptionsFromHash(hash);
            dynamic randomizer = RandomizerFactory.GetRandomizerAsDynamic(fieldOptions);

            // Format DateTime as ISO 8601
            if (fieldOptions is IFieldOptionsDateTime)
            {
                DateTime? date = randomizer.Generate();
                return date?.ToString("s", Context.Configuration.FormatProvider);
            }

            // If the IFieldOptionsGuid defines Uppercase, use the 'GenerateAsString' method.
            if (fieldOptions is IFieldOptionsGuid fieldOptionsGuid)
            {
                return fieldOptionsGuid.Uppercase ? randomizer.GenerateAsString() : randomizer.Generate();
            }

            return randomizer.Generate();
        }

        private static FieldOptionsAbstract GetFieldOptionsFromHash(IDictionary<string, object?> hash)
        {
            if (hash.TryGetValue("Type", out var value) && value is string randomType)
            {
                var newProperties = new Dictionary<string, object>();
                foreach (var item in hash.Where(p => p.Key != "Type"))
                {
                    if (item.Value is { })
                    {
                        newProperties.Add(item.Key, item.Value);
                    }
                }

                return FieldOptionsFactory.GetFieldOptions(randomType, newProperties);
            }
            
            throw new HandlebarsException($"The Type argument is missing.");
        }
    }
}