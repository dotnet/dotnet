// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using System.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Test;

internal class TestProjectWorkspaceStateGenerator : ProjectWorkspaceStateGenerator
{
    private readonly List<(Project workspaceProject, IProjectSnapshot projectSnapshot)> _updates;

    public TestProjectWorkspaceStateGenerator()
    {
        _updates = new List<(Project workspaceProject, IProjectSnapshot projectSnapshot)>();
    }

    public IReadOnlyList<(Project workspaceProject, IProjectSnapshot projectSnapshot)> UpdateQueue => _updates;

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
    }

    public override void Update(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        var update = (workspaceProject, projectSnapshot);
        _updates.Add(update);
    }
}
