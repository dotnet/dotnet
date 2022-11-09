// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    internal class InboundRequestProcessingHandler : IDisposable
    {
        private readonly ISet<MessageMethod> _fastProccessingMethods;
        private readonly Lazy<DedicatedAsynchronousProcessingThread> _processingThread;
        private bool _isDisposed;

        public InboundRequestProcessingHandler()
            : this(Enumerable.Empty<MessageMethod>())
        {
        }

        /// <summary>
        /// Requests from the processing methods provided in this set are handled on a dedicated thread.
        /// </summary>
        /// <param name="fastProcessingMethods"></param>
        public InboundRequestProcessingHandler(IEnumerable<MessageMethod> fastProcessingMethods)
        {
            if (fastProcessingMethods == null)
            {
                throw new ArgumentNullException(nameof(fastProcessingMethods));
            }
            _fastProccessingMethods = new HashSet<MessageMethod>(fastProcessingMethods);
            // Lazily initialize the processing thread. It is not needed if there are no time critical methods.
            _processingThread = new Lazy<DedicatedAsynchronousProcessingThread>(() =>
            {
                var thread = new DedicatedAsynchronousProcessingThread(TimeSpan.FromMilliseconds(50));
                thread.Start();
                return thread;
            });

        }

        /// <summary>
        /// Methods that are in the fast processing method list will be handled on a separate thread. Everything else will be queued on the threadpool.
        /// </summary>
        /// <param name="messageMethod"></param>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        internal void Handle(MessageMethod messageMethod, Func<Task> task, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (_fastProccessingMethods.Contains(messageMethod))
            {
                _processingThread.Value.Enqueue(task);
            }
            else
            {
                Task.Run(task, cancellationToken);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            if (_processingThread.IsValueCreated)
            {
                _processingThread.Value.Dispose();
            }
            GC.SuppressFinalize(this);

            _isDisposed = true;
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
