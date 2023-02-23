// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;

[JsonConverter(typeof(ProjectRazorJsonJsonConverter))]
internal sealed class ProjectRazorJson
{
    public ProjectRazorJson(string serializedFilePath, IProjectSnapshot project)
    {
        SerializedFilePath = serializedFilePath;
        FilePath = project.FilePath;
        Configuration = project.Configuration;
        RootNamespace = project.RootNamespace;
        ProjectWorkspaceState = project.ProjectWorkspaceState;

        var documents = new List<DocumentSnapshotHandle>();
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (project.GetDocument(documentFilePath) is { } document)
            {
                var documentHandle = new DocumentSnapshotHandle(
                    document.FilePath.AssumeNotNull(),
                    document.TargetPath.AssumeNotNull(),
                    document.FileKind.AssumeNotNull());

                documents.Add(documentHandle);
            }
        }

        Documents = documents;
    }

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
