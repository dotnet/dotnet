// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Contract for the job monitor's main execution loop.
    /// </summary>
    public interface IJobMonitorRunner
    {
        /// <summary>
        /// Runs the monitor loop. Returns 0 for success, 1 for failure.
        /// </summary>
        Task<int> RunAsync(CancellationToken cancellationToken);
    }
}
