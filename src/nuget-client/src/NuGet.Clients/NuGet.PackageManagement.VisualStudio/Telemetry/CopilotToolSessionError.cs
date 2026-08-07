// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.Telemetry
{
    /// <summary>
    /// Represents the error states when attempting to create or use a Copilot tool session.
    /// </summary>
    public enum CopilotToolSessionError
    {
        None,
        CopilotNotReady,
        ServiceBrokerNotAvailable,
        CopilotServiceNotAvailable,
        McpToolServiceNotAvailable,
        CopilotAccessDenied,
        ToolNotAvailable,
        McpServerInfoServiceNotAvailable,
        McpServerNotActive,
    }
}
