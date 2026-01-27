// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.Telemetry
{
    public enum FixVulnerabilitiesWithCopilotErrorType
    {
        None,
        CopilotNotReady,
        ServiceBrokerNotAvailable,
        CopilotServiceNotAvailable,
        McpToolServiceNotAvailable,
        CopilotAccessDenied
    }
}
