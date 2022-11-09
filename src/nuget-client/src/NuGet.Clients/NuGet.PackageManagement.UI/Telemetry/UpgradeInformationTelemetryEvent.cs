// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.PackageManagement.Telemetry
{
    internal class UpgradeInformationTelemetryEvent : TelemetryEvent
    {
        internal UpgradeInformationTelemetryEvent()
            : base("UpgradeInformation")
        {
        }

        internal void SetResult(IEnumerable<string> projectIds, NuGetOperationStatus status, int packageCount)
        {
            base["ProjectIds"] = string.Join(",", projectIds);
            base["Status"] = status;
            base["PackageCount"] = packageCount;
        }
    }
}
