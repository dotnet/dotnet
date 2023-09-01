// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using MessagePack;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class ProjectSnapshotHandleFormatter : MessagePackFormatter<ProjectSnapshotHandle>
{
    public static readonly MessagePackFormatter<ProjectSnapshotHandle> Instance = new ProjectSnapshotHandleFormatter();

    private ProjectSnapshotHandleFormatter()
    {
    }

    public override ProjectSnapshotHandle Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var projectIdString = DeserializeString(ref reader, options);
        var configuration = RazorConfigurationFormatter.Instance.AllowNull.Deserialize(ref reader, options);

        var rootNamespace = AllowNull.DeserializeString(ref reader, options);

        var projectId = ProjectId.CreateFromSerialized(Guid.Parse(projectIdString));

        return new(projectId, configuration, rootNamespace);
    }

    public override void Serialize(ref MessagePackWriter writer, ProjectSnapshotHandle value, MessagePackSerializerOptions options)
    {
        writer.Write(value.ProjectId.Id.ToString());

        if (value.Configuration is { } configuration)
        {
            RazorConfigurationFormatter.Instance.Serialize(ref writer, configuration, options);
        }
        else
        {
            writer.WriteNil();
        }

        writer.Write(value.RootNamespace);
    }
}
