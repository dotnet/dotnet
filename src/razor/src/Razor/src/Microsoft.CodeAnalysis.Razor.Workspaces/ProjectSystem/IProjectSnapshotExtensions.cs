// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.ProjectSystem;

internal static class IProjectSnapshotExtensions
{
    public static RazorProjectInfo ToRazorProjectInfo(this IProjectSnapshot project, string serializedFilePath)
    {
        using var documents = new PooledArrayBuilder<DocumentSnapshotHandle>();

        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (project.GetDocument(documentFilePath) is { } document)
            {
                var documentHandle = document.ToHandle();

                documents.Add(documentHandle);
            }
        }

        return new RazorProjectInfo(
            serializedFilePath: serializedFilePath,
            filePath: project.FilePath,
            configuration: project.Configuration,
            rootNamespace: project.RootNamespace,
            projectWorkspaceState: project.ProjectWorkspaceState,
            documents: documents.DrainToImmutable());
    }
}
