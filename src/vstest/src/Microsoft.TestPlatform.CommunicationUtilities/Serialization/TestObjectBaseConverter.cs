// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections;
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
    // Singleton converter handles all TestObject-derived types via the base class,
    // avoiding MakeGenericType + Activator.CreateInstance which fail under NativeAOT.
    private static readonly TestObjectBaseConverter Converter = new();

    public override bool CanConvert(Type typeToConvert)
    {
        // Only handle the abstract TestObject base type itself. TestCase and TestResult
        // have their own dedicated converters. Other derived types are not expected on
        // the wire protocol.
        return typeToConvert == typeof(TestObject);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Converter;
    }
}

internal class TestObjectBaseConverter : JsonConverter<TestObject>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(TestObject);
    }

    public override TestObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Always instantiate a TestCase as the carrier for the property bag.
        // TestObject is abstract, and the only concrete subclass that flows through
        // the wire protocol (other than TestCase/TestResult, which have their own
        // converters) is TestObject-as-generic-bag. Using TestCase preserves the
        // property key-value pairs for the consumer. We intentionally avoid
        // Activator.CreateInstance(typeToConvert) because it requires reflection
        // metadata that NativeAOT trims.
        var testObject = new TestCase();

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

            var testProperty = StjSafe.Deserialize<TestProperty>(keyElement, options);
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

    public override void Write(Utf8JsonWriter writer, TestObject value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Properties");
        writer.WriteStartArray();
        foreach (var property in value.GetProperties())
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            StjSafe.Serialize(writer, property.Key, options);
            writer.WritePropertyName("Value");
            if (property.Value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                WritePropertyValue(writer, property.Value, options);
            }
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }

    /// <summary>
    /// Writes a property value without using
    /// JsonSerializer.Serialize(writer, value, value.GetType()) which requires
    /// reflection metadata that NativeAOT trims.
    /// </summary>
    internal static void WritePropertyValue(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case string s: writer.WriteStringValue(s); break;
            case int i: writer.WriteNumberValue(i); break;
            case long l: writer.WriteNumberValue(l); break;
            case double d: writer.WriteNumberValue(d); break;
            case float f: writer.WriteNumberValue(f); break;
            case bool b: writer.WriteBooleanValue(b); break;
            case short s: writer.WriteNumberValue(s); break;
            case ushort us: writer.WriteNumberValue(us); break;
            case uint ui: writer.WriteNumberValue(ui); break;
            case ulong ul: writer.WriteNumberValue(ul); break;
            case byte by: writer.WriteNumberValue(by); break;
            case sbyte sb: writer.WriteNumberValue(sb); break;
            case decimal dec: writer.WriteNumberValue(dec); break;
            case char c: writer.WriteStringValue(c.ToString()); break;
            case DateTimeOffset dto: writer.WriteStringValue(dto); break;
            case DateTime dt: writer.WriteStringValue(dt); break;
            case Guid g: writer.WriteStringValue(g); break;
            case Uri u: writer.WriteStringValue(u.OriginalString); break;
            case JsonElement je: je.WriteTo(writer); break;
            case TimeSpan ts: writer.WriteStringValue(ts.ToString()); break;
            case Enum e:
                // Write enums as their underlying numeric value.
                writer.WriteNumberValue(Convert.ToInt64(e, CultureInfo.InvariantCulture));
                break;
            case string[] sa:
                writer.WriteStartArray();
                foreach (var item in sa) writer.WriteStringValue(item);
                writer.WriteEndArray();
                break;
            case KeyValuePair<string, string>[] kvps:
                writer.WriteStartArray();
                foreach (var kvp in kvps)
                {
                    writer.WriteStartObject();
                    writer.WriteString("Key", kvp.Key);
                    writer.WriteString("Value", kvp.Value);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                break;
            case IDictionary dict:
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in dict)
                {
                    writer.WritePropertyName(Convert.ToString(entry.Key, CultureInfo.InvariantCulture)!);
                    if (entry.Value is null) writer.WriteNullValue();
                    else WritePropertyValue(writer, entry.Value, options);
                }
                writer.WriteEndObject();
                break;
            case IEnumerable enumerable:
                writer.WriteStartArray();
                foreach (var item in enumerable)
                {
                    if (item is null) writer.WriteNullValue();
                    else WritePropertyValue(writer, item, options);
                }
                writer.WriteEndArray();
                break;
            default:
                // Last resort for types not handled above. Under NativeAOT this may
                // fail for types not in the source-gen context, but all known property
                // value types used in the wire protocol are handled explicitly.
                var element = StjSafe.SerializeToElement(value, value.GetType(), options);
                element.WriteTo(writer);
                break;
        }
    }
}

#endif
