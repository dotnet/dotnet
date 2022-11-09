// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NuGet.VisualStudio;

namespace NuGetConsole
{
    /// <summary>
    /// Extends <see cref="SharedOutputConsole"/> and adds the shared implementation for <see cref="IConsoleDispatcher"/> and <see cref="IConsole"/>.
    /// This class declares the methods that need implemented as abstract.
    /// It allows to avoid diferences in experience across VS Online Environments and VS.
    /// </summary>
    internal abstract class BaseOutputConsole : SharedOutputConsole, IConsoleDispatcher, IConsole
    {
        public abstract void StartConsoleDispatcher();

        public abstract Task StartConsoleDispatcherAsync();

        public void Start()
        {
            if (!IsStartCompleted)
            {
                StartConsoleDispatcher();
                StartCompleted?.Invoke(this, EventArgs.Empty);
            }

            IsStartCompleted = true;
        }

        public async Task StartAsync()
        {
            if (!IsStartCompleted)
            {
                await StartConsoleDispatcherAsync();
                StartCompleted?.Invoke(this, EventArgs.Empty);
            }

            IsStartCompleted = true;
        }

        public event EventHandler StartCompleted;

        event EventHandler IConsoleDispatcher.StartWaitingKey
        {
            add { }
            remove { }
        }

        public bool IsStartCompleted { get; private set; }

        public bool IsExecutingCommand
        {
            get { return false; }
        }

        public bool IsExecutingReadKey
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsKeyAvailable
        {
            get { throw new NotSupportedException(); }
        }

        public void AcceptKeyInput()
        {
        }

        public VsKeyInfo WaitKey()
        {
            throw new NotSupportedException();
        }

        public void ClearConsole()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ClearAsync());
        }

        public IHost Host { get; set; }

        public bool ShowDisclaimerHeader => false;

        public IConsoleDispatcher Dispatcher => this;
    }
}
