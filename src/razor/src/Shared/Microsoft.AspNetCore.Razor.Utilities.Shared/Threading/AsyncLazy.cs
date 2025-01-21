// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.Threading;

internal static class AsyncLazy
{
    public static AsyncLazy<T> Create<T, TArg>(Func<TArg, CancellationToken, Task<T>> asynchronousComputeFunction, TArg arg)
        => AsyncLazy<T>.Create(asynchronousComputeFunction, arg);

    public static AsyncLazy<T> Create<T>(Func<CancellationToken, Task<T>> asynchronousComputeFunction)
        => Create(
            asynchronousComputeFunction: static (asynchronousComputeFunction, cancellationToken) => asynchronousComputeFunction(cancellationToken),
            arg: asynchronousComputeFunction);

    public static AsyncLazy<T> Create<T>(T value)
        => AsyncLazy<T>.Create<T>(value);
}
