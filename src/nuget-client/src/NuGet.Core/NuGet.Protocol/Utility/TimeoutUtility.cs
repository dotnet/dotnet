// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public static class TimeoutUtility
    {
        /// <summary>
        /// Starts a task with a timeout. If the timeout occurs, a <see cref="TimeoutException"/>
        /// with no message will be thrown.
        /// </summary>
        public static async Task<T> StartWithTimeout<T>(
            Func<CancellationToken, Task<T>> getTask,
            TimeSpan timeout,
            string timeoutMessage,
            CancellationToken token)
        {
            /*
             * Implement timeout. Two operations are started and run in parallel:
             *
             *   1) The callers's task.
             *   2) A timer that fires after the duration of the timeout.
             *
             * If the timeout occurs first, the caller's task should be cancelled. If the 
             * caller's task completes before the timeout, the timeout should be cancelled.
             * If the timeout occurs first, consider the caller's task should be considered
             * a failure and a timeout exception is thrown. If the caller's task completes
             * first, it could be that the response came back or that the caller cancelled
             * the task.
             */
            using (var timeoutTcs = new CancellationTokenSource())
            using (var taskTcs = new CancellationTokenSource())
            using (token.Register(() => taskTcs.Cancel()))
            {
                var timeoutTask = Task.Delay(timeout, timeoutTcs.Token);
                var responseTask = getTask(taskTcs.Token);

                if (timeoutTask == await Task.WhenAny(responseTask, timeoutTask).ConfigureAwait(false))
                {
                    taskTcs.Cancel();

                    throw new TimeoutException(timeoutMessage);
                }

                timeoutTcs.Cancel();
                return await responseTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Starts a task with a timeout. If the timeout occurs, a <see cref="TimeoutException"/>
        /// with no message will be thrown.
        /// </summary>
        public static async Task StartWithTimeout(
            Func<CancellationToken, Task> getTask,
            TimeSpan timeout,
            string timeoutMessage,
            CancellationToken token)
        {
            await StartWithTimeout(
                async timeoutToken =>
                {
                    await getTask(timeoutToken).ConfigureAwait(false);
                    return true;
                },
                timeout,
                timeoutMessage,
                token).ConfigureAwait(false);
        }
    }
}
