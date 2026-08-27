// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.Common;

public static class ExecuteHelper
{
    // Grace period to wait for a killed process tree to fully exit. A parameterless
    // WaitForExit() after Kill(true) blocks until the redirected stdout/stderr pipes
    // reach EOF; a surviving grandchild (e.g. the web app spawned by `dotnet run`) that
    // inherited those handles can keep them open indefinitely, hanging the test until the
    // pipeline task timeout. Bounding the wait lets the run fail fast instead.
    public const int KillGraceMilliseconds = 30_000;

    public static (Process Process, string StdOut, string StdErr) ExecuteProcess(
        string fileName,
        string args,
        ITestOutputHelper outputHelper,
        bool logOutput = false,
        Action<Process>? configure = null,
        int millisecondTimeout = -1,
        Func<string, string>? outputSanitizer = null)
    {
        Process process = new()
        {
            EnableRaisingEvents = true,
            StartInfo =
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };

        configure?.Invoke(process);

        DateTimeOffset startedAt = DateTimeOffset.UtcNow;
        string command = $"{fileName} {args}";
        outputHelper.WriteLine(
            $"[{startedAt:O}] Starting command: {command}{Environment.NewLine}" +
            $"Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}" +
            $"Timeout: {(millisecondTimeout < 0 ? "infinite" : $"{millisecondTimeout} ms")}");

        StringBuilder stdOutput = new();
        process.OutputDataReceived += new DataReceivedEventHandler(
            (sender, e) =>
            {
                lock (stdOutput)
                {
                    stdOutput.AppendLine(e.Data);
                }
            });

        StringBuilder stdError = new();
        process.ErrorDataReceived += new DataReceivedEventHandler(
            (sender, e) =>
            {
                lock (stdError)
                {
                    stdError.AppendLine(e.Data);
                }
            });

        process.Start();
        int processId = process.Id;
        outputHelper.WriteLine($"[{DateTimeOffset.UtcNow:O}] Started PID {processId}: {command}");
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit(millisecondTimeout);

        if (!process.HasExited)
        {
            outputHelper.WriteLine(
                $"[{DateTimeOffset.UtcNow:O}] Command timed out; killing process tree rooted at PID {processId}: {command}");
            process.Kill(true);
            bool exitedAfterKill = process.WaitForExit(KillGraceMilliseconds);
            if (!exitedAfterKill)
            {
                outputHelper.WriteLine("Process tree did not fully exit within the kill grace period; " +
                    "abandoning wait to avoid hanging on inherited stdout/stderr handles.");
            }

            string timedOutOutput = GetOutput(stdOutput);
            string timedOutError = GetOutput(stdError);
            LogOutput(timedOutOutput, timedOutError);

            string msg = $"{command} (PID {processId}) timed out after {millisecondTimeout} milliseconds" +
                $"{Environment.NewLine}Working directory: {process.StartInfo.WorkingDirectory}" +
                $"{Environment.NewLine}Process tree exited after kill: {exitedAfterKill}" +
                $"{Environment.NewLine}Standard output:{Environment.NewLine}{Sanitize(timedOutOutput)}" +
                $"{Environment.NewLine}Standard error:{Environment.NewLine}{Sanitize(timedOutError)}";
            throw new InvalidOperationException(msg);
        }

        DateTimeOffset endedAt = DateTimeOffset.UtcNow;
        outputHelper.WriteLine(
            $"[{endedAt:O}] Finished PID {processId} with exit code {process.ExitCode} " +
            $"after {(endedAt - startedAt).TotalSeconds:F1} seconds: {command}");

        string output = GetOutput(stdOutput);
        string error = GetOutput(stdError);
        LogOutput(output, error);

        return (process, Sanitize(output), Sanitize(error));

        string GetOutput(StringBuilder builder)
        {
            lock (builder)
            {
                return builder.ToString().Trim();
            }
        }

        string Sanitize(string value) => outputSanitizer?.Invoke(value) ?? value;

        void LogOutput(string output, string error)
        {
            if (logOutput && !string.IsNullOrWhiteSpace(output))
            {
                outputHelper.WriteLine(Sanitize(output));
            }

            if (logOutput && !string.IsNullOrWhiteSpace(error))
            {
                outputHelper.WriteLine(Sanitize(error));
            }
        }
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
