// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Arcade.Common;

public interface IRetryHandler
{
    Task<bool> RunAsync(
        Func<int, Task<RetryResult>> actionAsync);

    Task<bool> RunAsync(
        Func<int, Task<RetryResult>> actionAsync,
        CancellationToken cancellationToken);
}

public readonly record struct RetryResult(bool Succeeded, TimeSpan? RetryAfter = null)
{
    public static RetryResult Success => new(true);

    public static RetryResult Retry(TimeSpan? retryAfter = null)
        => new(false, retryAfter);

    public static implicit operator RetryResult(bool succeeded)
        => succeeded ? Success : Retry();
}
