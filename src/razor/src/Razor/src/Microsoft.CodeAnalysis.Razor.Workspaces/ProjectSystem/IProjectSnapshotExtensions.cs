// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.ProjectSystem;

internal static class IProjectSnapshotExtensions
{
    public static ProjectRazorJson ToProjectRazorJson(this IProjectSnapshot project, string serializedFilePath)
    {
        var documents = new List<DocumentSnapshotHandle>();
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (project.GetDocument(documentFilePath) is { } document)
            {
                var documentHandle = document.ToHandle();

                documents.Add(documentHandle);
            }
        }

        return new ProjectRazorJson(
            serializedFilePath: serializedFilePath,
            filePath: project.FilePath,
            configuration: project.Configuration,
            rootNamespace: project.RootNamespace,
            projectWorkspaceState: project.ProjectWorkspaceState,
            documents: documents);
    }
}
