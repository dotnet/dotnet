// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IProjectSnapshotManager
{
    event EventHandler<ProjectChangeEventArgs> PriorityChanged;
    event EventHandler<ProjectChangeEventArgs> Changed;

    ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName);
    ImmutableArray<IProjectSnapshot> GetProjects();
    IProjectSnapshot GetLoadedProject(ProjectKey projectKey);
    bool TryGetLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project);

    bool IsDocumentOpen(string documentFilePath);
    ImmutableArray<string> GetOpenDocuments();

    void Update(Action<ProjectSnapshotManager.Updater> updater);
    void Update<TState>(Action<ProjectSnapshotManager.Updater, TState> updater, TState state);
    TResult Update<TResult>(Func<ProjectSnapshotManager.Updater, TResult> updater);
    TResult Update<TState, TResult>(Func<ProjectSnapshotManager.Updater, TState, TResult> updater, TState state);

    Task UpdateAsync(Action<ProjectSnapshotManager.Updater> updater, CancellationToken cancellationToken);
    Task UpdateAsync<TState>(Action<ProjectSnapshotManager.Updater, TState> updater, TState state, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TResult>(Func<ProjectSnapshotManager.Updater, TResult> updater, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TState, TResult>(Func<ProjectSnapshotManager.Updater, TState, TResult> updater, TState state, CancellationToken cancellationToken);

    Task UpdateAsync(Func<ProjectSnapshotManager.Updater, Task> updater, CancellationToken cancellationToken);
    Task UpdateAsync<TState>(Func<ProjectSnapshotManager.Updater, TState, Task> updater, TState state, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TResult>(Func<ProjectSnapshotManager.Updater, Task<TResult>> updater, CancellationToken cancellationToken);
    Task<TResult> UpdateAsync<TState, TResult>(Func<ProjectSnapshotManager.Updater, TState, Task<TResult>> updater, TState state, CancellationToken cancellationToken);
}
