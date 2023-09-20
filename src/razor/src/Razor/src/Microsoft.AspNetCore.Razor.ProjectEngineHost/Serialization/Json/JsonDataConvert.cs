// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static class JsonDataConvert
{
    public static void SerializeData(TextWriter writer, Action<JsonDataWriter> writeData)
    {
        using var jsonWriter = new JsonTextWriter(writer) { CloseOutput = false };

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

    public static void SerializeObject<T>(TextWriter writer, T? value, WriteProperties<T> writeProperties)
    {
        SerializeData(writer, dataWriter => dataWriter.WriteObject(value, writeProperties));
    }

    public static string SerializeObject<T>(T? value, WriteProperties<T> writeProperties)
    {
        var builder = new StringBuilder();

        using (var writer = new StringWriter(builder))
        {
            SerializeObject(writer, value, writeProperties);
        }

        return builder.ToString();
    }

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

    public static T? DeserializeObject<T>(TextReader reader, ReadProperties<T> readProperties)
    {
        return DeserializeData(reader, dataReader => dataReader.ReadObject(readProperties));
    }

    public static T? DeserializeObject<T>(string json, ReadProperties<T> readProperties)
    {
        using var reader = new StringReader(json);

        return DeserializeObject(reader, readProperties);
    }
}
