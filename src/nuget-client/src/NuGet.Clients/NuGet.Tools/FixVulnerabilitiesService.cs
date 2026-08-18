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
    internal class FixVulnerabilitiesService : IFixVulnerabilitiesService
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

        public async Task LaunchFixVulnerabilitiesAsync(
            FixVulnerabilitiesSource source,
            CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            NavigationOrigin navigationOrigin = source.NavigationOrigin;
            CopilotClientId clientId = new(source.CopilotClientId);

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
                McpServerConstants.NuGetSolverToolName,
                McpServerConstants.NuGetMcpServerNames,
                cancellationToken);

            if (!result.IsSuccess)
            {
                CopilotToolInvocationService.HandleSessionError(
                    result.Error,
                    Resources.Error_NuGetSolverNotAvailable,
                    Resources.Title_FixVulnerabilitiesWithCopilot,
                    NavigatedTelemetryEvent.CreateWithFixVulnerabilitiesWithCopilot(navigationOrigin, MapSessionErrorToTelemetry(result.Error)));
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
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.None, navigationOrigin);
            }
            catch (UnauthorizedAccessException ex)
            {
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotAccessDenied, navigationOrigin);
                ActivityLogger?.LogError(ex.Message);
                MessageHelper.ShowWarningMessage(Resources.Error_CopilotAccessDenied, Resources.Title_FixVulnerabilitiesWithCopilot);
            }
        }

        private static void SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType errorType, NavigationOrigin navigationOrigin)
        {
            TelemetryActivity.EmitTelemetryEvent(
                NavigatedTelemetryEvent.CreateWithFixVulnerabilitiesWithCopilot(navigationOrigin, errorType));
        }

        private static FixVulnerabilitiesWithCopilotErrorType MapSessionErrorToTelemetry(CopilotToolSessionError error)
        {
            return error switch
            {
                CopilotToolSessionError.None => FixVulnerabilitiesWithCopilotErrorType.None,
                CopilotToolSessionError.CopilotNotReady => FixVulnerabilitiesWithCopilotErrorType.CopilotNotReady,
                CopilotToolSessionError.ServiceBrokerNotAvailable => FixVulnerabilitiesWithCopilotErrorType.ServiceBrokerNotAvailable,
                CopilotToolSessionError.CopilotServiceNotAvailable => FixVulnerabilitiesWithCopilotErrorType.CopilotServiceNotAvailable,
                CopilotToolSessionError.McpToolServiceNotAvailable => FixVulnerabilitiesWithCopilotErrorType.McpToolServiceNotAvailable,
                CopilotToolSessionError.CopilotAccessDenied => FixVulnerabilitiesWithCopilotErrorType.CopilotAccessDenied,
                CopilotToolSessionError.ToolNotAvailable => FixVulnerabilitiesWithCopilotErrorType.NuGetSolverNotAvailable,
                CopilotToolSessionError.McpServerInfoServiceNotAvailable => FixVulnerabilitiesWithCopilotErrorType.McpServerInfoServiceNotAvailable,
                CopilotToolSessionError.McpServerNotActive => FixVulnerabilitiesWithCopilotErrorType.McpServerNotActive,
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }

        private string GetSolutionPath() => SolutionManager?.SolutionDirectory ?? string.Empty;
    }
}
