// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NuGet.Test.Utility
{
    public class CommandRunner
    {
        // Item1 of the returned tuple is the exit code. Item2 is the standard output, and Item3
        // is the error output.
        public static CommandRunnerResult Run(
            string process,
            string workingDirectory,
            string arguments,
            bool waitForExit,
            int timeOutInMilliseconds = 60000,
            Action<StreamWriter> inputAction = null,
            IDictionary<string, string> environmentVariables = null)
        {
            var psi = new ProcessStartInfo(Path.GetFullPath(process), arguments)
            {
                WorkingDirectory = Path.GetFullPath(workingDirectory),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = inputAction != null
            };

#if !IS_CORECLR
            psi.EnvironmentVariables["NuGetTestModeEnabled"] = "True";
#else
            psi.Environment["NuGetTestModeEnabled"] = "True";
#endif

            if (environmentVariables != null)
            {
                foreach (var pair in environmentVariables)
                {
#if !IS_CORECLR
                    psi.EnvironmentVariables[pair.Key] = pair.Value;
#else
                    psi.Environment[pair.Key] = pair.Value;
#endif
                }
            }

            int exitCode = 1;

            var output = new StringBuilder();
            var errors = new StringBuilder();

            Process p = null;

            using (p = new Process())
            {
                p.OutputDataReceived += OutputHandler;
                p.ErrorDataReceived += ErrorHandler;

                p.StartInfo = psi;
                p.Start();

                inputAction?.Invoke(p.StandardInput);

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (waitForExit)
                {
#if DEBUG
                    p.WaitForExit();
                    var processExited = true;
#else
                    var processExited = p.WaitForExit(timeOutInMilliseconds);
#endif
                    if (processExited)
                    {
                        p.WaitForExit();
                        exitCode = p.ExitCode;
                    }
                    else
                    {
                        Kill(p);
                        WaitForExit(p);

                        var processName = Path.GetFileName(process);

                        throw new TimeoutException($"{processName} timed out: {psi.Arguments}{Environment.NewLine}Output:{output}{Environment.NewLine}Error:{errors}");
                    }
                }
            }

            void OutputHandler(object sendingProcess, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.Data))
                    output.AppendLine(e.Data);
            }

            void ErrorHandler(object sendingProcess, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.Data))
                    errors.AppendLine(e.Data);
            }

            return new CommandRunnerResult(p, exitCode, output.ToString(), errors.ToString());
        }

        private static void Kill(Process process)
        {
            try
            {
                process.Kill();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Win32Exception)
            {
            }
        }

        private static void WaitForExit(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.WaitForExit();
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (Win32Exception)
            {
            }
            catch (SystemException)
            {
            }
        }
    }
}
