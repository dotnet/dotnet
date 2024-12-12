// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IProjectSnapshotManager
{
    event EventHandler<ProjectChangeEventArgs> PriorityChanged;
    event EventHandler<ProjectChangeEventArgs> Changed;

    bool IsSolutionClosing { get; }

    ImmutableArray<ProjectKey> GetProjectKeysWithFilePath(string projectFileName);
    ImmutableArray<IProjectSnapshot> GetProjects();

    bool ContainsProject(ProjectKey projectKey);
    bool TryGetProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project);

    bool IsDocumentOpen(string documentFilePath);
    ImmutableArray<string> GetOpenDocuments();

    Task UpdateAsync(Action<ProjectSnapshotManager.Updater> updater, CancellationToken cancellationToken);
    Task UpdateAsync<TState>(Action<ProjectSnapshotManager.Updater, TState> updater, TState state, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TResult>(Func<ProjectSnapshotManager.Updater, TResult> updater, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TState, TResult>(Func<ProjectSnapshotManager.Updater, TState, TResult> updater, TState state, CancellationToken cancellationToken);

    Task UpdateAsync(Func<ProjectSnapshotManager.Updater, Task> updater, CancellationToken cancellationToken);
    Task UpdateAsync<TState>(Func<ProjectSnapshotManager.Updater, TState, Task> updater, TState state, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TResult>(Func<ProjectSnapshotManager.Updater, Task<TResult>> updater, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TState, TResult>(Func<ProjectSnapshotManager.Updater, TState, Task<TResult>> updater, TState state, CancellationToken cancellationToken);
}
