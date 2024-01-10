// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.IO;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal sealed class RazorProjectInfo
{
    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            RazorProjectInfoResolver.Instance,
            StandardResolver.Instance));

    public string SerializedFilePath { get; }
    public string FilePath { get; }
    public RazorConfiguration Configuration { get; }
    public string? RootNamespace { get; }
    public string DisplayName { get; }
    public ProjectWorkspaceState ProjectWorkspaceState { get; }
    public ImmutableArray<DocumentSnapshotHandle> Documents { get; }

    public RazorProjectInfo(
        string serializedFilePath,
        string filePath,
        RazorConfiguration configuration,
        string? rootNamespace,
        string displayName,
        ProjectWorkspaceState projectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> documents)
    {
        SerializedFilePath = serializedFilePath;
        FilePath = filePath;
        Configuration = configuration;
        RootNamespace = rootNamespace;
        DisplayName = displayName;
        ProjectWorkspaceState = projectWorkspaceState;
        Documents = documents.NullToEmpty();
    }

    public void SerializeTo(Stream stream)
        => MessagePackSerializer.Serialize(stream, this, s_options);

    public static RazorProjectInfo? DeserializeFrom(Stream stream)
        => MessagePackSerializer.Deserialize<RazorProjectInfo>(stream, s_options);
}
