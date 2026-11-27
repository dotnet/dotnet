// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.Common;

public static class ExecuteHelper
{
    // Grace period to wait for a process to exit after it has been killed, so a kill that
    // doesn't take effect fails the test instead of hanging it until the pipeline timeout.
    public const int KillGraceMilliseconds = 30_000;
    public const int DefaultTimeoutMilliseconds = 60_000;
    public const int LongRunningTimeoutMilliseconds = 600_000;

    public static (Process Process, string StdOut, string StdErr) ExecuteProcess(
        string fileName,
        string args,
        ITestOutputHelper outputHelper,
        bool logOutput = false,
        Action<Process>? configure = null,
        int millisecondTimeout = DefaultTimeoutMilliseconds,
        Action<Process, ProcessOutputLine>? onOutputLine = null)
    {
        outputHelper.WriteLine($"Executing: {fileName} {args}");

        Process process = new()
        {
            StartInfo =
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };

        configure?.Invoke(process);
        process.Start();

        StringBuilder stdOutput = new();
        StringBuilder stdError = new();
        bool timedOut = false;
        try
        {
            foreach (ProcessOutputLine line in process.ReadAllLines(TimeSpan.FromMilliseconds(millisecondTimeout)))
            {
                (line.StandardError ? stdError : stdOutput).AppendLine(line.Content);
                onOutputLine?.Invoke(process, line);
            }
        }
        catch (TimeoutException)
        {
            timedOut = true;
        }

        if (timedOut || !process.WaitForExit(millisecondTimeout))
        {
            ProcessHangDiagnostics.CollectOnTimeout(
                process,
                millisecondTimeout,
                () => stdOutput + "--- stderr ---" + Environment.NewLine + stdError,
                message =>
                {
                    // Test output is only reported when the test finishes, so also write to the console, which the
                    // CI log shows live.
                    Console.WriteLine(message);
                    outputHelper.WriteLine(message);
                });

            outputHelper.WriteLine($"Killing: {fileName} {args}");
            process.Kill(true);
            if (!process.WaitForExit(KillGraceMilliseconds))
            {
                outputHelper.WriteLine("Process tree did not fully exit within the kill grace period; " +
                    "abandoning wait to avoid hanging on inherited stdout/stderr handles.");
            }
            ProcessStartInfo startInfo = process.StartInfo;
            string msg = $" {startInfo.FileName} {startInfo.Arguments} timed out after " +
                $"{millisecondTimeout} milliseconds" +
                $"{Environment.NewLine}Exit code: {process.ExitCode}";
            throw new InvalidOperationException(msg);
        }

        string output = stdOutput.ToString().Trim();
        if (logOutput && !string.IsNullOrWhiteSpace(output))
        {
            outputHelper.WriteLine(output);
        }

        string error = stdError.ToString().Trim();
        if (logOutput && !string.IsNullOrWhiteSpace(error))
        {
            outputHelper.WriteLine(error);
        }

        return (process, output, error);
    }

    public static string ExecuteProcessValidateExitCode(string fileName, string args, ITestOutputHelper outputHelper)
    {
        (Process Process, string StdOut, string StdErr) result = ExecuteHelper.ExecuteProcess(fileName, args, outputHelper);
        ValidateExitCode(result);

        return result.StdOut;
    }

    public static void ValidateExitCode((Process Process, string StdOut, string StdErr) result, int expectedExitCode = 0)
    {
        if (result.Process.ExitCode != expectedExitCode)
        {
            ProcessStartInfo startInfo = result.Process.StartInfo;
            string msg = $"Failed to execute {startInfo.FileName} {startInfo.Arguments}" +
                $"{Environment.NewLine}Exit code: {result.Process.ExitCode}" +
                $"{Environment.NewLine}{result.StdOut}" +
                $"{Environment.NewLine}{result.StdErr}";

            var ex = new InvalidOperationException(msg);
            if (result.StdErr.Contains("Microsoft.AspNetCore.Connections.AddressInUseException"))
            {
                ex.Data["IsAddressInUseException"] = true;
            }
            throw ex;
        }
    }
}
