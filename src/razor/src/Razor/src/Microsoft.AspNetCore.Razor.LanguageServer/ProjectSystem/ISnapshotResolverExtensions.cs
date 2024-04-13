// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static class ISnapshotResolverExtensions
{
    public static async Task<ImmutableArray<IProjectSnapshot>> TryResolveAllProjectsAsync(
        this ISnapshotResolver snapshotResolver,
        string documentFilePath,
        CancellationToken cancellationToken)
    {
        var potentialProjects = snapshotResolver.FindPotentialProjects(documentFilePath);

        using var projects = new PooledArrayBuilder<IProjectSnapshot>(capacity: potentialProjects.Length);

        foreach (var project in potentialProjects)
        {
            if (project.GetDocument(documentFilePath) is not null)
            {
                projects.Add(project);
            }
        }

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var miscProject = await snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);
        if (miscProject.GetDocument(normalizedDocumentPath) is not null)
        {
            projects.Add(miscProject);
        }

        return projects.DrainToImmutable();
    }
}
