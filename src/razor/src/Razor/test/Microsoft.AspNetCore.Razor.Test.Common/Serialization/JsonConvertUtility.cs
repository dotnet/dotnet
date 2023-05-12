// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Test.Common.Serialization;

internal static class JsonConvertUtility
{
    public static string? SerializeObject<T>(T value, params JsonConverter[] converters)
        => JsonConvert.SerializeObject(value, converters);

    public static string? SerializeObject<T>(T value, WriteProperties<T> writeProperties)
    {
        var builder = new StringBuilder();
        var stringWriter = new StringWriter(builder);
        var jsonWriter = new JsonTextWriter(stringWriter);
        var dataWriter = JsonDataWriter.Get(jsonWriter);
        try
        {
            dataWriter.WriteObject(value, writeProperties);
        }
        finally
        {
            JsonDataWriter.Return(dataWriter);
            jsonWriter.Close();
            stringWriter.Close();
        }

        return builder.ToString();
    }

    public static string? Serialize(RazorConfiguration configuration)
        => SerializeObject(configuration, ObjectWriters.WriteProperties);

    public static T? DeserializeObject<T>(string json, params JsonConverter[] converters)
        => JsonConvert.DeserializeObject<T>(json, converters);

    public static T? DeserializeObject<T>(string json, ReadProperties<T> readProperties)
    {
        var stringReader = new StringReader(json);
        var jsonReader = new JsonTextReader(stringReader);
        var dataReader = JsonDataReader.Get(jsonReader);
        try
        {
            jsonReader.Read();

            return dataReader.ReadObject(readProperties);
        }
        finally
        {
            JsonDataReader.Return(dataReader);
            jsonReader.Close();
            stringReader.Close();
        }
    }

    public static RazorConfiguration? DeserializeConfiguration(string json)
        => DeserializeObject(json, ObjectReaders.ReadConfigurationFromProperties);
}
