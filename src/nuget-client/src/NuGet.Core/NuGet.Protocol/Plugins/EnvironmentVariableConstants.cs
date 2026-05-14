// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    internal static class EnvironmentVariableConstants
    {
        internal const string EnableLog = "NUGET_PLUGIN_ENABLE_LOG";
        internal const string LogDirectoryPath = "NUGET_PLUGIN_LOG_DIRECTORY_PATH";
        internal const string HandshakeTimeout = "NUGET_PLUGIN_HANDSHAKE_TIMEOUT_IN_SECONDS";
        internal const string IdleTimeout = "NUGET_PLUGIN_IDLE_TIMEOUT_IN_SECONDS";
        internal const string PluginPaths = "NUGET_PLUGIN_PATHS";
        internal const string RequestTimeout = "NUGET_PLUGIN_REQUEST_TIMEOUT_IN_SECONDS";
        internal const string DesktopPluginPaths = "NUGET_NETFX_PLUGIN_PATHS";
        internal const string CorePluginPaths = "NUGET_NETCORE_PLUGIN_PATHS";
    }
}
