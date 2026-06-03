// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

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
    [Export(typeof(IFixVulnerabilitiesService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class FixVulnerablitiesService : IFixVulnerabilitiesService
    {
        private const string AgentModeResponderServiceMoniker = "Microsoft.VisualStudio.Copilot.AgentModeResponder";
        private const string ServiceName = "Microsoft.VisualStudio.Copilot.SolutionContextProvider";

        private static readonly ServiceRpcDescriptor ProviderDescriptor = CopilotDescriptors.CreateContextProviderDescriptor(ServiceName);
        private static readonly CopilotContextDescriptor ContextDescriptor = new CopilotContextDescriptor(
                    "SolutionFile",
                    "solution file context",
                    CopilotDefaultTypes.StringName);

        [Import(typeof(ICopilotToolInvocationService))]
        public ICopilotToolInvocationService? ToolInvocationService { get; set; }

        [Import(typeof(IVsSolutionManager))]
        public IVsSolutionManager? SolutionManager { get; set; }

        [Import(typeof(VisualStudioActivityLogger))]
        public ILogger? ActivityLogger { get; set; }

        public async Task LaunchFixVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            CopilotClientId clientId = new("Microsoft.VisualStudio.NuGet.VulnerabilitiesInfoBar");

            // Build the request first so we have a stable CorrelationId for function discovery
            CopilotRequest request = new(Resources.Prompt_FixNuGetPackageVulnerabilities)
            {
                Guidance = "Use absolute paths when invoking MCP Tools.",
                DirectedResponders = [new(AgentModeResponderServiceMoniker, new(CopilotDescriptors.CurrentResponderVersion))]
            };

            Assumes.Present(ToolInvocationService);

            CopilotToolSessionResult result = await ToolInvocationService.TryCreateToolSessionAsync(
                clientId,
                request.CorrelationId,
                McpServerConstants.NuGetSolverFullyQualifiedToolName,
                cancellationToken);

            if (!result.IsSuccess)
            {
                HandleSessionError(result.Error);
                return;
            }

            await using CopilotToolSession session = result.Session!;

            // Attach solution context and available functions to the request
            string solutionPathContext = $"The current solution file path is: {GetSolutionPath()}.";
            CopilotContext context = new CopilotContext(ProviderDescriptor.Moniker, ContextDescriptor, request.CorrelationId, solutionPathContext);
            CopilotRequest requestWithFunctionsAndContext = request.WithFunctions(session.Functions).WithContext(context);

            try
            {
                _ = await session.Thread.Session.SendRequestAsync(requestWithFunctionsAndContext, cancellationToken);
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.None);
            }
            catch (UnauthorizedAccessException ex)
            {
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotAccessDenied);
                ActivityLogger?.LogError(ex.Message);
                ShowWarningMessage(Resources.Error_CopilotAccessDenied);
            }
        }

        private static void HandleSessionError(CopilotToolSessionError error)
        {
            (FixVulnerabilitiesWithCopilotErrorType telemetryError, string message) = error switch
            {
                CopilotToolSessionError.CopilotNotReady => (FixVulnerabilitiesWithCopilotErrorType.CopilotNotReady, Resources.Error_CopilotNotReady),
                CopilotToolSessionError.ServiceBrokerNotAvailable => (FixVulnerabilitiesWithCopilotErrorType.ServiceBrokerNotAvailable, Resources.Error_ServiceBrokerNotAvailable),
                CopilotToolSessionError.CopilotServiceNotAvailable => (FixVulnerabilitiesWithCopilotErrorType.CopilotServiceNotAvailable, Resources.Error_CopilotServiceNotAvailable),
                CopilotToolSessionError.McpToolServiceNotAvailable => (FixVulnerabilitiesWithCopilotErrorType.McpToolServiceNotAvailable, Resources.Error_McpToolServiceNotAvailable),
                CopilotToolSessionError.ToolNotAvailable => (FixVulnerabilitiesWithCopilotErrorType.NuGetSolverNotAvailable, Resources.Error_NuGetSolverNotAvailable),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };

            SendTelemetryEvent(telemetryError);
            ShowWarningMessage(message);
        }

        private static void ShowWarningMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            MessageHelper.ShowWarningMessage(message, Resources.Title_FixVulnerabilitiesWithCopilot);
        }

        private static void SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType errorType)
        {
            var evt = NavigatedTelemetryEvent.CreateWithVulnerabilityInfoBarFixWithCopilot(errorType);
            TelemetryActivity.EmitTelemetryEvent(evt);
        }

        private string GetSolutionPath() => SolutionManager?.SolutionDirectory ?? string.Empty;
    }
}
