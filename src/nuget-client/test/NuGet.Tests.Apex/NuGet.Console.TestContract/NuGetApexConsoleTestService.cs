// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using NuGetConsole;
using NuGetConsole.Implementation;
using NuGetConsole.Implementation.PowerConsole;

namespace NuGet.Console.TestContract
{
    [Export(typeof(NuGetApexConsoleTestService))]
    public class NuGetApexConsoleTestService
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(90);

        private async Task<IWpfConsole> GetWpfConsole()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            PowerConsoleWindow powershellConsole = null;
            var timer = Stopwatch.StartNew();

            while (powershellConsole?.ActiveHostInfo?.WpfConsole == null)
            {
                try
                {
                    var outputConsoleWindow = await ServiceLocator.GetComponentModelServiceAsync<IPowerConsoleWindow>();
                    powershellConsole = outputConsoleWindow as PowerConsoleWindow;
                }
                catch when (timer.Elapsed < _timeout)
                {
                    // Retry until the console is loaded
                    Thread.Sleep(100);
                }
            }

            return powershellConsole?.ActiveHostInfo?.WpfConsole;
        }

        public NuGetApexConsoleTestService()
        {
        }

        public ApexTestConsole GetApexTestConsole()
        {
            var consoleWindow = NuGetUIThreadHelper.JoinableTaskFactory.Run(() => GetPowershellConsole());
            var wpfConsole = NuGetUIThreadHelper.JoinableTaskFactory.Run(() => GetWpfConsole());

            if (consoleWindow == null)
            {
                throw new InvalidOperationException("Unable to get powershell window");
            }

            if (wpfConsole == null)
            {
                throw new InvalidOperationException("Unable to get wpfConsole");
            }

            return new ApexTestConsole(wpfConsole, consoleWindow);
        }

        private async Task<PowerConsoleToolWindow> GetPowershellConsole()
        {
            IVsWindowFrame window = null;
            PowerConsoleToolWindow powerConsole = null;
            var powerConsoleToolWindowGUID = new Guid("0AD07096-BBA9-4900-A651-0598D26F6D24");
            var stopwatch = Stopwatch.StartNew();

            var uiShell = await ServiceLocator.GetGlobalServiceAsync<SVsUIShell, IVsUIShell>();

            // Open PMC in VS
            do
            {
                if (VSConstants.S_OK == uiShell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, powerConsoleToolWindowGUID, out window))
                {
                    window.Show();
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            while (stopwatch.Elapsed < _timeout && window == null);

            // Get PowerConsoleToolWindow from the VS frame
            if (window != null)
            {
                if (VSConstants.S_OK == window.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out var toolPane))
                {
                    powerConsole = (PowerConsoleToolWindow)toolPane;
                    while (!powerConsole.IsLoaded && stopwatch.Elapsed < _timeout)
                    {
                        Thread.Sleep(100);
                    }

                    var dispatcherStarted = false;

                    while (!dispatcherStarted && stopwatch.Elapsed < _timeout)
                    {
                        try
                        {
                            await powerConsole.StartDispatcherAsync();
                            dispatcherStarted = true;
                        }
                        catch
                        {
                            // Ignore MEF cache exceptions here and retry. It is unclear why this happens
                            // but when running outside of Apex tests the same VSIX works fine.
                            Thread.Sleep(100);
                        }
                    }

                    while (!powerConsole.IsHostSuccessfullyInitialized() && stopwatch.Elapsed < _timeout)
                    {
                        Thread.Sleep(100);
                    }
                }
            }

            return powerConsole;
        }
    }
}
