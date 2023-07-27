// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static class ISnapshotResolverExtensions
{
    public static bool TryResolveAllProjects(this ISnapshotResolver snapshotResolver, string documentFilePath, out IProjectSnapshot[] projectSnapshots)
    {
        var potentialProjects = snapshotResolver.FindPotentialProjects(documentFilePath);

        using var _ = ListPool<IProjectSnapshot>.GetPooledObject(out var projects);

        foreach (var project in potentialProjects)
        {
            if (project.GetDocument(documentFilePath) is not null)
            {
                projects.Add(project);
            }
        }

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var miscProject = snapshotResolver.GetMiscellaneousProject();
        if (miscProject.GetDocument(normalizedDocumentPath) is not null)
        {
            projects.Add(miscProject);
        }

        projectSnapshots = projects.ToArray();

        return projects.Count > 0;
    }
}
