// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test;

internal static class IProjectSnapshotManagerExtensions
{
    public static ISolutionQueryOperations GetQueryOperations(this ProjectSnapshotManager projectManager)
        => new TestSolutionQueryOperations(projectManager);
}

file sealed class TestSolutionQueryOperations(ProjectSnapshotManager projectManager) : ISolutionQueryOperations
{
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    public IEnumerable<IProjectSnapshot> GetProjects()
    {
        return _projectManager.GetProjects().Cast<IProjectSnapshot>();
    }

    public ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath)
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
