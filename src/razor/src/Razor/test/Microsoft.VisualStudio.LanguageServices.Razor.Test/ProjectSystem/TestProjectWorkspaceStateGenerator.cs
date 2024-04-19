// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

internal class TestProjectWorkspaceStateGenerator : IProjectWorkspaceStateGenerator
{
    private readonly List<TestUpdate> _updates = [];

    public IReadOnlyList<TestUpdate> Updates => _updates;

    public void EnqueueUpdate(Project? workspaceProject, IProjectSnapshot projectSnapshot)
    {
        var update = new TestUpdate(workspaceProject, projectSnapshot);
        _updates.Add(update);
    }

    public void CancelUpdates()
    {
        foreach (var update in _updates)
        {
            update.IsCancelled = true;
        }
    }

    public void Clear()
    {
        _updates.Clear();
    }

    public record TestUpdate(Project? WorkspaceProject, IProjectSnapshot ProjectSnapshot)
    {
        public bool IsCancelled { get; set; }

        public override string ToString()
        {
            using var _ = StringBuilderPool.GetPooledObject(out var builder);

            builder.Append($"{{{nameof(WorkspaceProject)} = ");

            if (WorkspaceProject is null)
            {
                builder.Append("<null>");
            }
            else
            {
                builder.Append(WorkspaceProject.Name);
            }

            builder.Append($", {nameof(ProjectSnapshot)} = {ProjectSnapshot.DisplayName}}}");

            return builder.ToString();
        }
    }
}
