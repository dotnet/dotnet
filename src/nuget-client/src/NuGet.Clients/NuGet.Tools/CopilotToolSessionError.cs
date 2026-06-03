// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetVSExtension
{
    /// <summary>
    /// Represents the pre-flight error states when attempting to create a Copilot tool session.
    /// </summary>
    internal enum CopilotToolSessionError
    {
        None,
        CopilotNotReady,
        ServiceBrokerNotAvailable,
        CopilotServiceNotAvailable,
        McpToolServiceNotAvailable,
        ToolNotAvailable,
    }
}
