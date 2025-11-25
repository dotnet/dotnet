// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if JSONSERIALIZATION_PROJECTSYSTEM
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;

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
        writer.WriteIfNotDefault(nameof(value.FileKind), (int)value.FileKind, defaultValue: (int)RazorFileKind.Component);
    }

    public static void Write(JsonDataWriter writer, ProjectWorkspaceState? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectWorkspaceState value)
    {
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.TagHelpers), value.TagHelpers, Write);
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
