// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.Common;

/// <summary>
/// Captures diagnostics (process tree, open pipes, and process dumps) when a command executed by the
/// scenario tests appears to hang, so that the root cause can be investigated from the uploaded build logs.
/// </summary>
public static class HangDiagnostics
{
    // Bound the number and duration of dumps so that diagnostics can't exhaust disk space or the job timeout.
    private const int MaxCaptures = 1;
    private const int MaxDumps = 8;
    private const int ToolTimeoutMilliseconds = 120_000;
    private const string BuildServerReason = "build server";

    private static int s_captureCount;

    private static readonly string[] s_buildServerMarkers = ["/nodemode:", "VBCSCompiler", "rzc.dll server", "MSBuild.dll"];

    public static void Capture(Process process, string reason, string stdOut, string stdErr, ITestOutputHelper outputHelper)
    {
        if (Interlocked.Increment(ref s_captureCount) > MaxCaptures)
        {
            outputHelper.WriteLine($"Skipping hang diagnostics; already captured {MaxCaptures} times in this run.");
            return;
        }

        try
        {
            string outputDir = CreateOutputDirectory(process.Id);
            File.WriteAllText(Path.Combine(outputDir, "summary.txt"), CreateSummary(process, reason));
            File.WriteAllText(Path.Combine(outputDir, "stdout.txt"), stdOut);
            File.WriteAllText(Path.Combine(outputDir, "stderr.txt"), stdErr);

            if (OperatingSystem.IsLinux())
            {
                CaptureLinuxDiagnostics(process.Id, outputDir);
            }
            else if (!OperatingSystem.IsWindows())
            {
                File.WriteAllText(Path.Combine(outputDir, "ps.txt"), RunTool("ps", "-axo pid,ppid,pgid,stat,etime,command"));
            }

            outputHelper.WriteLine($"Hang diagnostics for process {process.Id} written to {outputDir}");
        }
        catch (Exception ex)
        {
            outputHelper.WriteLine($"Failed to capture hang diagnostics: {ex}");
        }
    }

    private static string CreateOutputDirectory(int pid)
    {
        string? baseDir = Environment.GetEnvironmentVariable(ScenarioTestFixture.BinlogDirEnvironmentVariable);
        if (string.IsNullOrEmpty(baseDir))
        {
            baseDir = Path.GetTempPath();
        }

        string outputDir = Path.Combine(baseDir, "hang-diagnostics", $"{DateTime.UtcNow:yyyyMMdd-HHmmss}-{pid}");
        Directory.CreateDirectory(outputDir);
        return outputDir;
    }

    private static string CreateSummary(Process process, string reason)
    {
        StringBuilder summary = new();
        summary.AppendLine($"Reason: {reason}");
        summary.AppendLine($"Time (UTC): {DateTime.UtcNow:O}");
        summary.AppendLine($"Command: {process.StartInfo.FileName} {process.StartInfo.Arguments}");
        summary.AppendLine($"Working directory: {process.StartInfo.WorkingDirectory}");
        summary.AppendLine($"Process id: {process.Id}");
        summary.AppendLine($"Test host process id: {Environment.ProcessId}");
        summary.AppendLine("Environment overrides:");
        foreach (string key in process.StartInfo.Environment.Keys.Where(IsInterestingEnvironmentVariable).Order())
        {
            summary.AppendLine($"  {key}={process.StartInfo.Environment[key]}");
        }
        return summary.ToString();
    }

    private static bool IsInterestingEnvironmentVariable(string key) =>
        key.StartsWith("DOTNET_", StringComparison.OrdinalIgnoreCase) ||
        key.StartsWith("MSBUILD", StringComparison.OrdinalIgnoreCase) ||
        key.StartsWith("NUGET_", StringComparison.OrdinalIgnoreCase) ||
        key is "HOME" or "TMPDIR";

