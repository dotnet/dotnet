// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal partial class TestProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher,
    CancellationToken disposalToken)
    : ProjectSnapshotManager(projectEngineFactoryProvider, dispatcher)
{
    private readonly CancellationToken _disposalToken = disposalToken;

    public Task<TestDocumentSnapshot> CreateAndAddDocumentAsync(ProjectSnapshot projectSnapshot, string filePath)
    {
        return UpdateAsync(
            updater =>
            {
                var documentSnapshot = TestDocumentSnapshot.Create(projectSnapshot, filePath);
                updater.DocumentAdded(projectSnapshot.Key, documentSnapshot.HostDocument, new DocumentSnapshotTextLoader(documentSnapshot));

                return documentSnapshot;
            },
            _disposalToken);
    }

    public Listener ListenToNotifications() => new(this);

    public Task UpdateAsync(Action<Updater> updater)
        => UpdateAsync(updater, _disposalToken);

    public Task UpdateAsync<TState>(Action<Updater, TState> updater, TState state)
        => UpdateAsync(updater, state, _disposalToken);

    public Task<TResult> UpdateAsync<TResult>(Func<Updater, TResult> updater)
        => UpdateAsync(updater, _disposalToken);

    public Task<TResult> UpdateAsync<TState, TResult>(Func<Updater, TState, TResult> updater, TState state)
        => UpdateAsync(updater, state, _disposalToken);

    public Task UpdateAsync(Func<Updater, Task> updater)
        => UpdateAsync(updater, _disposalToken);

    public Task UpdateAsync<TState>(Func<Updater, TState, Task> updater, TState state)
        => UpdateAsync(updater, state, _disposalToken);

    public Task<TResult> UpdateAsync<TResult>(Func<Updater, Task<TResult>> updater)
        => UpdateAsync(updater, _disposalToken);

    public Task<TResult> UpdateAsync<TState, TResult>(Func<Updater, TState, Task<TResult>> updater, TState state)
        => UpdateAsync(updater, state, _disposalToken);
}
