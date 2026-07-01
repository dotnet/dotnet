// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Abstracts Azure DevOps REST API interactions needed by the job monitor.
    /// </summary>
    public interface IAzureDevOpsService
    {
        /// <summary>
        /// Returns the build timeline records for the current build.
        /// Used to determine whether non-monitor pipeline jobs have completed.
        /// </summary>
        Task<IReadOnlyList<AzureDevOpsTimelineRecord>> GetTimelineRecordsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the set of Helix job names that have already been processed
        /// by a prior monitor invocation. A job is considered processed once its Azure DevOps
        /// test run has been completed and tagged with the Helix job name.
        /// </summary>
        Task<IReadOnlySet<string>> GetProcessedHelixJobNamesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the set of Helix (job name, work item name) pairs for which a prior
        /// monitor invocation uploaded at least one failed test result. Keyed by Helix
        /// job name with the value being the set of work item names that had at least
        /// one failed test. Used by the retry pass so that work items whose Helix exit
        /// code was zero but whose tests failed are still resubmitted on a fresh monitor
        /// invocation.
        /// </summary>
        Task<IReadOnlyDictionary<string, IReadOnlySet<string>>> GetFailedTestWorkItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new test run in Azure DevOps and returns its ID.
        /// If a test run with this name already exists in-progress (orphaned from a prior crash),
        /// the implementation may reuse it.
        /// </summary>
        Task<int> CreateTestRunAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a test run as completed and tags it with the Helix job name so a subsequent
        /// monitor invocation can tell the job's results have already been uploaded.
        /// When <paramref name="failedWorkItems"/> is non-empty, also persists the list of
        /// work item names that had failed tests as a structured attachment on the run so a
        /// later monitor invocation can recover the set of work items needing resubmission in
        /// a small fixed number of REST calls instead of paginating through individual failed
        /// test results.
        /// </summary>
        Task CompleteTestRunAsync(
            int testRunId,
            string helixJobName,
            IReadOnlyCollection<string> failedWorkItems,
            CancellationToken cancellationToken);

        /// <summary>
        /// Uploads test results for the specified work items into an existing test run.
        /// Returns a dictionary mapping each work item and job name to its upload summary.
        /// </summary>
        Task<IReadOnlyDictionary<(string JobName, string WorkItemName), TestResultUploadSummary>> UploadTestResultsAsync(
            int testRunId,
            IReadOnlyList<WorkItemTestResults> results,
            CancellationToken cancellationToken);
    }
}
