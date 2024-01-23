// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using System.Threading;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test;

internal class TestProjectWorkspaceStateGenerator : IProjectWorkspaceStateGenerator
{
    private readonly List<TestUpdate> _updates;

    public TestProjectWorkspaceStateGenerator()
    {
        _updates = new List<TestUpdate>();
    }

    public IReadOnlyList<TestUpdate> UpdateQueue => _updates;

    public void Update(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        var update = new TestUpdate(workspaceProject, projectSnapshot, cancellationToken);
        _updates.Add(update);
    }

    public void ClearQueue()
    {
        _updates.Clear();
    }

    public record TestUpdate(Project WorkspaceProject, IProjectSnapshot ProjectSnapshot, CancellationToken CancellationToken);
}
