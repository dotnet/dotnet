// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace TestUtilities;

public static class ExecuteHelper
{
    public const int DefaultTimeoutMilliseconds = 60_000;
    public const int LongRunningTimeoutMilliseconds = 600_000;
    private const int KillGraceMilliseconds = 30_000;

    public static (Process Process, string StdOut, string StdErr) ExecuteProcess(
        string fileName,
        string args,
        ITestOutputHelper outputHelper,
        bool logOutput = false,
        bool excludeInfo = false,
        Action<Process>? configureCallback = null,
        int millisecondTimeout = DefaultTimeoutMilliseconds,
        Action<Process, ProcessOutputLine>? outputLineCallback = null)
    {
        if (!excludeInfo)
        {
            outputHelper.WriteLine($"Executing: {fileName} {args}");
        }

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

        // The `dotnet test` execution context sets a number of dotnet related ENVs that cause issues when executing
        // dotnet commands.  Clear these to avoid side effects.
        foreach (string key in process.StartInfo.Environment.Keys.Where(key => key != "HOME" && key != "PATH").ToList())
        {
            process.StartInfo.Environment.Remove(key);
        }

        configureCallback?.Invoke(process);
        process.Start();

        StringBuilder stdOutput = new();
        StringBuilder stdError = new();
        bool timedOut = false;
        try
        {
            foreach (ProcessOutputLine line in process.ReadAllLines(TimeSpan.FromMilliseconds(millisecondTimeout)))
            {
                (line.StandardError ? stdError : stdOutput).AppendLine(line.Content);
                outputLineCallback?.Invoke(process, line);
            }
        }
        catch (TimeoutException)
        {
            timedOut = true;
        }

        if (timedOut || !process.WaitForExit(millisecondTimeout))
        {
            outputHelper.WriteLine($"Process did not exit. Killing {fileName} {args} after waiting {millisecondTimeout} milliseconds.");
            process.Kill(true);
            if (!process.WaitForExit(KillGraceMilliseconds))
            {
                throw new InvalidOperationException(
                    $"Process tree did not exit within {KillGraceMilliseconds} milliseconds after it was killed.");
            }
        }

        string output = stdOutput.ToString().Trim();
        string error = stdError.ToString().Trim();

        if (logOutput)
        {
            if (!string.IsNullOrWhiteSpace(output))
            {
                outputHelper.WriteLine(output);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                outputHelper.WriteLine(error);
            }
        }

        return (process, output, error);
    }

    public static string ExecuteProcessValidateExitCode(string fileName, string args, ITestOutputHelper outputHelper, string additionalOutput = "",
        int millisecondTimeout = DefaultTimeoutMilliseconds)
    {
        (Process Process, string StdOut, string StdErr) result = ExecuteHelper.ExecuteProcess(fileName, args, outputHelper, millisecondTimeout: millisecondTimeout);
        ValidateExitCode(result, additionalOutput: additionalOutput);

        return result.StdOut;
    }

    public static void ValidateExitCode((Process Process, string StdOut, string StdErr) result, int expectedExitCode = 0, string additionalOutput = "")
    {
        if (result.Process.ExitCode != expectedExitCode)
        {
            ProcessStartInfo startInfo = result.Process.StartInfo;
            string msg = $"Failed to execute {startInfo.FileName} {startInfo.Arguments}" +
                $"{Environment.NewLine}Exit code: {result.Process.ExitCode}" +
                $"{Environment.NewLine}{result.StdOut}" +
                $"{Environment.NewLine}{result.StdErr}" +
                (!string.IsNullOrWhiteSpace(additionalOutput) ? $"{Environment.NewLine}{additionalOutput}" : "");
  
            throw new InvalidOperationException(msg);
        }
    }
}
