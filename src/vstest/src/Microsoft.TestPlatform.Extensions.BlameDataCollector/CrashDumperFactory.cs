// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

using NuGet.Frameworks;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal class CrashDumperFactory : ICrashDumperFactory
{
    public ICrashDumper Create(string targetFramework)
    {
        ValidateArg.NotNull(targetFramework, nameof(targetFramework));
        EqtTrace.Info($"CrashDumperFactory: Creating dumper for {RuntimeInformation.OSDescription} with target framework {targetFramework}.");

        var tfm = NuGetFramework.Parse(targetFramework);

        if (tfm == null || tfm.IsUnsupported)
        {
            EqtTrace.Error($"CrashDumperFactory: Could not parse target framework {targetFramework}, to a supported framework version.");
            throw new NotSupportedException($"Could not parse target framework {targetFramework}, to a supported framework version.");
        }

        var isNet50OrNewer = tfm.Framework == ".NETCoreApp" && tfm.Version >= Version.Parse("5.0.0.0");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (!isNet50OrNewer)
            {
                EqtTrace.Info($"CrashDumperFactory: This is Windows on {targetFramework} which is not net5.0 or newer, returning ProcDumpCrashDumper that uses ProcDump utility.");
                return new ProcDumpDumper();
            }

            // On net5.0 we don't have the capability to crash dump on exit, which is useful in rare cases
            // like when user exits the testhost process with a random exit code, adding this evn variable
            // to force using procdump. This relies on netclient dumper actualy doing all it's work in blame collector
            // where it sets all the environment variables, so in effect we will have two crash dumpers active at the same time.
            // This proven to be working okay while net5.0 could not create dumps from Task.Run, and I was using this same technique
            // to get dump of testhost. This needs PROCDUMP_PATH set to directory with procdump.exe, or having it in path.
            var procdumpOverride = Environment.GetEnvironmentVariable("VSTEST_DUMP_FORCEPROCDUMP")?.Trim();
            var forceUsingProcdump = !procdumpOverride.IsNullOrWhiteSpace() && procdumpOverride != "0";
            if (forceUsingProcdump)
            {
                EqtTrace.Info($"CrashDumperFactory: This is Windows on {targetFramework}. Forcing the use of ProcDumpCrashDumper that uses ProcDump utility, via VSTEST_DUMP_FORCEPROCDUMP={procdumpOverride}.");
                return new ProcDumpDumper();
            }

            EqtTrace.Info($"CrashDumperFactory: This is Windows on {targetFramework}, returning the .NETClient dumper which uses env variables to collect crashdumps of testhost and any child process that is .NET 5 or newer.");
            return new NetClientCrashDumper(new FileHelper());
        }

        if (isNet50OrNewer)
        {
            EqtTrace.Info($"CrashDumperFactory: This is {RuntimeInformation.OSDescription} on {targetFramework} .NETClient dumper which uses env variables to collect crashdumps of testhost and any child process that is .NET 5 or newer.");
            return new NetClientCrashDumper(new FileHelper());
        }

        throw new PlatformNotSupportedException($"Unsupported operating system: {RuntimeInformation.OSDescription}, and framework: {targetFramework}.");
    }
}
