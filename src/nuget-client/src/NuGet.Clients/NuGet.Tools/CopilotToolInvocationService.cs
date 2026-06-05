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
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using NuGet.VisualStudio;

namespace NuGetVSExtension
{
    [Export(typeof(ICopilotToolInvocationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class CopilotToolInvocationService : ICopilotToolInvocationService
    {
        private const string AuthStatusDetermined = "c936efcc-6baa-4ad3-9c2b-7ba750acf18f";
        private static readonly Guid CopilotReadyUIContext = new(AuthStatusDetermined);

        [Import(typeof(SVsFullAccessServiceBroker))]
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

            // 3. Acquire Copilot service, ownership transfers to CopilotToolSession on success
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

                // 4. Acquire MCP tool function provider and get available functions
                ICopilotFunctionProvider? cfp = await ServiceBroker.GetProxyAsync<ICopilotFunctionProvider>(CopilotDescriptors.McpToolService, cancellationToken);
                using (cfp as IDisposable)
                {
                    if (cfp is null)
                    {
                        return CopilotToolSessionResult.Failure(CopilotToolSessionError.McpToolServiceNotAvailable);
                    }

                    // 5. Verify the required tool is available. We match on ServerNameOfFunction + Group
                    //    (the same logical NuGet MCP tool can be exposed under different Group values
                    //    depending on how it was installed â€” in-VS vs. Anthropic/GitHub MCP registry).
                    IReadOnlyList<CopilotFunctionDescriptor>? functions = await cfp.GetFunctionsAsync(correlationId, cancellationToken);
                    if (!IsToolAvailable(functions, mcpToolName, acceptableMcpServerNames))
                    {
                        return CopilotToolSessionResult.Failure(CopilotToolSessionError.ToolNotAvailable);
                    }

                    // 6. Start Copilot thread
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
    }
}