    private static void CaptureLinuxDiagnostics(int pid, string outputDir)
    {
        File.WriteAllText(Path.Combine(outputDir, "ps.txt"), RunTool("ps", "-eo pid,ppid,pgid,sid,etimes,stat,args --forest"));

        Dictionary<int, string> suspects = FindSuspectProcesses(pid);
        File.WriteAllText(Path.Combine(outputDir, "suspects.txt"), DescribeProcesses(suspects));

        // Build servers are long-lived and shared, so they're dumped last and without heap to bound dump size.
        foreach ((int suspect, string reason) in suspects.OrderBy(s => s.Value == BuildServerReason).Take(MaxDumps))
        {
            uint dumpType = reason == BuildServerReason ? DiagnosticsIpc.MiniDumpType : DiagnosticsIpc.WithHeapDumpType;
            WriteDump(suspect, dumpType, outputDir);
        }
    }

    /// <summary>
    /// Returns the processes most likely involved in the hang, keyed by pid with the reason they were selected:
    /// descendants of the hung command, processes sharing a pipe with the test host (i.e. holding a
    /// redirected stdout/stderr handle open), and long-lived build server processes.
    /// </summary>
    private static Dictionary<int, string> FindSuspectProcesses(int rootPid)
    {
        Dictionary<int, string> suspects = new();
        foreach (int descendant in ProcFs.GetDescendants(rootPid).Prepend(rootPid))
        {
            suspects.TryAdd(descendant, "hung command or descendant");
        }

        HashSet<string> hostPipes = ProcFs.GetPipes(Environment.ProcessId);
        foreach (int pid in ProcFs.EnumeratePids().Where(p => p != Environment.ProcessId))
        {
            if (ProcFs.GetPipes(pid).Overlaps(hostPipes))
            {
                suspects.TryAdd(pid, "shares a pipe with the test host");
            }
            else if (s_buildServerMarkers.Any(ProcFs.GetCommandLine(pid).Contains))
            {
                suspects.TryAdd(pid, BuildServerReason);
            }
        }

        return suspects;
    }

    private static string DescribeProcesses(Dictionary<int, string> processes)
    {
        StringBuilder description = new();
        foreach ((int pid, string reason) in processes)
        {
            description.AppendLine($"== pid {pid} ({reason}): {ProcFs.GetCommandLine(pid)}");
            foreach ((string fd, string target) in ProcFs.GetFileDescriptors(pid))
            {
                description.AppendLine($"   fd {fd} -> {target}");
            }
        }
        return description.ToString();
    }

    private static void WriteDump(int pid, uint dumpType, string outputDir)
    {
        string? socket = DiagnosticsIpc.FindSocket(pid, ProcFs.GetEnvironmentVariable(pid, "TMPDIR"));
        if (socket is null)
        {
            return;
        }

        string dumpPath = Path.Combine(outputDir, $"dump.{pid}.dmp");
        string outcome;
        try
        {
            outcome = DiagnosticsIpc.WriteDump(socket, dumpPath, dumpType, ToolTimeoutMilliseconds);
        }
        catch (Exception ex)
        {
            outcome = $"Dump failed: {ex.Message}";
        }
        File.AppendAllText(Path.Combine(outputDir, "dumps.txt"), $"pid {pid}: {outcome}{Environment.NewLine}");
    }

    private static string RunTool(string fileName, string args)
    {
        try
        {
            using Process tool = new()
            {
                StartInfo = { FileName = fileName, Arguments = args, RedirectStandardOutput = true, RedirectStandardError = true }
            };
            tool.Start();
            Task<string> stdOut = tool.StandardOutput.ReadToEndAsync();
            Task<string> stdErr = tool.StandardError.ReadToEndAsync();
            if (!tool.WaitForExit(ToolTimeoutMilliseconds))
            {
                tool.Kill(true);
                return $"{fileName} {args} timed out";
            }

            Task.WaitAll([stdOut, stdErr], ToolTimeoutMilliseconds);
            return $"{(stdOut.IsCompleted ? stdOut.Result : string.Empty)}{(stdErr.IsCompleted ? stdErr.Result : string.Empty)}";
        }
        catch (Exception ex)
        {
            return $"Failed to run {fileName} {args}: {ex.Message}";
        }
    }
}
