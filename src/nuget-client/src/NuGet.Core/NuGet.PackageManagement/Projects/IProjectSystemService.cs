// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Represents generic service API as provided by a project system.
    /// </summary>
    public interface IProjectSystemService
    {
        /// <summary>
        /// Saves the underlying project.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>Completion task</returns>
        Task SaveProjectAsync(CancellationToken token);
    }
}
