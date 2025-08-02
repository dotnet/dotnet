// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace Test.Utility.Threading
{
    public sealed class DispatcherThreadFixture : IDisposable
    {
        private readonly DispatcherThread _dispatcherThread;
        private readonly JoinableTaskContextNode _joinableTaskContextNode;

        public JoinableTaskFactory JoinableTaskFactory => _joinableTaskContextNode.Factory;

        public DispatcherThreadFixture()
        {
            // ThreadHelper in VS requires a persistent dispatcher thread.  Because
            // each unit test executes on a new thread, we create our own
            // persistent thread that acts like a UI thread. This will be invoked just
            // once for the module.
            _dispatcherThread = new DispatcherThread();

            _dispatcherThread.Invoke(() =>
            {
                // Internally this calls ThreadHelper.SetUIThread(), which
                // causes ThreadHelper to remember this thread for the
                // lifetime of the process as the dispatcher thread.
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                var serviceProvider = ServiceProvider.GlobalProvider;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            });

            _joinableTaskContextNode = new JoinableTaskContextNode(
#pragma warning disable VSSDK005 // Avoid instantiating JoinableTaskContext
                new JoinableTaskContext(_dispatcherThread.Thread, _dispatcherThread.SyncContext));
#pragma warning restore VSSDK005 // Avoid instantiating JoinableTaskContext
        }

        public void Dispose()
        {
            _dispatcherThread.Dispose();
        }
    }
}
