// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.RemoteExecutor;
using Xunit;

namespace System.Diagnostics.Tests
{
    public partial class ProcessTestBase : FileCleanupTestBase
    {
        protected static readonly int WaitInMS = 30 * 1000 * PlatformDetection.SlowRuntimeTimeoutModifier;
        protected Process _process;
        protected readonly List<Process> _processes = new List<Process>();

        protected Process CreateDefaultProcess([CallerMemberName] string callerName = null)
        {
            Assert.Null(_process);
            _process = CreateProcessLong(callerName);
            _process.Start();
            AddProcessForDispose(_process);
            return _process;
        }

        protected override void Dispose(bool disposing)
        {
            // Wait for all started processes to complete
            foreach (Process p in _processes)
            {
                try
                {
                    p.Kill();
                    Assert.True(p.WaitForExit(WaitInMS));
                    p.WaitForExit(); // wait for event handlers to complete
                }
                catch (InvalidOperationException) { } // in case it was never started
            }

            base.Dispose(disposing);
        }

        protected void AddProcessForDispose(Process p)
        {
            lock (_processes)
            {
                _processes.Add(p);
            }
        }

        protected Process CreateProcess(Func<int> method = null)
        {
            Process p = null;
            using (RemoteInvokeHandle handle = RemoteExecutor.Invoke(method ?? (() => RemoteExecutor.SuccessExitCode), new RemoteInvokeOptions { Start = false }))
            {
                p = handle.Process;
                handle.Process = null;
            }
            AddProcessForDispose(p);
            return p;
        }

        protected Process CreateProcess(Func<Task<int>> method)
        {
            Process p = null;
            using (RemoteInvokeHandle handle = RemoteExecutor.Invoke(method, new RemoteInvokeOptions { Start = false }))
            {
                p = handle.Process;
                handle.Process = null;
            }
            AddProcessForDispose(p);
            return p;
        }

        protected Process CreateProcess(Func<string, int> method, string arg, bool autoDispose = true)
        {
            Process p = null;
            using (RemoteInvokeHandle handle = RemoteExecutor.Invoke(method, arg, new RemoteInvokeOptions { Start = false }))
            {
                p = handle.Process;
                handle.Process = null;
            }

            if (autoDispose)
            {
                AddProcessForDispose(p);
            }

            return p;
        }

        protected Process CreateProcess(Func<string, string, int> method, string arg1, string arg2, bool autoDispose = true)
        {
            Process p = null;
            using (RemoteInvokeHandle handle = RemoteExecutor.Invoke(method, arg1, arg2, new RemoteInvokeOptions { Start = false }))
            {
                p = handle.Process;
                handle.Process = null;
            }

            if (autoDispose)
            {
                AddProcessForDispose(p);
            }

            return p;
        }

        protected Process CreateProcess(Func<string, Task<int>> method, string arg)
        {
            Process p = null;
            using (RemoteInvokeHandle handle = RemoteExecutor.Invoke(method, arg, new RemoteInvokeOptions { Start = false }))
            {
                p = handle.Process;
                handle.Process = null;
            }
            AddProcessForDispose(p);
            return p;
        }

        protected void StartSleepKillWait(Process p)
        {
            p.Start();
            Thread.Sleep(200);
            KillWait(p);
        }

        protected void KillWait(Process p)
        {
            p.Kill();
            Assert.True(p.WaitForExit(WaitInMS));
            p.WaitForExit(); // wait for event handlers to complete
        }

        /// <summary>
        /// Checks if the program is installed
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        protected static bool IsProgramInstalled(string program)
        {
            return GetProgramPath(program) != null;
        }

        /// <summary>
        /// Return program path
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        protected static string GetProgramPath(string program)
        {
            string path;
            string pathEnvVar = Environment.GetEnvironmentVariable("PATH");
            char separator = PlatformDetection.IsWindows ? ';' : ':';
            if (pathEnvVar != null)
            {
                var pathParser = new StringParser(pathEnvVar, separator, skipEmpty: true);
                while (pathParser.MoveNext())
                {
                    string subPath = pathParser.ExtractCurrent();
                    path = Path.Combine(subPath, program);
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            return null;
        }
    }
}
