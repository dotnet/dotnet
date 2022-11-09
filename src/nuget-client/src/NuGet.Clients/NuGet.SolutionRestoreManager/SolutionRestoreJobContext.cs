// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Holds data to be shared between restore job executions.
    /// </summary>
    internal sealed class SolutionRestoreJobContext
    {
        public INuGetProjectContext NuGetProjectContext { get; } = new EmptyNuGetProjectContext();
    }
}
