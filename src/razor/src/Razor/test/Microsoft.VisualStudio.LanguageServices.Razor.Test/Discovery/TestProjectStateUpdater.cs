// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal class TestProjectStateUpdater : IProjectStateUpdater
{
    private readonly List<TestUpdate> _updates = [];

    public IReadOnlyList<TestUpdate> Updates => _updates;

    public void EnqueueUpdate(ProjectKey key, ProjectId? id)
    {
        var update = new TestUpdate(key, id);
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

    public record TestUpdate(ProjectKey Key, ProjectId? Id)
    {
        public bool IsCancelled { get; set; }

        public override string ToString()
        {
            using var _ = StringBuilderPool.GetPooledObject(out var builder);

            builder.Append($"{{{nameof(Id)} = ");

            if (Id is null)
            {
                builder.Append("<null>");
            }
            else
            {
                builder.Append(Id);
            }

            builder.Append($", {nameof(Key)} = {Key}}}");

            return builder.ToString();
        }
    }
}
