// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor;
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
                var documentHandle = new DocumentSnapshotHandle(
                    document.FilePath.AssumeNotNull(),
                    document.TargetPath.AssumeNotNull(),
                    document.FileKind.AssumeNotNull());

                documents.Add(documentHandle);
            }
        }

        return new ProjectRazorJson(
            serializedOriginFilePath: serializedFilePath,
            filePath: project.FilePath,
            configuration: project.Configuration,
            rootNamespace: project.RootNamespace,
            projectWorkspaceState: project.ProjectWorkspaceState,
            documents: documents);
    }
}
