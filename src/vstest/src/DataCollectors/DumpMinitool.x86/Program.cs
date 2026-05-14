// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using Microsoft.TestPlatform.Extensions.BlameDataCollector;

namespace DumpMinitool;

internal class Program
{
    static int Main(string[] args)
    {
        DebuggerBreakpoint.WaitForDebugger("VSTEST_DUMPTOOL_DEBUG");
        Console.WriteLine($"Dump minitool: Started with arguments {string.Join(" ", args)}");
        if (args?.Length != 6)
        {
            Console.WriteLine($"There were {args?.Length ?? 0} arguments. Provide exactly 6 arguments: --file <fullyResolvedPath> --processId <processId> --dumpType <dumpType>");
            return 2;
        }

        var outputFile = args[1];
        var processId = int.Parse(args[3], CultureInfo.InvariantCulture);
        var dumpType = (MiniDumpTypeOption)Enum.Parse(typeof(MiniDumpTypeOption), args[5]);

        Console.WriteLine($"Output file: '{outputFile}'");
        Console.WriteLine($"Process id: {processId}");
        Console.WriteLine($"Dump type: {dumpType}");

        // Dump the process!
        try
        {
            MiniDumpWriteDump.CollectDumpUsingMiniDumpWriteDump(Process.GetProcessById(processId), outputFile, dumpType);
            Console.WriteLine("Dumped process.");
            return 0;
        }
        catch (Exception err)
        {
            Console.WriteLine($"Error dumping process {err}");

            return 1;
        }
    }
}

internal static class DebuggerBreakpoint
{
    internal static void WaitForDebugger(string environmentVariable)
    {
        if (string.IsNullOrWhiteSpace(environmentVariable))
        {
            throw new ArgumentException($"'{nameof(environmentVariable)}' cannot be null or whitespace.", nameof(environmentVariable));
        }

        var debugEnabled = Environment.GetEnvironmentVariable(environmentVariable);
        if (!string.IsNullOrEmpty(debugEnabled) && debugEnabled.Equals("1", StringComparison.Ordinal))
        {
            Console.WriteLine("Waiting for debugger attach...");

            var currentProcess = Process.GetCurrentProcess();
            Console.WriteLine("Process Id: {0}, Name: {1}", currentProcess.Id, currentProcess.ProcessName);

            while (!Debugger.IsAttached)
            {
                Thread.Sleep(1000);
            }

            Debugger.Break();
        }
    }
}
