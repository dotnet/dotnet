// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

/// <summary>
/// Helper functions for process info.
/// </summary>
internal static class ProcessCodeMethods
{
    private const int InvalidProcessId = -1;

    public static void Suspend(this Process process)
    {
        if (process.HasExited)
        {
            EqtTrace.Verbose($"ProcessCodeMethods.Suspend: Process {process.Id} - {process.ProcessName} already exited, skipping.");
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // There is no supported or documented API to suspend a process. If there was you should call it here.
            // SuspendWindows(process);
        }
        else
        {
            // TODO: do not suspend on Mac and Linux, this prevents the process from being dumped when we use the net client dumper, checking if we can post a different signal
            // SuspendLinuxMacOs(process);
        }
    }

    public static List<ProcessTreeNode> GetProcessTree(this Process process)
    {
        var childProcesses = Process.GetProcesses()
            .Where(p => IsChildCandidate(p, process))
            .ToList();

        var acc = new List<ProcessTreeNode>();
        foreach (var c in childProcesses)
        {
            try
            {
                var parentId = GetParentPid(c);

                // c.ParentId = parentId;
                acc.Add(new ProcessTreeNode { ParentId = parentId, Process = c });
            }
            catch
            {
                // many things can go wrong with this
                // just ignore errors
            }
        }

        var level = 1;
        var limit = 10;
        ResolveChildren(process, acc, level, limit);

        return new List<ProcessTreeNode> { new() { Process = process, Level = 0 } }.Concat(acc.Where(a => a.Level > 0)).ToList();
    }

    /// <summary>
    /// Returns the parent id of a process or -1 if it fails.
    /// </summary>
    /// <param name="process">The process to find parent of.</param>
    /// <returns>The pid of the parent process.</returns>
    internal static int GetParentPid(Process process)
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? GetParentPidWindows(process)
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? GetParentPidLinux(process)
                : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
                    GetParentPidMacOs(process)
                    : throw new PlatformNotSupportedException();
    }

    internal static int GetParentPidWindows(Process process)
    {
        try
        {
            var handle = process.Handle;
            var res = NtQueryInformationProcess(handle, 0, out var pbi, Marshal.SizeOf<ProcessBasicInformation>(), out int size);

            var p = res != 0 ? InvalidProcessId : pbi.InheritedFromUniqueProcessId.ToInt32();

            return p;
        }
        catch (Exception ex)
        {
            EqtTrace.Verbose($"ProcessCodeMethods.GetParentPidLinux: Error getting parent of process {process.Id} - {process.ProcessName}, {ex}.");
            return InvalidProcessId;
        }
    }

    /// <summary>Read the /proc file system for information about the parent.</summary>
    /// <param name="process">The process to get the parent process from.</param>
    /// <returns>The process id.</returns>
    internal static int GetParentPidLinux(Process process)
    {
        var pid = process.Id;

        // read /proc/<pid>/stat
        // 4th column will contain the ppid, 92 in the example below
        // ex: 93 (bash) S 92 93 2 4294967295 ...
        var path = $"/proc/{pid}/stat";
        try
        {
            var stat = File.ReadAllText(path);
            var parts = stat.Split(' ');

            return parts.Length < 5 ? InvalidProcessId : int.Parse(parts[3], CultureInfo.CurrentCulture);
        }
        catch (Exception ex)
        {
            EqtTrace.Verbose($"ProcessCodeMethods.GetParentPidLinux: Error getting parent of process {process.Id} - {process.ProcessName}, {ex}.");
            return InvalidProcessId;
        }
    }

    internal static int GetParentPidMacOs(Process process)
    {
        try
        {
            var output = new StringBuilder();
            var err = new StringBuilder();
            Process ps = new();
            ps.StartInfo.FileName = "ps";
            ps.StartInfo.Arguments = $"-o ppid= {process.Id}";
            ps.StartInfo.UseShellExecute = false;
            ps.StartInfo.RedirectStandardOutput = true;
            ps.OutputDataReceived += (_, e) => output.Append(e.Data);
            ps.ErrorDataReceived += (_, e) => err.Append(e.Data);
            ps.Start();
            ps.BeginOutputReadLine();
            ps.WaitForExit(5_000);

            var o = output.ToString();
            var parent = int.TryParse(o.Trim(), out var ppid) ? ppid : InvalidProcessId;

            if (err.ToString() is string error && !error.IsNullOrWhiteSpace())
            {
                EqtTrace.Verbose($"ProcessCodeMethods.GetParentPidMacOs: Error getting parent of process {process.Id} - {process.ProcessName}, {error}.");
            }

            return parent;
        }
        catch (Exception ex)
        {
            EqtTrace.Verbose($"ProcessCodeMethods.GetParentPidMacOs: Error getting parent of process {process.Id} - {process.ProcessName}, {ex}.");
            return InvalidProcessId;
        }
    }

    private static void ResolveChildren(Process parent, List<ProcessTreeNode> acc, int level, int limit)
    {
        if (limit < 0)
        {
            // hit recursion limit, just returning
            return;
        }

        // only take children that are newer than the parent, because process ids (PIDs) get recycled
        var children = acc.Where(p => p.ParentId == parent.Id && p.Process?.StartTime > parent.StartTime).ToList();

        foreach (var child in children)
        {
            child.Level = level;
            ResolveChildren(child.Process!, acc, level + 1, limit);
        }
    }

    private static bool IsChildCandidate(Process child, Process parent)
    {
        // this is extremely slow under debugger, but fast without it
        try
        {
            return child.StartTime > parent.StartTime && child.Id != parent.Id;
        }
        catch
        {
            /* access denied or process has exits most likely */
            return false;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcessBasicInformation
    {
        public readonly IntPtr ExitStatus;
        public readonly IntPtr PebBaseAddress;
        public readonly IntPtr AffinityMask;
        public readonly IntPtr BasePriority;
        public readonly IntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;
    }

    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern int NtQueryInformationProcess(
        IntPtr processHandle,
        int processInformationClass,
        out ProcessBasicInformation processInformation,
        int processInformationLength,
        out int returnLength);

    // This call is undocumented api, and should not be used.
    //[DllImport("ntdll.dll", SetLastError = true)]
    //private static extern IntPtr NtSuspendProcess(IntPtr processHandle);
}
