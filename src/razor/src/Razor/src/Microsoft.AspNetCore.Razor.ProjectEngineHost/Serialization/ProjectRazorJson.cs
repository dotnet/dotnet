// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

[JsonConverter(typeof(ProjectRazorJsonJsonConverter))]
internal sealed class ProjectRazorJson
{
    public ProjectRazorJson(
        string serializedOriginFilePath,
        string filePath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        ProjectWorkspaceState? projectWorkspaceState,
        IReadOnlyList<DocumentSnapshotHandle> documents)
    {
        SerializedFilePath = serializedOriginFilePath;
        FilePath = filePath;
        Configuration = configuration;
        RootNamespace = rootNamespace;
        ProjectWorkspaceState = projectWorkspaceState;
        Documents = documents;
    }

    public string SerializedFilePath { get; }

    public string FilePath { get; }

    public RazorConfiguration? Configuration { get; }

    public string? RootNamespace { get; }

    public ProjectWorkspaceState? ProjectWorkspaceState { get; }

    public IReadOnlyList<DocumentSnapshotHandle> Documents { get; }
}
