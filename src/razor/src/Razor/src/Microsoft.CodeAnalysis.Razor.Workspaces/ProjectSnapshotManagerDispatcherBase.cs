// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class ProjectSnapshotManagerDispatcherBase : ProjectSnapshotManagerDispatcher, IDisposable
{
    private readonly ProjectSnapshotManagerTaskScheduler _dispatcherScheduler;

    public ProjectSnapshotManagerDispatcherBase(string threadName)
    {
        if (threadName is null)
        {
            throw new ArgumentNullException(nameof(threadName));
        }

        _dispatcherScheduler = new ProjectSnapshotManagerTaskScheduler(threadName, LogException);
    }

    public abstract void LogException(Exception ex);

    public void Dispose()
    {
        _dispatcherScheduler.Dispose();
    }

    public override bool IsDispatcherThread => Thread.CurrentThread.ManagedThreadId == _dispatcherScheduler.ThreadId;

    public override TaskScheduler DispatcherScheduler => _dispatcherScheduler;

    private class ProjectSnapshotManagerTaskScheduler : TaskScheduler, IDisposable
    {
        private readonly Thread _thread;
        private readonly BlockingCollection<Task> _tasks = new();
        private readonly Action<Exception> _logException;
        private bool _disposed;
        private readonly object _disposalLock = new();

        public ProjectSnapshotManagerTaskScheduler(string threadName, Action<Exception> logException)
        {
            _thread = new Thread(ThreadStart)
            {
                Name = threadName,
                IsBackground = true,
            };

            _thread.Start();
            _logException = logException;
        }

        public int ThreadId => _thread.ManagedThreadId;

        public override int MaximumConcurrencyLevel => 1;

        protected override void QueueTask(Task task)
        {
            lock (_disposalLock)
            {
                if (_disposed)
                {
                    return;
                }
            }

            _tasks.Add(task);
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

        protected override IEnumerable<Task> GetScheduledTasks() => _tasks.ToArray();

        private void ThreadStart()
        {
            while (!_disposed)
            {
                try
                {
                    var task = _tasks.Take();
                    TryExecuteTask(task);
                }
                catch (ThreadAbortException ex)
                {
                    // Fires when things shut down or in tests. Log exception and bail out.
                    _logException(ex);
                    return;
                }
                catch (Exception ex)
                {
                    lock (_disposalLock)
                    {
                        if (_disposed)
                        {
                            // Graceful teardown
                            return;
                        }
                    }

                    // Unexpected exception. Log and throw.
                    _logException(ex);
                    throw;
                }
            }
        }

        public void Dispose()
        {
            lock (_disposalLock)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                _tasks.CompleteAdding();
            }
        }
    }
}
