// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class RazorProjectInfoFormatter : TopLevelFormatter<RazorProjectInfo>
{
    public static readonly TopLevelFormatter<RazorProjectInfo> Instance = new RazorProjectInfoFormatter();

    private RazorProjectInfoFormatter()
    {
    }

    public override RazorProjectInfo Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(7);

        var version = reader.ReadInt32();

        if (version != SerializationFormat.Version)
        {
            throw new RazorProjectInfoSerializationException(SR.Unsupported_razor_project_info_version_encountered);
        }

        var projectKeyId = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var filePath = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var configuration = reader.DeserializeOrNull<RazorConfiguration>(options) ?? RazorConfiguration.Default;
        var projectWorkspaceState = reader.DeserializeOrNull<ProjectWorkspaceState>(options) ?? ProjectWorkspaceState.Default;
        var rootNamespace = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var displayName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var documents = reader.Deserialize<ImmutableArray<DocumentSnapshotHandle>>(options);

        return new RazorProjectInfo(new ProjectKey(projectKeyId), filePath, configuration, rootNamespace, displayName, projectWorkspaceState, documents);
    }

    public override void Serialize(ref MessagePackWriter writer, RazorProjectInfo value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(7);

        writer.Write(SerializationFormat.Version);
        CachedStringFormatter.Instance.Serialize(ref writer, value.ProjectKey.Id, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.FilePath, options);
        writer.Serialize(value.Configuration, options);
        writer.Serialize(value.ProjectWorkspaceState, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.RootNamespace, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.DisplayName, options);
        writer.Serialize(value.Documents, options);
    }
}
