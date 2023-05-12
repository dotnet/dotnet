// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

namespace Microsoft.VisualStudio.LiveShare.Razor.Serialization;

internal class ProjectSnapshotHandleProxyJsonConverter : ObjectJsonConverter<ProjectSnapshotHandleProxy>
{
    public static readonly ProjectSnapshotHandleProxyJsonConverter Instance = new();

    protected override ProjectSnapshotHandleProxy ReadFromProperties(JsonDataReader reader)
    {
        Data data = default;
        reader.ReadProperties(ref data, Data.PropertyMap);

        return new ProjectSnapshotHandleProxy(data.FilePath, data.Configuration, data.RootNamespace, data.ProjectWorkspaceState);
    }

    private record struct Data(
        Uri FilePath,
        RazorConfiguration Configuration,
        string? RootNamespace,
        ProjectWorkspaceState? ProjectWorkspaceState)
    {
        public static readonly PropertyMap<Data> PropertyMap = new(
            (nameof(FilePath), ReadFilePath),
            (nameof(Configuration), ReadConfiguration),
            (nameof(RootNamespace), ReadRootNamespace),
            (nameof(ProjectWorkspaceState), ReadProjectWorkspaceState));

        private static void ReadFilePath(JsonDataReader reader, ref Data data)
            => data.FilePath = reader.ReadNonNullUri();

        private static void ReadConfiguration(JsonDataReader reader, ref Data data)
            => data.Configuration = reader.ReadNonNullObject(ObjectReaders.ReadConfigurationFromProperties);

        private static void ReadRootNamespace(JsonDataReader reader, ref Data data)
            => data.RootNamespace = reader.ReadString();

        private static void ReadProjectWorkspaceState(JsonDataReader reader, ref Data data)
            => data.ProjectWorkspaceState = reader.ReadObject(ObjectReaders.ReadProjectWorkspaceStateFromProperties);
    }

    protected override void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandleProxy value)
    {
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.Write(nameof(value.RootNamespace), value.RootNamespace);
        writer.WriteObject(nameof(value.ProjectWorkspaceState), value.ProjectWorkspaceState, ObjectWriters.WriteProperties);
        writer.WriteObject(nameof(value.Configuration), value.Configuration, ObjectWriters.WriteProperties);
    }
}
