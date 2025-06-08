using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HandlebarsDotNet.Helpers.Tests.DynamicLinq;

public class JArrayMergerTests
{
    [Fact]
    public void MergeToCommonStructure_WithIdenticalObjects_ReturnsOriginalArray()
    {
        // Arrange
        var json =
            """
            [
                {
                    "name": "Emily",
                    "phone_numbers": []
                },
                {
                    "name": "Jason",
                    "phone_numbers": ["555-1234"]
                }
            ]
            """;
        var originalArray = JArray.Parse(json);

        // Act
        var result = JArrayMerger.MergeToCommonStructure(originalArray);

        // Assert
        result.Should().BeSameAs(originalArray, "structure is already identical");
    }

    [Fact]
    public void MergeToCommonStructure_WithDifferentProperties_MergesSuccessfully()
    {
        // Arrange
        var json =
            """
            [
                {
                    "name": "Emily",
                    "age": 25
                },
                {
                    "name": "Jason",
                    "phone": "555-1234"
                }
            ]
            """;
        var originalArray = JArray.Parse(json);

        // Act
        var result = JArrayMerger.MergeToCommonStructure(originalArray);

        // Assert
        result.Should().NotBeSameAs(originalArray, "a new merged array should be created");
        result.Should().HaveCount(2);

        // First object should have all properties
        var firstItem = result[0].Should().BeOfType<JObject>().Subject;
        firstItem.Should().ContainKey("name");
        firstItem.Should().ContainKey("age");
        firstItem.Should().ContainKey("phone");
        firstItem["name"]!.Value<string>().Should().Be("Emily");
        firstItem["age"]!.Value<int>().Should().Be(25);
        firstItem["phone"]!.Value<string>().Should().Be("", "default value for missing property");

        // Second object should have all properties
        var secondItem = result[1].Should().BeOfType<JObject>().Subject;
        secondItem.Should().ContainKey("name");
        secondItem.Should().ContainKey("age");
        secondItem.Should().ContainKey("phone");
        secondItem["name"]!.Value<string>().Should().Be("Jason");
        secondItem["age"]!.Value<int>().Should().Be(0, "default value for missing property");
        secondItem["phone"]!.Value<string>().Should().Be("555-1234");
    }

    [Fact]
    public void MergeToCommonStructure_WithNestedObjects_MergesNestedStructures()
    {
        // Arrange
        var json =
            """
            [
                {
                    "name": "Emily",
                    "contact": {
                        "email": "emily@test.com"
                    }
                },
                {
                    "name": "Jason",
                    "contact": {
                        "phone": "555-1234"
                    }
                }
            ]
            """;
        var originalArray = JArray.Parse(json);

        // Act
        var result = JArrayMerger.MergeToCommonStructure(originalArray);

        // Assert
        result.Should().NotBeSameAs(originalArray);
        result.Should().HaveCount(2);

        // Check first item has merged nested structure
        var firstContact = result[0]["contact"].Should().BeOfType<JObject>().Subject;
        firstContact.Should().ContainKeys("email", "phone");
        firstContact["email"]!.Value<string>().Should().Be("emily@test.com");
        firstContact["phone"]!.Value<string>().Should().Be("", "default value for missing nested property");

        // Check second item has merged nested structure
        var secondContact = result[1]["contact"].Should().BeOfType<JObject>().Subject;
        secondContact.Should().ContainKeys("email", "phone");
        secondContact["email"]!.Value<string>().Should().Be("", "default value for missing nested property");
        secondContact["phone"]!.Value<string>().Should().Be("555-1234");
    }

    [Fact]
    public void MergeToCommonStructure_WithEmptyArray_ReturnsOriginalArray()
    {
        // Arrange
        var emptyArray = new JArray();

        // Act
        var result = JArrayMerger.MergeToCommonStructure(emptyArray);

        // Assert
        result.Should().BeSameAs(emptyArray);
        result.Should().BeEmpty();
    }

    [Fact]
    public void MergeToCommonStructure_WithArrayWithOneItem_ReturnsOriginalArray()
    {
        // Arrange
        var array = new JArray { true };

        // Act
        var result = JArrayMerger.MergeToCommonStructure(array);

        // Assert
        result.Should().BeSameAs(array);
    }

    [Fact]
    public void MergeToCommonStructure_WithIncompatibleTypes_ReturnsOriginalArray()
    {
        // Arrange
        var json =
            """
            [
                {
                    "name": "Emily",
                    "data": "text_value"
                },
                {
                    "name": "Jason",
                    "data": 12345
                }
            ]
            """;
        var originalArray = JArray.Parse(json);

        // Act
        var result = JArrayMerger.MergeToCommonStructure(originalArray);

        // Assert
        result.Should().BeSameAs(originalArray);
        result.Should().HaveCount(2);

        // Both items should have the same property names
        var firstItem = result[0].Should().BeOfType<JObject>().Subject;
        var secondItem = result[1].Should().BeOfType<JObject>().Subject;

        firstItem.Should().ContainKeys("name", "data");
        secondItem.Should().ContainKeys("name", "data");

        // Original values should be preserved
        firstItem["name"]!.Value<string>().Should().Be("Emily");
        firstItem["data"]!.Value<string>().Should().Be("text_value");
        secondItem["name"]!.Value<string>().Should().Be("Jason");
        secondItem["data"]!.Value<int>().Should().Be(12345);
    }
}