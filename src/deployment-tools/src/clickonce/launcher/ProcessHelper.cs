// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Deployment.Launcher
{
    internal class ProcessHelper
    {
        private readonly ProcessStartInfo psi;

        /// <summary>
        /// ProcessHelper constructor
        /// </summary>
        /// <param name="exe">Executable name</param>
        /// <param name="args">Arguments</param>
        public ProcessHelper(string exe, string args)
        {
            psi = new ProcessStartInfo(exe, args)
            {
                UseShellExecute = false
            };
        }

        /// <summary>
        /// Starts the process, with retries.
        /// Number of attempts and delay are specified in Constants class.
        /// </summary>
        public void StartProcessWithRetries()
        {
            Logger.LogInfo(Constants.InfoProcessToLaunch, psi.FileName, psi.Arguments);
            int count = 1;
            while (true)
            {
                try
                {
                    StartProcess();
                    return;
                }
                catch (Exception e)
                {
                    // Log each failure attempt
                    Logger.LogError(Constants.ErrorProcessStart, e.Message);

                    if (count++ < Constants.NumberOfProcessStartAttempts)
                    {
                        Logger.LogInfo(Constants.InfoProcessStartWaitRetry, Constants.DelayBeforeRetryMiliseconds);
                        Thread.Sleep(Constants.DelayBeforeRetryMiliseconds);
                        continue;
                    }
                    else
                    {
                        Logger.LogError(Constants.ErrorProcessFailedToLaunch, psi.FileName, psi.Arguments);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Starts the process.
        /// </summary>
        private void StartProcess()
        {
            if (null == Process.Start(psi))
            {
                throw new LauncherException(Constants.ErrorProcessNotStarted);
            }
        }
    }
}
