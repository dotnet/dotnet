// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Microsoft.TestPlatform.AttachVS;

internal class DebuggerUtility
{
    internal static bool AttachVSToProcess(int? pid, int? vsPid)
    {
        try
        {
            if (pid == null)
            {
                Trace($"FAIL: Pid is null.");
                return false;
            }

            var process = Process.GetProcessById(pid.Value);
            Trace($"Starting with pid '{pid}({process.ProcessName})', and vsPid '{vsPid}'");
            Trace($"Using pid: {pid} to get parent VS.");
            var vs = GetVsFromPid(Process.GetProcessById(vsPid ?? process.Id));

            if (vs != null)
            {
                Trace($"Parent VS is {vs.ProcessName} ({vs.Id}).");
                AttachTo(process, vs);
                return true;
            }

            Trace($"Parent VS not found, finding the first VS that started.");
            var firstVs = Process.GetProcesses()
                .Where(p => p.ProcessName == "devenv")
                .Select(p =>
                {
                    try
                    {
                        return new { Process = p, p.StartTime, p.HasExited };
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(p => p != null && !p.HasExited)
                .OrderBy(p => p!.StartTime)
                .FirstOrDefault();

            if (firstVs != null)
            {
                Trace($"Found VS {firstVs.Process.Id}");
                AttachTo(process, firstVs.Process);
                return true;
            }

            Trace("Could not find any started VS.");
        }
        catch (Exception ex)
        {
            Trace($"ERROR: {ex}, {ex.StackTrace}");
        }

        return false;
    }

    private static void AttachTo(Process process, Process vs)
    {
        var attached = AttachVs(vs, process.Id);
        if (attached)
        {
            // You won't see this in DebugView++ because at this point VS is already attached and all the output goes into Debug window in VS.
            Trace($"SUCCESS: Attached process: {process.ProcessName} ({process.Id})");
        }
        else
        {
            Trace($"FAIL: Could not attach process: {process.ProcessName} ({process.Id})");
        }
    }

    private static bool AttachVs(Process vs, int pid)
    {
        IBindCtx? bindCtx = null;
        IRunningObjectTable? runningObjectTable = null;
        IEnumMoniker? enumMoniker = null;
        try
        {
            var r = CreateBindCtx(0, out bindCtx);
            Marshal.ThrowExceptionForHR(r);
            if (bindCtx == null)
            {
                Trace($"BindCtx is null. Cannot attach VS.");
                return false;
            }
            bindCtx.GetRunningObjectTable(out runningObjectTable);
            if (runningObjectTable == null)
            {
                Trace($"RunningObjectTable is null. Cannot attach VS.");
                return false;
            }

            runningObjectTable.EnumRunning(out enumMoniker);
            if (enumMoniker == null)
            {
                Trace($"EnumMoniker is null. Cannot attach VS.");
                return false;
            }

            var dteSuffix = ":" + vs.Id;

            var moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, IntPtr.Zero) == 0 && moniker[0] != null)
            {

                moniker[0].GetDisplayName(bindCtx, null, out string dn);

                if (dn.StartsWith("!VisualStudio.DTE.") && dn.EndsWith(dteSuffix))
                {
                    object dbg, lps;
                    runningObjectTable.GetObject(moniker[0], out object dte);

                    // The COM object can be busy, we retry few times, hoping that it won't be busy next time.
                    for (var i = 0; i < 10; i++)
                    {
                        try
                        {
                            dbg = dte.GetType().InvokeMember("Debugger", BindingFlags.GetProperty, null, dte, null, CultureInfo.InvariantCulture)!;
                            lps = dbg.GetType().InvokeMember("LocalProcesses", BindingFlags.GetProperty, null, dbg, null, CultureInfo.InvariantCulture)!;
                            var lpn = (System.Collections.IEnumerator)lps.GetType().InvokeMember("GetEnumerator", BindingFlags.InvokeMethod, null, lps, null, CultureInfo.InvariantCulture)!;

                            while (lpn.MoveNext())
                            {
                                var pn = Convert.ToInt32(lpn.Current.GetType().InvokeMember("ProcessID", BindingFlags.GetProperty, null, lpn.Current, null, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

                                if (pn == pid)
                                {
                                    lpn.Current.GetType().InvokeMember("Attach", BindingFlags.InvokeMethod, null, lpn.Current, null, CultureInfo.InvariantCulture);
                                    return true;
                                }
                            }
                        }
                        // Catch the exception if it is COMException coming directly, or coming from methodInvocation, otherwise just let it be.
                        catch (Exception ex) when (ex is COMException || (ex is TargetInvocationException tie && tie.InnerException is COMException))
                        {
                            Trace($"ComException: Retrying in 250ms.\n{ex}");
                            Thread.Sleep(250);
                        }
                    }
                    Marshal.ReleaseComObject(moniker[0]);

                    break;
                }

                Marshal.ReleaseComObject(moniker[0]);
            }
            return false;
        }
        finally
        {
            if (enumMoniker != null)
            {
                try
                {
                    Marshal.ReleaseComObject(enumMoniker);
                }
                catch { }
            }
            if (runningObjectTable != null)
            {
                try
                {
                    Marshal.ReleaseComObject(runningObjectTable);
                }
                catch { }
            }
            if (bindCtx != null)
            {
                try
                {
                    Marshal.ReleaseComObject(bindCtx);
                }
                catch { }
            }
        }
    }

    private static Process? GetVsFromPid(Process process)
    {
        var parent = process;
        while (!IsVsOrNull(parent))
        {
            parent = GetParentProcess(parent);
        }

        return parent;
    }

    private static bool IsVsOrNull([NotNullWhen(false)] Process? process)
    {
        if (process == null)
        {
            Trace("Parent process is null..");
            return true;
        }

        var isVs = process.ProcessName.Equals("devenv", StringComparison.InvariantCultureIgnoreCase);
        if (isVs)
        {
            Trace($"Process {process.ProcessName} ({process.Id}) is VS.");
        }
        else
        {
            Trace($"Process {process.ProcessName} ({process.Id}) is not VS.");
        }

        return isVs;
    }

    private static bool IsCorrectParent(Process currentProcess, Process parent)
    {
        try
        {
            // Parent needs to start before the child, otherwise it might be a different process
            // that is just reusing the same PID.
            if (parent.StartTime <= currentProcess.StartTime)
            {
                return true;
            }

            Trace($"Process {parent.ProcessName} ({parent.Id}) is not a valid parent because it started after the current process.");
        }
        catch
        {
            // Access denied or process exited while we were holding the Process object.
        }

        return false;
    }

    private static Process? GetParentProcess(Process process)
    {
        int id = GetParentProcessId(process);
        if (id != -1)
        {
            try
            {
                var parent = Process.GetProcessById(id);
                if (IsCorrectParent(process, parent))
                    return parent;
            }
            catch
            {
                // throws when parent no longer runs
            }
        }

        return null;

        static int GetParentProcessId(Process process)
        {
            try
            {
                var handle = process.Handle;
                var res = NtQueryInformationProcess(handle, 0, out var pbi, Marshal.SizeOf<PROCESS_BASIC_INFORMATION>(), out int size);

                var p = res != 0 ? -1 : pbi.InheritedFromUniqueProcessId.ToInt32();

                return p;
            }
            catch
            {
                return -1;
            }
        }
    }

    private static void Trace(string message, [CallerMemberName] string? methodName = null)
    {
        System.Diagnostics.Trace.WriteLine($"[AttachVS]{methodName}: {message}");
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PROCESS_BASIC_INFORMATION
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
        out PROCESS_BASIC_INFORMATION processInformation,
        int processInformationLength,
        out int returnLength);

    [DllImport("ole32.dll")]
    private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);
}
