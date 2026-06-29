// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        /// Creates a new test run in Azure DevOps and returns its ID.
        /// If a test run with this name already exists in-progress (orphaned from a prior crash),
        /// the implementation may reuse it.
        /// </summary>
        Task<int> CreateTestRunAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a test run as completed and tags it with the Helix job name so a subsequent
        /// monitor invocation can tell the job's results have already been uploaded.
        /// </summary>
        Task CompleteTestRunAsync(int testRunId, string helixJobName, CancellationToken cancellationToken);

        /// <summary>
        /// Uploads test results for the specified work items into an existing test run.
        /// Returns the number of test results uploaded.
        /// </summary>
        Task<int> UploadTestResultsAsync(int testRunId, IReadOnlyList<WorkItemTestResults> results, CancellationToken cancellationToken);
    }
}
