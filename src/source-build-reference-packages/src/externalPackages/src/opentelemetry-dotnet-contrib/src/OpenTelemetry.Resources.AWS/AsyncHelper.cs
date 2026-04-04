// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources.AWS;

/// <summary>
/// A helper class for running asynchronous methods synchronously.
/// </summary>
internal static class AsyncHelper
{
    private static readonly TaskFactory TaskFactory = new(
        CancellationToken.None,
        TaskCreationOptions.None,
        TaskContinuationOptions.None,
        TaskScheduler.Default);

    /// <summary>
    /// Executes an async task which has a void return value synchronously.
    /// </summary>
    /// <param name="task">The async task to execute.</param>
    public static void RunSync(Func<Task> task)
    {
        TaskFactory
            .StartNew(() => task(), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Executes an async task which has a TResult return value synchronously.
    /// </summary>
    /// <typeparam name="TResult">The return type of the task.</typeparam>
    /// <param name="task">The async task to execute.</param>
    /// <returns>The result of the executed task.</returns>
    public static TResult RunSync<TResult>(Func<Task<TResult>> task)
    {
        return TaskFactory
            .StartNew(() => task(), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}
