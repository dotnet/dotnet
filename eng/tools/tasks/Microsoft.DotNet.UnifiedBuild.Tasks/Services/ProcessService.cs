// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.UnifiedBuild.Tasks.Services;

public record ProcessEnvironmentVariable(string Name, string Value);
public record ProcessResult(string Output, string Error, int ExitCode);

public class ProcessService(TaskLoggingHelper log, int timeout = 10 * 1000)
{
    private TaskLoggingHelper Log { get; } = log;
    private TimeSpan Timeout { get; } = TimeSpan.FromMilliseconds(timeout);

    public async Task<ProcessResult> RunProcessAsync(
        string command,
        string arguments,
        List<ProcessEnvironmentVariable>? environmentVariables = null,
        string workingDirectory = "",
        bool printOutput = false)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory
        };

        if (environmentVariables != null)
        {
            foreach (ProcessEnvironmentVariable env in environmentVariables)
            {
                processInfo.EnvironmentVariables[env.Name] = env.Value;
            }
        }

        using var process = new Process { StartInfo = processInfo, EnableRaisingEvents = true };

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        var outputTcs = new TaskCompletionSource();
        var errorTcs = new TaskCompletionSource();

        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data == null)
            {
                outputTcs.TrySetResult();
                return;
            }

            outputBuilder.AppendLine(e.Data);
            if (printOutput)
            {
                Log.LogMessage(MessageImportance.High, e.Data);
            }
        };

        process.ErrorDataReceived += (s, e) =>
        {
            if (e.Data == null)
            {
                errorTcs.TrySetResult();
                return;
            }

            errorBuilder.AppendLine(e.Data);
            if (printOutput)
            {
                Log.LogMessage(MessageImportance.High, e.Data);
            }
        };

        if (!process.Start())
        {
            throw new InvalidOperationException($"Failed to start process: {command} {arguments}");
        }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var timeoutCts = new CancellationTokenSource(Timeout);

        var exitedTask = Task.Run(() => process.WaitForExit(), timeoutCts.Token);

        try
        {
            await exitedTask;
            await Task.WhenAll(outputTcs.Task, errorTcs.Task);
        }
        catch (OperationCanceledException)
        {
            try { process.Kill(entireProcessTree: true); } catch { }
            throw new TimeoutException($"Process timed out after {Timeout.TotalMilliseconds} ms");
        }

        return new ProcessResult(
            Output: outputBuilder.ToString(),
            Error: errorBuilder.ToString(),
            ExitCode: process.ExitCode
        );
    }
}
