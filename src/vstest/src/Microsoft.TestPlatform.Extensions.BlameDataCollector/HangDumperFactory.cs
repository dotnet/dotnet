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
        var netdumpOverride = Environment.GetEnvironmentVariable("VSTEST_DUMP_FORCENETDUMP")?.Trim();
        EqtTrace.Verbose($"HangDumperFactory: Overrides for dumpers: VSTEST_DUMP_FORCEPROCDUMP={procdumpOverride};VSTEST_DUMP_FORCENETDUMP={netdumpOverride}");

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

            // On some system the interop dumper will thrown AccessViolationException, add an option to force procdump.
            var forceUsingNetdump = !netdumpOverride.IsNullOrWhiteSpace() && netdumpOverride != "0";
            if (forceUsingNetdump)
            {
                var isLessThan50 = tfm.FrameworkName == ".NETCoreApp" && Version.Parse(tfm.Version) < Version.Parse("5.0.0.0");
                if (!isLessThan50)
                {
                    EqtTrace.Info($"HangDumperFactory: This is Windows on {tfm.FrameworkName} {tfm.Version}, VSTEST_DUMP_FORCENETDUMP={netdumpOverride} is active, forcing use of .NetClientHangDumper");
                    return new NetClientHangDumper();
                }
                else
                {
                    EqtTrace.Info($"HangDumperFactory: This is Windows on {tfm.FrameworkName} {tfm.Version}, VSTEST_DUMP_FORCENETDUMP={netdumpOverride} is active, but only applies to .NET 5.0 and newer. Falling back to default hang dumper.");
                }
            }

            EqtTrace.Info($"HangDumperFactory: This is Windows, returning the default WindowsHangDumper that P/Invokes MiniDumpWriteDump.");
            return new WindowsHangDumper(new ProcessHelper(), LogWarning);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var isLessThan31 = tfm.FrameworkName == ".NETCoreApp" && Version.Parse(tfm.Version) < Version.Parse("3.1.0.0");
            if (isLessThan31)
            {
                EqtTrace.Info($"HangDumperFactory: This is Linux on netcoreapp2.1, returning SigtrapDumper.");

                return new SigtrapDumper();
            }

            EqtTrace.Info($"HangDumperFactory: This is Linux net6.0 or newer, returning the standard NETClient library dumper.");
            return new NetClientHangDumper();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var isLessThan50 = tfm.FrameworkName == ".NETCoreApp" && Version.Parse(tfm.Version) < Version.Parse("5.0.0.0");
            if (isLessThan50)
            {
                EqtTrace.Info($"HangDumperFactory: This is OSX on {targetFramework}, This combination of OS and framework is not supported.");

                throw new PlatformNotSupportedException($"Unsupported target framework {targetFramework} on OS {RuntimeInformation.OSDescription}");
            }

            EqtTrace.Info($"HangDumperFactory: This is OSX on net5.0 or newer, returning the standard NETClient library dumper.");
            return new NetClientHangDumper();
        }

        throw new PlatformNotSupportedException($"Unsupported operating system: {RuntimeInformation.OSDescription}");
    }
}
