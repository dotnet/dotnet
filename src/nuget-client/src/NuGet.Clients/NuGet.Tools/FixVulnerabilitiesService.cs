// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

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
    [Export(typeof(IFixVulnerabilitiesService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class FixVulnerabilitiesService : IFixVulnerabilitiesService
    {
        private const string AgentModeResponderServiceMoniker = "Microsoft.VisualStudio.Copilot.AgentModeResponder";

        [Import(typeof(ICopilotToolInvocationService))]
        public ICopilotToolInvocationService ToolInvocationService { get; set; } = null!;

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

            CopilotRequest requestWithFunctions = request.WithFunctions(session.Functions);
            CopilotUserMessage harnessRequest = new()
            {
                DisplayPrompt = Resources.Prompt_FixNuGetPackageVulnerabilities,
                Prompt = Resources.Prompt_FixNuGetPackageVulnerabilities,
                Agent = NuGetSdkAgent.AgentName,
            };

            try
            {
                await session.SendRequestAsync(requestWithFunctions, harnessRequest, cancellationToken);
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.None, navigationOrigin);
            }
            catch (UnauthorizedAccessException ex)
            {
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotAccessDenied, navigationOrigin);
                ActivityLogger?.LogError(ex.Message);
                MessageHelper.ShowWarningMessage(Resources.Error_CopilotAccessDenied, Resources.Title_FixVulnerabilitiesWithCopilot);
            }
            catch (CopilotRequestException ex)
            {
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotRequestFailed, navigationOrigin);
                ActivityLogger?.LogError(ex.ToString());
                MessageHelper.ShowWarningMessage(Resources.Error_CopilotRequestFailed, Resources.Title_FixVulnerabilitiesWithCopilot);
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
                CopilotToolSessionError.CopilotRequestFailed => FixVulnerabilitiesWithCopilotErrorType.CopilotRequestFailed,
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }
    }
}
