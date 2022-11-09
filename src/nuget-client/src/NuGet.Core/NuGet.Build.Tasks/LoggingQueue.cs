// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks.Dataflow;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a queue of logging messages that need to be processed.
    /// </summary>
    /// <typeparam name="T">The type of object to be added to the queue.</typeparam>
    public abstract class LoggingQueue<T> : IDisposable
    {
        private bool _disposed = false;

        /// <summary>
        /// Stores the queue of actions to be executed.
        /// </summary>
        private readonly ActionBlock<T> _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingQueue{T}" /> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The maximum number of messages that can be processed at a time.</param>
        protected LoggingQueue(int maxDegreeOfParallelism = 1)
        {
            _queue = new ActionBlock<T>(
                Process,
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                });
        }

        /// <summary>
        /// Enqueues a logging message to be processed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Enqueue(T obj)
        {
            return _queue.Post(obj);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Signals the queue that no more actions should be added
                _queue.Complete();

                // Waits for all actions in the queue to be completed
                _queue.Completion.Wait();
            }

            _disposed = true;
        }

        /// <summary>
        /// Processes an item in the queue.
        /// </summary>
        /// <param name="item">The item to be processed.</param>
        protected abstract void Process(T item);
    }
}
