// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor;

internal abstract class ProjectSnapshotManagerDispatcher
{
    public abstract bool IsDispatcherThread { get; }

    public abstract TaskScheduler DispatcherScheduler { get; }

    public Task RunOnDispatcherThreadAsync(Action action, CancellationToken cancellationToken)
        => Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, DispatcherScheduler);

    public Task RunOnDispatcherThreadAsync<TParameter>(Action<TParameter, CancellationToken> action, TParameter state, CancellationToken cancellationToken)
        => Task.Factory.StartNew(() => action(state, cancellationToken), cancellationToken, TaskCreationOptions.None, DispatcherScheduler);

    public Task<TResult> RunOnDispatcherThreadAsync<TResult>(Func<TResult> action, CancellationToken cancellationToken)
        => Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, DispatcherScheduler);

    public virtual void AssertDispatcherThread([CallerMemberName] string? caller = null)
    {
        if (!IsDispatcherThread)
        {
            caller = caller is null ? "The method" : $"'{caller}'";
            throw new InvalidOperationException(caller + " must be called on the project snapshot manager's thread.");
        }
    }
}
