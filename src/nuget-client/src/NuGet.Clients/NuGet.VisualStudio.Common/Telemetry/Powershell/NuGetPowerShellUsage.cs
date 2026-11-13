// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Threading;

namespace NuGet.VisualStudio.Common.Telemetry.PowerShell
{
    public static class NuGetPowerShellUsage
    {
        private static long CurrentSolutionSessionId;

        /// <summary>
        /// Identifies the current solution lifetime, or the interval without a solution.
        /// Capture this value before scheduling asynchronous initialization or package operations.
        /// </summary>
        public static long SolutionSessionId => Interlocked.Read(ref CurrentSolutionSessionId);

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

        public delegate void InitPs1ExecuteEventHandler(long solutionSessionId);

        /// <summary>
        /// Raised immediately before an automatic init.ps1 execution attempt during initialization or package installation,
        /// regardless of whether PMC or PMUI triggered it, after skipping already visited packages.
        /// The solution session identifies the lifetime in which the operation started, even if it has since closed.
        /// </summary>
        public static event InitPs1ExecuteEventHandler InitPs1ExecuteEvent;

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

        public static void RaiseInitPs1ExecuteEvent(long solutionSessionId)
        {
            InitPs1ExecuteEvent?.Invoke(solutionSessionId);
        }

        public static void RaisePmcWindowsLoadEvent(bool isLoad)
        {
            PmcWindowsEvent?.Invoke(isLoad);
        }

        public static void RaiseSolutionOpenEvent()
        {
            Interlocked.Increment(ref CurrentSolutionSessionId);
            SolutionOpenEvent?.Invoke();
        }

        public static void RaiseSolutionCloseEvent()
        {
            Interlocked.Increment(ref CurrentSolutionSessionId);
            SolutionCloseEvent?.Invoke();
        }
    }
}
