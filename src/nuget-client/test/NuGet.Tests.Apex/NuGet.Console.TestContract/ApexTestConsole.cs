// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NuGet.VisualStudio;
using NuGetConsole;
using NuGetConsole.Implementation;
using NuGetConsole.Implementation.Console;

namespace NuGet.Console.TestContract
{
    public class ApexTestConsole
    {
        private IWpfConsole _wpfConsole;
        private PowerConsoleToolWindow _consoleWindow;

        public ApexTestConsole(IWpfConsole WpfConsole, PowerConsoleToolWindow consoleWindow)
        {
            _wpfConsole = WpfConsole ?? throw new ArgumentNullException(nameof(WpfConsole));
            _consoleWindow = consoleWindow ?? throw new ArgumentNullException(nameof(consoleWindow));
        }

        private bool EnsureInitilizeConsole()
        {
            var stopwatch = Stopwatch.StartNew();
            var timeout = TimeSpan.FromMinutes(5);
            var loaded = false;

            do
            {
                // Avoid getting the host until the window has loaded.
                // If the host is loaded first, the window will stop responding.
                if (_consoleWindow.IsLoaded)
                {
                    UIInvoke(() =>
                    {
                        if (_wpfConsole.Dispatcher.IsStartCompleted && _wpfConsole.Host != null)
                        {
                            loaded = true;
                        }
                    });
                }

                if (!loaded)
                {
                    Thread.Sleep(100);
                }
            }
            while (!loaded && stopwatch.Elapsed < timeout);

            return loaded;
        }

        public bool ConsoleContainsMessage(string message)
        {
            if (!EnsureInitilizeConsole())
            {
                return false;
            }

            var snapshot = (_wpfConsole.Content as IWpfTextViewHost).TextView.TextBuffer.CurrentSnapshot;
            for (var i = 0; i < snapshot.LineCount; i++)
            {
                var snapshotLine = snapshot.GetLineFromLineNumber(i);
                var lineText = snapshotLine.GetText();

                var foundMessage = lineText.Contains(message);
                if (foundMessage)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetText()
        {
            if (!EnsureInitilizeConsole())
            {
                return null;
            }

            ITextSnapshot snapshot = (_wpfConsole.Content as IWpfTextViewHost).TextView.TextBuffer.CurrentSnapshot;

            return snapshot.GetText();
        }

        public void Clear()
        {
            if (!EnsureInitilizeConsole())
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(() => _wpfConsole.ClearAsync());
        }

        public void RunCommand(string command, TimeSpan timeout)
        {
            WaitForActionComplete(() => RunCommandWithoutWait(command), timeout);
        }

        public void RunCommandWithInputAndBackspace(string command, string input, TimeSpan timeout)
        {
            WaitForActionComplete(
                () =>
                {
                    RunCommandWithoutWait(command);
                    WaitForReadKey(timeout);

                    IOleCommandTarget commandTarget = null;
                    UIInvoke(() => commandTarget = (IOleCommandTarget)_wpfConsole.VsTextView);
                    foreach (char character in input)
                    {
                        ExecuteReadKeyCommand(
                            commandTarget,
                            VSConstants.VSStd2KCmdID.TYPECHAR,
                            timeout,
                            character);
                    }

                    ExecuteReadKeyCommand(commandTarget, VSConstants.VSStd2KCmdID.BACKSPACE, timeout);
                    UIInvoke(() => commandTarget.Execute(VSConstants.VSStd2KCmdID.RETURN));
                },
                timeout);
        }

        public void RunCommandWithoutWait(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                UIInvoke(async () =>
                {
                    var wpfHost = _wpfConsole.Host;
                    if (wpfHost.IsCommandEnabled)
                    {
                        await _wpfConsole.WriteLineAsync(command);
                        await Task.Run(() => wpfHost.Execute(_wpfConsole, command, null));
                    }
                });
            }
        }

        public void SetConsoleWidth(int consoleWidth)
        {
            _wpfConsole.SetConsoleWidth(consoleWidth);
        }

        public void WaitForActionComplete(Action action, TimeSpan timeout)
        {
            if (!EnsureInitilizeConsole())
            {
                return;
            }

            using (var semaphore = new ManualResetEventSlim())
            {
                var dispatcher = (IPrivateConsoleDispatcher)_wpfConsole.Dispatcher;
                dispatcher.SetExecutingCommand(true);
                var asynchost = (IAsyncHost)_wpfConsole.Host;
                void eventHandler(object s, EventArgs e) => semaphore.Set();
                asynchost.ExecuteEnd += eventHandler;

                try
                {
                    Clear();

                    // Run
                    action();

                    semaphore.Wait(timeout);
                }
                finally
                {
                    asynchost.ExecuteEnd -= eventHandler;
                    dispatcher.SetExecutingCommand(false);
                }
            }
        }

        private void WaitForReadKey(TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();
            while (!_wpfConsole.Dispatcher.IsExecutingReadKey && stopwatch.Elapsed < timeout)
            {
                Thread.Sleep(100);
            }

            if (!_wpfConsole.Dispatcher.IsExecutingReadKey)
            {
                throw new TimeoutException("The Package Manager Console did not begin waiting for input.");
            }
        }

        private void ExecuteReadKeyCommand(
            IOleCommandTarget commandTarget,
            VSConstants.VSStd2KCmdID command,
            TimeSpan timeout,
            object argument = null)
        {
            using var semaphore = new ManualResetEventSlim();
            EventHandler eventHandler = (sender, args) => semaphore.Set();
            _wpfConsole.Dispatcher.StartWaitingKey += eventHandler;

            try
            {
                UIInvoke(() => commandTarget.Execute(command, argument));

                if (!semaphore.Wait(timeout))
                {
                    throw new TimeoutException("The Package Manager Console did not request the next input key.");
                }

                WaitForReadKey(timeout);
            }
            finally
            {
                _wpfConsole.Dispatcher.StartWaitingKey -= eventHandler;
            }
        }

        private void UIInvoke(Action action)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                action();
            });
        }
    }
}
