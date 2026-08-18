// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Copilot;
using Microsoft.VisualStudio.Copilot.Internal.Mcp;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.VisualStudio;

namespace NuGetVSExtension
{
    [Export(typeof(ICopilotToolInvocationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class CopilotToolInvocationService : ICopilotToolInvocationService
    {
        private const string AuthStatusDetermined = "c936efcc-6baa-4ad3-9c2b-7ba750acf18f";
        private static readonly Guid CopilotReadyUIContext = new(AuthStatusDetermined);

        [Import(typeof(SVsFullAccessServiceBroker), AllowDefault = true)]
        public IServiceBroker? ServiceBroker { get; set; }

        public async Task<CopilotToolSessionResult> TryCreateToolSessionAsync(
            CopilotClientId clientId,
            CopilotCorrelationId correlationId,
            string mcpToolName,
            IReadOnlyCollection<string> acceptableMcpServerNames,
            CancellationToken cancellationToken)
        {
            // 1. Check if the user is signed-in to GitHub Copilot
            UIContext copilotReady = UIContext.FromUIContextGuid(CopilotReadyUIContext);
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            if (!copilotReady.IsActive)
            {
                return CopilotToolSessionResult.Failure(CopilotToolSessionError.CopilotNotReady);
            }

            // 2. Verify service broker is available
            if (ServiceBroker == null)
            {
                return CopilotToolSessionResult.Failure(CopilotToolSessionError.ServiceBrokerNotAvailable);
            }

            // 3. Verify the required MCP server is registered and active
            IMcpServerInfoService? mcpServerInfoService = await ServiceBroker.GetProxyAsync<IMcpServerInfoService>(McpServiceIdentities.ServerInfoService.Descriptor, cancellationToken);
            using (mcpServerInfoService as IDisposable)
            {
                if (mcpServerInfoService is null)
                {
                    return CopilotToolSessionResult.Failure(CopilotToolSessionError.McpServerInfoServiceNotAvailable);
                }

                // The NuGet MCP server may be registered under different names depending on how it
                // was installed (in-VS vs. Anthropic/GitHub MCP registry). It is considered available
                // if any of the acceptable names reports an Active or Suspended state.
                if (!await IsServerAvailableAsync(mcpServerInfoService, acceptableMcpServerNames, cancellationToken))
                {
                    return CopilotToolSessionResult.Failure(CopilotToolSessionError.McpServerNotActive);
                }
            }

            // 4. Acquire Copilot service, ownership transfers to CopilotToolSession on success
#pragma warning disable ISB001 // Dispose objects before losing scope - ownership is transferred to CopilotToolSession on success
            ICopilotService? copilotService = await ServiceBroker.GetProxyAsync<ICopilotService>(CopilotDescriptors.CopilotService, cancellationToken);
#pragma warning restore ISB001

            bool ownershipTransferred = false;
            try
            {
                if (copilotService is null)
                {
                    return CopilotToolSessionResult.Failure(CopilotToolSessionError.CopilotServiceNotAvailable);
                }

                // 5. Acquire MCP tool function provider and get available functions
                ICopilotFunctionProvider? cfp = await ServiceBroker.GetProxyAsync<ICopilotFunctionProvider>(CopilotDescriptors.McpToolService, cancellationToken);
                using (cfp as IDisposable)
                {
                    if (cfp is null)
                    {
                        return CopilotToolSessionResult.Failure(CopilotToolSessionError.McpToolServiceNotAvailable);
                    }

                    // 6. Verify the required tool is available. We match on ServerNameOfFunction + Group
                    //    (the same logical NuGet MCP tool can be exposed under different Group values
                    //    depending on how it was installed - in-VS vs. Anthropic/GitHub MCP registry).
                    IReadOnlyList<CopilotFunctionDescriptor>? functions = await cfp.GetFunctionsAsync(correlationId, cancellationToken);
                    if (!IsToolAvailable(functions, mcpToolName, acceptableMcpServerNames))
                    {
                        return CopilotToolSessionResult.Failure(CopilotToolSessionError.ToolNotAvailable);
                    }

                    // 7. Start Copilot thread
                    CopilotThreadOptions options = new(clientId);
                    CopilotThread thread = await copilotService.StartThreadAsync(options, cancellationToken);

                    CopilotToolSession session = new(
                        thread,
                        functions,
                        copilotServiceDisposable: copilotService as IDisposable);
                    ownershipTransferred = true;
                    return CopilotToolSessionResult.Success(session);
                }
            }
            finally
            {
                if (!ownershipTransferred)
                {
                    (copilotService as IDisposable)?.Dispose();
                }
            }
        }

        /// <summary>
        /// Handles a session creation error by showing a user-facing warning message and emitting a telemetry event.
        /// </summary>
        /// <param name="error">The error returned from <see cref="TryCreateToolSessionAsync"/>.</param>
        /// <param name="toolNotAvailableMessage">The user-facing message to display when <see cref="CopilotToolSessionError.ToolNotAvailable"/>.</param>
        /// <param name="warningTitle">The title for the warning message box.</param>
        /// <param name="telemetryEvent">Optional telemetry event to emit. If null, no telemetry is sent.</param>
        internal static void HandleSessionError(
            CopilotToolSessionError error,
            string toolNotAvailableMessage,
            string warningTitle,
            TelemetryEvent? telemetryEvent)
        {
            string message = error switch
            {
                CopilotToolSessionError.CopilotNotReady => Resources.Error_CopilotNotReady,
                CopilotToolSessionError.ServiceBrokerNotAvailable => Resources.Error_ServiceBrokerNotAvailable,
                CopilotToolSessionError.CopilotServiceNotAvailable => Resources.Error_CopilotServiceNotAvailable,
                CopilotToolSessionError.McpToolServiceNotAvailable => Resources.Error_McpToolServiceNotAvailable,
                CopilotToolSessionError.McpServerInfoServiceNotAvailable => Resources.Error_McpServerInfoServiceNotAvailable,
                CopilotToolSessionError.McpServerNotActive => Resources.Error_McpServerNotActive,
                CopilotToolSessionError.ToolNotAvailable => toolNotAvailableMessage,
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };

            if (telemetryEvent is not null)
            {
                TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
            }

            MessageHelper.ShowWarningMessage(message, warningTitle);
        }

        internal static bool IsToolAvailable(
            IReadOnlyList<CopilotFunctionDescriptor>? functions,
            string mcpToolName,
            IReadOnlyCollection<string> acceptableMcpServerNames)
        {
            return functions?
                .OfType<CopilotMcpFunctionDescriptor>()
                .Any(f => string.Equals(f.ServerNameOfFunction, mcpToolName, StringComparison.OrdinalIgnoreCase)
                       && f.Group is not null
                       && acceptableMcpServerNames.Contains(f.Group, StringComparer.OrdinalIgnoreCase)) ?? false;
        }

        internal static async Task<bool> IsServerAvailableAsync(
            IMcpServerInfoService mcpServerInfoService,
            IReadOnlyCollection<string> acceptableMcpServerNames,
            CancellationToken cancellationToken)
        {
            foreach (string serverName in acceptableMcpServerNames)
            {
                McpServerState? state = await mcpServerInfoService.GetServerStateAsync(serverName, cancellationToken);
                if (state is McpServerState.Active or McpServerState.Suspended)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
