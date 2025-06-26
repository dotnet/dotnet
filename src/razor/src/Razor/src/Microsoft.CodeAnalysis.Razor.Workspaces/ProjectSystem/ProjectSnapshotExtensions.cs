// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Serialization;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class ProjectSnapshotExtensions
{
    public static IDocumentSnapshot? GetDocument(this IProjectSnapshot project, string filePath)
        => project.TryGetDocument(filePath, out var result)
            ? result
            : null;

    public static IDocumentSnapshot GetRequiredDocument(this IProjectSnapshot project, string filePath)
        => project.GetDocument(filePath).AssumeNotNull();

    public static DocumentSnapshot? GetDocument(this ProjectSnapshot project, string filePath)
        => project.TryGetDocument(filePath, out var result)
            ? result
            : null;

    public static DocumentSnapshot GetRequiredDocument(this ProjectSnapshot project, string filePath)
        => project.GetDocument(filePath).AssumeNotNull();

    public static RazorProjectInfo ToRazorProjectInfo(this ProjectSnapshot project)
    {
        using var documents = new PooledArrayBuilder<DocumentSnapshotHandle>();

        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (project.TryGetDocument(documentFilePath, out var document))
            {
                var documentHandle = document.ToHandle();

                documents.Add(documentHandle);
            }
        }

        return new RazorProjectInfo(
            projectKey: project.Key,
            filePath: project.FilePath,
            configuration: project.Configuration,
            rootNamespace: project.RootNamespace,
            displayName: project.DisplayName,
            projectWorkspaceState: project.ProjectWorkspaceState,
            documents: documents.ToImmutableAndClear());
    }
}
