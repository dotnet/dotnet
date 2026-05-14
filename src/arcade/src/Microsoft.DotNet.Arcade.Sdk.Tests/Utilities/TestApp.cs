// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{

    public class TestApp : IDisposable
    {
        private readonly string _logOutputDir;

        public TestApp(string workDir, string logOutputDir, string[] sourceDirectories)
        {
            WorkingDirectory = workDir;
            _logOutputDir = Path.Combine(logOutputDir, Path.GetFileName(workDir));

            Directory.CreateDirectory(workDir);
            Directory.CreateDirectory(_logOutputDir);

            foreach (var dir in sourceDirectories)
            {
                CopyRecursive(dir, workDir);
            }
        }

        public string WorkingDirectory { get; }

        public int ExecuteBuild(ITestOutputHelper output, params string[] scriptArgs)
        {
            var cmd = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? @".\build.cmd"
                    : "./build.sh";

            return ExecuteScript(output, cmd, new [] { "-bl" }.Concat(scriptArgs));
        }

        private int ExecuteScript(ITestOutputHelper output, string fileName, IEnumerable<string> scriptArgs)
        {
            output.WriteLine("Working dir = " + WorkingDirectory);
            output.WriteLine("Log output  = " + _logOutputDir);

            var cmd = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "cmd.exe"
                    : "bash";

            var psi = new ProcessStartInfo
            {
                FileName = cmd,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,

                WorkingDirectory = WorkingDirectory,
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                psi.ArgumentList.Add("/C");
            }

            psi.ArgumentList.Add(fileName);
            psi.ArgumentList.AddRange(scriptArgs);

            return Run(output, psi);
        }

        public int Run(ITestOutputHelper output, ProcessStartInfo psi)
        {
            void Write(object sender, DataReceivedEventArgs e)
            {
                output.WriteLine(e.Data ?? string.Empty);
            }

            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            //psi.Environment["PATH"] = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH");

            var process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += Write;
            process.ErrorDataReceived += Write;
            output.WriteLine($"Starting: {process.StartInfo.FileName} {process.StartInfo.Arguments}");
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit(1000 * 60 * 3);

            CopyRecursive(Path.Combine(WorkingDirectory, "artifacts", "log"), _logOutputDir);

            process.OutputDataReceived -= Write;
            process.ErrorDataReceived -= Write;
            return process.ExitCode;
        }

        private static void CopyRecursive(string srcDir, string destDir)
        {
            foreach (var srcFileName in Directory.EnumerateFiles(srcDir, "*", SearchOption.AllDirectories))
            {
                var destFileName = Path.Combine(destDir, srcFileName.Substring(srcDir.Length).TrimStart(new[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar }));
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                File.Copy(srcFileName, destFileName);
            }
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(WorkingDirectory, recursive: true);
            }
            catch
            {
                // Sometimes antivirus scanning locks files and they can't be deleted. Retring after 500ms seems to get around this most of the time
                Thread.Sleep(500);
                Directory.Delete(WorkingDirectory, recursive: true);
            }
        }
    }
}
