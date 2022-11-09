// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal class WindowsHangDumper : IHangDumper
{
    private readonly Action<string> _logWarning;
    private readonly IProcessHelper _processHelper;

    public WindowsHangDumper(IProcessHelper processHelper, Action<string>? logWarning)
    {
        _logWarning = logWarning ?? (_ => { });
        _processHelper = processHelper;
    }

    private static Action<object?, string?> OutputReceivedCallback => (process, data) =>
        // useful for visibility when debugging this tool
        // Console.ForegroundColor = ConsoleColor.Cyan;
        // Console.WriteLine(data);
        // Console.ForegroundColor = ConsoleColor.White;
        // Log all standard output message of procdump in diag files.
        // Otherwise they end up coming on console in pipleine.
        EqtTrace.Info($"ProcDumpDumper.OutputReceivedCallback: Output received from procdump process: {data ?? "<null>"}");

    public void Dump(int processId, string outputDirectory, DumpTypeOption type)
    {
        var process = Process.GetProcessById(processId);
        var processTree = process.GetProcessTree().Where(p => p.Process?.ProcessName is not null and not "conhost" and not "WerFault").ToList();

        if (processTree.Count > 1)
        {
            var tree = processTree.OrderBy(t => t.Level);
            EqtTrace.Verbose("WindowsHangDumper.Dump: Dumping this process tree (from bottom):");
            foreach (var p in tree)
            {
                EqtTrace.Verbose($"WindowsHangDumper.Dump: {new string(' ', p.Level)}{(p.Level != 0 ? " +" : " >-")} {p.Process!.Id} - {p.Process.ProcessName}");
            }

            // logging warning separately to avoid interleving the messages in the log which make this tree unreadable
            _logWarning(Resources.Resources.DumpingTree);
            foreach (var p in tree)
            {
                _logWarning($"{new string(' ', p.Level)}{(p.Level != 0 ? "+-" : ">")} {p.Process!.Id} - {p.Process.ProcessName}");
            }
        }
        else
        {
            EqtTrace.Verbose($"NetClientHangDumper.Dump: Dumping {process.Id} - {process.ProcessName}.");
            var message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.Dumping, process.Id, process.ProcessName);
            _logWarning(message);
        }

        var bottomUpTree = processTree.OrderByDescending(t => t.Level).Select(t => t.Process);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            foreach (var p in bottomUpTree)
            {
                TPDebug.Assert(p != null);
                try
                {
                    p.Suspend();
                }
                catch (Exception ex)
                {
                    EqtTrace.Error($"WindowsHangDumper.Dump: Error suspending process {p.Id} - {p.ProcessName}: {ex}.");
                }
            }
        }

        foreach (var p in bottomUpTree)
        {
            TPDebug.Assert(p != null);

            try
            {
                var outputFile = Path.Combine(outputDirectory, $"{p.ProcessName}_{p.Id}_{DateTime.Now:yyyyMMddTHHmmss}_hangdump.dmp");
                CollectDump(_processHelper, p, outputFile, type);
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"WindowsHangDumper.Dump: Error dumping process {p.Id} - {p.ProcessName}: {ex}.");
            }

            try
            {
                EqtTrace.Verbose($"WindowsHangDumper.Dump: Killing process {p.Id} - {p.ProcessName}.");
                p.Kill();
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"WindowsHangDumper.Dump: Error killing process {p.Id} - {p.ProcessName}: {ex}.");
            }
        }
    }

    internal static void CollectDump(IProcessHelper processHelper, Process process, string outputFile, DumpTypeOption type)
    {
        if (process.HasExited)
        {
            EqtTrace.Verbose($"WindowsHangDumper.CollectDump: {process.Id} - {process.ProcessName} already exited, skipping.");
            return;
        }

        EqtTrace.Verbose($"WindowsHangDumper.CollectDump: Selected dump type {type}. Dumping {process.Id} - {process.ProcessName} in {outputFile}. ");

        var currentProcessArchitecture = processHelper.GetCurrentProcessArchitecture();
        var targetProcessArchitecture = processHelper.GetProcessArchitecture(process.Id);

        if (currentProcessArchitecture == targetProcessArchitecture)
        {
            EqtTrace.Verbose($"WindowsHangDumper.CollectDump: Both processes are {currentProcessArchitecture}, using PInvoke dumper directly.");
            MiniDumpWriteDump.CollectDumpUsingMiniDumpWriteDump(process, outputFile, FromDumpType(type));
        }
        else
        {
            var dumpMinitoolName = targetProcessArchitecture switch
            {
                PlatformArchitecture.X86 => "DumpMinitool.x86.exe",
                PlatformArchitecture.X64 => "DumpMinitool.exe",
                PlatformArchitecture.ARM64 => "DumpMinitool.arm64.exe",
                _ => string.Empty
            };

            if (dumpMinitoolName == string.Empty)
            {
                EqtTrace.Verbose($"WindowsHangDumper.CollectDump: The target process architecture is {targetProcessArchitecture}, we don't have a DumpMinitool for that, falling back to using PInvoke directly.");
                MiniDumpWriteDump.CollectDumpUsingMiniDumpWriteDump(process, outputFile, FromDumpType(type));
            }

            var args = $"--file \"{outputFile}\" --processId {process.Id} --dumpType {type}";
            var dumpMinitoolPath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)!, "dump", dumpMinitoolName);
            EqtTrace.Verbose($"WindowsHangDumper.CollectDump: The target process architecture is {targetProcessArchitecture}, dumping it via {dumpMinitoolName}.");

            if (!File.Exists(dumpMinitoolPath))
            {
                throw new FileNotFoundException("Could not find DumpMinitool", dumpMinitoolPath);
            }

            EqtTrace.Info($"ProcDumpDumper.CollectDump: Running DumpMinitool: '{dumpMinitoolPath} {args}'.");
            var dumpMiniTool = (Process)new ProcessHelper().LaunchProcess(
                dumpMinitoolPath,
                args,
                Path.GetDirectoryName(outputFile),
                null,
                null,
                null,
                OutputReceivedCallback);
            dumpMiniTool.WaitForExit();
            EqtTrace.Info($"ProcDumpDumper.CollectDump: {dumpMinitoolName} exited with exitcode: '{dumpMiniTool.ExitCode}'.");
        }

        EqtTrace.Verbose($"WindowsHangDumper.CollectDump: Finished dumping {process.Id} - {process.ProcessName} in {outputFile}. ");
    }

    private static MiniDumpTypeOption FromDumpType(DumpTypeOption type)
    {
        return type switch
        {
            DumpTypeOption.Full => MiniDumpTypeOption.Full,
            DumpTypeOption.WithHeap => MiniDumpTypeOption.WithHeap,
            DumpTypeOption.Mini => MiniDumpTypeOption.Mini,
            _ => throw new NotSupportedException($"Dump type {type} is not supported."),
        };
    }
}
