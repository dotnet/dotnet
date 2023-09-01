// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Serialization.Json.Converters;

internal partial class ProjectSnapshotHandleJsonConverter : ObjectJsonConverter<ProjectSnapshotHandle>
{
    public static readonly ProjectSnapshotHandleJsonConverter Instance = new();

    private ProjectSnapshotHandleJsonConverter()
    {
    }

    protected override ProjectSnapshotHandle ReadFromProperties(JsonDataReader reader)
        => ObjectReaders.ReadProjectSnapshotHandleFromProperties(reader);

    protected override void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandle value)
        => ObjectWriters.WriteProperties(writer, value);
}
