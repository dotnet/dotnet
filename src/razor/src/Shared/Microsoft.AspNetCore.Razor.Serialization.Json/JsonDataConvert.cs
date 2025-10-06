// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class JsonDataConvert
{
    public static void Serialize(RazorConfiguration value, TextWriter writer, bool indented = false)
        => SerializeObject(value, writer, indented, ObjectWriters.WriteProperties);

    public static string Serialize(RazorConfiguration value, bool indented = false)
        => SerializeObject(value, indented, ObjectWriters.WriteProperties);

    public static void Serialize(TagHelperDescriptor value, TextWriter writer, bool indented = false)
        => SerializeObject(value, writer, indented, ObjectWriters.WriteProperties);

    public static string Serialize(TagHelperDescriptor value, bool indented = false)
        => SerializeObject(value, indented, ObjectWriters.WriteProperties);

    public static void Serialize(ImmutableArray<TagHelperDescriptor> value, TextWriter writer, bool indented = false)
        => SerializeArray(value, writer, indented, ObjectWriters.Write);

    public static string Serialize(ImmutableArray<TagHelperDescriptor> value, bool indented = false)
        => SerializeArray(value, indented, ObjectWriters.Write);

    public static void Serialize(IEnumerable<TagHelperDescriptor> value, TextWriter writer, bool indented = false)
        => SerializeArray(value, writer, indented, ObjectWriters.Write);

    public static string Serialize(IEnumerable<TagHelperDescriptor> value, bool indented = false)
        => SerializeArray(value, indented, ObjectWriters.Write);

    public static void SerializeToFile(ImmutableArray<TagHelperDescriptor> value, string filePath, bool indented = false)
    {
        using var writer = new StreamWriter(filePath);
        SerializeArray(value, writer, indented, ObjectWriters.Write);
    }

    public static void SerializeToFile(IEnumerable<TagHelperDescriptor> value, string filePath, bool indented = false)
    {
        using var writer = new StreamWriter(filePath);
        SerializeArray(value, writer, indented, ObjectWriters.Write);
    }

    public static void SerializeData(TextWriter writer, bool indented, Action<JsonDataWriter> writeData)
    {
        using var jsonWriter = new JsonTextWriter(writer)
        {
            CloseOutput = false,
            Formatting = indented ? Formatting.Indented : Formatting.None
        };

        var dataWriter = JsonDataWriter.Get(jsonWriter);
        try
        {
            writeData(dataWriter);
        }
        finally
        {
            JsonDataWriter.Return(dataWriter);
        }
    }

    public static void SerializeData(TextWriter writer, Action<JsonDataWriter> writeData)
        => SerializeData(writer, indented: false, writeData);

    public static string SerializeData(bool indented, Action<JsonDataWriter> writeData)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        using (var writer = new StringWriter(builder))
        {
            SerializeData(writer, indented, writeData);
        }

        return builder.ToString();
    }

    public static string SerializeData(Action<JsonDataWriter> writeData)
        => SerializeData(indented: false, writeData);

    public static void SerializeObject<T>(T? value, TextWriter writer, bool indented, WriteProperties<T> writeProperties)
        => SerializeData(writer, indented, dataWriter => dataWriter.WriteObject(value, writeProperties));

    public static void SerializeObject<T>(T? value, TextWriter writer, WriteProperties<T> writeProperties)
        => SerializeObject(value, writer, indented: false, writeProperties: writeProperties);

    public static string SerializeObject<T>(T? value, bool indented, WriteProperties<T> writeProperties)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        using (var writer = new StringWriter(builder))
        {
            SerializeObject(value, writer, indented, writeProperties);
        }

        return builder.ToString();
    }

    public static string SerializeObject<T>(T? value, WriteProperties<T> writeProperties)
        => SerializeObject(value, indented: false, writeProperties);

    public static void SerializeArray<T>(ImmutableArray<T> value, TextWriter writer, bool indented, WriteValue<T> writeValue)
        => SerializeData(writer, indented, dataWriter => dataWriter.WriteArray(value, writeValue));

    public static void SerializeArray<T>(ImmutableArray<T> value, TextWriter writer, WriteValue<T> writeValue)
        => SerializeArray(value, writer, indented: false, writeValue: writeValue);

    public static string SerializeArray<T>(ImmutableArray<T> value, bool indented, WriteValue<T> writeValue)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        using (var writer = new StringWriter(builder))
        {
            SerializeArray(value, writer, indented, writeValue);
        }

        return builder.ToString();
    }

    public static string SerializeArray<T>(IEnumerable<T> value, WriteValue<T> writeValue)
        => SerializeArray(value, indented: false, writeValue);

    public static void SerializeArray<T>(IEnumerable<T> value, TextWriter writer, bool indented, WriteValue<T> writeValue)
        => SerializeData(writer, indented, dataWriter => dataWriter.WriteArray(value, writeValue));

    public static void SerializeArray<T>(IEnumerable<T> value, TextWriter writer, WriteValue<T> writeValue)
        => SerializeArray(value, writer, indented: false, writeValue: writeValue);

    public static string SerializeArray<T>(IEnumerable<T> value, bool indented, WriteValue<T> writeValue)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        using (var writer = new StringWriter(builder))
        {
            SerializeArray(value, writer, indented, writeValue);
        }

        return builder.ToString();
    }

    public static string SerializeArray<T>(ImmutableArray<T> value, WriteValue<T> writeValue)
        => SerializeArray(value, indented: false, writeValue);

    public static RazorConfiguration DeserializeConfiguration(TextReader reader)
        => DeserializeNonNullObject(reader, ObjectReaders.ReadConfigurationFromProperties);

    public static RazorConfiguration DeserializeConfiguration(string json)
        => DeserializeNonNullObject(json, ObjectReaders.ReadConfigurationFromProperties);

    public static RazorConfiguration DeserializeConfiguration(byte[] bytes)
        => DeserializeNonNullObject(bytes, ObjectReaders.ReadConfigurationFromProperties);

    public static TagHelperDescriptor DeserializeTagHelper(TextReader reader)
        => DeserializeNonNullObject(reader, ObjectReaders.ReadTagHelperFromProperties);

    public static TagHelperDescriptor DeserializeTagHelper(string json)
        => DeserializeNonNullObject(json, ObjectReaders.ReadTagHelperFromProperties);

    public static TagHelperDescriptor DeserializeTagHelper(byte[] bytes)
        => DeserializeNonNullObject(bytes, ObjectReaders.ReadTagHelperFromProperties);

    public static ImmutableArray<TagHelperDescriptor> DeserializeTagHelperArray(TextReader reader)
        => DeserializeArray(reader, ObjectReaders.ReadTagHelper);

    public static ImmutableArray<TagHelperDescriptor> DeserializeTagHelperArray(string json)
        => DeserializeArray(json, ObjectReaders.ReadTagHelper);

    public static ImmutableArray<TagHelperDescriptor> DeserializeTagHelperArray(byte[] bytes)
        => DeserializeArray(bytes, ObjectReaders.ReadTagHelper);

    public static T DeserializeData<T>(TextReader reader, Func<JsonDataReader, T> readData)
    {
        using var jsonReader = new JsonTextReader(reader) { CloseInput = false };

        jsonReader.Read();

        var dataReader = JsonDataReader.Get(jsonReader);
        try
        {
            return readData(dataReader);
        }
        finally
        {
            JsonDataReader.Return(dataReader);
        }
    }

    public static T DeserializeData<T>(string json, Func<JsonDataReader, T> readData)
    {
        using var reader = new StringReader(json);

        return DeserializeData(reader, readData);
    }

    public static T DeserializeData<T>(byte[] bytes, Func<JsonDataReader, T> readData)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return DeserializeData(reader, readData);
    }

    public static T? DeserializeObject<T>(TextReader reader, ReadProperties<T> readProperties)
        => DeserializeData(reader, dataReader => dataReader.ReadObject(readProperties));

    public static T? DeserializeObject<T>(string json, ReadProperties<T> readProperties)
    {
        using var reader = new StringReader(json);

        return DeserializeObject(reader, readProperties);
    }

    public static T? DeserializeObject<T>(byte[] bytes, ReadProperties<T> readProperties)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return DeserializeObject(reader, readProperties);
    }

    public static T DeserializeNonNullObject<T>(TextReader reader, ReadProperties<T> readProperties)
        => DeserializeData(reader, dataReader => dataReader.ReadNonNullObject(readProperties));

    public static T DeserializeNonNullObject<T>(string json, ReadProperties<T> readProperties)
    {
        using var reader = new StringReader(json);

        return DeserializeNonNullObject(reader, readProperties);
    }

    public static T DeserializeNonNullObject<T>(byte[] bytes, ReadProperties<T> readProperties)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return DeserializeNonNullObject(reader, readProperties);
    }

    public static ImmutableArray<T> DeserializeArray<T>(TextReader reader, ReadValue<T> readValue)
        => DeserializeData(reader, r => r.ReadImmutableArray(readValue));

    public static ImmutableArray<T> DeserializeArray<T>(string json, ReadValue<T> readValue)
    {
        using var reader = new StringReader(json);

        return DeserializeArray(reader, readValue);
    }

    public static ImmutableArray<T> DeserializeArray<T>(byte[] bytes, ReadValue<T> readValue)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return DeserializeArray(reader, readValue);
    }
}
