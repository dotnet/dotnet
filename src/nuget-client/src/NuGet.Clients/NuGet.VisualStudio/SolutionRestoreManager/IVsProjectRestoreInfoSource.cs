// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a package restore service API for integration with a project system.
    /// Implemented by the project-system.
    /// </summary>
    [ComImport]
    [Guid("35AD5FF2-6AB7-48E9-BCC0-189042410FA6")]
    public interface IVsProjectRestoreInfoSource
    {
        /// <summary>
        /// Project Unique Name.
        /// Must be equivalent to the name provided in the <see cref="IVsSolutionRestoreService3.NominateProjectAsync(string, IVsProjectRestoreInfo2, CancellationToken)"/> or equivalent.
        /// </summary>
        /// <remarks>Never <see langword="null"/>.</remarks>
        string Name { get; }

        /// <summary>
        /// Whether the source needs to do some work that could lead to a nomination. <br/>
        /// Called frequently, so it should be very efficient.
        /// </summary>
        bool HasPendingNomination { get; }

        /// <summary>
        /// NuGet calls this method to wait on a potential nomination. <br/>
        /// If the project has no pending restore data, it will return a completed task. <br/>
        /// Otherwise, the task will be completed once the project nominates. <br/>
        /// The task will be cancelled, if the source decide it no longer needs to nominate (for example: the restore state has no change) <br/>
        /// The task will be failed, if the source runs into a problem, and it cannot get the correct data to nominate (for example: DT build failed) <br/>
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task WhenNominated(CancellationToken cancellationToken);
    }
}
