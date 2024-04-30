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
    public static bool TryResolveAllProjects(
        this ISnapshotResolver snapshotResolver,
        string documentFilePath,
        out ImmutableArray<IProjectSnapshot> projects)
    {
        var potentialProjects = snapshotResolver.FindPotentialProjects(documentFilePath);

        using var builder = new PooledArrayBuilder<IProjectSnapshot>(capacity: potentialProjects.Length);

        foreach (var project in potentialProjects)
        {
            if (project.GetDocument(documentFilePath) is not null)
            {
                builder.Add(project);
            }
        }

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var miscProject = snapshotResolver.GetMiscellaneousProject();
        if (miscProject.GetDocument(normalizedDocumentPath) is not null)
        {
            builder.Add(miscProject);
        }

        projects = builder.DrainToImmutable();
        return projects.Length > 0;
    }
}
