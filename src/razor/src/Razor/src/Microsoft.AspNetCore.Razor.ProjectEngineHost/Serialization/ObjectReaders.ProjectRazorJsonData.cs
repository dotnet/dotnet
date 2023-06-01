// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal static partial class ObjectReaders
{
    private record struct ProjectRazorJsonData(
        string SerializedFilePath,
        string FilePath,
        RazorConfiguration? Configuration,
        string? RootNamespace,
        ProjectWorkspaceState? ProjectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> Documents,
        string? SerializationFormat)
    {
        public static readonly PropertyMap<ProjectRazorJsonData> PropertyMap = new(
            (nameof(SerializedFilePath), ReadSerializedFilePath),
            (nameof(FilePath), ReadFilePath),
            (nameof(Configuration), ReadConfiguration),
            (nameof(RootNamespace), ReadRootNamespace),
            (nameof(ProjectWorkspaceState), ReadProjectWorkspaceState),
            (nameof(Documents), ReadDocuments),
            (nameof(SerializationFormat), ReadSerializationFormat));

        private static void ReadSerializedFilePath(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.SerializedFilePath = reader.ReadNonNullString();

        private static void ReadFilePath(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.FilePath = reader.ReadNonNullString();

        private static void ReadConfiguration(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.Configuration = reader.ReadObject(ReadConfigurationFromProperties);

        private static void ReadRootNamespace(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.RootNamespace = reader.ReadString();

        private static void ReadProjectWorkspaceState(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.ProjectWorkspaceState = reader.ReadObject(ReadProjectWorkspaceStateFromProperties);

        private static void ReadDocuments(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.Documents = reader.ReadImmutableArray(static r => r.ReadNonNullObject(ReadDocumentSnapshotHandleFromProperties));

        private static void ReadSerializationFormat(JsonDataReader reader, ref ProjectRazorJsonData data)
            => data.SerializationFormat = reader.ReadString();
    }
}
