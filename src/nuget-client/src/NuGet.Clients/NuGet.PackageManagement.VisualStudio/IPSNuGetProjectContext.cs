// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Management.Automation;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.VisualStudio
{
    public interface IPSNuGetProjectContext : INuGetProjectContext
    {
        bool IsExecuting { get; }

        PSCmdlet CurrentPSCmdlet { get; }

        void ExecutePSScript(string scriptPath, bool throwOnFailure);
    }
}
