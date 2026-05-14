// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for the <see cref="TestObject"/> and derived entities.
/// </summary>
internal class TestObjectConverter : JsonConverter<List<KeyValuePair<TestProperty, object>>>
{
    /// <inheritdoc/>
    public override List<KeyValuePair<TestProperty, object>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var propertyList = new List<KeyValuePair<TestProperty, object>>();

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            return propertyList;
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (reader.TokenType != JsonTokenType.StartObject)
                continue;

            using var doc = JsonDocument.ParseValue(ref reader);
            var element = doc.RootElement;

            if (!element.TryGetProperty("Key", out var keyElement))
                continue;

            var testProperty = JsonSerializer.Deserialize<TestProperty>(keyElement, options);
            if (testProperty is null)
                continue;

            object? propertyData = null;
            if (element.TryGetProperty("Value", out var valueElement) && valueElement.ValueKind != JsonValueKind.Null)
            {
                // If the property is already a string. No need to convert again.
                if (valueElement.ValueKind == JsonValueKind.String)
                {
                    propertyData = valueElement.GetString();
                }
                else
                {
                    // On deserialization, the value for each TestProperty is always a string. It is up
                    // to the consumer to deserialize it further as appropriate.
                    propertyData = valueElement.GetRawText().Trim('"');
                }
            }

            propertyList.Add(new KeyValuePair<TestProperty, object>(testProperty, propertyData!));
        }

        return propertyList;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, List<KeyValuePair<TestProperty, object>> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var kvp in value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            JsonSerializer.Serialize(writer, kvp.Key, options);
            writer.WritePropertyName("Value");
            if (kvp.Value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, kvp.Value, kvp.Value.GetType(), options);
            }
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }
}

#endif

