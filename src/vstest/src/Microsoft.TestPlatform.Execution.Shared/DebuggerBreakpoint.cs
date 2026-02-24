// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if USE_EXTERN_ALIAS
extern alias Abstraction;
#endif

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#if USE_EXTERN_ALIAS
using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
#else
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
#endif
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Execution;

[System.Diagnostics.CodeAnalysis.SuppressMessage("ApiDesign", "RS0030:Do not used banned APIs", Justification = "StringUtils is not available for all TFMs of testhost")]
internal static class DebuggerBreakpoint
{
    internal static void AttachVisualStudioDebugger(string environmentVariable)
    {
        if (string.IsNullOrWhiteSpace(environmentVariable))
        {
            throw new ArgumentException($"'{nameof(environmentVariable)}' cannot be null or whitespace.", nameof(environmentVariable));
        }

        if (Debugger.IsAttached)
            return;

        var debugEnabled = Environment.GetEnvironmentVariable(environmentVariable);
        if (!string.IsNullOrEmpty(debugEnabled) && !debugEnabled.Equals("0", StringComparison.Ordinal))
        {
            int? vsPid = null;
            if (int.TryParse(debugEnabled, out int pid))
            {
                // The option is used to both enable and disable attaching (0 and 1)
                // and providing custom vs pid (any number higher than 1)
                vsPid = pid <= 1 ? null : (int?)pid;
            }

            if (vsPid == null)
            {
                ConsoleOutput.Instance.WriteLine("Attaching Visual Studio, either a parent or the one that was started first... To specify a VS instance to use, use the PID in the option, instead of 1.", OutputLevel.Information);
            }
            else
            {
                var processId =
#if NET6_0_OR_GREATER
                    Environment.ProcessId;
#else
                    Process.GetCurrentProcess().Id;
#endif

                ConsoleOutput.Instance.WriteLine($"Attaching Visual Studio with PID {vsPid} to the process '{Process.GetCurrentProcess().ProcessName}({processId})'...", OutputLevel.Information);
            }

            AttachVs(Process.GetCurrentProcess(), vsPid);

            Break();
        }
    }

    private static bool AttachVs(Process process, int? vsPid)
    {
        // The way we attach VS is not compatible with .NET Core 2.1 and .NET Core 3.1, but works in .NET Framework and .NET.
        // We could call the library code directly here for .NET, and .NET Framework, but then we would also need to package it
        // together with testhost. So instead we always run the executable, and pass path to it using env variable.

        const string env = "VSTEST_DEBUG_ATTACHVS_PATH";
        var vsAttachPath = Environment.GetEnvironmentVariable(env) ?? FindAttachVs();

        // Always set it so we propagate it to child processes even if it was not previously set.
        Environment.SetEnvironmentVariable(env, vsAttachPath);

        if (vsAttachPath == null)
        {
            throw new InvalidOperationException($"Cannot find AttachVS.exe tool.");
        }

        if (!File.Exists(vsAttachPath))
        {
            throw new InvalidOperationException($"Cannot start tool, path {vsAttachPath} does not exist.");
        }
        var attachVsProcess = Process.Start(vsAttachPath, $"{process.Id} {vsPid}");
        attachVsProcess.WaitForExit();

        return attachVsProcess.ExitCode == 0;
    }

    private static string? FindAttachVs()
    {
        return FindOnPath("AttachVS.exe");
    }

    private static string? FindOnPath(string exeName)
    {
        // TODO: Skip when PATH is not defined.
        var paths = Environment.GetEnvironmentVariable("PATH")!.Split(';');
        foreach (var p in paths)
        {
            var path = Path.Combine(p, exeName);
            if (File.Exists(path))
            {
                return path;
            }
        }

        return null;
    }

    internal static void WaitForDebugger(string environmentVariable)
    {
        if (Debugger.IsAttached)
            return;

        if (string.IsNullOrWhiteSpace(environmentVariable))
        {
            throw new ArgumentException($"'{nameof(environmentVariable)}' cannot be null or whitespace.", nameof(environmentVariable));
        }

        var debugEnabled = Environment.GetEnvironmentVariable(environmentVariable);
        if (!string.IsNullOrEmpty(debugEnabled) && debugEnabled.Equals("1", StringComparison.Ordinal))
        {
            ConsoleOutput.Instance.WriteLine("Waiting for debugger attach...", OutputLevel.Information);

            var currentProcess = Process.GetCurrentProcess();
            ConsoleOutput.Instance.WriteLine(
                $"Process Id: {currentProcess.Id}, Name: {currentProcess.ProcessName}",
                OutputLevel.Information);

            while (!Debugger.IsAttached)
            {
                Task.Delay(1000).GetAwaiter().GetResult();
            }

            Break();
        }
    }

    internal static void WaitForNativeDebugger(string environmentVariable)
    {
        if (string.IsNullOrWhiteSpace(environmentVariable))
        {
            throw new ArgumentException($"'{nameof(environmentVariable)}' cannot be null or whitespace.", nameof(environmentVariable));
        }

        // Check if native debugging is enabled and OS is windows.
        var nativeDebugEnabled = Environment.GetEnvironmentVariable(environmentVariable);

        if (!string.IsNullOrEmpty(nativeDebugEnabled) && nativeDebugEnabled.Equals("1", StringComparison.Ordinal)
                                                      && new PlatformEnvironment().OperatingSystem.Equals(PlatformOperatingSystem.Windows))
        {
            while (!IsDebuggerPresent())
            {
                Task.Delay(1000).Wait();
            }

            BreakNative();
        }
    }

    private static void Break()
    {
        if (ShouldNotBreak())
        {
            return;
        }

        Debugger.Break();
    }

    private static bool ShouldNotBreak()
    {
        return Environment.GetEnvironmentVariable("VSTEST_DEBUG_NOBP")?.Equals("1") ?? false;
    }

    private static void BreakNative()
    {
        if (ShouldNotBreak())
        {
            return;
        }

        DebugBreak();
    }

    // Native APIs for enabling native debugging.
    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsDebuggerPresent();

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    internal static extern void DebugBreak();
}
