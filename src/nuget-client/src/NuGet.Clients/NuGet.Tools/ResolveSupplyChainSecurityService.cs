// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
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
        private const string ServiceName = "Microsoft.VisualStudio.Copilot.SolutionContextProvider";

        private static readonly ServiceRpcDescriptor ProviderDescriptor = CopilotDescriptors.CreateContextProviderDescriptor(ServiceName);
        private static readonly CopilotContextDescriptor ContextDescriptor = new CopilotContextDescriptor(
                    "SolutionFile",
                    "solution file context",
                    CopilotDefaultTypes.StringName);

        [Import(typeof(ICopilotToolInvocationService))]
        public ICopilotToolInvocationService ToolInvocationService { get; set; } = null!;

        [Import(typeof(IVsSolutionManager), AllowDefault = true)]
        public IVsSolutionManager? SolutionManager { get; set; }

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

            string solutionPathContext = $"The current solution file path is: {GetSolutionPath()}.";
            CopilotContext context = new CopilotContext(ProviderDescriptor.Moniker, ContextDescriptor, request.CorrelationId, solutionPathContext);
            CopilotRequest requestWithFunctionsAndContext = request.WithFunctions(session.Functions).WithContext(context);

            try
            {
                _ = await session.Thread.Session.SendRequestAsync(requestWithFunctionsAndContext, cancellationToken);
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
        }

        private string GetSolutionPath() => SolutionManager?.SolutionDirectory ?? string.Empty;
    }
}
