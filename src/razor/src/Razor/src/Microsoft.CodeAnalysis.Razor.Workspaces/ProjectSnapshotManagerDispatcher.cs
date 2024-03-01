// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Razor;

internal abstract class ProjectSnapshotManagerDispatcher : IDisposable
{
    private readonly CustomScheduler _scheduler;

    public TaskScheduler Scheduler => _scheduler;
    public bool IsRunningOnDispatcher => TaskScheduler.Current == _scheduler;

    protected ProjectSnapshotManagerDispatcher(IErrorReporter errorReporter)
    {
        _scheduler = new CustomScheduler(errorReporter);
    }

    public void Dispose()
    {
        _scheduler.Dispose();
    }

    public Task RunAsync(Action action, CancellationToken cancellationToken)
        => Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, Scheduler);

    public Task RunAsync<TArg>(Action<TArg, CancellationToken> action, TArg arg, CancellationToken cancellationToken)
        => Task.Factory.StartNew(() => action(arg, cancellationToken), cancellationToken, TaskCreationOptions.None, Scheduler);

    public Task<TResult> RunAsync<TResult>(Func<TResult> action, CancellationToken cancellationToken)
        => Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, Scheduler);

    public void AssertRunningOnDispatcher([CallerMemberName] string? caller = null)
    {
        if (!IsRunningOnDispatcher)
        {
            caller = caller is null ? "The method" : $"'{caller}'";
            throw new InvalidOperationException(caller + " must be called on the project snapshot manager's thread.");
        }
    }

    private class CustomScheduler : TaskScheduler, IDisposable
    {
        private readonly AsyncQueue<Task> _taskQueue = new();
        private readonly IErrorReporter _errorReporter;
        private readonly CancellationTokenSource _disposeTokenSource;

        public override int MaximumConcurrencyLevel => 1;

        public CustomScheduler(IErrorReporter errorReporter)
        {
            _taskQueue = new();
            _errorReporter = errorReporter;
            _disposeTokenSource = new();

            _ = Task.Run(ProcessQueueAsync);
        }

        private async Task ProcessQueueAsync()
        {
            var disposeToken = _disposeTokenSource.Token;

            try
            {
                while (!disposeToken.IsCancellationRequested)
                {
                    try
                    {
                        var task = await _taskQueue.DequeueAsync(disposeToken).ConfigureAwait(false);

                        var result = TryExecuteTask(task);
                        Debug.Assert(result);
                    }
                    catch (OperationCanceledException)
                    {
                        // Silently proceed
                    }
                    catch (Exception ex)
                    {
                        // We don't want to crash our loop, so we report the exception and continue.
                        _errorReporter.ReportError(ex);
                    }
                }
            }
            finally
            {
                if (_taskQueue.IsCompleted)
                {
                    while (_taskQueue.TryDequeue(out _))
                    {
                        // Intentionally empty
                    }
                }
            }
        }

        protected override void QueueTask(Task task)
        {
            if (_disposeTokenSource.IsCancellationRequested)
            {
                return;
            }

            _taskQueue.TryEnqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If the task was previously queued it means that we're ensuring it's running on our single threaded scheduler.
            // Otherwise, we can't enforce that behavior and therefore need it to be re-queued before execution.
            if (taskWasPreviouslyQueued)
            {
                return TryExecuteTask(task);
            }

            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
            => _taskQueue.ToArray();

        public void Dispose()
        {
            _taskQueue.Complete();
            _disposeTokenSource.Cancel();
            _disposeTokenSource.Dispose();
        }
    }
}
