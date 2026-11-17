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

    public const string HangTimeoutEnvironmentVariable = "SCENARIO_TESTS_HANG_TIMEOUT_SECONDS";

    // Upper bound for commands executed without an explicit timeout. No single command is expected to come
    // close to this; exceeding it is treated as a hang and diagnostics are captured before failing the test.
    public static int HangTimeoutMilliseconds { get; } =
        int.TryParse(Environment.GetEnvironmentVariable(HangTimeoutEnvironmentVariable), out int seconds) && seconds > 0
            ? seconds * 1000
            : 10 * 60 * 1000;

    public static (Process Process, string StdOut, string StdErr) ExecuteProcess(
        string fileName,
        string args,
        ITestOutputHelper outputHelper,
        bool logOutput = false,
        Action<Process>? configure = null,
        int millisecondTimeout = -1)
    {
        outputHelper.WriteLine($"Executing: {fileName} {args}");

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
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (millisecondTimeout == -1)
        {
            WaitForExitOrCaptureHang(process, stdOutput, stdError, outputHelper);
        }
        else
        {
            process.WaitForExit(millisecondTimeout);
        }

        if (!process.HasExited)
        {
            outputHelper.WriteLine($"Killing: {fileName} {args}");
            process.Kill(true);
            if (!process.WaitForExit(KillGraceMilliseconds))
            {
                outputHelper.WriteLine("Process tree did not fully exit within the kill grace period; " +
                    "abandoning wait to avoid hanging on inherited stdout/stderr handles.");
            }
            ProcessStartInfo startInfo = process.StartInfo;
            string msg = $" {startInfo.FileName} {startInfo.Arguments} timed out after " +
                $"{(millisecondTimeout == -1 ? HangTimeoutMilliseconds : millisecondTimeout)} milliseconds" +
                $"{Environment.NewLine}Exit code: {process.ExitCode}";
            throw new InvalidOperationException(msg);
        }

        string output;
        lock (stdOutput)
        {
            output = stdOutput.ToString().Trim();
        }
        if (logOutput && !string.IsNullOrWhiteSpace(output))
        {
            outputHelper.WriteLine(output);
        }

        string error;
        lock (stdError)
        {
            error = stdError.ToString().Trim();
        }
        if (logOutput && !string.IsNullOrWhiteSpace(error))
        {
            outputHelper.WriteLine(error);
        }

        return (process, output, error);
    }

    /// <summary>
    /// Waits for a command without an explicit timeout to exit and for its redirected output to reach EOF,
    /// bounded by <see cref="HangTimeoutMilliseconds"/>. On a hang, captures diagnostics; if the process itself
    /// is stuck it is left running for the caller's kill path, and if only the output pipes are held open
    /// (by a surviving descendant) the command fails since its output can't be trusted to be complete.
    /// </summary>
    private static void WaitForExitOrCaptureHang(
        Process process, StringBuilder stdOutput, StringBuilder stdError, ITestOutputHelper outputHelper)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (!process.WaitForExit(HangTimeoutMilliseconds))
        {
            CaptureHang(process, $"Process did not exit within {HangTimeoutMilliseconds} ms", stdOutput, stdError, outputHelper);
            return;
        }

        // A finite WaitForExit doesn't wait for the async output readers to reach EOF; the parameterless overload does.
        int remaining = (int)Math.Max(0, HangTimeoutMilliseconds - stopwatch.ElapsedMilliseconds);
        if (!Task.Run(() => process.WaitForExit()).Wait(remaining))
        {
            string reason = $"Process exited with code {process.ExitCode} but its stdout/stderr were still open " +
                $"after {HangTimeoutMilliseconds} ms (likely held by a surviving descendant process)";
            CaptureHang(process, reason, stdOutput, stdError, outputHelper);
            throw new InvalidOperationException($"{process.StartInfo.FileName} {process.StartInfo.Arguments}: {reason}");
        }
    }

    private static void CaptureHang(
        Process process, string reason, StringBuilder stdOutput, StringBuilder stdError, ITestOutputHelper outputHelper)
    {
        outputHelper.WriteLine($"Hang detected: {process.StartInfo.FileName} {process.StartInfo.Arguments}: {reason}");

        string output;
        lock (stdOutput)
        {
            output = stdOutput.ToString();
        }

        string error;
        lock (stdError)
        {
            error = stdError.ToString();
        }

        HangDiagnostics.Capture(process, reason, output, error, outputHelper);
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
