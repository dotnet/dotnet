// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.Common;

public static class ExecuteHelper
{
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
        process.WaitForExit(millisecondTimeout);

        if (!process.HasExited)
        {
            outputHelper.WriteLine($"Killing: {fileName} {args}");
            process.Kill(true);
            process.WaitForExit();
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
