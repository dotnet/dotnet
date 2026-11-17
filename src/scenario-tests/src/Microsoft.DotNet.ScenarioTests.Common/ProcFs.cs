// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.ScenarioTests.Common;

/// <summary>
/// Minimal, best-effort helpers for inspecting processes via the Linux /proc filesystem.
/// Processes may exit at any point while being inspected, so all accessors tolerate failures.
/// </summary>
internal static class ProcFs
{
    public static IEnumerable<int> EnumeratePids() =>
        Directory.EnumerateDirectories("/proc")
            .Select(Path.GetFileName)
            .Select(name => int.TryParse(name, out int pid) ? pid : -1)
            .Where(pid => pid > 0)
            .ToArray();

    public static IEnumerable<int> GetDescendants(int rootPid)
    {
        ILookup<int, int> childrenByParent = EnumeratePids()
            .Select(pid => (Pid: pid, ParentPid: GetParentPid(pid)))
            .Where(p => p.ParentPid.HasValue)
            .ToLookup(p => p.ParentPid!.Value, p => p.Pid);

        List<int> descendants = new();
        Queue<int> pending = new([rootPid]);
        while (pending.TryDequeue(out int current))
        {
            foreach (int child in childrenByParent[current])
            {
                descendants.Add(child);
                pending.Enqueue(child);
            }
        }
        return descendants;
    }

    public static int? GetParentPid(int pid)
    {
        // Format: "pid (comm) state ppid ..."; comm may contain spaces or parentheses so parse after the last ')'.
        string? stat = TryRead($"/proc/{pid}/stat");
        int commEnd = stat?.LastIndexOf(')') ?? -1;
        if (commEnd < 0)
        {
            return null;
        }

        string[] fields = stat!.Substring(commEnd + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fields.Length > 1 && int.TryParse(fields[1], out int parentPid) ? parentPid : null;
    }

    public static string GetCommandLine(int pid) =>
        TryRead($"/proc/{pid}/cmdline")?.Replace('\0', ' ').Trim() ?? string.Empty;

    public static IEnumerable<(string Fd, string Target)> GetFileDescriptors(int pid)
    {
        try
        {
            return Directory.EnumerateFileSystemEntries($"/proc/{pid}/fd")
                .Select(path => (Fd: Path.GetFileName(path), Target: TryGetLinkTarget(path)))
                .ToArray();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return [];
        }
    }

    public static HashSet<string> GetPipes(int pid) =>
        GetFileDescriptors(pid)
            .Select(fd => fd.Target)
            .Where(target => target.StartsWith("pipe:", StringComparison.Ordinal))
            .ToHashSet();

    public static string? GetEnvironmentVariable(int pid, string name) =>
        TryRead($"/proc/{pid}/environ")?
            .Split('\0')
            .Where(entry => entry.StartsWith(name + "=", StringComparison.Ordinal))
            .Select(entry => entry.Substring(name.Length + 1))
            .FirstOrDefault();

    private static string TryGetLinkTarget(string path)
    {
        try
        {
            return new FileInfo(path).LinkTarget ?? string.Empty;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return string.Empty;
        }
    }

    private static string? TryRead(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }
}
