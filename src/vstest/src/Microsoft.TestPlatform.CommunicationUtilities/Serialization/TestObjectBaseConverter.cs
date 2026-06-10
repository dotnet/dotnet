// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// Converter factory for <see cref="TestObject"/>-derived types that don't have their own
/// dedicated converters (e.g. not TestCase or TestResult). Serializes only the property bag
/// as "Properties", matching the DataContract serialization behavior.
/// </summary>
internal class TestObjectBaseConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(TestObject).IsAssignableFrom(typeToConvert)
            && typeToConvert != typeof(TestCase)
            && typeToConvert != typeof(TestResult);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(TestObjectBaseConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

internal class TestObjectBaseConverter<T> : JsonConverter<T> where T : TestObject, new()
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var testObject = new T();

        using var doc = JsonDocument.ParseValue(ref reader);
        var data = doc.RootElement;

        if (!data.TryGetProperty("Properties", out var properties) || properties.GetArrayLength() == 0)
        {
            return testObject;
        }

        foreach (var prop in properties.EnumerateArray())
        {
            if (!prop.TryGetProperty("Key", out var keyElement))
                continue;

            var testProperty = JsonSerializer.Deserialize<TestProperty>(keyElement.GetRawText(), options);
            if (testProperty is null)
                continue;

            if (!prop.TryGetProperty("Value", out var valueElement))
                continue;

            object? propertyData = null;
            if (valueElement.ValueKind != JsonValueKind.Null)
            {
                if (valueElement.ValueKind == JsonValueKind.String)
                {
                    propertyData = valueElement.GetString();
                }
                else
                {
                    propertyData = valueElement.GetRawText().Trim('"');
                }
            }

            testObject.SetPropertyValue(testProperty, propertyData, CultureInfo.InvariantCulture);
        }

        return testObject;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

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
