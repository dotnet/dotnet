// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.Utilities;

// NOTE: This code is copied and modified slightly from dotnet/roslyn:
// https://github.com/dotnet/roslyn/blob/98cd097bf122677378692ebe952b71ab6e5bb013/src/Workspaces/Core/Portable/Shared/Utilities/AsyncBatchingWorkQueue%601.cs

/// <inheritdoc cref="AsyncBatchingWorkQueue{TItem, TResult}"/>
internal class AsyncBatchingWorkQueue<TItem>(
    TimeSpan delay,
    Func<ImmutableArray<TItem>, CancellationToken, ValueTask> processBatchAsync,
    IEqualityComparer<TItem>? equalityComparer,
    Action? idleAction,
    CancellationToken cancellationToken) : AsyncBatchingWorkQueue<TItem, VoidResult>(delay, Convert(processBatchAsync), equalityComparer, idleAction, cancellationToken)
{
    public AsyncBatchingWorkQueue(
        TimeSpan delay,
        Func<ImmutableArray<TItem>, CancellationToken, ValueTask> processBatchAsync,
        CancellationToken cancellationToken)
        : this(delay,
               processBatchAsync,
               equalityComparer: null,
               cancellationToken)
    {
    }

    public AsyncBatchingWorkQueue(
        TimeSpan delay,
        Func<ImmutableArray<TItem>, CancellationToken, ValueTask> processBatchAsync,
        IEqualityComparer<TItem>? equalityComparer,
        CancellationToken cancellationToken)
        : this(delay,
              processBatchAsync,
              equalityComparer,
              idleAction: null,
              cancellationToken)
    {
    }

    private static Func<ImmutableArray<TItem>, CancellationToken, ValueTask<VoidResult>> Convert(Func<ImmutableArray<TItem>, CancellationToken, ValueTask> processBatchAsync)
        => async (items, ct) =>
        {
            await processBatchAsync(items, ct).ConfigureAwait(false);
            return default;
        };

    public new Task WaitUntilCurrentBatchCompletesAsync()
        => base.WaitUntilCurrentBatchCompletesAsync();
}
