// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.VisualStudio;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal interface IRunspaceManager
    {
        Tuple<RunspaceDispatcher, NuGetPSHost> GetRunspace(IConsole console, string hostName);
    }
}
