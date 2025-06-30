// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal partial class ProjectSnapshotManager
{
    private sealed class Dispatcher : IDisposable
    {
        private readonly ILogger _logger;
        private readonly CustomScheduler _scheduler;

        public Dispatcher(LanguageServerFeatureOptions featureOptions, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.GetOrCreateLogger<Dispatcher>();
            _scheduler = new(featureOptions, _logger);
        }

        public TaskScheduler Scheduler => _scheduler;
        public bool IsRunningOnDispatcher => TaskScheduler.Current == _scheduler;

        public void Dispose()
        {
            _scheduler.Dispose();
        }

        public Task RunAsync(Action action, CancellationToken cancellationToken)
        {
            if (IsRunningOnDispatcher)
            {
                action();
                return Task.CompletedTask;
            }

            return Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, Scheduler);
        }

        public Task RunAsync<TState>(Action<TState> action, TState state, CancellationToken cancellationToken)
        {
            if (IsRunningOnDispatcher)
            {
                action(state);
                return Task.CompletedTask;
            }

            return Task.Factory.StartNew(() => action(state), cancellationToken, TaskCreationOptions.None, Scheduler);
        }

        public Task<TResult> RunAsync<TResult>(Func<TResult> func, CancellationToken cancellationToken)
        {
            if (IsRunningOnDispatcher)
            {
                var result = func();
                return Task.FromResult(result);
            }

            return Task.Factory.StartNew(func, cancellationToken, TaskCreationOptions.None, Scheduler);
        }

        public Task<TResult> RunAsync<TState, TResult>(Func<TState, TResult> func, TState state, CancellationToken cancellationToken)
        {
            if (IsRunningOnDispatcher)
            {
                var result = func(state);
                return Task.FromResult(result);
            }

            return Task.Factory.StartNew(() => func(state), cancellationToken, TaskCreationOptions.None, Scheduler);
        }

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
            private readonly LanguageServerFeatureOptions _featureOptions;
            private readonly ILogger _logger;
            private readonly CancellationTokenSource _disposeTokenSource;

            public override int MaximumConcurrencyLevel => 1;

            public CustomScheduler(LanguageServerFeatureOptions featureOptions, ILogger logger)
            {
                _taskQueue = new();
                _featureOptions = featureOptions;
                _logger = logger;
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

                            Debug.Assert(!_featureOptions.UseRazorCohostServer, "If cohosting is on we should never have a task queued up in the dispatcher.");

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
                            _logger.LogError(ex);
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
                if (_disposeTokenSource.IsCancellationRequested)
                {
                    return;
                }

                _taskQueue.Complete();
                _disposeTokenSource.Cancel();
                _disposeTokenSource.Dispose();
            }
        }
    }
}
