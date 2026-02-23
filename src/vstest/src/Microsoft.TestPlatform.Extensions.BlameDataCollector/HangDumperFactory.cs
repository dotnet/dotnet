// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal class HangDumperFactory : IHangDumperFactory
{
    public Action<string>? LogWarning { get; set; }

    public IHangDumper Create(string targetFramework)
    {
        ValidateArg.NotNull(targetFramework, nameof(targetFramework));

        EqtTrace.Info($"HangDumperFactory: Creating dumper for {RuntimeInformation.OSDescription} with target framework {targetFramework}.");
        var procdumpOverride = Environment.GetEnvironmentVariable("VSTEST_DUMP_FORCEPROCDUMP")?.Trim();
        EqtTrace.Verbose($"HangDumperFactory: Overrides for dumpers: VSTEST_DUMP_FORCEPROCDUMP={procdumpOverride}");

        var tfm = Framework.FromString(targetFramework);

        if (tfm == null)
        {
            EqtTrace.Error($"HangDumperFactory: Could not parse target framework {targetFramework}, to a supported framework version.");
            throw new NotSupportedException($"Could not parse target framework {targetFramework}, to a supported framework version.");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // On some system the interop dumper will thrown AccessViolationException, add an option to force procdump.
            var forceUsingProcdump = !procdumpOverride.IsNullOrWhiteSpace() && procdumpOverride != "0";
            if (forceUsingProcdump)
            {
                EqtTrace.Info($"HangDumperFactory: This is Windows on  Forcing the use of ProcDumpHangDumper that uses ProcDump utility, via VSTEST_DUMP_FORCEPROCDUMP={procdumpOverride}.");
                return new ProcDumpDumper();
            }

            if (tfm.FrameworkName == ".NETCoreApp")
            {
                EqtTrace.Info($"HangDumperFactory: This is Windows on {tfm.FrameworkName} {tfm.Version}, returning the standard NETClient library dumper.");
                return new NetClientHangDumper();
            }

            EqtTrace.Info($"HangDumperFactory: This is Windows, returning the default WindowsHangDumper that P/Invokes MiniDumpWriteDump.");
            return new WindowsHangDumper(new ProcessHelper(), LogWarning);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            EqtTrace.Info($"HangDumperFactory: This is Linux returning the standard NETClient library dumper.");
            return new NetClientHangDumper();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            EqtTrace.Info($"HangDumperFactory: This is OSX returning the standard NETClient library dumper.");
            return new NetClientHangDumper();
        }

        throw new PlatformNotSupportedException($"Unsupported operating system: {RuntimeInformation.OSDescription}");
    }
}
