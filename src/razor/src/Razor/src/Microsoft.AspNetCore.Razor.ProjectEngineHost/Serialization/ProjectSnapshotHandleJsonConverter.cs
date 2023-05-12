// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal partial class ProjectSnapshotHandleJsonConverter : ObjectJsonConverter<ProjectSnapshotHandle>
{
    public static readonly ProjectSnapshotHandleJsonConverter Instance = new();

    protected override ProjectSnapshotHandle ReadFromProperties(JsonDataReader reader)
    {
        Data data = default;
        reader.ReadProperties(ref data, Data.PropertyMap);

        return new(data.FilePath, data.Configuration, data.RootNamespace);
    }

    protected override void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandle value)
    {
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.WriteObject(nameof(value.Configuration), value.Configuration, ObjectWriters.WriteProperties);
        writer.Write(nameof(value.RootNamespace), value.RootNamespace);
    }
}
