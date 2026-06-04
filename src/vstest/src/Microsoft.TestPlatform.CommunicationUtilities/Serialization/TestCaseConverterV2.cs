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
/// Converter used by v2 protocol serializer to serialize TestCase object to and from v2 json.
/// In v2, core properties are flat fields; only custom properties use the Properties array.
/// </summary>
internal class TestCaseConverterV2 : JsonConverter<TestCase>
{
    /// <inheritdoc/>
    public override TestCase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var testCase = new TestCase();

        using var doc = JsonDocument.ParseValue(ref reader);
        var data = doc.RootElement;

        if (data.TryGetProperty("FullyQualifiedName", out var fqn))
            testCase.FullyQualifiedName = fqn.GetString()!;
        if (data.TryGetProperty("ExecutorUri", out var uri) && uri.ValueKind != JsonValueKind.Null)
            testCase.ExecutorUri = new Uri(uri.GetString()!);
        if (data.TryGetProperty("Source", out var source))
            testCase.Source = source.GetString()!;
        if (data.TryGetProperty("Id", out var id) && id.ValueKind != JsonValueKind.Null)
            testCase.Id = GuidPolyfill.Parse(id.GetString()!, CultureInfo.InvariantCulture);
        if (data.TryGetProperty("DisplayName", out var display) && display.ValueKind != JsonValueKind.Null)
            testCase.DisplayName = display.GetString()!;
        if (data.TryGetProperty("CodeFilePath", out var codePath) && codePath.ValueKind != JsonValueKind.Null)
            testCase.CodeFilePath = codePath.GetString();
        if (data.TryGetProperty("LineNumber", out var lineNum))
            testCase.LineNumber = lineNum.GetInt32();

        // Process custom properties (e.g. Traits)
        if (data.TryGetProperty("Properties", out var properties) && properties.GetArrayLength() > 0)
        {
            foreach (var prop in properties.EnumerateArray())
            {
                if (!prop.TryGetProperty("Key", out var keyElement))
                    continue;

                var testProperty = JsonSerializer.Deserialize<TestProperty>(keyElement, options);
                if (testProperty is null)
                    continue;

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
                testCase.SetPropertyValue(testProperty, propertyData);
            }
        }

        return testCase;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestCase value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Id", value.Id.ToString());
        writer.WriteString("FullyQualifiedName", value.FullyQualifiedName);
        writer.WriteString("DisplayName", value.DisplayName);
        writer.WriteString("ExecutorUri", value.ExecutorUri?.OriginalString);
        writer.WriteString("Source", value.Source);
        writer.WriteString("CodeFilePath", value.CodeFilePath);
        writer.WriteNumber("LineNumber", value.LineNumber);

        // Custom properties (e.g. Traits) — only non-core properties from the store
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
