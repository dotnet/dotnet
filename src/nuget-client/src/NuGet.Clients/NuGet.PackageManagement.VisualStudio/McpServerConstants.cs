// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class McpServerConstants
    {
        // Display name of the NuGet "fix vulnerabilities" tool exposed by the NuGet MCP server.
        // Must match the tool name defined in the NuGet MCP Server.
        public const string NuGetSolverToolName = "fix_vulnerable_packages";

        // This value must match the server name registered in mcp.json. Keep both in sync.
        public const string NuGetMcpServerName = "NuGet";

        // MCP server name when the NuGet MCP server is installed via the Anthropic or GitHub MCP Registry.
        public const string ComMicrosoftNuGetMcpServerName = "com.microsoft/nuget";

        // MCP server names under which a NuGet MCP tool may legitimately be exposed.
        public static readonly IReadOnlyCollection<string> NuGetMcpServerNames = new[]
        {
            NuGetMcpServerName,
            ComMicrosoftNuGetMcpServerName,
        };
    }
}
