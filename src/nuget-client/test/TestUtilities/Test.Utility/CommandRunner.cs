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
            string filename,
            string workingDirectory,
            string arguments,
            int timeOutInMilliseconds = 60000,
            Action<StreamWriter> inputAction = null,
            IDictionary<string, string> environmentVariables = null)
        {
            var output = new StringBuilder();
            var error = new StringBuilder();

            using var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(Path.GetFullPath(filename), arguments)
                {
                    WorkingDirectory = Path.GetFullPath(workingDirectory),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = inputAction != null
                }
            };

            process.StartInfo.Environment["NuGetTestModeEnabled"] = bool.TrueString;

            if (environmentVariables != null)
            {
                foreach (var pair in environmentVariables)
                {
                    process.StartInfo.EnvironmentVariables[pair.Key] = pair.Value;
                }
            }

            process.OutputDataReceived += (_, args) =>
            {
                if (args.Data != null)
                {
                    output.AppendLine(args.Data);
                }
            };

            process.ErrorDataReceived += (_, args) =>
            {
                if (args.Data != null)
                {
                    error.AppendLine(args.Data);
                }
            };

            process.Start();

            inputAction?.Invoke(process.StandardInput);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            bool processExited = process.WaitForExit(timeOutInMilliseconds);

            if (processExited)
            {
                process.WaitForExit();

                return new CommandRunnerResult(process, process.ExitCode, output.ToString(), error.ToString());
            }

            Kill(process);

            throw new TimeoutException($"{process.StartInfo.FileName} {process.StartInfo.Arguments} timed out after {TimeSpan.FromMilliseconds(timeOutInMilliseconds).TotalSeconds:N0} seconds:{Environment.NewLine}Output:{output}{Environment.NewLine}Error:{error}");

            void Kill(Process process)
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
}
