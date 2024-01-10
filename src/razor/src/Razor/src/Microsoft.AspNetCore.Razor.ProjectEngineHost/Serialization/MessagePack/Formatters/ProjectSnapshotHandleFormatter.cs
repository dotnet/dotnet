// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class ProjectSnapshotHandleFormatter : TopLevelFormatter<ProjectSnapshotHandle>
{
    public static readonly TopLevelFormatter<ProjectSnapshotHandle> Instance = new ProjectSnapshotHandleFormatter();

    private ProjectSnapshotHandleFormatter()
    {
    }

    public override ProjectSnapshotHandle Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(3);

        var id = GuidFormatter.Instance.Deserialize(ref reader, options);
        var projectId = ProjectId.CreateFromSerialized(id);

        var configuration = reader.DeserializeOrNull<RazorConfiguration>(options) ?? RazorConfiguration.Default;
        var rootNamespace = CachedStringFormatter.Instance.Deserialize(ref reader, options);

        return new(projectId, configuration, rootNamespace);
    }

    public override void Serialize(ref MessagePackWriter writer, ProjectSnapshotHandle value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(3);

        GuidFormatter.Instance.Serialize(ref writer, value.ProjectId.Id, options);
        writer.Serialize(value.Configuration, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.RootNamespace, options);
    }
}
