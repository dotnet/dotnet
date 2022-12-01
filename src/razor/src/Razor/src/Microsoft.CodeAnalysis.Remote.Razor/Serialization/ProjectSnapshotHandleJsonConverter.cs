// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Serialization;

internal class ProjectSnapshotHandleJsonConverter : JsonConverter
{
    public static readonly ProjectSnapshotHandleJsonConverter Instance = new();

    public override bool CanConvert(Type objectType)
    {
        return typeof(ProjectSnapshotHandle).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        var (_, _, _, _, filePath, configuration, rootNamespace) = reader.ReadProperties(static (propertyName, arg) =>
        {
            var (reader, objectType, existingValue, serializer, filePath, configuration, rootNamespace) = (arg.reader, arg.objectType, arg.existingValue, arg.serializer, arg.filePath, arg.configuration, arg.rootNamespace);
            switch (propertyName)
            {
                case nameof(ProjectSnapshotHandle.FilePath):
                    if (reader.Read())
                    {
                        filePath = (string)reader.Value!;
                    }

                    break;
                case nameof(ProjectSnapshotHandle.Configuration):
                    if (reader.Read())
                    {
                        configuration = RazorConfigurationJsonConverter.Instance.ReadJson(reader, objectType, existingValue, serializer) as RazorConfiguration;
                    }

                    break;
                case nameof(ProjectSnapshotHandle.RootNamespace):
                    if (reader.Read())
                    {
                        rootNamespace = (string)reader.Value!;
                    }

                    break;
            }

            return (reader, objectType, existingValue, serializer, filePath, configuration, rootNamespace);
        }, (reader, objectType, existingValue, serializer, filePath: (string?)null, configuration: (RazorConfiguration?)null, rootNamespace: (string?)null));

        return new ProjectSnapshotHandle(filePath!, configuration, rootNamespace);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var handle = (ProjectSnapshotHandle)value!;

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ProjectSnapshotHandle.FilePath));
        writer.WriteValue(handle.FilePath);

        if (handle.Configuration is null)
        {
            writer.WritePropertyName(nameof(ProjectSnapshotHandle.Configuration));
            writer.WriteNull();
        }
        else
        {
            writer.WritePropertyName(nameof(ProjectSnapshotHandle.Configuration));
            serializer.Serialize(writer, handle.Configuration);
        }

        if (handle.RootNamespace is null)
        {
            writer.WritePropertyName(nameof(ProjectSnapshotHandle.RootNamespace));
            writer.WriteNull();
        }
        else
        {
            writer.WritePropertyName(nameof(ProjectSnapshotHandle.RootNamespace));
            writer.WriteValue(handle.RootNamespace);
        }

        writer.WriteEndObject();
    }
}
