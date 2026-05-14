// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DotNet.MacOsPkg.Core
{
    public static class ExecuteHelper
    {
        public static string Run(string command, string arguments = "", string workingDirectory = "")
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            string output = string.Empty;
            string escapedArgs = $"-c \"{command} {arguments}\"";

            ProcessStartInfo processStartInfo = CreateProcessStartInfo("sh", escapedArgs, workingDirectory);
            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit(60000); // 60 seconds
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command '{command} {arguments}' failed with exit code {process.ExitCode}: {process.StandardError.ReadToEnd()}");
                }
            }
            return output;
        }

        private static ProcessStartInfo CreateProcessStartInfo(string command, string arguments, string workingDirectory = "") =>
            new ProcessStartInfo
            {
                FileName = command,
                Arguments = $@"{arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            };
    }
}
