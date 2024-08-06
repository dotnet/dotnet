// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

[Export(typeof(IProjectCollectionResolver)), Shared]
[method: ImportingConstructor]
internal class RemoteProjectCollectionResolver(ProjectSnapshotFactory projectSnapshotFactory) : IProjectCollectionResolver
{
    public IEnumerable<IProjectSnapshot> EnumerateProjects(IDocumentSnapshot snapshot)
    {
        Debug.Assert(snapshot is RemoteDocumentSnapshot);

        var projects = ((RemoteDocumentSnapshot)snapshot).TextDocument.Project.Solution.Projects;

        foreach (var project in projects)
        {
            yield return projectSnapshotFactory.GetOrCreate(project);
        }
    }
}
