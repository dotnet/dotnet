// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// Converter used by v1 protocol serializer to serialize TestCase object to and from v1 json
/// </summary>
public class TestCaseConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return typeof(TestCase) == objectType;
    }

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var testCase = new TestCase();

        var data = JObject.Load(reader);
        var properties = data["Properties"];

        if (properties == null || !properties.HasValues)
        {
            return testCase;
        }

        // Every class that inherits from TestObject uses a properties store for <Property, Object>
        // key value pairs.
        foreach (var property in properties.Values<JToken>())
        {
            var testProperty = property?["Key"]?.ToObject<TestProperty>(serializer);

            if (testProperty == null)
            {
                return null;
            }

            // Let the null values be passed in as null data
            var token = property?["Value"];
            string? propertyData = null;

            if (token == null)
            {
                return null;
            }

            if (token.Type != JTokenType.Null)
            {
                // If the property is already a string. No need to convert again.
                if (token.Type == JTokenType.String)
                {
                    propertyData = token.ToObject<string>(serializer);
                }
                else
                {
                    // On deserialization, the value for each TestProperty is always a string. It is up
                    // to the consumer to deserialize it further as appropriate.
                    propertyData = token.ToString(Formatting.None).Trim('"');
                }
            }

            switch (testProperty.Id)
            {
                case "TestCase.Id":
                    testCase.Id = GuidPolyfill.Parse(propertyData!, CultureInfo.InvariantCulture);
                    break;
                case "TestCase.ExecutorUri":
                    testCase.ExecutorUri = new Uri(propertyData!); break;
                case "TestCase.FullyQualifiedName":
                    testCase.FullyQualifiedName = propertyData!; break;
                case "TestCase.DisplayName":
                    testCase.DisplayName = propertyData!; break;
                case "TestCase.Source":
                    testCase.Source = propertyData!; break;
                case "TestCase.CodeFilePath":
                    testCase.CodeFilePath = propertyData; break;
                case "TestCase.LineNumber":
                    testCase.LineNumber = int.Parse(propertyData!, CultureInfo.CurrentCulture); break;
                default:
                    // No need to register member properties as they get registered as part of TestCaseProperties class.
                    testProperty = TestProperty.Register(testProperty.Id, testProperty.Label, testProperty.GetValueType(), testProperty.Attributes, typeof(TestObject));
                    testCase.SetPropertyValue(testProperty, propertyData);
                    break;
            }
        }

        return testCase;
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }

        // P2 to P1
        var testCase = (TestCase)value;

        writer.WriteStartObject();
        writer.WritePropertyName("Properties");
        writer.WriteStartArray();

        // Version Note: In 15.0.0, if some properties in TestCase were not set, they were not serialized.
        // Starting 15.1.0, test platform sends in default values for properties that were not set. This is not
        // a breaking change.

        // TestCase.FullyQualifiedName
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.FullyQualifiedName, serializer);
        writer.WriteValue(testCase.FullyQualifiedName);
        writer.WriteEndObject();

        // TestCase.ExecutorUri
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.ExecutorUri, serializer);
        writer.WriteValue(testCase.ExecutorUri.OriginalString);
        writer.WriteEndObject();

        // TestCase.Source
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.Source, serializer);
        writer.WriteValue(testCase.Source);
        writer.WriteEndObject();

        // TestCase.CodeFilePath
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.CodeFilePath, serializer);
        writer.WriteValue(testCase.CodeFilePath);
        writer.WriteEndObject();

        // TestCase.DisplayName
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.DisplayName, serializer);
        writer.WriteValue(testCase.DisplayName);
        writer.WriteEndObject();

        // TestCase.Id
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.Id, serializer);
        writer.WriteValue(testCase.Id);
        writer.WriteEndObject();

        // TestCase.LineNumber
        writer.WriteStartObject();
        AddProperty(writer, TestCaseProperties.LineNumber, serializer);
        writer.WriteValue(testCase.LineNumber);
        writer.WriteEndObject();

        foreach (var property in testCase.GetProperties())
        {
            serializer.Serialize(writer, property);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    private static void AddProperty(JsonWriter writer, TestProperty property, JsonSerializer serializer)
    {
        writer.WritePropertyName("Key");
        serializer.Serialize(writer, property);
        writer.WritePropertyName("Value");
    }
}
