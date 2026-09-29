// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Copilot;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;

namespace NuGetVSExtension
{
    [Export(typeof(IResolveSupplyChainSecurityService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class ResolveSupplyChainSecurityService : IResolveSupplyChainSecurityService
    {
        private const string AgentModeResponderServiceMoniker = "Microsoft.VisualStudio.Copilot.AgentModeResponder";

        [Import(typeof(ICopilotToolInvocationService))]
        public ICopilotToolInvocationService ToolInvocationService { get; set; } = null!;

        [Import(typeof(VisualStudioActivityLogger), AllowDefault = true)]
        public ILogger? ActivityLogger { get; set; }

        public async Task LaunchResolveAsync(
            ResolveSupplyChainSecuritySource source,
            string prompt,
            CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(prompt))
            {
                throw new ArgumentException("The prompt cannot be null or empty.", nameof(prompt));
            }

            CopilotClientId clientId = new(source.CopilotClientId);

            CopilotRequest request = new(prompt)
            {
                Guidance = "Use absolute paths when invoking MCP Tools.",
                DirectedResponders = [new(AgentModeResponderServiceMoniker, new(CopilotDescriptors.CurrentResponderVersion))]
            };

            Assumes.Present(ToolInvocationService);

            CopilotToolSessionResult result = await ToolInvocationService.TryCreateToolSessionAsync(
                clientId,
                request.CorrelationId,
                McpServerConstants.PackageSourceMappingToolName,
                McpServerConstants.NuGetMcpServerNames,
                cancellationToken);

            if (!result.IsSuccess)
            {
                CopilotToolInvocationService.HandleSessionError(
                    result.Error,
                    Resources.Error_SupplyChainSecurityToolNotAvailable,
                    Resources.Title_ResolveSupplyChainSecurityWithCopilot,
                    NavigatedTelemetryEvent.CreateWithResolveSupplyChainSecurity(source.NavigationOrigin, result.Error));
                return;
            }

            await using CopilotToolSession session = result.Session!;

            CopilotRequest requestWithFunctions = request.WithFunctions(session.Functions);
            CopilotUserMessage harnessRequest = new()
            {
                DisplayPrompt = prompt,
                Prompt = prompt,
                Agent = NuGetSdkAgent.AgentName,
            };

            try
            {
                await session.SendRequestAsync(requestWithFunctions, harnessRequest, cancellationToken);
                TelemetryActivity.EmitTelemetryEvent(
                    NavigatedTelemetryEvent.CreateWithResolveSupplyChainSecurity(source.NavigationOrigin, CopilotToolSessionError.None));
            }
            catch (UnauthorizedAccessException ex)
            {
                TelemetryActivity.EmitTelemetryEvent(
                    NavigatedTelemetryEvent.CreateWithResolveSupplyChainSecurity(
                        source.NavigationOrigin,
                        CopilotToolSessionError.CopilotAccessDenied));
                ActivityLogger?.LogError(ex.Message);
                MessageHelper.ShowWarningMessage(Resources.Error_CopilotAccessDenied, Resources.Title_ResolveSupplyChainSecurityWithCopilot);
            }
            catch (CopilotRequestException ex)
            {
                TelemetryActivity.EmitTelemetryEvent(
                    NavigatedTelemetryEvent.CreateWithResolveSupplyChainSecurity(
                        source.NavigationOrigin,
                        CopilotToolSessionError.CopilotRequestFailed));
                ActivityLogger?.LogError(ex.ToString());
                MessageHelper.ShowWarningMessage(Resources.Error_CopilotRequestFailed, Resources.Title_ResolveSupplyChainSecurityWithCopilot);
            }
        }
    }
}
