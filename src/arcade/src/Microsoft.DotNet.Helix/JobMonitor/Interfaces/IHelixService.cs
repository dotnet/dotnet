// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;

namespace Microsoft.DotNet.Helix.JobMonitor;

/// <summary>
/// Abstracts Helix API interactions needed by the job monitor.
/// </summary>
public interface IHelixService
{
    /// <summary>
    /// Returns Helix jobs associated with the current build/stage.
    /// Implementations should query Helix using both the given <paramref name="source"/>
    /// and the job property <c>BuildId=<paramref name="buildId"/></c>. The build property
    /// must be filtered by the service rather than by retrieving every job for a long-lived
    /// branch source and narrowing the result locally.
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
    Task<WorkItemTestResults> DownloadTestResultsAsync(
        string jobName,
        string workItemName,
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
    /// Resubmits the specified failed or unfinished work items from a Helix job as a new job.
    /// The new job copies correlation payloads and queue from the original, but only includes
    /// the specified work items. Returns the new job's info, or null if resubmission is not
    /// possible (e.g. the original queue no longer exists).
    /// The new job must preserve BuildId and StageName properties so it is discoverable by
    /// GetJobsForBuildAsync, and must be stamped with <paramref name="targetStageAttempt"/>
    /// (the resubmitting monitor's own stage attempt) so the monitor gates on its own
    /// resubmission. It preserves the original submitter's System.JobAttempt and records
    /// <paramref name="monitorJobAttempt"/> separately for diagnostics.
    /// </summary>
    Task<HelixJobInfo> ResubmitWorkItemsAsync(
        HelixJobInfo originalJob,
        IReadOnlyCollection<WorkItemSummary> failedWorkItems,
        string targetStageAttempt,
        string monitorJobAttempt,
        CancellationToken cancellationToken);
}
