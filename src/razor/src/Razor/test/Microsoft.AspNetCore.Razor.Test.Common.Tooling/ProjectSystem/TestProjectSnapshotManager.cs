// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal partial class TestProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ILoggerFactory loggerFactory,
    CancellationToken disposalToken,
    Action<ProjectSnapshotManager.Updater>? initializer = null)
    : ProjectSnapshotManager(projectEngineFactoryProvider, languageServerFeatureOptions.ToCompilerOptions(), loggerFactory, initializer)
{
    private readonly CancellationToken _disposalToken = disposalToken;

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
