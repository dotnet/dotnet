// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio
{
    public static class McpServerConstants
    {
        // This value must match the server name registered in mcp.json. Keep both in sync.
        public const string NuGetMcpServerName = "nuget";

        // Tool names below must match their respective tool names defined in the NuGet MCP Server.
        public const string NuGetSolverToolName = "get-nuget-solver";

        // Fully qualified tool names are in the format of "{serverName}_{toolName}".
        public const string NuGetSolverFullyQualifiedToolName = $"{NuGetMcpServerName}_{NuGetSolverToolName}";
    }
}
