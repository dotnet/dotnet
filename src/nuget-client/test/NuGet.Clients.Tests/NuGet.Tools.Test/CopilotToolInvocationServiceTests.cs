// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Copilot;
using NuGet.PackageManagement.VisualStudio;
using NuGetVSExtension;
using Xunit;

namespace NuGet.Tools.Test
{
    public class CopilotToolInvocationServiceTests
    {
        private const string ValidNuGetToolName = McpServerConstants.NuGetSolverToolName;

        private static readonly IReadOnlyCollection<string> AcceptableGroups = McpServerConstants.NuGetMcpServerNames;

        private static readonly ServiceMoniker TestServiceMoniker = new("test.moniker");

        private static CopilotMcpFunctionDescriptor CreateMcpDescriptor(
            string serverNameOfFunction = ValidNuGetToolName,
            string group = McpServerConstants.NuGetMcpServerName,
            string? name = null)
        {
            return new CopilotMcpFunctionDescriptor(
                providerMoniker: TestServiceMoniker,
                serverNameOfFunction: serverNameOfFunction,
                configurationPath: string.Empty,
                name: name ?? $"mcp_{group}_{serverNameOfFunction}",
                displayName: "test display name",
                description: "desc",
                confirmation: CopilotConfirmationRequirement.NotRequired)
            {
                Group = group,
            };
        }

        [Theory]
        // Group matching
        [InlineData(McpServerConstants.NuGetMcpServerName, ValidNuGetToolName, null, true)]   // Visual Studio group
        [InlineData(McpServerConstants.ComMicrosoftNuGetMcpServerName, ValidNuGetToolName, null, true)]   // MCP registry group
        [InlineData("nuget", ValidNuGetToolName, null, true)]   // Group match is case-insensitive
        [InlineData("someone.else/nuget", ValidNuGetToolName, null, false)]  // Unknown group
        [InlineData(null, ValidNuGetToolName, null, false)]  // Null group
        // ServerNameOfFunction matching
        [InlineData(McpServerConstants.NuGetMcpServerName, "some_other_tool", null, false)]  // Wrong ServerNameOfFunction
        [InlineData(McpServerConstants.NuGetMcpServerName, "FIX_VULNERABLE_PACKAGES", null, true)]  // ServerNameOfFunction match is case-insensitive
        // Regression guard: previous impl matched on CopilotFunctionDescriptor.Name; now we match on ServerNameOfFunction.
        [InlineData(McpServerConstants.NuGetMcpServerName, "some_other_tool", ValidNuGetToolName, false)]  // Name matches but ServerNameOfFunction does not
        public void IsToolAvailable_SingleMcpDescriptor_ReturnsExpected(
            string? group,
            string serverNameOfFunction,
            string? name,
            bool expected)
        {
            var functions = new List<CopilotFunctionDescriptor>
            {
                CreateMcpDescriptor(serverNameOfFunction: serverNameOfFunction, group: group!, name: name),
            };

            Assert.Equal(expected, CopilotToolInvocationService.IsToolAvailable(functions, ValidNuGetToolName, AcceptableGroups));
        }

        [Fact]
        public void IsToolAvailable_NullOrEmptyFunctions_ReturnsFalse()
        {
            Assert.False(CopilotToolInvocationService.IsToolAvailable(functions: null, ValidNuGetToolName, AcceptableGroups));
            Assert.False(CopilotToolInvocationService.IsToolAvailable(new List<CopilotFunctionDescriptor>(), ValidNuGetToolName, AcceptableGroups));
        }

        [Fact]
        public void IsToolAvailable_NonMcpDescriptorWithMatchingName_ReturnsFalse()
        {
            // A non-MCP descriptor (e.g. a local Copilot function) must not match even if its Name
            // collides with the composed fully-qualified MCP tool name.
            var localFn = new CopilotLocalFunctionDescriptor(
                name: $"mcp_{McpServerConstants.NuGetMcpServerName}_{ValidNuGetToolName}",
                description: "desc",
                confirmation: CopilotConfirmationRequirement.NotRequired)
            {
                Group = McpServerConstants.NuGetMcpServerName,
            };

            var functions = new List<CopilotFunctionDescriptor> { localFn };

            Assert.False(CopilotToolInvocationService.IsToolAvailable(functions, ValidNuGetToolName, AcceptableGroups));
        }

        [Fact]
        public void IsToolAvailable_NoiseBeforeMatch_ReturnsTrue()
        {
            var functions = new List<CopilotFunctionDescriptor>
            {
                CreateMcpDescriptor(serverNameOfFunction: "unrelated_tool"),
                CreateMcpDescriptor(group: McpServerConstants.ComMicrosoftNuGetMcpServerName),
            };

            Assert.True(CopilotToolInvocationService.IsToolAvailable(functions, ValidNuGetToolName, AcceptableGroups));
        }
    }
}
