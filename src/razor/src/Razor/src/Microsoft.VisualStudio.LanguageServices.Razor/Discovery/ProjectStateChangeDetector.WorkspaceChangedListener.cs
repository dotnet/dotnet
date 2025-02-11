// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal partial class ProjectStateChangeDetector
{
    private class WorkspaceChangedListener(ImmutableArray<WorkspaceChangeKind> kinds)
    {
        private readonly ImmutableArray<WorkspaceChangeKind> _kinds = kinds;
        private readonly TaskCompletionSource<bool> _completionSource = new();
        private int _index;

        public Task Task => _completionSource.Task;

        public void WorkspaceChanged(WorkspaceChangeKind kind)
        {
            if (_index == _kinds.Length)
            {
                throw new InvalidOperationException($"Expected {_kinds.Length} WorkspaceChanged events but received another {kind}.");
            }

            if (_kinds[_index] != kind)
            {
                throw new InvalidOperationException($"Expected WorkspaceChanged event #{_index + 1} to be {_kinds[_index]} but it was {kind}.");
            }

            _index++;

            if (_index == _kinds.Length)
            {
                _completionSource.TrySetResult(true);
            }
        }
    }
}
