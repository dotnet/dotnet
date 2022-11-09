// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.VisualStudio.Common.Telemetry.PowerShell
{
    public class SolutionCloseEvent : TelemetryEvent
    {
        public SolutionCloseEvent(
            bool firstTimeLoadedFromPmc,
            bool firstTimeLoadedFromPmui,
            bool initPs1LoadedFromPmcFirst,
            bool initPs1LoadPmc,
            bool initPs1LoadPmui,
            bool loadedFromPmc,
            bool loadedFromPmui,
            bool nuGetCommandUsed,
            int pmcExecuteCommandCount,
            int pmcWindowLoadCount,
            int pmuiExecuteCommandCount,
            bool solutionLoaded) : base(NuGetPowerShellUsageCollector.SolutionClose)
        {
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.FirstTimeLoadedFromPmc] = firstTimeLoadedFromPmc;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.FirstTimeLoadedFromPmui] = firstTimeLoadedFromPmui;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.InitPs1LoadedFromPmcFirst] = initPs1LoadedFromPmcFirst;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.InitPs1LoadPmc] = initPs1LoadPmc;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.InitPs1LoadPmui] = initPs1LoadPmui;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.LoadedFromPmc] = loadedFromPmc;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.LoadedFromPmui] = loadedFromPmui;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.NuGetCommandUsed] = nuGetCommandUsed;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.PmcExecuteCommandCount] = pmcExecuteCommandCount;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.PmcWindowLoadCount] = pmcWindowLoadCount;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.PmuiExecuteCommandCount] = pmuiExecuteCommandCount;
            base[NuGetPowerShellUsageCollector.PowerShellHost + NuGetPowerShellUsageCollector.SolutionLoaded] = solutionLoaded;
        }
    }
}
