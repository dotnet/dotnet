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
/// Converter used by v1 protocol serializer to serialize TestResult object to and from v1 json
/// </summary>
internal class TestResultConverter : JsonConverter<TestResult>
{
    /// <inheritdoc/>
    public override TestResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var data = doc.RootElement;

        var testCaseElement = data.GetProperty("TestCase");
        var testCase = JsonSerializer.Deserialize<TestCase>(testCaseElement, options)!;
        var testResult = new TestResult(testCase);

        // Add attachments for the result
        if (data.TryGetProperty("Attachments", out var attachments) && attachments.GetArrayLength() > 0)
        {
            foreach (var attachment in attachments.EnumerateArray())
            {
                if (attachment.ValueKind != JsonValueKind.Null)
                {
                    testResult.Attachments.Add(JsonSerializer.Deserialize<AttachmentSet>(attachment, options)!);
                }
            }
        }

        // Add messages for the result
        if (data.TryGetProperty("Messages", out var messages) && messages.GetArrayLength() > 0)
        {
            foreach (var message in messages.EnumerateArray())
            {
                if (message.ValueKind != JsonValueKind.Null)
                {
                    testResult.Messages.Add(JsonSerializer.Deserialize<TestResultMessage>(message, options)!);
                }
            }
        }

        if (!data.TryGetProperty("Properties", out var properties) || properties.GetArrayLength() == 0)
        {
            return testResult;
        }

        // Every class that inherits from TestObject uses a properties store for <Property, Object>
        // key value pairs.
        foreach (var property in properties.EnumerateArray())
        {
            var testProperty = JsonSerializer.Deserialize<TestProperty>(property.GetProperty("Key"), options)!;

            // Let the null values be passed in as null data
            var token = property.GetProperty("Value");
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
                case "TestResult.DisplayName":
                    testResult.DisplayName = propertyData; break;
                case "TestResult.ComputerName":
                    testResult.ComputerName = propertyData ?? string.Empty; break;
                case "TestResult.Outcome":
                    testResult.Outcome = (TestOutcome)Enum.Parse(typeof(TestOutcome), propertyData!); break;
                case "TestResult.Duration":
                    testResult.Duration = TimeSpan.Parse(propertyData!, CultureInfo.InvariantCulture); break;
                case "TestResult.StartTime":
                    testResult.StartTime = DateTimeOffset.Parse(propertyData!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind); break;
                case "TestResult.EndTime":
                    testResult.EndTime = DateTimeOffset.Parse(propertyData!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind); break;
                case "TestResult.ErrorMessage":
                    testResult.ErrorMessage = propertyData; break;
                case "TestResult.ErrorStackTrace":
                    testResult.ErrorStackTrace = propertyData; break;
                default:
                    // No need to register member properties as they get registered as part of TestResultProperties class.
                    testProperty = TestProperty.Register(testProperty.Id, testProperty.Label, testProperty.GetValueType(), testProperty.Attributes, typeof(TestObject));
                    testResult.SetPropertyValue(testProperty, propertyData);
                    break;
            }
        }

        return testResult;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestResult value, JsonSerializerOptions options)
    {
        // P2 to P1
        writer.WriteStartObject();
        writer.WritePropertyName("TestCase");
        JsonSerializer.Serialize(writer, value.TestCase, options);
        writer.WritePropertyName("Attachments");
        JsonSerializer.Serialize(writer, value.Attachments, options);
        writer.WritePropertyName("Messages");
        JsonSerializer.Serialize(writer, value.Messages, options);

        writer.WritePropertyName("Properties");
        writer.WriteStartArray();

        // Version Note: In 15.0.0, if some properties in TestResult were not set, they were not serialized.
        // Starting 15.1.0, test platform sends in default values for properties that were not set. This is not
        // a breaking change.

        // TestResult.Outcome
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.Outcome, options);
        writer.WriteNumberValue((int)value.Outcome);
        writer.WriteEndObject();

        // TestResult.ErrorMessage
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.ErrorMessage, options);
        writer.WriteStringValue(value.ErrorMessage);
        writer.WriteEndObject();

        // TestResult.ErrorStackTrace
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.ErrorStackTrace, options);
        writer.WriteStringValue(value.ErrorStackTrace);
        writer.WriteEndObject();

        // TestResult.DisplayName
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.DisplayName, options);
        writer.WriteStringValue(value.DisplayName);
        writer.WriteEndObject();

        // TestResult.ComputerName
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.ComputerName, options);
        writer.WriteStringValue(value.ComputerName ?? string.Empty);
        writer.WriteEndObject();

        // TestResult.Duration
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.Duration, options);
        writer.WriteStringValue(value.Duration.ToString());
        writer.WriteEndObject();

        // TestResult.StartTime
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.StartTime, options);
        writer.WriteStringValue(value.StartTime);
        writer.WriteEndObject();

        // TestResult.EndTime
        writer.WriteStartObject();
        WriteProperty(writer, TestResultProperties.EndTime, options);
        writer.WriteStringValue(value.EndTime);
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

