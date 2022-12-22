// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public abstract class OmniSharpProjectSnapshotManagerDispatcher
{
    internal ProjectSnapshotManagerDispatcher InternalDispatcher { get; private protected set; }

    public abstract TaskScheduler DispatcherScheduler { get; }

    public Task RunOnDispatcherThreadAsync(Action action, CancellationToken cancellationToken)
        => InternalDispatcher.RunOnDispatcherThreadAsync(action, cancellationToken);

    public Task<TResult> RunOnDispatcherThreadAsync<TResult>(Func<TResult> action, CancellationToken cancellationToken)
        => InternalDispatcher.RunOnDispatcherThreadAsync(action, cancellationToken);

    public abstract void AssertDispatcherThread([CallerMemberName] string caller = null);
}
