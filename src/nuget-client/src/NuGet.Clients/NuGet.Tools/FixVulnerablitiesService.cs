// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Copilot;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.ServiceBroker;
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
        private const string AuthStatusDetermined = "c936efcc-6baa-4ad3-9c2b-7ba750acf18f";
        private const string ServiceName = "Microsoft.VisualStudio.Copilot.SolutionContextProvider";

        private static readonly Guid CopilotReadyUIContext = new(AuthStatusDetermined);
        private static readonly ServiceRpcDescriptor ProviderDescriptor = CopilotDescriptors.CreateContextProviderDescriptor(ServiceName);
        private static readonly CopilotContextDescriptor ContextDescriptor = new CopilotContextDescriptor(
                    "SolutionFile",
                    "solution file context",
                    CopilotDefaultTypes.StringName);

        [Import(typeof(SVsFullAccessServiceBroker))]
        public IServiceBroker? ServiceBroker { get; set; }

        [Import(typeof(IVsSolutionManager))]
        public IVsSolutionManager? SolutionManager { get; set; }

        [Import(typeof(VisualStudioActivityLogger))]
        public ILogger? ActivityLogger { get; set; }

        public async Task LaunchFixVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            UIContext copilotReady = UIContext.FromUIContextGuid(CopilotReadyUIContext);
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            if (!copilotReady.IsActive)
            {
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotNotReady);
                ShowWarningMessage(Resources.Error_CopilotNotReady);
                return;
            }

            if (ServiceBroker == null)
            {
                // Unlikely to occur and would indicate a problem with VS, but should still be handled.
                SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.ServiceBrokerNotAvailable);
                ShowWarningMessage(Resources.Error_ServiceBrokerNotAvailable);
                return;
            }

            ICopilotService? copilotService = await ServiceBroker.GetProxyAsync<ICopilotService>(CopilotDescriptors.CopilotService, cancellationToken);
            using (copilotService as IDisposable)
            {
                if (copilotService is null)
                {
                    SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotServiceNotAvailable);
                    ShowWarningMessage(Resources.Error_CopilotServiceNotAvailable);
                    return;
                }

                // Create an identifier that will be visible in the session's telemetry
                CopilotClientId clientId = new("Microsoft.VisualStudio.NuGet.VulnerabilitiesInfoBar");
                CopilotThreadOptions options = new(clientId);

                await using var thread = await copilotService.StartThreadAsync(options, cancellationToken);
                ICopilotFunctionProvider? cfp = await ServiceBroker.GetProxyAsync<ICopilotFunctionProvider>(CopilotDescriptors.McpToolService, cancellationToken);
                using (cfp as IDisposable)
                {
                    if (cfp is null)
                    {
                        SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.McpToolServiceNotAvailable);
                        ShowWarningMessage(Resources.Error_McpToolServiceNotAvailable);
                        return;
                    }

                    // Requests from this session will be visible in the Chat window
                    CopilotRequest request = new(Resources.Prompt_FixNuGetPackageVulnerabilities)
                    {
                        Intent = CopilotIntent.None,
                        Guidance = "Use absolute paths when invoking MCP Tools.",
                        DirectedResponders = [new(AgentModeResponderServiceMoniker, new(CopilotDescriptors.CurrentResponderVersion))]
                    };

                    // Prepare a request with the list of functions available to it and the solution path context.
                    string solutionPathContext = $"The current solution file path is: {GetSolutionPath()}.";
                    CopilotContext context = new CopilotContext(ProviderDescriptor.Moniker, ContextDescriptor, request.CorrelationId, solutionPathContext);
                    IReadOnlyList<CopilotFunctionDescriptor> functions = await cfp.GetFunctionsAsync(request.CorrelationId, cancellationToken);
                    CopilotRequest requestWithFunctionsAndContext = request.WithFunctions(functions).WithContext(context);

                    try
                    {
                        _ = await thread.Session.SendRequestAsync(requestWithFunctionsAndContext, cancellationToken);
                        SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.None);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        SendTelemetryEvent(FixVulnerabilitiesWithCopilotErrorType.CopilotAccessDenied);
                        ActivityLogger?.LogError(ex.Message);
                        ShowWarningMessage(Resources.Error_CopilotAccessDenied);
                    }
                }
            }
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
