// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using NuGet.Common;
using NuGet.VisualStudio;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    public static class PowerShellHostService
    {
        private static IRunspaceManager RunspaceManager;

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Can't dispose an object if we want to return it.")]
        internal static IHost CreateHost(string name, IRestoreEvents restoreEvents, IEnvironmentVariableReader environmentVariableReader, bool isAsync)
        {
            RunspaceManager ??= new RunspaceManager(environmentVariableReader);

            IHost host;
            if (isAsync)
            {
                host = new AsyncPowerShellHost(name, restoreEvents, RunspaceManager, environmentVariableReader);
            }
            else
            {
                host = new SyncPowerShellHost(name, restoreEvents, RunspaceManager, environmentVariableReader);
            }

            return host;
        }
    }
}
