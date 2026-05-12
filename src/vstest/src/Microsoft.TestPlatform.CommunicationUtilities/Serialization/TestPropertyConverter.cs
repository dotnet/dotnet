// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="TestProperty"/> that serializes only the data-member properties
/// and skips the <see cref="TestProperty.ValidateValueCallback"/> delegate which cannot be serialized.
/// </summary>
internal class TestPropertyConverter : JsonConverter<TestProperty>
{
    /// <inheritdoc/>
    public override TestProperty? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var element = doc.RootElement;

        var id = element.TryGetProperty("Id", out var idProp) ? idProp.GetString() : null;
        var label = element.TryGetProperty("Label", out var labelProp) ? labelProp.GetString() : null;
        var category = element.TryGetProperty("Category", out var catProp) ? catProp.GetString() : null;
        var description = element.TryGetProperty("Description", out var descProp) ? descProp.GetString() : null;
        var attributes = element.TryGetProperty("Attributes", out var attrProp) ? (TestPropertyAttributes)attrProp.GetInt32() : default;
        var valueType = element.TryGetProperty("ValueType", out var vtProp) ? vtProp.GetString() : null;

        if (id is null || label is null)
        {
            return null;
        }

        // Try to find an already-registered property first to avoid registration conflicts
        // (e.g., TestCase.ExecutorUri is already registered as System.Uri)
        var testProperty = TestProperty.Find(id);
        if (testProperty is not null)
        {
            return testProperty;
        }

        // Resolve the actual type from the ValueType string
        var resolvedType = typeof(string);
        if (valueType is not null)
        {
            resolvedType = Type.GetType(valueType) ?? typeof(string);
        }

        // Register the property so it's known to the test platform
        testProperty = TestProperty.Register(id, label, category ?? string.Empty, description ?? string.Empty, resolvedType, null, attributes, typeof(TestObject));

        // Override the value type if specified (Register may have set it to the resolved type's name)
        if (valueType is not null)
        {
            testProperty.ValueType = valueType;
        }

        return testProperty;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestProperty value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Id", value.Id);
        writer.WriteString("Label", value.Label);
        writer.WriteString("Category", value.Category);
        writer.WriteString("Description", value.Description);
        writer.WriteNumber("Attributes", (int)value.Attributes);
        writer.WriteString("ValueType", value.ValueType);
        writer.WriteEndObject();
    }
}

#endif
