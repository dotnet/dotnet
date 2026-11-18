// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.DotNet.ScenarioTests.Common;

/// <summary>
/// Waits for a child process and, when it is still running after a threshold, records what it and its
/// descendants are doing before continuing to wait. Test jobs intermittently hang on a child process that
/// never exits and are killed by the pipeline timeout with no information about what was stuck; these
/// diagnostics (process tree, holders of the inherited stdout/stderr pipes, and dumps of the stuck .NET
/// processes) are written next to the job logs so the hang can be root-caused.
/// The wait semantics are unchanged: diagnostics are collected once and the original wait then continues.
/// </summary>
public static class ProcessHangDiagnostics
{
    public const string ThresholdMinutesEnvironmentVariable = "TEST_HANG_DIAGNOSTICS_THRESHOLD_MINUTES";
    public const string DirectoryEnvironmentVariable = "TEST_HANG_DIAGNOSTICS_DIR";

    private const int DefaultThresholdMinutes = 10;
    private const int MaxDumps = 4;
    private const int DumpTimeoutMilliseconds = 120_000;
    private const int MaxOutputLines = 200;
    private const int MaxCommandLineLength = 1000;
    private static int s_dumpCount;

    /// <summary>
    /// Same semantics as <see cref="Process.WaitForExit(int)"/>, plus diagnostics when the process outlives the threshold.
    /// </summary>
    public static bool WaitForExit(Process process, int millisecondTimeout, Func<string> getOutputSoFar, Action<string> log)
    {
        int thresholdMilliseconds = GetThresholdMilliseconds();
        if (thresholdMilliseconds <= 0 || (millisecondTimeout != Timeout.Infinite && millisecondTimeout <= thresholdMilliseconds))
        {
            return process.WaitForExit(millisecondTimeout);
        }

        if (millisecondTimeout == Timeout.Infinite ? WaitForExitAndOutput(process, thresholdMilliseconds) : process.WaitForExit(thresholdMilliseconds))
        {
            return true;
        }

        CollectDiagnostics(process, thresholdMilliseconds, getOutputSoFar, log);

        return process.WaitForExit(millisecondTimeout == Timeout.Infinite ? Timeout.Infinite : millisecondTimeout - thresholdMilliseconds);
    }

