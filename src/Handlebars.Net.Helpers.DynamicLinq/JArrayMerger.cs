using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Helpers;

internal static class JArrayMerger
{
    public static JArray MergeToCommonStructure(JArray originalArray)
    {
        if (originalArray.Count is 0 or 1)
        {
            // If array is empty, or has only one item, return as-is
            return originalArray;
        }

        // Create a merged template from all items
        var mergedTemplate = CreateMergedTemplate(originalArray);
        if (mergedTemplate == null)
        {
            return originalArray;
        }

        // Apply the merged template to all items
        var mergedItems = new List<JToken>();
        foreach (var item in originalArray)
        {
            var mergedItem = ApplyTemplate(item, mergedTemplate);
            mergedItems.Add(mergedItem);
        }

        var newArray = new JArray(mergedItems);

        // Check if the new array is the same as the original
        if (JToken.DeepEquals(originalArray, newArray))
        {
            return originalArray;
        }

        return newArray;
    }

    private static JToken? CreateMergedTemplate(JArray array)
    {
        if (array.Count == 0)
        {
            return null;
        }

        // Start with the first item as base template
        var template = array[0].DeepClone();

        // Merge each subsequent item into the template
        for (var i = 1; i < array.Count; i++)
        {
            template = MergeTokens(template, array[i]);
            if (template == null)
            {
                return null; // Cannot merge - incompatible types
            }
        }

        return template;
    }

    private static JToken? MergeTokens(JToken template, JToken item)
    {
        // If types don't match, merging is not possible
        if (template.Type != item.Type)
        {
            return null;
        }

        return template.Type switch
        {
            JTokenType.Object => MergeObjects((JObject)template, (JObject)item),
            JTokenType.Array => MergeArrays((JArray)template, (JArray)item),
            _ => template
        };
    }

    private static JObject MergeObjects(JObject template, JObject item)
    {
        var result = (JObject)template.DeepClone();

        // Add all properties from item that don't exist in template
        foreach (var property in item.Properties())
        {
            if (result.Property(property.Name) == null)
            {
                result[property.Name] = property.Value.DeepClone();
            }
            else
            {
                // Property exists in both - try to merge their values
                var mergedValue = MergeTokens(result[property.Name]!, property.Value);
                if (mergedValue != null)
                {
                    result[property.Name] = mergedValue;
                }
            }
        }

        return result;
    }

    private static JArray MergeArrays(JArray template, JArray _)
    {
        return (JArray)template.DeepClone();
    }

    private static JToken ApplyTemplate(JToken item, JToken template)
    {
        if (template.Type != item.Type)
        {
            return item; // Cannot apply template to different type
        }

        return template.Type switch
        {
            JTokenType.Object => ApplyObjectTemplate((JObject)item, (JObject)template),
            JTokenType.Array => ApplyArrayTemplate((JArray)item, (JArray)template),
            _ => item
        };
    }

    private static JObject ApplyObjectTemplate(JObject item, JObject template)
    {
        var result = new JObject();

        // Add all properties from template
        foreach (var templateProp in template.Properties())
        {
            var itemProp = item.Property(templateProp.Name);
            if (itemProp != null)
            {
                // Property exists in item - apply template recursively
                result[templateProp.Name] = ApplyTemplate(itemProp.Value, templateProp.Value);
            }
            else
            {
                // Property doesn't exist in item - add default value
                result[templateProp.Name] = CreateDefaultValue(templateProp.Value);
            }
        }

        return result;
    }

    private static JArray ApplyArrayTemplate(JArray item, JArray _)
    {
        // For arrays, return the original item array
        // This could be enhanced to apply template to array elements
        return item;
    }

    private static JToken CreateDefaultValue(JToken templateValue)
    {
        return templateValue.Type switch
        {
            JTokenType.String => new JValue(string.Empty),
            JTokenType.Integer => new JValue(0),
            JTokenType.Float => new JValue(0.0),
            JTokenType.Boolean => new JValue(false),
            JTokenType.Array => CreateEmptyArrayFromTemplate((JArray)templateValue),
            JTokenType.Object => CreateEmptyObjectFromTemplate((JObject)templateValue),
            JTokenType.Null => JValue.CreateNull(),
            _ => JValue.CreateNull()
        };
    }

    private static JArray CreateEmptyArrayFromTemplate(JArray templateArray)
    {
        var result = new JArray();

        // If the template array has elements, create default instances of the same types
        if (templateArray.Count > 0)
        {
            // Get the unique element types from the template array
            var elementTypes = templateArray
                .Select(element => element.Type)
                .Distinct();

            // Create default instances for each unique type found in the template
            foreach (var elementType in elementTypes)
            {
                // Find a representative element of this type from the template
                var templateElement = templateArray.First(e => e.Type == elementType);

                // Create a default value based on this template element
                var defaultElement = CreateDefaultValue(templateElement);
                result.Add(defaultElement);
            }
        }

        return result;
    }

    private static JObject CreateEmptyObjectFromTemplate(JObject templateObject)
    {
        var result = new JObject();

        // Create an object with the same property structure but with default values
        foreach (var property in templateObject.Properties())
        {
            result[property.Name] = CreateDefaultValue(property.Value);
        }

        return result;
    }
}