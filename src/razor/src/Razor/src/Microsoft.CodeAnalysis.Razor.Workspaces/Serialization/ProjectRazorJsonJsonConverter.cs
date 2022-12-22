// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;

internal class ProjectRazorJsonJsonConverter : JsonConverter
{
    public static readonly ProjectRazorJsonJsonConverter Instance = new ProjectRazorJsonJsonConverter();
    private const string SerializationFormatPropertyName = "SerializationFormat";

    public override bool CanConvert(Type objectType)
    {
        return typeof(ProjectRazorJson).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        var (_, _, _, _, serializationFormat, serializationFilePath, filePath, configuration, rootNamespace, projectWorkspaceState, documents) = reader.ReadProperties(static (propertyName, arg) =>
        {
            var (reader, objectType, existingValue, serializer, serializationFormat, serializationFilePath, filePath, configuration, rootNamespace, projectWorkspaceState, documents) = (arg.reader, arg.objectType, arg.existingValue, arg.serializer, arg.serializationFormat, arg.serializationFilePath, arg.filePath, arg.configuration, arg.rootNamespace, arg.projectWorkspaceState, arg.documents);
            switch (propertyName)
            {
                case SerializationFormatPropertyName:
                    if (reader.Read())
                    {
                        serializationFormat = (string)reader.Value;
                    }

                    break;

                case nameof(ProjectRazorJson.SerializedFilePath):
                    if (reader.Read())
                    {
                        serializationFilePath = (string)reader.Value;
                    }

                    break;
                case nameof(ProjectRazorJson.FilePath):
                    if (reader.Read())
                    {
                        filePath = (string)reader.Value;
                    }

                    break;
                case nameof(ProjectRazorJson.Configuration):
                    if (reader.Read())
                    {
                        configuration = RazorConfigurationJsonConverter.Instance.ReadJson(reader, objectType, existingValue, serializer) as RazorConfiguration;
                    }

                    break;
                case nameof(ProjectRazorJson.RootNamespace):
                    if (reader.Read())
                    {
                        rootNamespace = (string)reader.Value;
                    }

                    break;
                case nameof(ProjectRazorJson.ProjectWorkspaceState):
                    if (reader.Read())
                    {
                        projectWorkspaceState = serializer.Deserialize<ProjectWorkspaceState>(reader);
                    }

                    break;
                case nameof(ProjectRazorJson.Documents):
                    if (reader.Read())
                    {
                        documents = serializer.Deserialize<DocumentSnapshotHandle[]>(reader);
                    }

                    break;
            }

            return (reader, objectType, existingValue, serializer, serializationFormat, serializationFilePath, filePath, configuration, rootNamespace, projectWorkspaceState, documents);
        }, (reader, objectType, existingValue, serializer, serializationFormat: (string)null, serializationFilePath: (string)null, filePath: (string)null, configuration: (RazorConfiguration)null, rootNamespace: (string)null, projectWorkspaceState: (ProjectWorkspaceState)null, documents: (DocumentSnapshotHandle[])null));

        // We need to add a serialization format to the project response to indicate that this version of the code is compatible with what's being serialized.
        // This scenario typically happens when a user has an incompatible serialized project snapshot but is using the latest Razor bits.

        if (string.IsNullOrEmpty(serializationFormat) || serializationFormat != ProjectSerializationFormat.Version)
        {
            // Unknown serialization format.
            return null;
        }

        return new ProjectRazorJson(serializationFilePath, filePath, configuration, rootNamespace, projectWorkspaceState, documents);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var projectRazorJson = (ProjectRazorJson)value;

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ProjectRazorJson.SerializedFilePath));
        writer.WriteValue(projectRazorJson.SerializedFilePath);

        writer.WritePropertyName(nameof(ProjectRazorJson.FilePath));
        writer.WriteValue(projectRazorJson.FilePath);

        if (projectRazorJson.Configuration is null)
        {
            writer.WritePropertyName(nameof(ProjectRazorJson.Configuration));
            writer.WriteNull();
        }
        else
        {
            writer.WritePropertyName(nameof(ProjectRazorJson.Configuration));
            serializer.Serialize(writer, projectRazorJson.Configuration);
        }

        if (projectRazorJson.ProjectWorkspaceState is null)
        {
            writer.WritePropertyName(nameof(ProjectRazorJson.ProjectWorkspaceState));
            writer.WriteNull();
        }
        else
        {
            writer.WritePropertyName(nameof(ProjectRazorJson.ProjectWorkspaceState));
            serializer.Serialize(writer, projectRazorJson.ProjectWorkspaceState);
        }

        writer.WritePropertyName(nameof(ProjectRazorJson.RootNamespace));
        writer.WriteValue(projectRazorJson.RootNamespace);

        writer.WritePropertyName(nameof(ProjectRazorJson.Documents));
        serializer.Serialize(writer, projectRazorJson.Documents);

        writer.WritePropertyName(SerializationFormatPropertyName);
        writer.WriteValue(ProjectSerializationFormat.Version);

        writer.WriteEndObject();
    }
}
