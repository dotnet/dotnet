// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.Utilities;

// NOTE: This code is copied and modified slightly from dotnet/roslyn:
// https://github.com/dotnet/roslyn/blob/98cd097bf122677378692ebe952b71ab6e5bb013/src/Workspaces/Core/Portable/Shared/Utilities/AsyncBatchingWorkQueue%600.cs

/// <inheritdoc cref="AsyncBatchingWorkQueue{TItem, TResult}"/>
internal class AsyncBatchingWorkQueue(
    TimeSpan delay,
    Func<CancellationToken, ValueTask> processBatchAsync,
    CancellationToken cancellationToken) : AsyncBatchingWorkQueue<VoidResult>(delay, Convert(processBatchAsync), EqualityComparer<VoidResult>.Default, cancellationToken)
{
    private static Func<ImmutableArray<VoidResult>, CancellationToken, ValueTask> Convert(Func<CancellationToken, ValueTask> processBatchAsync)
        => (items, ct) => processBatchAsync(ct);

    public void AddWork(bool cancelExistingWork = false)
        => base.AddWork(default(VoidResult), cancelExistingWork);
}
