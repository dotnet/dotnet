// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if JSONSERIALIZATION_PROJECTSYSTEM
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class ObjectWriters
{
    public static void Write(JsonDataWriter writer, ProjectSnapshotHandle? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandle value)
    {
        writer.Write(nameof(value.ProjectId), value.ProjectId.Id.ToString());
        writer.WriteObject(nameof(value.Configuration), value.Configuration, WriteProperties);
        writer.WriteIfNotNull(nameof(value.RootNamespace), value.RootNamespace);
    }

    public static void Write(JsonDataWriter writer, DocumentSnapshotHandle? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, DocumentSnapshotHandle value)
    {
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.Write(nameof(value.TargetPath), value.TargetPath);
        writer.Write(nameof(value.FileKind), value.FileKind);
    }

    public static void Write(JsonDataWriter writer, ProjectWorkspaceState? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectWorkspaceState value)
    {
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.TagHelpers), value.TagHelpers, Write);
    }

    public static void Write(JsonDataWriter writer, RazorProjectInfo value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorProjectInfo value)
    {
        writer.Write(WellKnownPropertyNames.Version, SerializationFormat.Version);
        writer.Write(nameof(value.ProjectKey), value.ProjectKey.Id);
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.WriteObject(nameof(value.Configuration), value.Configuration, WriteProperties);
        writer.WriteObject(nameof(value.ProjectWorkspaceState), value.ProjectWorkspaceState, WriteProperties);
        writer.Write(nameof(value.RootNamespace), value.RootNamespace);
        writer.WriteArray(nameof(value.Documents), value.Documents, Write);
    }
}
#endif
