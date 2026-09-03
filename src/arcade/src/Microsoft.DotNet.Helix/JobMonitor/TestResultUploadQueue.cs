// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Fire-and-forget queue for AzDO test-result uploads. Each queued upload runs as an
    /// independent task with indefinite retry on transient errors. Both normal completion
    /// and cancellation paths must drain the queue before exiting so that results in
    /// flight when the runner exits are not abandoned.
    /// </summary>
    internal sealed class TestResultUploadQueue
    {
        private readonly ILogger _logger;
        private readonly JobMonitorOptions _options;
        private readonly IAzureDevOpsService _azdo;
        private readonly IHelixService _helix;
        private readonly Func<CancellationToken, Task> _delay;
        private readonly List<Task> _pending = [];

        public TestResultUploadQueue(
            ILogger logger,
            JobMonitorOptions options,
            IAzureDevOpsService azdo,
            IHelixService helix,
            Func<CancellationToken, Task> delay)
        {
            _logger = logger;
            _options = options;
            _azdo = azdo;
            _helix = helix;
            _delay = delay;
        }

        public void Enqueue(HelixJobInfo helixJob, IReadOnlyCollection<WorkItemSummary> workItems, CancellationToken cancellationToken)
        {
            IReadOnlyList<string> workItemNames = [.. workItems.Select(w => w.Name)];
            // Detached from the runner's cancellation token so that the drain path can finish
            // in-flight uploads on its own cancellation budget when the runner is canceled.
            Task uploadTask = Task.Run(() => UploadAsync(helixJob, workItemNames, cancellationToken), CancellationToken.None);
            _pending.Add(uploadTask);
        }

        public void Prune()
        {
            _pending.RemoveAll(static task => task.IsCompleted);
        }

        public async Task DrainAsync(CancellationToken cancellationToken)
        {
            Prune();
            if (_pending.Count == 0)
            {
                return;
            }

            _logger.LogInformation("Waiting for {Count} pending test result upload(s) to complete.", _pending.Count);
            try
            {
                await Task.WhenAll(_pending).WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // One or more upload tasks were cancelled; treat as best-effort drained.
            }
            Prune();
        }

        private async Task UploadAsync(
            HelixJobInfo helixJob,
            IReadOnlyCollection<string> workItemNames,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int testRunId = 0;

                try
                {
                    testRunId = await _azdo.CreateTestRunAsync(helixJob.TestRunName, helixJob.JobName, cancellationToken);
                    IReadOnlyList<WorkItemTestResults> downloaded = await _helix.DownloadTestResultsAsync(
                        helixJob.JobName,
                        workItemNames,
                        _options.WorkingDirectory,
                        cancellationToken);

                    int uploadedCount = await _azdo.UploadTestResultsAsync(testRunId, downloaded, cancellationToken);
                    await CompleteTestRunAsync(testRunId, helixJob, cancellationToken);

                    _logger.LogInformation("{UploadedCount} test results for job '{JobName}' processed.",
                        uploadedCount,
                        helixJob.DisplayName);
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex,
                        "Failed to upload test results for job {JobName} to Azure DevOps. Test run ID was {TestRunId}. Retrying after delay.",
                        helixJob.DisplayName,
                        testRunId);
                    await _delay(cancellationToken);
                }
            }
        }

        private async Task CompleteTestRunAsync(int testRunId, HelixJobInfo helixJob, CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    await _azdo.CompleteTestRunAsync(testRunId, cancellationToken);
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex,
                        "Failed to complete Azure DevOps test run {TestRunId} for job {JobName}. Retrying after delay.",
                        testRunId,
                        helixJob.JobName);
                    await _delay(cancellationToken);
                }
            }
        }
    }
}
