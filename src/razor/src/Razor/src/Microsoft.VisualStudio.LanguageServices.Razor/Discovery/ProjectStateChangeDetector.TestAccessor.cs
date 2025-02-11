// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal partial class ProjectStateChangeDetector
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(ProjectStateChangeDetector instance)
    {
        public void CancelExistingWork()
        {
            instance._workQueue.CancelExistingWork();
        }

        public async Task WaitUntilCurrentBatchCompletesAsync()
        {
            await instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
        }

        public Task ListenForWorkspaceChangesAsync(params WorkspaceChangeKind[] kinds)
        {
            if (instance._workspaceChangedListener is not null)
            {
                throw new InvalidOperationException($"There's already a {nameof(WorkspaceChangedListener)} installed.");
            }

            var listener = new WorkspaceChangedListener(kinds.ToImmutableArray());
            instance._workspaceChangedListener = listener;

            return listener.Task;
        }

        public void WorkspaceChanged(WorkspaceChangeEventArgs e)
        {
            instance.Workspace_WorkspaceChanged(instance, e);
        }
    }
}
