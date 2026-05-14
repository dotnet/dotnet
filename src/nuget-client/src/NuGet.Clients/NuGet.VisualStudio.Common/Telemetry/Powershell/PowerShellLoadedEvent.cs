// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.VisualStudio.Common.Telemetry.PowerShell
{
    public class PowerShellLoadedEvent : TelemetryEvent
    {
        public PowerShellLoadedEvent(bool isPmc, string psVersion)
            : base(NuGetPowerShellUsageCollector.PowerShellLoaded)
        {
            base[NuGetPowerShellUsageCollector.Trigger] = isPmc ? NuGetPowerShellUsageCollector.Pmc : NuGetPowerShellUsageCollector.Pmui;
            base[NuGetPowerShellUsageCollector.PSVersion] = psVersion;
        }
    }
}
