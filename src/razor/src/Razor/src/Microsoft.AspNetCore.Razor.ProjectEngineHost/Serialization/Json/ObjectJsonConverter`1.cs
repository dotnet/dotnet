// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal abstract class ObjectJsonConverter<T> : JsonConverter<T>
    where T : class
{
    protected abstract T ReadFromProperties(JsonDataReader reader);
    protected abstract void WriteProperties(JsonDataWriter writer, T value);

    public sealed override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        reader.ReadToken(JsonToken.StartObject);

        T result;

        var dataReader = JsonDataReader.Get(reader);
        try
        {
            result = ReadFromProperties(dataReader);
        }
        finally
        {
            JsonDataReader.Return(dataReader);
        }

        // JSON.NET serialization expects that we don't advance passed the end object token,
        // but we should verify that it's there.
        reader.CheckToken(JsonToken.EndObject);

        return result;
    }

    public sealed override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        var dataWriter = JsonDataWriter.Get(writer);
        try
        {
            WriteProperties(dataWriter, value);
        }
        finally
        {
            JsonDataWriter.Return(dataWriter);
        }

        writer.WriteEndObject();
    }
}
