// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal class NetClientHangDumper : IHangDumper
{
    public void Dump(int processId, string outputDirectory, DumpTypeOption type)
    {
        var process = Process.GetProcessById(processId);
        var processTree = process.GetProcessTree();

        if (EqtTrace.IsVerboseEnabled)
        {
            if (processTree.Count > 1)
            {
                EqtTrace.Verbose("NetClientHangDumper.Dump: Dumping this process tree (from bottom):");
                ConsoleOutput.Instance.Information(false, "Blame: Dumping this process tree (from bottom):");

                foreach (var p in processTree.OrderBy(t => t.Level))
                {
                    EqtTrace.Verbose($"NetClientHangDumper.Dump: {(p.Level != 0 ? " + " : " > ")}{new string('-', p.Level)} {p.Process!.Id} - {p.Process.ProcessName}");
                    ConsoleOutput.Instance.Information(false, $"Blame: {(p.Level != 0 ? " + " : " > ")}{new string('-', p.Level)} {p.Process.Id} - {p.Process.ProcessName}");
                }
            }
            else
            {
                EqtTrace.Verbose($"NetClientHangDumper.Dump: Dumping {process.Id} - {process.ProcessName}.");
                ConsoleOutput.Instance.Information(false, $"Blame: Dumping {process.Id} - {process.ProcessName}");
            }
        }

        var bottomUpTree = processTree.OrderByDescending(t => t.Level).Select(t => t.Process);

        // Do not suspend processes with NetClient dumper it stops the diagnostic thread running in
        // them and hang dump request will get stuck forever, because the process is not co-operating.
        // Instead we start one task per dump asynchronously, and hope that the parent process will start dumping
        // before the child process is done dumping. This way if the parent is waiting for the children to exit,
        // we will be dumping it before it observes the child exiting and we get a more accurate results. If we did not
        // do this, then parent that is awaiting child might exit before we get to dumping it.
        var tasks = new List<Task>();
        var timeout = new CancellationTokenSource();
        timeout.CancelAfter(TimeSpan.FromMinutes(5));
        foreach (var p in bottomUpTree)
        {
            TPDebug.Assert(p != null);
            tasks.Add(Task.Run(
                () =>
                {
                    try
                    {
                        var outputFile = Path.Combine(outputDirectory, $"{p.ProcessName}_{p.Id}_{DateTime.Now:yyyyMMddTHHmmss}_hangdump.dmp");
                        EqtTrace.Verbose($"NetClientHangDumper.CollectDump: Selected dump type {type}. Dumping {process.Id} - {process.ProcessName} in {outputFile}. ");

                        var client = new DiagnosticsClient(p.Id);

                        // Connecting the dump generation logging to verbose output to avoid changing the interfaces again -> EqtTrace.IsVerboseEnabled
                        // before we test this on some big repo.
                        client.WriteDump(type == DumpTypeOption.Full ? DumpType.Full : DumpType.Normal, outputFile, logDumpGeneration: false);
                    }
                    catch (Exception ex)
                    {
                        EqtTrace.Error($"NetClientHangDumper.Dump: Error dumping process {p.Id} - {p.ProcessName}: {ex}.");
                    }
                }, timeout.Token));
        }

        try
        {
            Task.WhenAll(tasks).GetAwaiter().GetResult();
        }
        catch (TaskCanceledException)
        {
            EqtTrace.Error($"NetClientHangDumper.Dump: Hang dump timed out.");
        }

        foreach (var p in bottomUpTree)
        {
            TPDebug.Assert(p != null);
            try
            {
                EqtTrace.Verbose($"NetClientHangDumper.Dump: Killing process {p.Id} - {p.ProcessName}.");
                p.Kill();
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"NetClientHangDumper.Dump: Error killing process {p.Id} - {p.ProcessName}: {ex}.");
            }
        }
    }
}
