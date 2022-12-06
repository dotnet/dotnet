// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using NuGet.VisualStudio;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    public static class PowerShellHostService
    {
        private static readonly IRunspaceManager _runspaceManager = new RunspaceManager();

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Can't dispose an object if we want to return it.")]
        public static IHost CreateHost(string name, IRestoreEvents restoreEvents, bool isAsync)
        {
            IHost host;
            if (isAsync)
            {
                host = new AsyncPowerShellHost(name, restoreEvents, _runspaceManager);
            }
            else
            {
                host = new SyncPowerShellHost(name, restoreEvents, _runspaceManager);
            }

            return host;
        }
    }
}
