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
using Microsoft.VisualStudio.Copilot.Sdk;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using NuGet.PackageManagement.VisualStudio;

namespace NuGetVSExtension
{
#pragma warning disable VSCOPILOT_BACKEND // Experimental SDK harness contracts.
    [CopilotSdkAgent(
        AgentName,
        DisplayName = "NuGet",
        Description = "Helps resolve NuGet package management issues.",
        IncludeBuiltInContributions = true,
        Usage = AgentUsage.Programmatic)]
    internal sealed class NuGetSdkAgent : CopilotSdkAgentHooks, ICopilotSdkAgent
    {
        internal const string AgentName = "nuget";

        [Import(typeof(SVsFullAccessServiceBroker), AllowDefault = true)]
        public IServiceBroker? ServiceBroker { get; set; }

        public Task<string> GetSystemPromptAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                "Use the available NuGet MCP tools to address the user's NuGet package management request. " +
                "Use absolute paths when invoking MCP tools.");
        }

        public async Task<IReadOnlyList<CopilotFunctionDescriptor>> GetToolsAsync(CancellationToken cancellationToken)
        {
            IServiceBroker serviceBroker = ServiceBroker
                ?? throw new InvalidOperationException("The Visual Studio service broker is unavailable.");

            ICopilotFunctionProvider? functionProvider = await serviceBroker.GetProxyAsync<ICopilotFunctionProvider>(
                CopilotDescriptors.McpToolService,
                cancellationToken);

            using (functionProvider as IDisposable)
            {
                if (functionProvider is null)
                {
                    throw new InvalidOperationException("The Copilot MCP tool service is unavailable.");
                }

                IReadOnlyList<CopilotFunctionDescriptor>? functions = await functionProvider.GetFunctionsAsync(
                    CopilotCorrelationId.New(),
                    cancellationToken);

                return SelectInBoxNuGetMcpTools(functions);
            }
        }

        internal static IReadOnlyList<CopilotMcpFunctionDescriptor> SelectInBoxNuGetMcpTools(
            IReadOnlyList<CopilotFunctionDescriptor>? functions)
        {
            if (functions is null)
            {
                return [];
            }

            return functions
                .OfType<CopilotMcpFunctionDescriptor>()
                .Where(function => string.Equals(
                    function.Group,
                    McpServerConstants.NuGetMcpServerName,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
#pragma warning restore VSCOPILOT_BACKEND
}
