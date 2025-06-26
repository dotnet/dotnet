// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal sealed class TestComponentAvailabilityService(ProjectSnapshotManager projectManager) : AbstractComponentAvailabilityService
{
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    protected override ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath)
    {
        using var projects = new PooledArrayBuilder<IProjectSnapshot>();

        foreach (var project in _projectManager.GetProjects())
        {
            if (project.ContainsDocument(documentFilePath))
            {
                projects.Add(project);
            }
        }

        return projects.ToImmutableAndClear();
    }
}

