// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// Converter that deserializes JSON values into primitive .NET types instead of JsonElement.
/// Without this, STJ puts JsonElement into object-typed dictionaries like IDictionary&lt;string, object&gt;,
/// and downstream code that expects IConvertible/primitives fails with InvalidCastException.
/// </summary>
internal class ObjectConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ReadValue(ref reader, options);
    }

    internal static object? ReadValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.TryGetDateTimeOffset(out var dto) ? dto : reader.GetString(),
            JsonTokenType.Number => reader.TryGetInt32(out var i) ? i : reader.TryGetInt64(out var l) ? l : reader.GetDouble(),
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Null => null,
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone(),
        };
    }

    internal static List<object?> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var list = new List<object?>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            list.Add(ReadValue(ref reader, options));
        }

        return list;
    }

    internal static Dictionary<string, object?> ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var dict = new Dictionary<string, object?>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            var key = reader.GetString()!;
            reader.Read();
            dict[key] = ReadValue(ref reader, options);
        }

        return dict;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

/// <summary>
/// Converter factory for dictionary types with object values (IDictionary&lt;string, object&gt;,
/// Dictionary&lt;string, object&gt;). Uses <see cref="ObjectConverter"/> to deserialize
/// values as primitives (int, long, double, string, bool) instead of JsonElement.
/// STJ's built-in dictionary converter does not invoke custom object converters for values.
/// </summary>
internal class ObjectDictionaryConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert == typeof(IDictionary<string, object>)
            || typeToConvert == typeof(Dictionary<string, object>)
            || typeToConvert == typeof(IDictionary<string, object?>)
            || typeToConvert == typeof(Dictionary<string, object?>))
        {
            return true;
        }

        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new ObjectDictionaryConverter();
    }
}

internal class ObjectDictionaryConverter : JsonConverter<IDictionary<string, object>>
{
    public override IDictionary<string, object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var dict = new Dictionary<string, object>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            var key = reader.GetString()!;
            reader.Read();
            dict[key] = ObjectConverter.ReadValue(ref reader, options)!;
        }

        return dict;
    }

    public override void Write(Utf8JsonWriter writer, IDictionary<string, object> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);
            JsonSerializer.Serialize(writer, kvp.Value, kvp.Value?.GetType() ?? typeof(object), options);
        }

        writer.WriteEndObject();
    }
}

#endif
