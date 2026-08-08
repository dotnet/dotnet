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
    [Export(typeof(IPackageSourceMappingService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class PackageSourceMappingService : IPackageSourceMappingService
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

        public async Task LaunchReviewPackageSourceMappingAsync(CancellationToken cancellationToken)
        {
            CopilotClientId clientId = new("Microsoft.VisualStudio.NuGet.PackageSourceMapper");

            CopilotRequest request = new(Resources.Prompt_ReviewPackageSourceMapping)
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
                    Resources.Error_PackageSourceMappingToolNotAvailable,
                    Resources.Title_PackageSourceMappingWithCopilot,
                    NavigatedTelemetryEvent.CreateWithReviewPackageSourceMappingCommand(result.Error));
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
                    NavigatedTelemetryEvent.CreateWithReviewPackageSourceMappingCommand(CopilotToolSessionError.None));
            }
            catch (UnauthorizedAccessException ex)
            {
                TelemetryActivity.EmitTelemetryEvent(
                    NavigatedTelemetryEvent.CreateWithReviewPackageSourceMappingCommand(CopilotToolSessionError.CopilotAccessDenied));
                ActivityLogger?.LogError(ex.Message);
                MessageHelper.ShowWarningMessage(Resources.Error_CopilotAccessDenied, Resources.Title_PackageSourceMappingWithCopilot);
            }
        }

        private string GetSolutionPath() => SolutionManager?.SolutionDirectory ?? string.Empty;
    }
}
