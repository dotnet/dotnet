// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static partial class IProjectSnapshotManagerExtensions
{
    private sealed class SolutionQueryOperations(IProjectSnapshotManager projectManager) : ISolutionQueryOperations
    {
        private readonly IProjectSnapshotManager _projectManager = projectManager;

        public IEnumerable<IProjectSnapshot> GetProjects()
        {
            return _projectManager.GetProjects();
        }

        public ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath)
        {
            using var projects = new PooledArrayBuilder<IProjectSnapshot>();

            foreach (var project in _projectManager.GetProjects())
            {
                // Always exclude the miscellaneous project.
                if (project.Key == MiscFilesHostProject.Instance.Key)
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
