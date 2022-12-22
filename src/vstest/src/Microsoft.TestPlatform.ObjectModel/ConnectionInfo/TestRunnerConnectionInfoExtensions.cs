// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Extension methods for <see cref="TestRunnerConnectionInfo"/>.
/// </summary>
public static class TestRunnerConnectionInfoExtensions
{
    /// <summary>
    /// Creates a default command line options string from <see cref="TestRunnerConnectionInfo"/>.
    /// </summary>
    /// <param name="connectionInfo">Connection info for the test host.</param>
    /// <returns>Command line option string.</returns>
    public static string ToCommandLineOptions(this TestRunnerConnectionInfo connectionInfo)
    {
        var options = $"--port {connectionInfo.Port} --endpoint {connectionInfo.ConnectionInfo.Endpoint} --role {(connectionInfo.ConnectionInfo.Role == ConnectionRole.Client ? "client" : "host")} --parentprocessid {connectionInfo.RunnerProcessId}";
        if (!StringUtils.IsNullOrEmpty(connectionInfo.LogFile))
        {
            options += " --diag " + connectionInfo.LogFile;
            options += " --tracelevel " + connectionInfo.TraceLevel;
        }

        return options;
    }
}