    // An infinite WaitForExit also waits for the redirected stdout/stderr to reach EOF; WaitForExitAsync has the same
    // behavior and can be bounded, which catches descendants that keep the inherited pipes open after the process exits.
    private static bool WaitForExitAndOutput(Process process, int millisecondTimeout)
    {
        using CancellationTokenSource cts = new(millisecondTimeout);
        try
        {
            process.WaitForExitAsync(cts.Token).GetAwaiter().GetResult();
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    private static int GetThresholdMilliseconds()
    {
        string? value = Environment.GetEnvironmentVariable(ThresholdMinutesEnvironmentVariable);
        int minutes = int.TryParse(value, out int parsed) ? parsed : DefaultThresholdMinutes;
        return minutes <= 0 ? 0 : (int)Math.Min(int.MaxValue, minutes * 60_000L);
    }

    private static void CollectDiagnostics(Process process, int thresholdMilliseconds, Func<string> getOutputSoFar, Action<string> log)
    {
        try
        {
            string directory = GetDiagnosticsDirectory();
            Directory.CreateDirectory(directory);
            string prefix = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}-pid{process.Id}";
            string reportPath = Path.Combine(directory, prefix + ".txt");

            StringBuilder report = new();
            report.AppendLine($"Command: {process.StartInfo.FileName} {process.StartInfo.Arguments}");
            report.AppendLine($"Working directory: {process.StartInfo.WorkingDirectory}");
            report.AppendLine($"Test host pid: {Environment.ProcessId}, child pid: {process.Id}");
            report.AppendLine($"Collected at (UTC): {DateTime.UtcNow:O}, after waiting {thresholdMilliseconds / 60_000} minutes.");
            bool exited = process.HasExited;
            report.AppendLine(exited
                ? $"The process has EXITED (exit code {process.ExitCode}) but its redirected stdout/stderr did not reach EOF: another process still holds the inherited pipes (see 'Pipe holders')."
                : "The process is still RUNNING.");
            report.AppendLine();

            List<int> dumpCandidates = [];
            if (!exited)
            {
                dumpCandidates.Add(process.Id);
            }

            if (OperatingSystem.IsLinux())
            {
                Dictionary<int, ProcessEntry> processes = ReadLinuxProcesses();
                List<ProcessEntry> tree = GetSubtree(processes, process.Id);
                AppendProcesses(report, "Process tree of the child", tree);
                dumpCandidates.AddRange(tree.Select(p => p.Pid));

                List<ProcessEntry> pipeHolders = GetLinuxPipeHolders(processes);
                AppendProcesses(report, "Pipe holders (processes sharing a pipe with the test host, other than its own stdio)", pipeHolders);
                dumpCandidates.AddRange(pipeHolders.Select(p => p.Pid));
            }
            else if (OperatingSystem.IsMacOS())
            {
                List<ProcessEntry> tree = GetSubtree(ReadPsProcesses(), process.Id);
                AppendProcesses(report, "Process tree of the child", tree);
                dumpCandidates.AddRange(tree.Select(p => p.Pid));
            }

            report.AppendLine("Output of the child so far (last lines):");
            string[] outputLines = getOutputSoFar().Split('\n');
            foreach (string line in outputLines.Skip(Math.Max(0, outputLines.Length - MaxOutputLines)))
            {
                report.AppendLine("  " + line.TrimEnd('\r'));
            }
            report.AppendLine();

            File.WriteAllText(reportPath, report.ToString());
            log($"Process {process.Id} ({process.StartInfo.FileName} {process.StartInfo.Arguments}) has not completed after {thresholdMilliseconds / 60_000} minutes. Hang diagnostics: {reportPath}");

            StringBuilder dumps = new("Dumps:" + Environment.NewLine);
            foreach (int pid in dumpCandidates.Distinct())
            {
                if (Interlocked.Increment(ref s_dumpCount) > MaxDumps)
                {
                    dumps.AppendLine($"  Dump limit of {MaxDumps} per test run reached.");
                    break;
                }

                string dumpPath = Path.Combine(directory, $"{prefix}-dump-{pid}.dmp");
                dumps.AppendLine($"  {pid}: {TryWriteDump(pid, dumpPath)}");
            }

            File.AppendAllText(reportPath, dumps.ToString());
        }
        catch (Exception ex)
        {
            log($"Failed to collect hang diagnostics for process {process.Id}: {ex}");
        }
    }

    private static string GetDiagnosticsDirectory()
    {
        string? configured = Environment.GetEnvironmentVariable(DirectoryEnvironmentVariable);
        if (!string.IsNullOrEmpty(configured))
        {
            return configured;
        }

        // The binlog directory is published with the job logs, including when the job is canceled by its timeout.
        string? binlogDir = Environment.GetEnvironmentVariable(ScenarioTestFixture.BinlogDirEnvironmentVariable);
        return Path.Combine(string.IsNullOrEmpty(binlogDir) ? Path.GetTempPath() : binlogDir, "hang-diagnostics");
    }

    private sealed record ProcessEntry(int Pid, int ParentPid, string State, string CommandLine, string? Note = null);

    private static void AppendProcesses(StringBuilder report, string title, List<ProcessEntry> processes)
    {
        report.AppendLine($"{title}:");
        if (processes.Count == 0)
        {
            report.AppendLine("  (none)");
        }

        foreach (ProcessEntry p in processes)
        {
            report.AppendLine($"  pid={p.Pid} ppid={p.ParentPid} state={p.State}{(p.Note is null ? "" : " " + p.Note)} {p.CommandLine}");
        }

        report.AppendLine();
    }

    private static List<ProcessEntry> GetSubtree(Dictionary<int, ProcessEntry> processes, int rootPid)
    {
        List<ProcessEntry> result = [];
        Queue<int> pending = new([rootPid]);
        HashSet<int> seen = [];
        while (pending.Count > 0)
        {
            int pid = pending.Dequeue();
            if (!seen.Add(pid))
            {
                continue;
            }

            if (processes.TryGetValue(pid, out ProcessEntry? entry))
            {
                result.Add(entry);
            }

            foreach (ProcessEntry child in processes.Values.Where(p => p.ParentPid == pid))
            {
                pending.Enqueue(child.Pid);
            }
        }

        return result;
    }

    private static Dictionary<int, ProcessEntry> ReadLinuxProcesses()
    {
        Dictionary<int, ProcessEntry> processes = [];
        foreach (string dir in Directory.EnumerateDirectories("/proc"))
        {
            if (!int.TryParse(Path.GetFileName(dir), out int pid))
            {
                continue;
            }

            try
            {
                // Format: "pid (comm) state ppid ..."; comm may contain spaces and parentheses.
                string stat = File.ReadAllText(Path.Combine(dir, "stat"));
                int commEnd = stat.LastIndexOf(')');
                string[] fields = stat[(commEnd + 2)..].Split(' ');
                string commandLine = File.ReadAllText(Path.Combine(dir, "cmdline")).Replace('\0', ' ').Trim();
                if (commandLine.Length == 0)
                {
                    commandLine = "[" + stat[(stat.IndexOf('(') + 1)..commEnd] + "]";
                }

                processes[pid] = new ProcessEntry(pid, int.Parse(fields[1]), fields[0], Truncate(commandLine));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or FormatException or IndexOutOfRangeException or ArgumentOutOfRangeException)
            {
                // The process exited or is not accessible.
            }
        }

        return processes;
    }

    private static List<ProcessEntry> GetLinuxPipeHolders(Dictionary<int, ProcessEntry> processes)
    {
        int self = Environment.ProcessId;
        HashSet<string> ownPipes = GetLinuxPipes(self, excludeStdio: true);
        List<ProcessEntry> holders = [];
        foreach (ProcessEntry entry in processes.Values)
        {
            if (entry.Pid == self)
            {
                continue;
            }

            string[] shared = GetLinuxPipes(entry.Pid, excludeStdio: false).Where(ownPipes.Contains).ToArray();
            if (shared.Length > 0)
            {
                holders.Add(entry with { Note = "pipes=" + string.Join(",", shared) });
            }
        }

        return holders;
    }

    private static HashSet<string> GetLinuxPipes(int pid, bool excludeStdio)
    {
        HashSet<string> pipes = [];
        try
        {
            foreach (string fd in Directory.EnumerateFileSystemEntries($"/proc/{pid}/fd"))
            {
                if (excludeStdio && Path.GetFileName(fd) is "0" or "1" or "2")
                {
                    continue;
                }

                try
                {
                    string? target = new FileInfo(fd).LinkTarget;
                    if (target is not null && target.StartsWith("pipe:", StringComparison.Ordinal))
                    {
                        pipes.Add(target);
                    }
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                }
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }

        return pipes;
    }

    private static Dictionary<int, ProcessEntry> ReadPsProcesses()
    {
        Dictionary<int, ProcessEntry> processes = [];
        using Process ps = new()
        {
            StartInfo =
            {
                FileName = "ps",
                Arguments = "-axo pid=,ppid=,stat=,command=",
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }
        };

        ps.Start();
        string output = ps.StandardOutput.ReadToEndAsync().WaitAsync(TimeSpan.FromSeconds(30)).GetAwaiter().GetResult();
        foreach (string line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            string[] parts = line.Trim().Split(' ', 4, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3 && int.TryParse(parts[0], out int pid) && int.TryParse(parts[1], out int ppid))
            {
                processes[pid] = new ProcessEntry(pid, ppid, parts[2], Truncate(parts.Length > 3 ? parts[3] : ""));
            }
        }

        return processes;
    }

    private static string Truncate(string value) =>
        value.Length <= MaxCommandLineLength ? value : value[..MaxCommandLineLength] + "...";

    /// <summary>
    /// Asks a .NET process to write a dump of itself through its diagnostics IPC channel (the same request
    /// `dotnet-dump collect` sends), so no extra tools or packages are needed on the machine.
    /// </summary>
    private static string TryWriteDump(int pid, string dumpPath)
    {
        try
        {
            using Stream? channel = ConnectDiagnosticsChannel(pid);
            if (channel is null)
            {
                return "no .NET diagnostics endpoint (not a .NET process, or diagnostics are disabled)";
            }

            using CancellationTokenSource cts = new(DumpTimeoutMilliseconds);
            channel.WriteAsync(CreateDumpRequest(dumpPath), cts.Token).AsTask().GetAwaiter().GetResult();

            byte[] header = new byte[IpcHeaderSize];
            channel.ReadExactlyAsync(header, cts.Token).AsTask().GetAwaiter().GetResult();
            int payloadSize = BitConverter.ToUInt16(header, 14) - IpcHeaderSize;
            byte[] payload = new byte[Math.Max(0, payloadSize)];
            channel.ReadExactlyAsync(payload, cts.Token).AsTask().GetAwaiter().GetResult();

            uint result = payload.Length >= 4 ? BitConverter.ToUInt32(payload, 0) : 0;
            return header[17] == IpcResponseOk ? $"written to {dumpPath}" : $"dump request failed with 0x{result:X8}";
        }
        catch (Exception ex)
        {
            return $"dump request failed: {ex.GetType().Name}: {ex.Message}";
        }
    }

    // Diagnostics IPC protocol: https://github.com/dotnet/diagnostics/blob/main/documentation/design-docs/ipc-protocol.md
    private const int IpcHeaderSize = 20;
    private const byte DumpCommandSet = 0x01;
    private const byte GenerateCoreDumpCommandId = 0x01;
    private const byte IpcResponseOk = 0x00;
    private const uint DumpTypeWithHeap = 2;

    private static byte[] CreateDumpRequest(string dumpPath)
    {
        using MemoryStream payload = new();
        using (BinaryWriter writer = new(payload, Encoding.Unicode, leaveOpen: true))
        {
            writer.Write((uint)(dumpPath.Length + 1));
            writer.Write(Encoding.Unicode.GetBytes(dumpPath + "\0"));
            writer.Write(DumpTypeWithHeap);
            writer.Write(0u); // diagnostics flags
        }

        using MemoryStream message = new();
        using (BinaryWriter writer = new(message))
        {
            writer.Write(Encoding.ASCII.GetBytes("DOTNET_IPC_V1\0"));
            writer.Write((ushort)(IpcHeaderSize + payload.Length));
            writer.Write(DumpCommandSet);
            writer.Write(GenerateCoreDumpCommandId);
            writer.Write((ushort)0);
            writer.Write(payload.ToArray());
        }

        return message.ToArray();
    }

    private static Stream? ConnectDiagnosticsChannel(int pid)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            NamedPipeClientStream pipe = new(".", $"dotnet-diagnostic-{pid}", PipeDirection.InOut, PipeOptions.Asynchronous);
            try
            {
                pipe.Connect(5_000);
                return pipe;
            }
            catch (Exception ex) when (ex is TimeoutException or IOException)
            {
                pipe.Dispose();
                return null;
            }
        }

        string? socketPath = FindDiagnosticsSocket(pid);
        if (socketPath is null)
        {
            return null;
        }

        Socket socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
        try
        {
            socket.Connect(new UnixDomainSocketEndPoint(socketPath));
            return new NetworkStream(socket, ownsSocket: true);
        }
        catch (SocketException)
        {
            socket.Dispose();
            return null;
        }
    }

    private static string? FindDiagnosticsSocket(int pid)
    {
        // The runtime creates the socket in its own TMPDIR, which may differ from the test host's.
        List<string> directories = [Path.GetTempPath(), "/tmp/"];
        if (OperatingSystem.IsLinux())
        {
            try
            {
                string? tmpDir = File.ReadAllText($"/proc/{pid}/environ")
                    .Split('\0')
                    .FirstOrDefault(v => v.StartsWith("TMPDIR=", StringComparison.Ordinal))?["TMPDIR=".Length..];
                if (!string.IsNullOrEmpty(tmpDir))
                {
                    directories.Insert(0, tmpDir);
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
            }
        }

        return directories
            .Where(Directory.Exists)
            .SelectMany(d => Directory.EnumerateFiles(d, $"dotnet-diagnostic-{pid}-*-socket"))
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .FirstOrDefault();
    }
}
