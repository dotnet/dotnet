// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal sealed class RazorProjectInfo
{
    public string SerializedFilePath { get; }
    public string FilePath { get; }
    public RazorConfiguration? Configuration { get; }
    public string? RootNamespace { get; }
    public ProjectWorkspaceState? ProjectWorkspaceState { get; }
    public ImmutableArray<DocumentSnapshotHandle> Documents { get; }

    public RazorProjectInfo(
        string serializedFilePath,
        string filePath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        ProjectWorkspaceState? projectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> documents)
    {
        SerializedFilePath = serializedFilePath;
        FilePath = filePath;
        Configuration = configuration;
        RootNamespace = rootNamespace;
        ProjectWorkspaceState = projectWorkspaceState;
        Documents = documents.NullToEmpty();
    }
}
