// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    /// <summary>
    /// This class represents a dedicated asynchronous task processing thread.
    /// Uses a queue to execute all the tasks added through the invocation of <see cref="Enqueue(Func{Task})"/>.
    /// The tasks queued here cannot be awaited (think Task.Run, rather than Task.WhenAny/WhenAll).
    /// This implementation is internal on purpose as this is specifically tailed to the plugin V2 use-case.
    /// </summary>
    internal sealed class DedicatedAsynchronousProcessingThread : IDisposable
    {
        private Task _processingThread;
        private bool _isDisposed;
        private bool _isClosed;
        private TimeSpan _pollingDelay;
        private readonly ConcurrentQueue<Func<Task>> _taskQueue = new ConcurrentQueue<Func<Task>>();

        /// <summary>
        /// DedicatedAsync processing thread.
        /// </summary>
        /// <param name="pollingDelay">The await delay when there are no tasks in the queue.</param>
        public DedicatedAsynchronousProcessingThread(TimeSpan pollingDelay)
        {
            _pollingDelay = pollingDelay;
        }

        internal void Start()
        {
            ThrowIfDisposed();
            ThrowIfAlreadyStarted();

            _processingThread = Task.Factory.StartNew(
                ProcessAsync,
                CancellationToken.None,
                TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default);
        }

        /// <summary>
        /// Queueus a task for execution.
        /// </summary>
        /// <param name="task">Task to be executed.</param>
        internal void Enqueue(Func<Task> task)
        {
            ThrowIfDisposed();
            ThrowIfNotAlreadyStarted();
            _taskQueue.Enqueue(task);
        }

        private async Task ProcessAsync()
        {
            while (!_isClosed)
            {
                try
                {
                    if (_taskQueue.TryDequeue(out var result))
                    {
                        await result();
                    }
                    else
                    {
                        await Task.Delay(_pollingDelay.Milliseconds);
                    }
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isClosed = true;

            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        private void ThrowIfAlreadyStarted()
        {
            if (_processingThread != null)
            {
                throw new InvalidOperationException("The processing thread is already started.");
            }
        }

        private void ThrowIfNotAlreadyStarted()
        {
            if (_processingThread == null)
            {
                throw new InvalidOperationException("The processing thread is not started yet.");
            }
        }
        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
