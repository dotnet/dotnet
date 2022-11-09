// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Common.Telemetry.PowerShell
{
    public static class NuGetPowerShellUsage
    {
        public delegate void SolutionOpenHandler();
        public static event SolutionOpenHandler SolutionOpenEvent;

        public delegate void SolutionCloseHandler();
        public static event SolutionCloseHandler SolutionCloseEvent;

        public delegate void PowerShellLoadEventHandler(bool isPMC);
        public static event PowerShellLoadEventHandler PowerShellLoadEvent;

        public delegate void PowerShellCommandExecuteEventHandler(bool isPMC);
        public static event PowerShellCommandExecuteEventHandler PowerShellCommandExecuteEvent;

        public delegate void NuGetCmdletExecutedEventHandler();
        public static event NuGetCmdletExecutedEventHandler NuGetCmdletExecutedEvent;

        public delegate void InitPs1LoadEventHandler(bool isPMC);
        public static event InitPs1LoadEventHandler InitPs1LoadEvent;

        public delegate void PmcWindowEventHandler(bool isLoad);
        public static event PmcWindowEventHandler PmcWindowsEvent;

        public static void RaisePowerShellLoadEvent(bool isPMC)
        {
            PowerShellLoadEvent?.Invoke(isPMC);
        }

        public static void RaiseCommandExecuteEvent(bool isPMC)
        {
            PowerShellCommandExecuteEvent?.Invoke(isPMC);
        }

        public static void RaiseNuGetCmdletExecutedEvent()
        {
            NuGetCmdletExecutedEvent?.Invoke();
        }

        public static void RaiseInitPs1LoadEvent(bool isPMC)
        {
            InitPs1LoadEvent?.Invoke(isPMC);
        }

        public static void RaisePmcWindowsLoadEvent(bool isLoad)
        {
            PmcWindowsEvent?.Invoke(isLoad);
        }

        public static void RaiseSolutionOpenEvent()
        {
            SolutionOpenEvent?.Invoke();
        }

        public static void RaiseSolutionCloseEvent()
        {
            SolutionCloseEvent?.Invoke();
        }
    }
}
