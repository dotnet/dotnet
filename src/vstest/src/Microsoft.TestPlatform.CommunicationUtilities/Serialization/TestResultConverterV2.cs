// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// Converter used by v2 protocol serializer to serialize TestResult object to and from v2 json.
/// In v2, core properties are flat fields; only custom properties use the Properties array.
/// </summary>
internal class TestResultConverterV2 : JsonConverter<TestResult>
{
    /// <inheritdoc/>
    public override TestResult? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var data = doc.RootElement;

        // TestCase must come first to construct the TestResult
        var testCaseElement = data.GetProperty("TestCase");
        var testCase = JsonSerializer.Deserialize<TestCase>(testCaseElement, options)!;
        var testResult = new TestResult(testCase);

        // Attachments
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

        // Messages
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

        // Flat properties
        if (data.TryGetProperty("Outcome", out var outcome))
            testResult.Outcome = (TestOutcome)outcome.GetInt32();
        if (data.TryGetProperty("ErrorMessage", out var errorMsg) && errorMsg.ValueKind != JsonValueKind.Null)
            testResult.ErrorMessage = errorMsg.GetString();
        if (data.TryGetProperty("ErrorStackTrace", out var errorStack) && errorStack.ValueKind != JsonValueKind.Null)
            testResult.ErrorStackTrace = errorStack.GetString();
        if (data.TryGetProperty("DisplayName", out var displayName) && displayName.ValueKind != JsonValueKind.Null)
            testResult.DisplayName = displayName.GetString();
        if (data.TryGetProperty("ComputerName", out var computerName) && computerName.ValueKind != JsonValueKind.Null)
            testResult.ComputerName = computerName.GetString();
        if (data.TryGetProperty("Duration", out var duration) && duration.ValueKind != JsonValueKind.Null)
            testResult.Duration = TimeSpan.Parse(duration.GetString()!, CultureInfo.InvariantCulture);
        if (data.TryGetProperty("StartTime", out var startTime) && startTime.ValueKind != JsonValueKind.Null)
            testResult.StartTime = DateTimeOffset.Parse(startTime.GetString()!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        if (data.TryGetProperty("EndTime", out var endTime) && endTime.ValueKind != JsonValueKind.Null)
            testResult.EndTime = DateTimeOffset.Parse(endTime.GetString()!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

        // Custom properties
        if (data.TryGetProperty("Properties", out var properties) && properties.GetArrayLength() > 0)
        {
            foreach (var prop in properties.EnumerateArray())
            {
                if (!prop.TryGetProperty("Key", out var keyElement))
                    continue;

                var testProperty = JsonSerializer.Deserialize<TestProperty>(keyElement, options)!;

                if (!prop.TryGetProperty("Value", out var valueElement))
                    continue;

                string? propertyData = null;
                if (valueElement.ValueKind != JsonValueKind.Null)
                {
                    propertyData = valueElement.ValueKind == JsonValueKind.String
                        ? valueElement.GetString()
                        : valueElement.GetRawText().Trim('"');
                }

                testProperty = TestProperty.Register(testProperty.Id, testProperty.Label, testProperty.GetValueType(), testProperty.Attributes, typeof(TestObject));
                testResult.SetPropertyValue(testProperty, propertyData);
            }
        }

        return testResult;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestResult value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // TestCase
        writer.WritePropertyName("TestCase");
        JsonSerializer.Serialize(writer, value.TestCase, options);

        // Attachments
        writer.WritePropertyName("Attachments");
        JsonSerializer.Serialize(writer, value.Attachments, options);

        // Flat properties
        writer.WriteNumber("Outcome", (int)value.Outcome);
        writer.WriteString("ErrorMessage", value.ErrorMessage);
        writer.WriteString("ErrorStackTrace", value.ErrorStackTrace);
        writer.WriteString("DisplayName", value.DisplayName);

        // Messages
        writer.WritePropertyName("Messages");
        JsonSerializer.Serialize(writer, value.Messages, options);

        writer.WriteString("ComputerName", value.ComputerName);
        writer.WriteString("Duration", value.Duration.ToString());
        writer.WriteString("StartTime", value.StartTime);
        writer.WriteString("EndTime", value.EndTime);

        // Custom properties (non-core)
        writer.WritePropertyName("Properties");
        writer.WriteStartArray();
        foreach (var property in value.GetProperties())
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            JsonSerializer.Serialize(writer, property.Key, options);
            writer.WritePropertyName("Value");
            if (property.Value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, property.Value, property.Value.GetType(), options);
            }
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}

#endif
