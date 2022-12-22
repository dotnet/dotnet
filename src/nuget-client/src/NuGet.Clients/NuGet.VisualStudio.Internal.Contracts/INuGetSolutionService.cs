// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Internal.Contracts
{
    /// <summary>
    /// Contains methods to install packages into a project within the current solution.
    /// </summary>
    public interface INuGetSolutionService
    {
        /// <summary>
        /// Runs a solution restore operation.
        /// Returns only when the restore has completed.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RestoreSolutionAsync(CancellationToken cancellationToken);
    }
}
