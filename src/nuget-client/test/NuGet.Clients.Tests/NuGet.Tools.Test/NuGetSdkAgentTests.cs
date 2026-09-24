// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Copilot;
using NuGet.PackageManagement.VisualStudio;
using NuGetVSExtension;
using Xunit;

namespace NuGet.Tools.Test
{
    public class NuGetSdkAgentTests
    {
        private static readonly ServiceMoniker TestServiceMoniker = new("test.moniker");

        [Fact]
        public void SelectInBoxNuGetMcpTools_InBoxAndRegistryTools_ReturnsInBoxTools()
        {
            CopilotMcpFunctionDescriptor inBoxTool = CreateMcpDescriptor(
                McpServerConstants.NuGetMcpServerName,
                McpServerConstants.NuGetSolverToolName);
            CopilotMcpFunctionDescriptor registryTool = CreateMcpDescriptor(
                McpServerConstants.ComMicrosoftNuGetMcpServerName,
                McpServerConstants.NuGetSolverToolName);

            IReadOnlyList<CopilotMcpFunctionDescriptor> result = NuGetSdkAgent.SelectInBoxNuGetMcpTools(
                [registryTool, inBoxTool]);

            Assert.Collection(result, tool => Assert.Same(inBoxTool, tool));
        }

        [Fact]
        public void SelectInBoxNuGetMcpTools_OnlyRegistryTools_ReturnsEmpty()
        {
            CopilotMcpFunctionDescriptor registryTool = CreateMcpDescriptor(
                McpServerConstants.ComMicrosoftNuGetMcpServerName,
                McpServerConstants.NuGetSolverToolName);

            IReadOnlyList<CopilotMcpFunctionDescriptor> result = NuGetSdkAgent.SelectInBoxNuGetMcpTools(
                [registryTool]);

            Assert.Empty(result);
        }

        [Fact]
        public void SelectInBoxNuGetMcpTools_MultipleInBoxTools_ReturnsAllInBoxTools()
        {
            CopilotMcpFunctionDescriptor fixTool = CreateMcpDescriptor(
                McpServerConstants.NuGetMcpServerName,
                McpServerConstants.NuGetSolverToolName);
            CopilotMcpFunctionDescriptor reviewTool = CreateMcpDescriptor(
                McpServerConstants.NuGetMcpServerName,
                McpServerConstants.PackageSourceMappingToolName);

            IReadOnlyList<CopilotMcpFunctionDescriptor> result = NuGetSdkAgent.SelectInBoxNuGetMcpTools(
                [fixTool, reviewTool]);

            Assert.Equal([fixTool, reviewTool], result);
        }

        [Fact]
        public void SelectInBoxNuGetMcpTools_NullFunctions_ReturnsEmpty()
        {
            IReadOnlyList<CopilotMcpFunctionDescriptor> result = NuGetSdkAgent.SelectInBoxNuGetMcpTools(
                functions: null);

            Assert.Empty(result);
        }

        private static CopilotMcpFunctionDescriptor CreateMcpDescriptor(string group, string toolName)
        {
            return new CopilotMcpFunctionDescriptor(
                providerMoniker: TestServiceMoniker,
                serverNameOfFunction: toolName,
                configurationPath: string.Empty,
                name: $"mcp_{group}_{toolName}",
                displayName: toolName,
                description: "desc",
                confirmation: CopilotConfirmationRequirement.NotRequired)
            {
                Group = group,
            };
        }
    }
}
