// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Threading;

namespace Test.Utility.Threading
{
    internal sealed class DispatcherThread : IDisposable
    {
        private readonly Thread _thread;
        private Dispatcher _dispatcher;
        private readonly object _invokeSyncRoot = new object();
        private SynchronizationContext _syncContext;
        private Exception _invokeException;
        private bool _isInvoking;
        private int _isClosed;

        public DispatcherThread()
        {
            using (var resetEvent = new AutoResetEvent(initialState: false))
            {
                _thread = new Thread(() =>
                {
                    // This is necessary to make sure a dispatcher exists for this thread.
                    var unused = Dispatcher.CurrentDispatcher;

                    unused.UnhandledException += new DispatcherUnhandledExceptionEventHandler(OnUnhandledException);

                    resetEvent.Set();

                    Dispatcher.Run();
                })
                {
                    Name = GetType().FullName,
                    IsBackground = true
                };
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();

                resetEvent.WaitOne();

                AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            }

            _dispatcher = Dispatcher.FromThread(_thread);
        }

        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            // Need to dispose of the dispatch thread prior to the app domain going away.
            Close();
        }

        public Thread Thread => _thread;

        public SynchronizationContext SyncContext
        {
            get
            {
                if (_syncContext == null)
                {
                    _syncContext = new DispatcherSynchronizationContext(_dispatcher);
                }
                return _syncContext;
            }
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // We don't deal with unhandled exceptions from BeginInvoke,
            // since there's no one to throw them to.
            if (_isInvoking)
            {
                // e.Exception should be a TargetInvocationException from calling Invoke,
                // the InnerExceptionis the one to forward on
                if (e.Exception is TargetInvocationException)
                {
                    _invokeException = e.Exception.InnerException;
                }
                else
                {
                    _invokeException = e.Exception;
                }

                e.Handled = true;
            }
        }

        public void Invoke(Action action)
        {
            if (_isClosed != 0)
            {
                throw new ObjectDisposedException(nameof(DispatcherThread));
            }

            lock (_invokeSyncRoot)
            {
                _isInvoking = true;
                _invokeException = null;

                try
                {
                    _dispatcher.Invoke(DispatcherPriority.Normal, action);

                    if (_invokeException != null)
                    {
                        throw _invokeException;
                    }
                }
                finally
                {
                    _isInvoking = false;
                }
            }
        }

        public DispatcherOperation BeginInvoke(Action action)
        {
            if (_isClosed != 0)
            {
                throw new ObjectDisposedException(nameof(DispatcherThread));
            }

            return _dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Close()
        {
            if (Interlocked.CompareExchange(ref _isClosed, 1, 0) == 0)
            {
                _dispatcher.InvokeShutdown();
                _dispatcher = null;

                try
                {
                    if (!_thread.Join(TimeSpan.FromSeconds(20)))
                    {
                        _thread.Abort();
                    }
                }
                catch (ThreadAbortException)
                {
                }
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
