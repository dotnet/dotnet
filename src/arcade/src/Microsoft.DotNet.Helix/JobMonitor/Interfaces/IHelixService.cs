// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Abstracts Helix API interactions needed by the job monitor.
    /// </summary>
    public interface IHelixService
    {
        /// <summary>
        /// Returns Helix jobs associated with the current build/stage.
        /// Implementations should query Helix using the given <paramref name="source"/>
        /// filter (which scopes the query to the repo/branch/PR the build is for, mirroring
        /// what the Helix job submitter records on each submission) and then narrow the
        /// result to jobs stamped with <paramref name="buildId"/>.
        /// </summary>
        Task<IReadOnlyList<HelixJobInfo>> GetJobsForBuildAsync(
            string source,
            string buildId,
            CancellationToken cancellationToken);

        /// <summary>
        /// Downloads test result files for a completed Helix job's work items
        /// and returns metadata about each work item's results.
        /// Work items without recognizable test result files may be omitted from the result.
        /// Individual file download failures should not prevent other result files from being downloaded.
        /// </summary>
        Task<IReadOnlyList<WorkItemTestResults>> DownloadTestResultsAsync(
            string jobName,
            IReadOnlyCollection<string> workItemNames,
            string workingDirectory, CancellationToken cancellationToken);

        /// <summary>
        /// Lists work items for the specified Helix job.
        /// </summary>
        Task<IReadOnlyCollection<WorkItemSummary>> ListWorkItemsAsync(
            string jobName,
            CancellationToken cancellationToken);

        /// <summary>
        /// Requests cancellation for the specified Helix job.
        /// </summary>
        Task CancelJobAsync(
            string jobName,
            CancellationToken cancellationToken);

        /// <summary>
        /// Resubmits the specified failed work items from a completed Helix job as a new job.
        /// The new job copies correlation payloads and queue from the original, but only includes
        /// the specified work items. Returns the new job's info, or null if resubmission is not possible.
        /// The new job must preserve BuildId and StageName properties so it is discoverable by GetJobsAsync.
        /// </summary>
        Task<HelixJobInfo> ResubmitWorkItemsAsync(
            HelixJobInfo originalJob,
            IReadOnlyCollection<WorkItemSummary> failedWorkItems,
            CancellationToken cancellationToken);
    }
}
