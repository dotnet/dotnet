// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IProjectSnapshotExtensions
{
    public static RazorProjectInfo ToRazorProjectInfo(this IProjectSnapshot project)
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
            documents: documents.DrainToImmutable());
    }

    public static ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(this IProjectSnapshot projectSnapshot, IDocumentSnapshot document)
    {
        if (projectSnapshot is not ProjectSnapshot project)
        {
            throw new InvalidOperationException("This method can only be called with a ProjectSnapshot.");
        }

        return project.GetRelatedDocuments(document);
    }
}
