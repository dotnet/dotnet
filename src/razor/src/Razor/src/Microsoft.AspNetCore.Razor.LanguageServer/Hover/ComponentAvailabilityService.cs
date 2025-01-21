// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Tooltip;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

internal sealed class ComponentAvailabilityService(ProjectSnapshotManager projectManager) : AbstractComponentAvailabilityService
{
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    protected override ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath)
    {
        using var projects = new PooledArrayBuilder<IProjectSnapshot>();

        foreach (var project in _projectManager.GetProjects())
        {
            // Always exclude the miscellaneous project.
            if (project.Key == MiscFilesProject.Key)
            {
                continue;
            }

            if (project.ContainsDocument(documentFilePath))
            {
                projects.Add(project);
            }
        }

        return projects.DrainToImmutable();
    }
}
