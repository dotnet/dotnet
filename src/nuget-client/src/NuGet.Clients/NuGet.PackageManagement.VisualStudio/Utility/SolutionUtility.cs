// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Threading;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.VisualStudio
{
    internal static class SolutionUtility
    {
        internal static async ValueTask<NuGetProject?> GetNuGetProjectAsync(
            AsyncLazy<IVsSolutionManager> lazySolutionManager,
            string projectId,
            CancellationToken cancellationToken)
        {
            IVsSolutionManager? solutionManager = await lazySolutionManager.GetValueAsync(cancellationToken);
            Assumes.NotNull(solutionManager);

            NuGetProject project = (await solutionManager.GetNuGetProjectsAsync())
                .FirstOrDefault(p =>
                    projectId.Equals(
                        p.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId),
                        StringComparison.OrdinalIgnoreCase));

            return project;
        }
    }
}
