// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// Converter used by v1 protocol serializer to serialize TestCase object to and from v1 json
/// </summary>
internal class TestCaseConverter : JsonConverter<TestCase>
{
    /// <inheritdoc/>
    public override TestCase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var testCase = new TestCase();

        using var doc = JsonDocument.ParseValue(ref reader);
        var data = doc.RootElement;

        if (!data.TryGetProperty("Properties", out var properties) || properties.GetArrayLength() == 0)
        {
            return testCase;
        }

        // Every class that inherits from TestObject uses a properties store for <Property, Object>
        // key value pairs.
        foreach (var property in properties.EnumerateArray())
        {
            if (!property.TryGetProperty("Key", out var keyElement))
            {
                return null;
            }

            var testProperty = JsonSerializer.Deserialize<TestProperty>(keyElement.GetRawText(), options);

            if (testProperty is null)
            {
                return null;
            }

            // Let the null values be passed in as null data
            if (!property.TryGetProperty("Value", out var token))
            {
                return null;
            }

            string? propertyData = null;

            if (token.ValueKind != JsonValueKind.Null)
            {
                // If the property is already a string. No need to convert again.
                if (token.ValueKind == JsonValueKind.String)
                {
                    propertyData = token.GetString();
                }
                else
                {
                    // On deserialization, the value for each TestProperty is always a string. It is up
                    // to the consumer to deserialize it further as appropriate.
                    propertyData = token.GetRawText().Trim('"');
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
    public override void Write(Utf8JsonWriter writer, TestCase value, JsonSerializerOptions options)
    {
        // P2 to P1
        writer.WriteStartObject();
        writer.WritePropertyName("Properties");
        writer.WriteStartArray();

        // Version Note: In 15.0.0, if some properties in TestCase were not set, they were not serialized.
        // Starting 15.1.0, test platform sends in default values for properties that were not set. This is not
        // a breaking change.

        // TestCase.FullyQualifiedName
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.FullyQualifiedName, options);
        writer.WriteStringValue(value.FullyQualifiedName);
        writer.WriteEndObject();

        // TestCase.ExecutorUri
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.ExecutorUri, options);
        writer.WriteStringValue(value.ExecutorUri.OriginalString);
        writer.WriteEndObject();

        // TestCase.Source
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.Source, options);
        writer.WriteStringValue(value.Source);
        writer.WriteEndObject();

        // TestCase.CodeFilePath
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.CodeFilePath, options);
        writer.WriteStringValue(value.CodeFilePath);
        writer.WriteEndObject();

        // TestCase.DisplayName
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.DisplayName, options);
        writer.WriteStringValue(value.DisplayName);
        writer.WriteEndObject();

        // TestCase.Id
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.Id, options);
        writer.WriteStringValue(value.Id.ToString());
        writer.WriteEndObject();

        // TestCase.LineNumber
        writer.WriteStartObject();
        WriteProperty(writer, TestCaseProperties.LineNumber, options);
        writer.WriteNumberValue(value.LineNumber);
        writer.WriteEndObject();

        foreach (var property in value.GetProperties())
        {
            JsonSerializer.Serialize(writer, property, options);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    private static void WriteProperty(Utf8JsonWriter writer, TestProperty property, JsonSerializerOptions options)
    {
        writer.WritePropertyName("Key");
        JsonSerializer.Serialize(writer, property, options);
        writer.WritePropertyName("Value");
    }
}

#endif

