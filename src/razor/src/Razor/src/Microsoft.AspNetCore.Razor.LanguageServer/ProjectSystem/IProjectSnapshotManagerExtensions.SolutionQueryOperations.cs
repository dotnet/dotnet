// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static partial class ProjectSnapshotManagerExtensions
{
    private sealed class SolutionQueryOperations(ProjectSnapshotManager projectManager) : ISolutionQueryOperations
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
}
