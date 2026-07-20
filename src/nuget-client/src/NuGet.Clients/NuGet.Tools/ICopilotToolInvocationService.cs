// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Copilot;

namespace NuGetVSExtension
{
    /// <summary>
    /// Handles shared pre-flight checks for invoking NuGet MCP tools via Copilot.
    /// Verifies Copilot readiness, acquires required services, and validates tool availability.
    /// </summary>
    internal interface ICopilotToolInvocationService
    {
        /// <summary>
        /// Attempts to create a ready-to-use Copilot tool session.
        /// On success, the returned session owns all underlying resources and must be disposed by the caller.
        /// </summary>
        /// <param name="clientId">The Copilot client identity for telemetry attribution.</param>
        /// <param name="correlationId">The correlation ID from the caller's <see cref="CopilotRequest"/> (used for function discovery).</param>
        /// <param name="mcpToolName">The MCP tool name to look for. Matched against <see cref="CopilotMcpFunctionDescriptor.ServerNameOfFunction"/>.</param>
        /// <param name="acceptableMcpServerNames">The set of MCP server names under which the tool is considered acceptable. Matched against <see cref="CopilotFunctionDescriptor.Group"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A result indicating success (with session) or failure (with error type).</returns>
        Task<CopilotToolSessionResult> TryCreateToolSessionAsync(
            CopilotClientId clientId,
            CopilotCorrelationId correlationId,
            string mcpToolName,
            IReadOnlyCollection<string> acceptableMcpServerNames,
            CancellationToken cancellationToken);
    }
}
